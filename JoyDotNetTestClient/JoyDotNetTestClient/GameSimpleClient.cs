using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Joy.Test.Client
{
    public class GameSimpleClient
    {
        private static readonly string s_ServerIP = "127.0.0.1";
        private static readonly int s_ServerPort = 9960;

        private readonly Socket m_Socket;

        public GameSimpleClient()
        { 
            m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public async Task LinkToServer()
        {
            IPEndPoint serverInfo = new IPEndPoint(IPAddress.Parse(s_ServerIP), s_ServerPort);
            await m_Socket.ConnectAsync(serverInfo);

            if (m_Socket.Connected && m_Socket.Poll(1000, SelectMode.SelectWrite))
            {
                Console.WriteLine($"连接服务器成功！{m_Socket.RemoteEndPoint}, {m_Socket.LocalEndPoint}");
            }
            else
            {
                Console.WriteLine($"连接服务器失败！{m_Socket.RemoteEndPoint}, {m_Socket.LocalEndPoint}");
            }
        }

        public void Disconnect()
        {
            if (m_Socket != null)
            {
                Console.WriteLine($"关闭服务器连接1！{m_Socket.RemoteEndPoint}, {m_Socket.LocalEndPoint}");
                m_Socket.Close();
            }
        }
    }
}