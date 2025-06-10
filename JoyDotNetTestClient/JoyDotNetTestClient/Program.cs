
using Joy.Test.Client;

Console.WriteLine("Hello, World!");

List<GameSimpleClient> simpleClients = new List<GameSimpleClient>();

while (true)
{
    string? inputValue = Console.ReadLine();
    if (inputValue == null) { break; }
    if (inputValue.Equals("link"))
    {
        GameSimpleClient client = new GameSimpleClient();
        await client.LinkToServer();
        simpleClients.Add(client);
    }
    else if (inputValue.StartsWith("close"))
    {
        string[] inputs = inputValue.Split(" ");
        if (inputs.Length >= 2)
        {
            try
            {
                int closeIndex = int.Parse(inputs[1]);
                if (closeIndex < simpleClients.Count)
                {
                    GameSimpleClient client = simpleClients[closeIndex];
                    client?.Disconnect();
                    simpleClients.RemoveAt(closeIndex);
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


