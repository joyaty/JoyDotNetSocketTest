// See https://aka.ms/new-console-template for more information

using Joy.Server;
using Joy.Util;

namespace Joy.Test.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello, Server!");

            // IOCPServer server = new IOCPServer(3, 32);
            // server.Init();
            // server.Start(new IPEndPoint(IPAddress.Parse(s_IPAddress), s_Port));

            // GameSimpleServer server = new GameSimpleServer();
            // server.Initialize(s_IPAddress, s_Port);
            // while (server.IsRunning)
            // {
            //     try
            //     {
            //         Thread.Sleep(30);
            //         server.Update();
            //     }
            //     catch (Exception exception)
            //     {
            //         Console.WriteLine(exception);
            //     }
            // }
            // server.Terminate();


            ServerLauncher serverLauncher = new ServerLauncher();
            serverLauncher.StartServer();

            // 卡住服务器主线程，等待任意输入，关闭服务器
            LogHelper.Debug("Press any to shutdown the server process.");
            Console.Read();

            // 关闭服务器
            serverLauncher.ShutDown();
            
        }
    }
}


