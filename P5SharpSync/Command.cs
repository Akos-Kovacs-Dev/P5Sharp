using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace P5SharpSync
{




    internal sealed class Command
    {



        private IVsOutputWindowPane outputPane;

        private async Task InitializeOutputPaneAsync()
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            IVsOutputWindow outWindow = await package.GetServiceAsync(typeof(SVsOutputWindow)) as IVsOutputWindow;

            Guid paneGuid = new Guid("4f5b446f-f274-45f2-9eb7-6d1a3b3c430f");
            outWindow.CreatePane(ref paneGuid, "P5SharpSync Log", 1, 1);
            outWindow.GetPane(ref paneGuid, out outputPane);
        }

        private void Log(string message)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            outputPane?.OutputString($"{DateTime.Now:HH:mm:ss} - {message}\n");
        }


        private Func<string, Task> LogMessage()
        {
            return async (string code) =>
            {
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                Log(code);
            };
        }



        public const int CommandId = 0x0100;
        public static readonly Guid CommandSet = new Guid("88d8d572-de71-4c7d-aebd-ddb483ad5da9");
        private readonly AsyncPackage package;
        private OleMenuCommand menuCommand;
        private Server tcpServer;
        private string localIP;
        private int port = 12345;

        public static Command Instance { get; private set; }

        private Command(AsyncPackage package, OleMenuCommandService commandService)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var commandId = new CommandID(CommandSet, CommandId);
            menuCommand = new OleMenuCommand(Execute, commandId);
            menuCommand.BeforeQueryStatus += (s, e) => UpdateMenuText();
            commandService.AddCommand(menuCommand);

            localIP = GetFirstIPv4Address();
        }

        public static class Prompt
        {
            public static string ShowDialog(string text, string caption)
            {
                Form prompt = new Form()
                {
                    Width = 400,
                    Height = 150,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    Text = caption,
                    StartPosition = FormStartPosition.CenterScreen
                };

                Label label = new Label() { Left = 20, Top = 20, Text = text, Width = 340 };
                TextBox textBox = new TextBox() { Left = 20, Top = 50, Width = 340 };
                Button confirmation = new Button() { Text = "OK", Left = 270, Width = 90, Top = 80 };

                confirmation.DialogResult = DialogResult.OK;
                prompt.AcceptButton = confirmation;

                prompt.Controls.Add(label);
                prompt.Controls.Add(textBox);
                prompt.Controls.Add(confirmation);

                return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
            }
        }


        public static async Task InitializeAsync(AsyncPackage package)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            var commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;

            Instance = new Command(package, commandService);

            await Instance.InitializeOutputPaneAsync(); // now safe
        }

        public string ProjectDirectory = string.Empty;

        private async void Execute(object sender, EventArgs e)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            Log("Tools/P5Sync clicked");

            try
            {
                var dte = await package.GetServiceAsync(typeof(DTE)) as DTE;
                Project project = (dte?.ActiveSolutionProjects as Array)?.OfType<Project>().FirstOrDefault();

                if (project == null || string.IsNullOrEmpty(project.FullName))
                {
                    await ShowErrorAsync("Unable to find the active project.");
                    return;
                }

                ProjectDirectory = Path.GetDirectoryName(project.FullName);
                if (string.IsNullOrEmpty(ProjectDirectory))
                {
                    await ShowErrorAsync("Unable to determine the project directory.");
                    return;
                }

                Log($"Active project: {project.FullName}");
                Log($"Project directory: {ProjectDirectory}");

                // Show control dialog even if tcpServer is not created
                string status = tcpServer?.IsRunning == true ? "Running" : "Stopped";
                var files = tcpServer?.GetWatchedFileList();
                var dialog = new ServerControlDialog(status, files?.ToList() ?? new List<string>(), ProjectDirectory,localIP,port );
                DialogResult result = dialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    // Update IP and port from the dialog
                    localIP = dialog.NewIP;
                    port = dialog.NewPort;

                    switch (dialog.SelectedAction)
                    {
                        case "Restart":
                        case "Start":
                            if (tcpServer == null)
                            {
                                tcpServer = new Server(port, LogMessage());
                                Log($"TCP Server Created: {localIP}:{port}");
                                _ = tcpServer.StartAsync(ProjectDirectory);
                                Log("TCP Server Started");
                            }
                            else
                            {
                                tcpServer.Restart(ProjectDirectory);
                                Log($"Server restarted at {localIP}:{port}");
                            }
                            await ShowMessageAsync("P5Sharp Server running! Check Output Window for logs.");
                            break;
                        case "ClearFiles":
                            tcpServer?.ClearWatchedFiles();
                            Log("Watched files cleared.");
                            break;
                        case "Stop":
                            tcpServer?.Stop();
                            tcpServer = null;
                            Log("Server stopped by user.");
                            await ShowMessageAsync("Server stopped.");
                            break;

                        case "ChangeDir":
                            if (Directory.Exists(dialog.NewProjectDirectory))
                            {
                                ProjectDirectory = dialog.NewProjectDirectory;
                                if (tcpServer != null)
                                {
                                    tcpServer.Restart(ProjectDirectory);
                                    Log($"Project directory changed to: {ProjectDirectory}");
                                }
                            }
                            break;
                    }
                }




                UpdateMenuText();
            }
            catch (Exception ex)
            {
                await ShowErrorAsync($"Command execution failed:\n{ex.Message}");
            }
        }


        private void UpdateMenuText()
        {
            string status = tcpServer?.IsRunning == true ? "Running" : "Stopped";
            menuCommand.Text = $"P5SharpSync ({status})";

        }

        private string GetFirstIPv4Address()
        {
            foreach (var ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (ni.OperationalStatus != OperationalStatus.Up || ni.NetworkInterfaceType == NetworkInterfaceType.Loopback)
                    continue;

                foreach (var addr in ni.GetIPProperties().UnicastAddresses)
                {
                    if (addr.Address.AddressFamily == AddressFamily.InterNetwork)
                        return addr.Address.ToString();
                }
            }

            return "127.0.0.1";
        }

        private void ShowMessage(string message)
        {
            VsShellUtilities.ShowMessageBox(
                package,
                message,
                "P5SharpSync",
                OLEMSGICON.OLEMSGICON_WARNING,
                OLEMSGBUTTON.OLEMSGBUTTON_OK,
                OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
        }

        private async Task ShowErrorAsync(string message)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            VsShellUtilities.ShowMessageBox(
                package,
                message,
                "P5SharpSync Error",
                OLEMSGICON.OLEMSGICON_CRITICAL,
                OLEMSGBUTTON.OLEMSGBUTTON_OK,
                OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
        }

        private async Task ShowMessageAsync(string message)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            VsShellUtilities.ShowMessageBox(
                package,
                message,
                "P5SharpSync",
                OLEMSGICON.OLEMSGICON_INFO,
                OLEMSGBUTTON.OLEMSGBUTTON_OK,
                OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
        }
    }
}
