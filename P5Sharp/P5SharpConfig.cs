using System.Runtime.InteropServices;

namespace P5Sharp
{
    public class P5SharpConfig
    {
        /// <summary>
        /// The IP address to connect to.
        /// </summary>
        public string IP { get; init; }

        /// <summary>
        /// The port number to use.
        /// </summary>
        public int Port { get; init; }

         

        /// <summary>
        /// When is true will NOT use P5SharpSync extension and will run faster (Windows only).
        /// Indicates whether to use a local TPC server.
        /// </summary>
        public bool LocalTPCServer { get; init; }

        public void Validate()
        {
            if (LocalTPCServer)
            {

                if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    throw new PlatformNotSupportedException("LocalTPCServer is only supported on Windows.");
                }

                var ProjectPath = FindProjectRoot();

                if (string.IsNullOrWhiteSpace(ProjectPath))
                {
                    throw new ArgumentException("ProjectPath is required when using LocalTPCServer.", nameof(ProjectPath));
                }

                if (!Directory.Exists(ProjectPath))
                {
                    throw new DirectoryNotFoundException($"The directory at path '{ProjectPath}' does not exist.");
                }

                if (LocalTPCServer && Environment.OSVersion.Platform == PlatformID.Win32NT)
                {


                    if (!Directory.Exists(ProjectPath))
                        throw new Exception("Project path invalid!");

                    var _server = new Server(Port, message =>
                    {
                        //Console.WriteLine($"Log: {message}");
                        return Task.CompletedTask;
                    });

                    _ = Task.Run(() => _server.StartAsync(ProjectPath));
                }

            }


        }

        private string? FindProjectRoot(string? startDir = null)
        {
            string? dir = startDir ?? AppContext.BaseDirectory;

            while (!string.IsNullOrEmpty(dir))
            {
                if (Directory.GetFiles(dir, "*.csproj").Length > 0)
                    return dir;

                dir = Directory.GetParent(dir)?.FullName;
            }

            return null;
        }
    }
}
