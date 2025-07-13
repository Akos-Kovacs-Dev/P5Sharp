using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace P5SharpSync
{
    public class Server
    {
        private int Port;
        private bool _isRunning = false;
        public bool IsRunning => _isRunning;

        private TcpListener _listener;
        private readonly object _lock = new object();
        private string _projectDir;

        private Func<string, Task> _logcallback;

        private readonly List<TcpClient> _clients = new List<TcpClient>();

        private readonly Dictionary<string, FileSystemWatcher> _globalWatchers = new Dictionary<string, FileSystemWatcher>();
        private readonly Dictionary<TcpClient, List<string>> _clientFileMap = new Dictionary<TcpClient, List<string>>();

        public Server(int port, Func<string, Task> logcallback)
        {
            this.Port = port;
            _logcallback = logcallback;
        }

        public List<string> GetWatchedFileList()
        {
            lock (_lock)
            {
                return _globalWatchers.Keys.ToList();
            }
        }


        public async Task StartAsync(string projectDir)
        {
            if (_isRunning)
                return;

            _projectDir = projectDir;
            _listener = new TcpListener(IPAddress.Any, Port);
            _listener.Start();
            _isRunning = true;

            while (_isRunning)
            {
                try
                {
                    TcpClient client = await _listener.AcceptTcpClientAsync();
                    lock (_clients)
                    {
                        _clients.Add(client);
                    }
                    _ = Task.Run(() => HandleClientAsync(client));
                }
                catch (Exception ex)
                {
                    await _logcallback.Invoke("Error accepting client: " + ex.Message);
                }
            }
        }

        public void ClearWatchedFiles()
        {
            lock (_lock)
            {
                foreach (var watcher in _globalWatchers.Values)
                {
                    watcher.Dispose();
                }

                _globalWatchers.Clear();
                _clientFileMap.Clear();

                _logcallback?.Invoke("All file watchers removed for all clients.");
            }
        }



        public void Stop()
        {
            try
            {
                ClearWatchedFiles();
                _listener?.Stop();
                _logcallback?.Invoke("TCP server stopped");
            }
            catch (Exception ex)
            {
                _logcallback?.Invoke("Error stopping server: " + ex.Message);
            }
            finally
            {
                _isRunning = false;
                lock (_clients)
                {
                    foreach (var client in _clients)
                    {
                        client.Close();
                    }
                    _clients.Clear();
                }
            }
        }

        public void Restart(string projectDir)
        {
            Stop();
            _ = StartAsync(projectDir);
        }

        private async Task HandleClientAsync(TcpClient client)
        {
            try
            {
                var stream = client.GetStream();
                var writer = new StreamWriter(stream) { AutoFlush = true };
                var reader = new StreamReader(stream);
                await _logcallback.Invoke("Client connected.");

                var requestJson = await reader.ReadLineAsync();
                if (!string.IsNullOrEmpty(requestJson))
                {
                    try
                    {
                        await _logcallback.Invoke("Files to watch request from client: " + requestJson);
                        var newClientFilesToWatch = JsonConvert.DeserializeObject<List<string>>(requestJson);
                        var invalidFiles = new List<string>();

                        foreach (var file in newClientFilesToWatch)
                        {
                            var fullPath = Path.Combine(_projectDir, file);
                            if (!File.Exists(fullPath))
                            {
                                invalidFiles.Add(file);
                                await _logcallback.Invoke("File NOT found: " + file);
                            }
                            else
                            {
                                await _logcallback.Invoke("File found: " + file);
                            }
                        }

                        foreach (var file in invalidFiles)
                            newClientFilesToWatch.Remove(file);

                        StartWatchingFiles(client, newClientFilesToWatch);
                        OnFileChanged("Initial load.");

                    }
                    catch (Exception ex)
                    {
                        await _logcallback.Invoke("Failed to parse client request: " + ex.Message);
                    }
                }

                while ((await reader.ReadLineAsync()) != null) { }
            }
            catch (Exception ex)
            {
                await _logcallback.Invoke("Client disconnected or error: " + ex.Message);
            }
            finally
            {
                lock (_clients)
                {
                    _clients.Remove(client);
                }
                client.Close();
            }
        }

        private void StartWatchingFiles(TcpClient client, List<string> filesToWatch)
        {
            lock (_lock)
            {
                if (!_clientFileMap.ContainsKey(client))
                {
                    _clientFileMap[client] = new List<string>();
                }

                foreach (var file in filesToWatch)
                {
                    var fullPath = Path.GetFullPath(Path.Combine(_projectDir, file));

                    if (!_globalWatchers.ContainsKey(fullPath))
                    {
                        var watcher = new FileSystemWatcher
                        {
                            Path = Path.GetDirectoryName(fullPath),
                            Filter = Path.GetFileName(fullPath),
                            IncludeSubdirectories = false,
                            NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.Size,
                            EnableRaisingEvents = true
                        };

                        watcher.Changed += (s, e) => OnFileChanged(e.FullPath);
                        watcher.Created += (s, e) => OnFileChanged(e.FullPath);
                        watcher.Renamed += (s, e) => OnFileChanged(e.FullPath);

                        _globalWatchers[fullPath] = watcher;
                    }

                    if (!_clientFileMap[client].Contains(fullPath))
                        _clientFileMap[client].Add(fullPath);
                }
            }

            _logcallback?.Invoke($"Client registered {filesToWatch.Count} file(s) to watch.");
        }


        private async void OnFileChanged(string changedFilePath)
        {
            // Ignore temporary/swap files
            if (changedFilePath.Contains("~") || changedFilePath.EndsWith(".tmp"))
                return;

            _logcallback?.Invoke("File changed on: " + changedFilePath);

            try
            {
                lock (_lock)
                {
                    foreach (var kvp in _clientFileMap)
                    {
                        string mainfile = "";
                        var client = kvp.Key;
                        var fileList = kvp.Value;

                        if (!fileList.Contains(changedFilePath) && changedFilePath != "Initial load.")
                            continue; // This client isn't interested in this file

                        if (string.IsNullOrEmpty(mainfile))
                            mainfile = fileList.First();

                        var usingDirectives = new HashSet<string>();
                        var codeContent = new StringBuilder();
                        usingDirectives.Add("using SkiaSharp;");
                        usingDirectives.Add("using System.Collections.Generic;");
                        usingDirectives.Add("using System.Linq;");

                        foreach (var filePath in fileList)
                        {
                            if (!File.Exists(filePath)) continue;

                            const int maxRetries = 5;
                            const int delayMs = 100;
                            bool success = false;

                            for (int attempt = 0; attempt < maxRetries && !success; attempt++)
                            {
                                try
                                {
                                    using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                                    using (var reader = new StreamReader(fs))
                                    {
                                        string line;
                                        bool insideNamespace = false;
                                        int braceDepth = 0;
                                        bool startedBody = false;

                                        while ((line = reader.ReadLine()) != null)
                                        {
                                            string trimmedLine = line.Trim();

                                            if (trimmedLine.StartsWith("using "))
                                            {
                                                usingDirectives.Add(trimmedLine);
                                            }
                                            else if (trimmedLine.StartsWith("namespace "))
                                            {
                                                insideNamespace = true;

                                                // Look for inline opening brace (e.g. `namespace Foo {`)
                                                if (trimmedLine.Contains("{"))
                                                {
                                                    braceDepth++;
                                                    startedBody = true;
                                                }
                                                continue; // Skip namespace line
                                            }
                                            else if (insideNamespace)
                                            {
                                                if (!startedBody)
                                                {
                                                    // Skip lines until we hit the opening brace
                                                    if (trimmedLine == "{")
                                                    {
                                                        braceDepth++;
                                                        startedBody = true;
                                                    }
                                                    continue;
                                                }

                                                // Track opening/closing braces
                                                if (trimmedLine.Contains("{")) braceDepth++;
                                                if (trimmedLine.Contains("}")) braceDepth--;

                                                // Stop processing when exiting namespace
                                                if (braceDepth == 0)
                                                {
                                                    insideNamespace = false;
                                                    startedBody = false;
                                                    continue;
                                                }

                                                codeContent.AppendLine(line);
                                            }
                                            else
                                            {
                                                codeContent.AppendLine(line); // Lines outside any namespace
                                            }
                                        }
                                    }
                                    success = true;
                                    _logcallback?.Invoke("Read file for merge: " + filePath);
                                }
                                catch (IOException ex) when (attempt < maxRetries - 1)
                                {
                                    _logcallback?.Invoke($"Retry {attempt + 1}/{maxRetries} for locked file: {filePath}");
                                    Thread.Sleep(delayMs); // or await Task.Delay(delayMs) if outside lock
                                }
                                catch (Exception ex)
                                {
                                    _logcallback?.Invoke($"Unexpected error reading file {filePath}: {ex.Message}");
                                    break;
                                }
                            }
                        }

                        // Merge content
                        var finalContent = new StringBuilder();

                        foreach (var u in usingDirectives.OrderBy(s => s))
                            finalContent.AppendLine(u);

                        finalContent.AppendLine();
                        finalContent.Append("return new " + Path.GetFileNameWithoutExtension(mainfile) + "();\n");
                        finalContent.Append(codeContent.ToString());

                        var test = codeContent.ToString();

                        BroadcastToClient(client, changedFilePath, finalContent.ToString());
                        _logcallback?.Invoke("Merged content broadcast to client.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logcallback?.Invoke("Unhandled error in OnFileChanged: " + ex.Message);
            }
        }



        public class FileContentMessage
        {
            public string content { get; set; }
        }

        private void BroadcastToClient(TcpClient client, string filePath, string content)
        {
            var message = JsonConvert.SerializeObject(new { content });
            var utf8NoBom = new UTF8Encoding(false);

            try
            {
                if (client.Connected)
                {
                    var stream = client.GetStream();
                    var writer = new StreamWriter(stream, utf8NoBom) { AutoFlush = true };
                    writer.WriteLine(message);
                }
            }
            catch (Exception ex)
            {
                _logcallback?.Invoke("Broadcast error to client: " + ex.Message);
                client.Close();
                _clients.Remove(client);
                _clientFileMap.Remove(client);
            }
        }




    }
}
