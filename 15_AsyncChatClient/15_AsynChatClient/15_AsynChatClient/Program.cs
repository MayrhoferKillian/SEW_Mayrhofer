using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

class ChatClient
{
    private const string ServerIp = "127.0.0.1"; // Lokal zum Testen
    private const int Port = 5000;

    static async Task Main(string[] args)
    {
        Console.WriteLine("--- Async Chat Client ---");

        try
        {
            using var client = new TcpClient();
            await client.ConnectAsync(ServerIp, Port);
            Console.WriteLine("Verbunden mit dem Server!");

            using var stream = client.GetStream();

            var receiveTask = ReceiveMessages(stream);

            var sendTask = SendMessages(stream);

            await Task.WhenAny(receiveTask, sendTask);

            Console.WriteLine("Verbindung getrennt.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fehler: {ex.Message}");
        }
    }

    private static async Task ReceiveMessages(NetworkStream stream)
    {
        byte[] buffer = new byte[1024];
        try
        {
            while (true)
            {
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                if (bytesRead == 0) break; 

                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"\n[Server]: {message}");
                Console.Write("> ");
            }
        }
        catch {}
    }

    private static async Task SendMessages(NetworkStream stream)
    {
        while (true)
        {
            string? input = Console.ReadLine();
            if (string.IsNullOrEmpty(input)) continue;
            if (input.ToLower() == "exit") break;

            byte[] data = Encoding.UTF8.GetBytes(input);
            await stream.WriteAsync(data, 0, data.Length);
        }
    }
}