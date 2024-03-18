using System;
using System.Windows.Forms;
using Microsoft.Win32;

namespace ssh_vpn
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            string keyName = "ssh_vpn";
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(keyName))
            {
                key.SetValue("ip", txt_ip.Text);
                key.SetValue("port", txt_port.Value);
                key.SetValue("username", txt_username.Text);
                key.SetValue("password", txt_password.Text);

                MessageBox.Show("Successfully saved.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            string keyName = "ssh_vpn";
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(keyName))
            {
                if (key == null) return;
                int port = 22;

                txt_ip.Text = key.GetValue("ip") as string;

                if (int.TryParse(key.GetValue("port") as string, out port))
                    txt_port.Value = port;
               

                txt_username.Text = key.GetValue("username") as string;
                txt_password.Text = key.GetValue("password") as string;
            }
        }
    }
}
