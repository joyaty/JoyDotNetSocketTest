
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Joy.Test.Client
{
    public class GameSimpleClient
    {
        private static readonly string s_ServerIP = "192.168.17.65";
        private static readonly int s_ServerPort = 9960;

        private readonly Socket m_Socket;

        /// <summary>
        /// Socket连接成功回调委托声明
        /// </summary>
        /// <param name="client"></param>
        public delegate void OnSocketConnectedCallback(GameSimpleClient client);
        /// <summary>
        /// Socket连接断开回调委托声明
        /// </summary>
        /// <param name="client"></param>
        public delegate void OnSocketCloseCallback(GameSimpleClient client);

        /// <summary>
        /// Socket连接成功回调处理
        /// </summary>
        private readonly OnSocketConnectedCallback m_OnConnectSuccess;
        /// <summary>
        /// Socket关闭回调处理
        /// </summary>
        private readonly OnSocketCloseCallback m_OnCloseCallback;

        private int m_SendIndex = 0;

        private Random m_RandomGen;

        public GameSimpleClient(OnSocketConnectedCallback onConnectSuccess, OnSocketCloseCallback onSocketCloseCallback)
        {
            m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            m_OnConnectSuccess = onConnectSuccess;
            m_OnCloseCallback = onSocketCloseCallback;
            m_SendIndex = 0;
            m_RandomGen = new Random();
        }

        public async void LinkToServer()
        {
            try
            {
                IPEndPoint serverInfo = new IPEndPoint(IPAddress.Parse(s_ServerIP), s_ServerPort);
                await m_Socket.ConnectAsync(serverInfo);
                if (m_Socket.Connected)
                {
                    // 连接成功回调
                    m_OnConnectSuccess?.Invoke(this);
                    Console.WriteLine($"连接服务器成功！{m_Socket.RemoteEndPoint}, {m_Socket.LocalEndPoint}");
                }
                else
                {
                    Console.WriteLine($"连接服务器失败！{m_Socket.RemoteEndPoint}, {m_Socket.LocalEndPoint}");
                }
            }
            catch (SocketException socketException)
            {
                Disconnect();
                Console.WriteLine($"连接服务器出现异常：{socketException}");
            }
        }

        public void Disconnect()
        {
            if (m_Socket != null)
            {
                Console.WriteLine($"关闭服务器连接1！{m_Socket.RemoteEndPoint}, {m_Socket.LocalEndPoint}");
                m_Socket.Close();
                m_OnCloseCallback?.Invoke(this);
            }
        }

        public async Task SendMessage()
        {
            if (m_Socket == null || !m_Socket.Connected)
            {
                Console.WriteLine("Socket未连接，无法发送消息");
                return;
            }

            // byte[] data = Encoding.ASCII.GetBytes($"Hello {++m_SendIndex} from {m_Socket.LocalEndPoint}");
            long randomValue = m_RandomGen.NextInt64();
            byte[] data = BitConverter.GetBytes(randomValue);
            int sendBytes = await m_Socket.SendAsync(data);
            Console.WriteLine($"{m_Socket.LocalEndPoint}:发送的字节数:{sendBytes},数据{randomValue}");
        }
    }
}