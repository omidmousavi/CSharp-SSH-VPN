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

        SshClient sshClient = new SshClient("0.0.0.0","0000","0000");
        ForwardedPortDynamic portForwarded = new ForwardedPortDynamic(9000);

        private void button1_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            string keyName = "ssh_vpn";
            
            string password = "";
            string username = "";
            string ip = "";

            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(keyName))
            {
                if (key == null)
                {
                    MessageBox.Show("Error : You should set SSH server settings...", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    ip = key.GetValue("ip") as string;
                    username = key.GetValue("username") as string;
                    password = key.GetValue("password") as string;
                }
            }

            sshClient = new SshClient(ip, username, password);

            try
            {
                sshClient.Connect();
                sshClient.AddForwardedPort(portForwarded);
                portForwarded.Start();
                
                RegistryKey registry = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings", true);
                registry.SetValue("ProxyEnable", 1);
                registry.SetValue("ProxyServer", "socks5://127.0.0.1:9000");

                Cursor.Current = Cursors.Default;
                button3.Enabled = true;
                button1.Enabled = false;

                panel1.BackColor = Color.Green;
                label1.Text = "   Connected";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex);
            }
        }


        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel1.LinkVisited = true;
            linkLabel1.LinkBehavior = LinkBehavior.HoverUnderline;
            linkLabel1.Links[0].LinkData = "https://github.com/omidmousavi/csharp-ssh-vpn";

            // Open the URL in the default web browser
            System.Diagnostics.Process.Start(linkLabel1.Links[0].LinkData.ToString());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            portForwarded.Stop();
            sshClient.Disconnect();

            RegistryKey registry = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings", true);
            registry.SetValue("ProxyEnable", 0);
            registry.SetValue("ProxyServer", "");

            button1.Enabled = true;
            button3.Enabled = false;

            panel1.BackColor = Color.Red;
            label1.Text = "Not connected";
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
                    button3_Click(null, null);
                }
            }
        }

        private void opensettingsform_Click(object sender, EventArgs e)
        {
            SettingsForm SettingsForm = new SettingsForm();
            SettingsForm.Show();
        }

        bool back_status = false;
        private void timer_check_status_Tick(object sender, EventArgs e)
        {

            if (!sshClient.IsConnected && sshClient.IsConnected != back_status)
            {

                portForwarded.Stop();
                sshClient.Disconnect();

                RegistryKey registry = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings", true);
                registry.SetValue("ProxyEnable", 0);
                registry.SetValue("ProxyServer", "");

                button1.Enabled = true;
                button3.Enabled = false;

                panel1.BackColor = Color.Red;
                label1.Text = "Not connected";
            }

            back_status = sshClient.IsConnected;
        }
    }
}
