using System.Net.Sockets;

namespace Joy.Test.Server
{
    /// <summary>
    /// 处理客户端连接
    /// </summary>
    public class ClientSession
    {
        /// <summary>
        /// 客户端与服务器的连接
        /// </summary>
        private readonly Socket m_ClientSocket;

        /// <summary>
        /// 接收缓冲区
        /// </summary>
        private ByteArray m_ReceiveBuffer;

        public ClientSession(Socket socket)
        {
            m_ClientSocket = socket;
            m_ReceiveBuffer = new ByteArray(4);
        }

        public void Init()
        {
            m_ClientSocket.BeginReceive(m_ReceiveBuffer.bytes, 0, m_ReceiveBuffer.RemainCount, SocketFlags.None, new AsyncCallback(OnReceiveMessage), m_ClientSocket);
        }

        /// <summary>
        /// 消息接收回调
        /// </summary>
        /// <param name="asyncResult"></param>
        private void OnReceiveMessage(IAsyncResult asyncResult)
        {
            bool isException = false;
            try
            {
                int length = m_ClientSocket.EndReceive(asyncResult);
                if (length == 0)
                {
                    Console.WriteLine("收到空信息！结束连接");
                    m_ClientSocket.Dispose();
                }

                // 继续接收后续的信息
                if (m_ClientSocket.Connected)
                {
                    m_ClientSocket.BeginReceive(m_ReceiveBuffer.bytes, 0, m_ReceiveBuffer.RemainCount, SocketFlags.None, new AsyncCallback(OnReceiveMessage), m_ClientSocket);
                }
            }
            catch (Exception exception)
            {
                isException = true;
                Console.WriteLine(exception.ToString());
            }
            finally
            {
                if (isException)
                {
                    SafeClose(m_ClientSocket);
                }
            }
        }

        /// <summary>
        /// 安全关闭Socket，释放连接信息
        /// </summary>
        /// <param name="socket"></param>
        private static void SafeClose(Socket socket)
        {
            try
            {
                // 通知远端停止发送信息
                socket.Shutdown(SocketShutdown.Send);
                // 等待可能还在传输的数据
                Thread.Sleep(50);
            }
            catch { }
            finally
            {
                // 关闭Socket，释放资源
                socket.Close();
            }
        }
    }
}