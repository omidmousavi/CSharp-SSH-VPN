namespace ssh_vpn
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btnToggle = new System.Windows.Forms.Button();
            this.githubLink = new System.Windows.Forms.LinkLabel();
            this.btnOpenSettings = new System.Windows.Forms.Button();
            this.timer_check_status = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnGh = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnToggle
            // 
            this.btnToggle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnToggle.Font = new System.Drawing.Font("Microsoft YaHei", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnToggle.Location = new System.Drawing.Point(12, 12);
            this.btnToggle.Name = "btnToggle";
            this.btnToggle.Size = new System.Drawing.Size(260, 73);
            this.btnToggle.TabIndex = 0;
            this.btnToggle.Text = "Connect";
            this.btnToggle.UseVisualStyleBackColor = true;
            this.btnToggle.Click += new System.EventHandler(this.btnToggle_Click);
            // 
            // githubLink
            // 
            this.githubLink.AutoSize = true;
            this.githubLink.Location = new System.Drawing.Point(12, 256);
            this.githubLink.Name = "githubLink";
            this.githubLink.Size = new System.Drawing.Size(65, 13);
            this.githubLink.TabIndex = 2;
            this.githubLink.TabStop = true;
            this.githubLink.Text = "Github page";
            this.githubLink.Visible = false;
            // 
            // btnOpenSettings
            // 
            this.btnOpenSettings.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOpenSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.btnOpenSettings.Location = new System.Drawing.Point(73, 161);
            this.btnOpenSettings.Name = "btnOpenSettings";
            this.btnOpenSettings.Size = new System.Drawing.Size(199, 52);
            this.btnOpenSettings.TabIndex = 1;
            this.btnOpenSettings.Text = "Open Settings";
            this.btnOpenSettings.UseVisualStyleBackColor = true;
            this.btnOpenSettings.Click += new System.EventHandler(this.btnOpenSettings_Click);
            // 
            // timer_check_status
            // 
            this.timer_check_status.Interval = 1000;
            this.timer_check_status.Tick += new System.EventHandler(this.timer_check_status_Tick);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipText = "SSH VPN Disconected...";
            this.notifyIcon1.Text = "SSH VPN NOTIF";
            this.notifyIcon1.Visible = true;
            // 
            // lblStatus
            // 
            this.lblStatus.BackColor = System.Drawing.Color.Red;
            this.lblStatus.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft YaHei", 14.25F);
            this.lblStatus.ForeColor = System.Drawing.SystemColors.Control;
            this.lblStatus.Location = new System.Drawing.Point(12, 91);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(260, 64);
            this.lblStatus.TabIndex = 6;
            this.lblStatus.Text = "Not Connected";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnGh
            // 
            this.btnGh.BackColor = System.Drawing.SystemColors.Desktop;
            this.btnGh.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnGh.BackgroundImage")));
            this.btnGh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnGh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGh.FlatAppearance.BorderSize = 0;
            this.btnGh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGh.Location = new System.Drawing.Point(12, 161);
            this.btnGh.Name = "btnGh";
            this.btnGh.Size = new System.Drawing.Size(55, 52);
            this.btnGh.TabIndex = 5;
            this.btnGh.UseVisualStyleBackColor = false;
            this.btnGh.Click += new System.EventHandler(this.btnGh_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(283, 227);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnGh);
            this.Controls.Add(this.btnOpenSettings);
            this.Controls.Add(this.githubLink);
            this.Controls.Add(this.btnToggle);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "SSH VPN";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.LinkLabel githubLink;
        private System.Windows.Forms.Button btnOpenSettings;
        private System.Windows.Forms.Timer timer_check_status;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.Button btnGh;
        private System.Windows.Forms.Label lblStatus;
        public System.Windows.Forms.Button btnToggle;
    }
}

