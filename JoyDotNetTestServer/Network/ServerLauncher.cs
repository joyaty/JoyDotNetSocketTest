using Joy.Util;

namespace Joy.Server
{
    public class ServerLauncher
    {
        private SocketListener m_ListenerService;

        private string m_IP = string.Empty;
        private ushort m_Port;

        public ServerLauncher()
        {
            m_ListenerService = new SocketListener();
            m_IP = string.Empty;
            m_Port = 9960;
        }

        /// <summary>
        /// 启动Unity服务器
        /// </summary>
        public void StartServer()
        {
            if (string.IsNullOrEmpty(m_IP))
            {
                foreach (var item in System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList)
                {
                    LogHelper.Debug("查询设备IP:{0}", item);
                    if (item.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork && item.ToString().Contains("192.168"))
                    { // 本地局域网
                        StartServer(item.ToString(), m_Port);
                        break;
                    }
                }
            }
            else
            {
                StartServer(m_IP, m_Port);
            }
        }

        /// <summary>
        /// 启动服务器
        /// </summary>
        /// <param name="ip">绑定的IP</param>
        /// <param name="port">绑定的端口</param>
        private void StartServer(string ip, ushort port)
        {
            m_IP = ip;
            m_Port = port;
            m_ListenerService.Start(m_IP, m_Port);
            LogHelper.Debug("启动服务器ip: {0}:{1}", m_IP, m_Port);
        }

        /// <summary>
        /// 关闭服务器
        /// </summary>
        public void ShutDown()
        {
            LogHelper.Debug("关闭服务器中...");
            m_ListenerService.ShutDown();
            m_ListenerService = null;
            LogHelper.Debug("关闭服务器完成");
        }
    }
}