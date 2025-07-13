using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

public class ServerControlDialog : Form
{
    public string SelectedAction { get; private set; }
    public string NewProjectDirectory { get; private set; }
    public string NewIP => txtIP.Text;
    public int NewPort => int.TryParse(txtPort.Text, out var p) ? p : 12345;

    private TextBox txtProjectDir;
    private TextBox txtIP;
    private TextBox txtPort;
    private Button btnRestart;
    private Button btnClearFiles;
    private Button btnStopServer;

    public ServerControlDialog(string status, List<string> files, string projectDirectory, string currentIP, int currentPort)
    {
        InitializeComponent();
        Text = "P5SharpSync - Server Control";
        Width = 640;
        Height = 540;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterScreen;

        Label lblProjectDir = new Label() { Text = "Project Directory:", Top = 10, Left = 10, Width = 120 };
        txtProjectDir = new TextBox() { Text = projectDirectory, Top = 30, Left = 10, Width = 500, ReadOnly = true };
        Button btnChangeDir = new Button() { Text = "Change Dir", Top = 28, Left = 520, Width = 90 };

        btnChangeDir.Click += (s, e) =>
        {
            var folderDialog = new FolderBrowserDialog
            {
                Description = "Select new project directory",
                SelectedPath = txtProjectDir.Text
            };

            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                txtProjectDir.Text = folderDialog.SelectedPath;
                NewProjectDirectory = folderDialog.SelectedPath;
                SelectedAction = "ChangeDir";
                DialogResult = DialogResult.OK;
            }
        };

        Label lblStatus = new Label() { Text = $"Server Status: {status}", Top = 65, Left = 10, Width = 600 };

        Label lblIP = new Label() { Text = "Server IP:", Top = 90, Left = 10, Width = 100 };
        txtIP = new TextBox() { Text = currentIP, Top = 115, Left = 10, Width = 300 };

        Label lblPort = new Label() { Text = "Server Port:", Top = 145, Left = 10, Width = 100 };
        txtPort = new TextBox() { Text = currentPort.ToString(), Top = 170, Left = 10, Width = 300 };

        TextBox txtFiles = new TextBox()
        {
            Top = 200,
            Left = 10,
            Width = 600,
            Height = 220,
            Multiline = true,
            ReadOnly = true,
            ScrollBars = ScrollBars.Vertical,
            Text = files.Count > 0 ? string.Join(Environment.NewLine, files) : "No watched files."
        };

        string restartButtonText = status.Equals("Stopped", StringComparison.OrdinalIgnoreCase) ? "Start" : "Restart";

        btnRestart = new Button() { Text = restartButtonText, Left = 10, Width = 120, Top = 440 };
        btnClearFiles = new Button() { Text = "Clear Watched Files", Left = 140, Width = 150, Top = 440 };
        btnStopServer = new Button() { Text = "Stop Server", Left = 300, Width = 120, Top = 440 };
        Button btnCancel = new Button() { Text = "Cancel", Left = 430, Width = 120, Top = 440 };
        btnStopServer.Visible = status.Equals("Running", StringComparison.OrdinalIgnoreCase);

        btnRestart.Click += (s, e) =>
        {
            if (!System.Net.IPAddress.TryParse(txtIP.Text, out var ip) || ip.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork)
            {
                MessageBox.Show("Please enter a valid IPv4 address.", "Invalid IP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtIP.Focus();
                return;
            }

            if (!int.TryParse(txtPort.Text, out int parsedPort) || parsedPort < 1 || parsedPort > 65535)
            {
                MessageBox.Show("Please enter a valid port number (1–65535).", "Invalid Port", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPort.Focus();
                return;
            }

            SelectedAction = restartButtonText == "Start" ? "Start" : "Restart";
            DialogResult = DialogResult.OK;
        };

        btnClearFiles.Click += (s, e) =>
        {
            SelectedAction = "ClearFiles";
            DialogResult = DialogResult.OK;
        };

        btnStopServer.Click += (s, e) =>
        {
            SelectedAction = "Stop";
            DialogResult = DialogResult.OK;
        };

        btnCancel.Click += (s, e) =>
        {
            SelectedAction = "Cancel";
            DialogResult = DialogResult.Cancel;
        };

        Controls.AddRange(new Control[]
        {
            lblProjectDir, txtProjectDir, btnChangeDir,
            lblStatus, lblIP, txtIP,
            lblPort, txtPort,
            txtFiles,
            btnRestart, btnClearFiles, btnStopServer, btnCancel
        });
    }

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerControlDialog));
        this.SuspendLayout();
        this.ClientSize = new System.Drawing.Size(284, 261);
        this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        this.Name = "ServerControlDialog";
        this.ResumeLayout(false);
    }
}
