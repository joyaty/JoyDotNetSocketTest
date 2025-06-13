
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Joy.Test.Client
{
    public class GameSimpleClient
    {
        private static readonly string s_ServerIP = "192.168.17.65";
        private static readonly int s_ServerPort = 8089; // 9960

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

        public const byte CheckCode1 = 0x5A;
        public const byte CheckCode2 = 0xA5;

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

        public async void SendMessage()
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

        public async void SendMessage(int cmdId)
        {
            if (m_Socket == null || !m_Socket.Connected)
            {
                Console.WriteLine("Socket未连接，无法发送消息");
                return;
            }

            string message = GenerateMessage(cmdId);
            int byteLength = System.Text.Encoding.UTF8.GetByteCount(message);
            byte[] sendBytes = new byte[byteLength + 6];
            sendBytes[0] = CheckCode1;
            sendBytes[1] = CheckCode2;
            sendBytes[2] = (byte)(byteLength);
            sendBytes[3] = (byte)(byteLength >> 8);
            sendBytes[4] = (byte)(byteLength >> 16);
            sendBytes[5] = (byte)(byteLength >> 24);

            int writeByteLength = System.Text.Encoding.UTF8.GetBytes(message, 0, message.Length, sendBytes, 6);
            if (byteLength != writeByteLength)
            {
                Console.WriteLine($"数据写入丢失！消息包数据字节长度:{byteLength}, 消息写入字节长度:{writeByteLength}。");
                return;
            }
            int sendLength = await m_Socket.SendAsync(sendBytes);
            Console.WriteLine($"{m_Socket.LocalEndPoint}:发送的字节数:{sendLength},数据:{message}");
        }

        private string GenerateMessage(int cmdId)
        {
            NetBoxMessage msg = new NetBoxMessage();
            msg.msg = "";
            msg.code = 200;
            if (cmdId == (int)NetCmd.CreateGame)
            {
                NetRcvCreateGame netRcvCreateGame = new NetRcvCreateGame()
                {
                    game_mode = 1,
                    ballCount = -1,
                    isZhuiFenMode = false,
                };
                msg.cmd = cmdId;
                msg.data = JsonSerializer.Serialize(netRcvCreateGame);
            }
            else if (cmdId == (int)NetCmd.Shot)
            {
                NetRcvShot netRcvShot = new NetRcvShot()
                {
                    gameId = 1,
                    round = 1,
                    needPutBall = false,
                    px = 0,
                    py = 0,
                    pz = 0,
                    playerNextHitBall = 1,
                    hitBallDetail = PhysicsShotData.GetShotData((float)m_RandomGen.NextDouble() * 0.3f + 0.7f),
                };
                msg.cmd = cmdId;
                msg.data = JsonSerializer.Serialize(netRcvShot);
            }
            string message = JsonSerializer.Serialize(msg);
            return message;
        }
    }
}