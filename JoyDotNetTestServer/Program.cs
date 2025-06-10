// See https://aka.ms/new-console-template for more information

using System.Net;

namespace Joy.Test.Server
{
    public class Program
    {
        private static readonly string s_IPAddress = "127.0.0.1";
        private static readonly int s_Port = 9960;

        public static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            IOCPServer server = new IOCPServer(3, 32);
            server.Init();
            server.Start(new IPEndPoint(IPAddress.Parse(s_IPAddress), s_Port));

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
        }
    }
}


