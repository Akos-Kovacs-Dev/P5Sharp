using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace P5Sharp
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    namespace P5Sharp
    {
        public class Client : IDisposable
        {
            private readonly string _serverIp;
            private readonly int _serverPort;
            private Func<string, Task> _callback;
            private CancellationTokenSource _cts;
            private Task _listeningTask;
            private List<string> _watchedFiles;
            private TcpClient _tcpClient;
            private bool _isListening;

            public Client(string serverIp, int serverPort)
            {
                _serverIp = serverIp;
                _serverPort = serverPort;
            }

            public async Task StartTcpCommunicationAsync(List<string> files, Func<string, Task> callback)
            {
                _watchedFiles = files;
                _callback = callback;

                await StartListeningAsync();
            }

            private async Task StartListeningAsync()
            {
                if (_isListening)
                    return;

                _isListening = true;

                try
                {
                    _cts = new CancellationTokenSource();
                    _tcpClient = new TcpClient();
                    await _tcpClient.ConnectAsync(_serverIp, _serverPort);

                    var stream = _tcpClient.GetStream();
                    var writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };
                    var reader = new StreamReader(stream, Encoding.UTF8);

                    string json = JsonConvert.SerializeObject(_watchedFiles);
                    await writer.WriteLineAsync(json);

                    Console.WriteLine("Listening for file content updates...\n");

                    _listeningTask = Task.Run(async () =>
                    {
                        while (!_cts.Token.IsCancellationRequested)
                        {
                            try
                            {
                                if (stream.DataAvailable)
                                {
                                    var line = await reader.ReadLineAsync();
                                    if (line == null) break;

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
                                else
                                {
                                    await Task.Delay(100);
                                }
                            }
                            catch (IOException)
                            {
                                break;
                            }
                        }

                        CleanupConnection();
                    }, _cts.Token);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    CleanupConnection();
                }
            }

            public void Pause()
            {
                if (!_isListening)
                    return;

                Console.WriteLine("Pausing TCP listener...");
                _cts?.Cancel();
                _isListening = false; // ✅ Add this line
            }


            public void ResumeAsync()
            {
                if (_isListening)
                    return;

                Console.WriteLine("Resuming TCP listener...");
                _= StartListeningAsync();
            }

            private void CleanupConnection()
            {
                _isListening = false;
                try
                {
                    _tcpClient?.Close();
                    _tcpClient?.Dispose();
                    _tcpClient = null;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Cleanup error: " + ex.Message);
                }
            }

            public class FileContentMessage
            {
                public string content { get; set; }
            }

            public void Dispose()
            {
                throw new NotImplementedException();
            }
        }
    }

}
