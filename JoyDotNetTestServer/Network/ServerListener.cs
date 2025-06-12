
using System;
using System.Net;
using System.Net.Sockets;
using Joy.Util;

namespace Joy.Server
{
    /// <summary>
    /// 服务器Socket启动监听，创建Session维护连接
    /// </summary>
    public class SocketListener
    {
        /// <summary>
        /// 服务器监听连接Socket
        /// </summary>
        private Socket m_Listener = null;

        /// <summary>
        /// 启动监听
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public void Start(string ip, ushort port)
        {
            m_Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // 设置socket的一些属性
            m_Listener.ReceiveBufferSize = ushort.MaxValue;
            m_Listener.SendBufferSize = ushort.MaxValue;
            m_Listener.SendTimeout = 3000;
            // 绑定IP和端口
            IPEndPoint bindInfo = new IPEndPoint(IPAddress.Parse(ip), port);
            m_Listener.Bind(bindInfo);
            // 设置等待队列上限
            m_Listener.Listen(100000);

            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            args.Completed += new EventHandler<SocketAsyncEventArgs>(OnAcceptCompleted);
            StartAccept(args);

            LogHelper.Debug("启动主线程：{0}, {1}", Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.Name);
        }

        /// <summary>
        /// 关闭服务器
        /// </summary>
        public void ShutDown()
        {

        }

        /// <summary>
        /// 启动服务器监听接受处理线程
        /// </summary>
        /// <param name="acceptArgs"></param>
        private void StartAccept(SocketAsyncEventArgs acceptArgs)
        {
            bool willRaiseEvent = false;
            while (!willRaiseEvent)
            {
                acceptArgs.AcceptSocket = null; // 重置以便重复使用
                willRaiseEvent = m_Listener.AcceptAsync(acceptArgs);
                if (!willRaiseEvent)
                { // 连接同步完成，同步处理，返回循环处理下一个连接请求
                    ProcessAccept(acceptArgs);
                }
            }
        }

        /// <summary>
        /// 异步处理连接请求(线程调用)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="acceptArgs"></param>
        private void OnAcceptCompleted(object sender, SocketAsyncEventArgs acceptArgs)
        {
            // 处理连接异步回调
            ProcessAccept(acceptArgs);
            // 处理下一个连接请求
            StartAccept(acceptArgs);
        }

        /// <summary>
        /// 处理远端连接请求
        /// </summary>
        /// <param name="acceptArgs"></param>
        private void ProcessAccept(SocketAsyncEventArgs acceptArgs)
        {
            if (acceptArgs.AcceptSocket == null)
            {
                LogHelper.Error("错误！收到空的远端连接信息，忽略处理!");
                return;
            }
            LogHelper.Debug("收到连接请求，处理线程:{0}，远端连接信息:{1}", Thread.CurrentThread.ManagedThreadId, acceptArgs.AcceptSocket.RemoteEndPoint);

            Session session = new Session(acceptArgs.AcceptSocket);
            session.Init();
        }
    }
}