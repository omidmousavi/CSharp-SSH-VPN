using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using Renci.SshNet;

namespace ssh_vpn
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SshClient sshClient = new SshClient("0.0.0.0",22,"0000","0000");
        ForwardedPortDynamic portForwarded = new ForwardedPortDynamic(9000);

        private void btnConnect_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            
            btnConnect.Text = "Connecting...";

            string password = registery_get_data("password");
            string username = registery_get_data("username");
            string ip = registery_get_data("ip");
            int port;

            if (!int.TryParse(registery_get_data("port"), out port)) port = 22;

            if (password == null || password == null || ip == null)
            {
                MessageBox.Show("Error : You should set SSH server settings...", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnConnect.Text = "Connect";
                return;
            }

            sshClient = new SshClient(ip, port, username, password);

            try
            {
                sshClient.Connect();
                sshClient.AddForwardedPort(portForwarded);
                portForwarded.Start();

                set_windows_proxy();

                Cursor.Current = Cursors.Default;

                button3.Enabled = true;
                btnConnect.Enabled = false;

                panel1.BackColor = Color.Green;
                label1.Text = "   Connected";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex);
                btnConnect.Text = "Connect";
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            portForwarded.Stop();
            sshClient.Disconnect();

            btnConnect.Text = "Connect";

            unset_windows_proxy();

            btnConnect.Enabled = true;
            button3.Enabled = false;

            panel1.BackColor = Color.Red;
            label1.Text = "Not connected";

            Cursor.Current = Cursors.Default;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (sshClient.IsConnected)
            {
                var result = MessageBox.Show("Do you want to exit? If you click on yes, the VPN will be disconnected...", "Exit Program?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    btnDisconnect_Click(null, null);
                }
            }
        }

        private void opensettingsform_Click(object sender, EventArgs e)
        {
            SettingsForm SettingsForm = new SettingsForm();
            SettingsForm.Show();
        }

        private void githubLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            githubLink.LinkVisited = true;
            githubLink.LinkBehavior = LinkBehavior.HoverUnderline;
            githubLink.Links[0].LinkData = "https://github.com/omidmousavi/csharp-ssh-vpn";

            // Open the URL in the default web browser
            System.Diagnostics.Process.Start(githubLink.Links[0].LinkData.ToString());
        }

        bool back_status = false;
        int seconds = 0;
        private void timer_check_status_Tick(object sender, EventArgs e)
        {

            if (!sshClient.IsConnected && sshClient.IsConnected != back_status)
            {

                portForwarded.Stop();
                sshClient.Disconnect();

                unset_windows_proxy();

                btnConnect.Enabled = true;
                button3.Enabled = false;

                panel1.BackColor = Color.Red;
                label1.Text = "Not connected";

                btnConnect.Text = "Connect";

                notifyIcon1.BalloonTipText = "SSH VPN Disconected...";
                notifyIcon1.BalloonTipTitle = "";
                notifyIcon1.Icon = SystemIcons.Warning;
                notifyIcon1.ShowBalloonTip(10);
            }
            else if (sshClient.IsConnected && sshClient.IsConnected != back_status)
            {
                seconds = 0;
                btnConnect.Text = "00:00:00";
            }
            else if (sshClient.IsConnected)
            {
                seconds++;

                int hours = seconds / 3600;
                int minutes = (seconds % 3600) / 60;
                int remainingSeconds = seconds % 60;
                btnConnect.Text = hours.ToString("D2") + ":" + minutes.ToString("D2") + ":" + remainingSeconds.ToString("D2");
            }

            back_status = sshClient.IsConnected;
        }

        private void set_windows_proxy()
        {
            RegistryKey registry = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings", true);
            registry.SetValue("ProxyEnable", 1);
            registry.SetValue("ProxyServer", "socks5://127.0.0.1:9000");

            //WinINetInterop.InternetSetOption(IntPtr.Zero, WinINetInterop.INTERNET_OPTION_SETTINGS_CHANGED, IntPtr.Zero, 0);
            //WinINetInterop.InternetSetOption(IntPtr.Zero, WinINetInterop.INTERNET_OPTION_REFRESH, IntPtr.Zero, 0);
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
                {
                    return "";
                }
                else
                {
                    return key.GetValue(name) as string;                    
                }
            }            
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
