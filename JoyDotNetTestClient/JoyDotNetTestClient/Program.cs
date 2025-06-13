
using Joy.Test.Client;

namespace Joy.Test.Client
{
    public class Program
    {
        private static readonly List<GameSimpleClient> s_SimpleClients = new List<GameSimpleClient>();

        public static void Main(string[] args)
        {
            Console.WriteLine("Hello, Client!");
            while (true)
            {
                string? inputValue = Console.ReadLine();
                if (inputValue == null) { break; }
                if (inputValue.Equals("link"))
                {
                    GameSimpleClient client = new GameSimpleClient(OnSocketConnected, OnSocketClose);
                    client.LinkToServer();

                }
                else if (inputValue.StartsWith("close"))
                {
                    string[] inputs = inputValue.Split(" ");
                    if (inputs.Length >= 2)
                    {
                        try
                        {
                            int closeIndex = int.Parse(inputs[1]);
                            if (closeIndex < s_SimpleClients.Count)
                            {
                                GameSimpleClient client = s_SimpleClients[closeIndex];
                                client?.Disconnect();
                                s_SimpleClients.RemoveAt(closeIndex);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                    }
                }
                else if (inputValue.StartsWith("send"))
                {
                    string[] inputs = inputValue.Split(" ");
                    if (inputs.Length == 2)
                    {
                        try
                        {
                            int index = int.Parse(inputs[1]);
                            if (index < s_SimpleClients.Count)
                            {
                                GameSimpleClient client = s_SimpleClients[index];
                                client?.SendMessage();
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                    }
                    else if (inputs.Length == 3)
                    {
                        try
                        {
                            int index = int.Parse(inputs[1]);
                            int cmdId = int.Parse(inputs[2]);
                            if (index < s_SimpleClients.Count)
                            {
                                GameSimpleClient client = s_SimpleClients[index];
                                client?.SendMessage(cmdId);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                    }
                }
                else if (inputValue.Equals("quit"))
                {
                    break;
                }
            }
        }

        private static void OnSocketConnected(GameSimpleClient client)
        {
            if (client == null) { return; }
            s_SimpleClients.Add(client);
        }

        private static void OnSocketClose(GameSimpleClient client)
        {
            if (client == null) { return; }
            s_SimpleClients.Remove(client);
        }
    }

}