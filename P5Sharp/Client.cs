using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace P5Sharp
{
    public class Client
    {
        private readonly string _serverIp;
        private readonly int _serverPort;
        private Func<string, Task> _callback;
        public Client(string serverIp, int serverPort)
        {
            _serverIp = serverIp;
            _serverPort = serverPort;
        }

        public async Task StartTcpCommunicationAsync(List<string> files, Func<string, Task> callback)
        {
            try
            {
                _callback = callback;
                var client = new TcpClient();
                await client.ConnectAsync(_serverIp, _serverPort);

                var stream = client.GetStream();
                var writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };
                var reader = new StreamReader(stream, Encoding.UTF8);

                // Send file watch list

                string json = Newtonsoft.Json.JsonConvert.SerializeObject(files);
                await writer.WriteLineAsync(json);

                Console.WriteLine("Listening for file content updates...\n");

                // Read plain text (not JSON)
                while (true)
                {
                    var line = await reader.ReadLineAsync();
                    if (line == null) break;

                    //Console.Clear();
                    try
                    {
                        var message = JsonConvert.DeserializeObject<FileContentMessage>(line);
                        Console.WriteLine("Received file content:");
                        Console.WriteLine(message.content);
                        await _callback(message.content);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Invalid JSON received: " + ex.Message);
                        Console.WriteLine("Raw: " + line);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
        public class FileContentMessage
        {
            public string content { get; set; }
        }
    }
}
