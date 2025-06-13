
using System.Buffers;
using System.Net.Sockets;
using Joy.Util;

namespace Joy.Server
{
    /// <summary>
    /// 描述服务器当前与某个远端的连接
    /// </summary>
    public class Session
    {
        /// <summary>
        /// 连接的Socket
        /// </summary>
        private Socket m_Socket;

        /// <summary>
        /// 接收缓冲区
        /// </summary>
        private byte[] m_ReceiveBuffer;

        /// <summary>
        /// 发送缓冲区
        /// </summary>
        private byte[] m_SendBuffer;

        // private Memory<byte> m_ReceiveBuffer;

        public Session(Socket socket)
        {
            m_Socket = socket;
            m_ReceiveBuffer = new byte[16];
            m_SendBuffer = new byte[16];
        }

        /// <summary>
        /// 初始化连接
        /// </summary>
        public void Init()
        {
            // 创建接收的异步事件对象，绑定异步回调和接收缓冲区
            SocketAsyncEventArgs receiveEventArgs = new SocketAsyncEventArgs();
            receiveEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnReceiveCompleted);
            receiveEventArgs.SetBuffer(m_ReceiveBuffer, 0, m_ReceiveBuffer.Length);
            StartReceive(receiveEventArgs);
            // 创建发送的异步事件对象，绑定异步回调和发送缓冲区
            SocketAsyncEventArgs sendEventArgs = new SocketAsyncEventArgs();
            sendEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnSendCompleted);
            sendEventArgs.SetBuffer(m_SendBuffer, 0, m_SendBuffer.Length);
            StartSend(sendEventArgs);
        }

        /// <summary>
        /// 启动异步接收消息
        /// </summary>
        private void StartReceive(SocketAsyncEventArgs recieveEventArgs)
        {
            if (!m_Socket.ReceiveAsync(recieveEventArgs))
            { // 同步接收消息完成，处理接收
                ProcessReceive(recieveEventArgs);
            }
        }

        /// <summary>
        /// 异步接收回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="receiveEventArgs"></param>
        private void OnReceiveCompleted(object sender, SocketAsyncEventArgs receiveEventArgs)
        {
            // 处理消息
            ProcessReceive(receiveEventArgs);
        }

        /// <summary>
        /// 处理消息接收
        /// </summary>
        /// <param name="receiveEventArgs"></param>
        private void ProcessReceive(SocketAsyncEventArgs receiveEventArgs)
        {
            LogHelper.Debug("消息接收线程:{0},{1}, 远端:{2}, 接受的字节数:{3}", Thread.CurrentThread.ManagedThreadId
                , Thread.CurrentThread.Name, m_Socket.RemoteEndPoint, receiveEventArgs.BytesTransferred);

            int length = receiveEventArgs.BytesTransferred;
            if (length == 0)
            { // 远端Socket关闭
                LogHelper.Warning("socket.receive返回0字节，正常是远端Socket被调用了Close，执行释放Socket资源操作!");
                SafeReleaseSocket();
                return;
            }
            int offset = receiveEventArgs.Offset;
            LogHelper.Debug("收到消息: offset = {0}, 消息长度 = {1}", offset, length);
            if (length == 4)
            {
                int value = BitConverter.ToInt32(receiveEventArgs.Buffer, offset);
                LogHelper.Debug("解析消息: IntValue = {0}", value);
            }
            else if (length == 8)
            {
                long value = BitConverter.ToInt64(receiveEventArgs.Buffer, offset);
                LogHelper.Debug("收到消息: LongValue = {0}", value);
            }

            StartReceive(receiveEventArgs);
        }

        /// <summary>
        /// 发送字符串数据
        /// </summary>
        /// <param name="message"></param>
        public void SendMessage(string message)
        {
            if (m_Socket == null || !m_Socket.Connected)
            {
                LogHelper.Error("Socket连接已断开，无法发送数据。ThreadId:{0}", Thread.CurrentThread.ManagedThreadId);
                return;
            }
            if (string.IsNullOrEmpty(message))
            {
                LogHelper.Warning("发送的目标数据字符串为空，不处理。ThreadId:{0}, SocketInfo:{1}", Thread.CurrentThread.ManagedThreadId, m_Socket.RemoteEndPoint);
                return;
            }
            // 消息的字节长度
            int byteLength = System.Text.Encoding.UTF8.GetByteCount(message);
            byte[] bytes = new byte[4096];
            System.Text.Encoding.UTF8.GetBytes(message, 0, message.Length, bytes, 0);
        }

        /// <summary>
        /// 启动异步发送消息
        /// </summary>
        private void StartSend(SocketAsyncEventArgs sendEventArgs)
        {
            string message = "";
            System.Text.Encoding.UTF8.GetBytes(message);

            if (!m_Socket.SendAsync(sendEventArgs))
            { // 同步接收消息完成，处理接收
                ProcessSendMessage(sendEventArgs);
            }
        }

        /// <summary>
        /// 异步发送回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="sendEventArgs"></param>
        private void OnSendCompleted(object sender, SocketAsyncEventArgs sendEventArgs)
        {
            // 处理发送消息
            ProcessSendMessage(sendEventArgs);
            // 发送后续的消息
            StartSend(sendEventArgs);
        }

        /// <summary>
        /// 处理消息发送
        /// </summary>
        /// <param name="sendEventArgs"></param>
        private void ProcessSendMessage(SocketAsyncEventArgs sendEventArgs)
        {
            LogHelper.Debug("消息发送线程:{0},{1}, 远端:{2}, 接受的字节数:{3}", Thread.CurrentThread.ManagedThreadId
                , Thread.CurrentThread.Name, m_Socket.RemoteEndPoint, sendEventArgs.BytesTransferred);
        }

        private void SafeReleaseSocket()
        {
            try
            {
                m_Socket.Shutdown(SocketShutdown.Send);
            }
            catch { }
            finally
            {
                m_Socket.Close();
            }
        }

    }
}