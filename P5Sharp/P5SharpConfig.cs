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
        /// Only needed for local TPC server on Windows.
        /// Example: C:\Users\YourUserName\Documents\YourProject\
        /// </summary>
        public string ProjectPath { get; init; }

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


                if (string.IsNullOrWhiteSpace(ProjectPath))
                {
                    throw new ArgumentException("ProjectPath is required when using LocalTPCServer.", nameof(ProjectPath));
                }

                if (!Directory.Exists(ProjectPath))
                {
                    throw new DirectoryNotFoundException($"The directory at path '{ProjectPath}' does not exist.");
                }

            }


        }
    }
}
