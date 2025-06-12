
using System.Net;
using System.Net.Sockets;

namespace Joy.Test.Server
{
    /// <summary>
    /// 游戏服务器
    /// </summary>
    public class GameSimpleServer
    {
        /// <summary>
        /// 监听
        /// </summary>
        private Socket? m_Listener = null;

        private bool m_ServerIsRunning = false;
        public bool IsRunning => m_ServerIsRunning;

        private readonly List<ClientSession> m_ClientHandlers = new List<ClientSession>();

        /// <summary>
        /// 游戏服务器初始化
        /// </summary>
        public void Initialize(string ip, int port)
        {
            m_Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var listenInfo = new IPEndPoint(IPAddress.Parse(ip), port);
            m_Listener.Bind(listenInfo);
            m_Listener.Listen(1000);
            m_ServerIsRunning = true;
            Console.WriteLine($"启动线程:{Thread.CurrentThread.ManagedThreadId}");
            m_Listener.BeginAccept(new AsyncCallback(ClientSocketAccept), m_Listener);
            Console.WriteLine($"服务器启动监听:{listenInfo}");
            // m_Listener.Accept();
            // StartAccept();
        }

        private async void StartAccept()
        {
            while (true)
            {
                if (!m_ServerIsRunning) { break; }
                if (m_Listener == null) { break; }
                Socket client = await m_Listener.AcceptAsync();
                Console.WriteLine($"收到远端连接: {client.RemoteEndPoint}");
                // 创建连接Session
                m_ClientHandlers.Add(new ClientSession(client));
                m_ClientHandlers.Last().Init();
            }
        }

        /// <summary>
        /// 游戏服务器每帧更新
        /// </summary>
        public void Update()
        {

        }

        /// <summary>
        /// 游戏服务器结束
        /// </summary>
        public void Terminate()
        {
            if (m_Listener != null)
            {
                m_Listener.Close();
                m_Listener.Dispose();
                m_Listener = null;
            }
        }

        /// <summary>
        /// 接收到客户端连接
        /// </summary>
        /// <param name="asyncResult"></param>
        private void ClientSocketAccept(IAsyncResult asyncResult)
        {
            Socket? listener = asyncResult.AsyncState as Socket;
            if (listener != null)
            {
                Socket client = listener.EndAccept(asyncResult);
                Console.WriteLine($"收到远端连接: {client.RemoteEndPoint}，ThreadId:{Thread.CurrentThread.ManagedThreadId}");
                // TODO 处理连接
                // 接收下一条连接
                listener.BeginAccept(new AsyncCallback(ClientSocketAccept), listener);
            }
        }
    }
}