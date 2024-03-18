using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;
using Renci.SshNet;


namespace ssh_vpn
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            SystemEvents.SessionEnding += new SessionEndingEventHandler(SystemEvents_SessionEnding);
        }

        SshClient sshClient = new SshClient("0.0.0.0", 22, "0000", "0000");
        ForwardedPortDynamic portForwarded = new ForwardedPortDynamic(9000);


        void Connect()
        {
            btnToggle.Text = "Connecting...";

            string password = registery_get_data("password");
            string username = registery_get_data("username");
            string ip = registery_get_data("ip");
            int port;

            if (!int.TryParse(registery_get_data("port"), out port)) port = 22;

            if (password == "" || password == "" || username == "" || ip == "")
            {
                MessageBox.Show("Error : You should set SSH server settings...", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Invoke((MethodInvoker)delegate
                {
                    btnOpenSettings_Click(null, null);
                    btnToggle.Text = "Connect";
                });
                return;
            }


            ThreadPool.QueueUserWorkItem(new WaitCallback((state) =>
            {
                sshClient = new SshClient(ip, port, username, password);

                try
                {
                    sshClient.Connect();
                    sshClient.AddForwardedPort(portForwarded);
                    portForwarded.Start();

                    set_windows_proxy();

                    Invoke((MethodInvoker)delegate
                    {
                        lblStatus.BackColor = Color.Green;
                        lblStatus.Text = "Connected      00:00:00";
                        btnToggle.Text = "Disconnect";

                        timer_check_status.Enabled = true;
                        timer_check_status.Start();
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error : " + ex.Message, "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Invoke((MethodInvoker)delegate { btnToggle.Text = "Connect"; });
                }
                finally
                {
                    Invoke((MethodInvoker)delegate { btnToggle.Enabled = true; });
                }
            }));
        }

        void Disconnect()
        {
            btnToggle.Text = "Disconnecting...";

            portForwarded.Stop();
            sshClient.Disconnect();
            unset_windows_proxy();

            btnToggle.Text = "Connect";
            lblStatus.BackColor = Color.Red;
            lblStatus.Text = "Not Connected";

            timer_check_status.Enabled = false;
            timer_check_status.Stop();
            seconds = 0;

            btnToggle.Enabled = true;
        }


        private void btnToggle_Click(object sender, EventArgs e)
        {
            btnToggle.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;

            if (sshClient.IsConnected)
                Disconnect();

            else Connect();

            Cursor.Current = Cursors.Default;
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!sshClient.IsConnected) return;

            DialogResult result = MessageBox.Show("Do you really wish to exit? the connection will be stopped.", "Exit Program?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
                e.Cancel = true;
            else
                Disconnect();

        }
        private void SystemEvents_SessionEnding(object sender, SessionEndingEventArgs e)
        {
            if (sshClient.IsConnected)
                btnToggle_Click(null, null);

            Application.Exit();
        }

        private void btnOpenSettings_Click(object sender, EventArgs e)
        {
            SettingsForm settingsForm = new SettingsForm();
            settingsForm.ShowDialog();
        }

        bool back_status = false;
        int seconds = 0;
        private void timer_check_status_Tick(object sender, EventArgs e)
        {
            if (!sshClient.IsConnected && sshClient.IsConnected != back_status)
            {
                Disconnect();

                notifyIcon1.Icon = SystemIcons.Warning;
                notifyIcon1.ShowBalloonTip(10);
            }
            else if (sshClient.IsConnected && sshClient.IsConnected != back_status)
            {
                seconds = 0;
                lblStatus.Text = "Connected      00:00:00";
            }
            else if (sshClient.IsConnected)
            { 
                seconds++;

                int hours = seconds / 3600;
                int minutes = (seconds % 3600) / 60;
                int remainingSeconds = seconds % 60;
                lblStatus.Text = "Connected      " + hours.ToString("D2") + ":" + minutes.ToString("D2") + ":" + remainingSeconds.ToString("D2");
            }

            back_status = sshClient.IsConnected;
        }

        private void set_windows_proxy()
        {
            RegistryKey registry = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings", true);
            registry.SetValue("ProxyEnable", 1);
            registry.SetValue("ProxyServer", "socks5://127.0.0.1:9000");

            WinINetInterop.InternetSetOption(IntPtr.Zero, WinINetInterop.INTERNET_OPTION_SETTINGS_CHANGED, IntPtr.Zero, 0);
            WinINetInterop.InternetSetOption(IntPtr.Zero, WinINetInterop.INTERNET_OPTION_REFRESH, IntPtr.Zero, 0);
        }

        private void unset_windows_proxy()
        {
            RegistryKey registry = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings", true);
            registry.SetValue("ProxyEnable", 0);
            registry.SetValue("ProxyServer", "");
        }

        private string registery_get_data(string name)
        {
            string keyName = "ssh_vpn";
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(keyName))
            {
                if (key == null)
                    return "";
                else
                    return key.GetValue(name) as string;
            }
        }

        private void btnGh_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/omidmousavi/csharp-ssh-vpn");
        }
    }

    public static class WinINetInterop
    {
        public const int INTERNET_OPTION_SETTINGS_CHANGED = 39;
        public const int INTERNET_OPTION_REFRESH = 37;

        [System.Runtime.InteropServices.DllImport("wininet.dll", SetLastError = true, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        public static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int lpdwBufferLength);
    }
}
