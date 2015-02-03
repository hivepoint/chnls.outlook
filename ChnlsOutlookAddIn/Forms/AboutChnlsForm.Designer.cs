namespace chnls.Forms
{
    partial class AboutChnlsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutChnlsForm));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.labelHeading = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.labelSupport = new System.Windows.Forms.Label();
            this.labelServer = new System.Windows.Forms.Label();
            this.labelVersion = new System.Windows.Forms.Label();
            this.buttonUpdate = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.labelUser = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::chnls.Properties.Resources.logo_square_256x256;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.Location = new System.Drawing.Point(12, 13);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(170, 170);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // labelHeading
            // 
            this.labelHeading.AutoSize = true;
            this.labelHeading.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelHeading.Location = new System.Drawing.Point(187, 13);
            this.labelHeading.Name = "labelHeading";
            this.labelHeading.Size = new System.Drawing.Size(177, 18);
            this.labelHeading.TabIndex = 2;
            this.labelHeading.Text = "Email Channels Add-In";
            this.labelHeading.Click += new System.EventHandler(this.labelHeading_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(187, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "HivePoint, Inc.";
            // 
            // labelSupport
            // 
            this.labelSupport.AutoSize = true;
            this.labelSupport.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelSupport.Location = new System.Drawing.Point(187, 114);
            this.labelSupport.Name = "labelSupport";
            this.labelSupport.Size = new System.Drawing.Size(176, 13);
            this.labelSupport.TabIndex = 5;
            this.labelSupport.Text = "Feedback: hivepoint.uservoice.com";
            this.labelSupport.Click += new System.EventHandler(this.labelSupport_Click);
            // 
            // labelServer
            // 
            this.labelServer.AutoSize = true;
            this.labelServer.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelServer.Location = new System.Drawing.Point(187, 74);
            this.labelServer.Name = "labelServer";
            this.labelServer.Size = new System.Drawing.Size(116, 13);
            this.labelServer.TabIndex = 6;
            this.labelServer.Text = "Server: https://chnls.io";
            this.labelServer.Click += new System.EventHandler(this.labelServer_Click);
            // 
            // labelVersion
            // 
            this.labelVersion.AutoSize = true;
            this.labelVersion.Location = new System.Drawing.Point(187, 34);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(66, 13);
            this.labelVersion.TabIndex = 7;
            this.labelVersion.Text = "Version: 333";
            // 
            // buttonUpdate
            // 
            this.buttonUpdate.Location = new System.Drawing.Point(308, 160);
            this.buttonUpdate.Name = "buttonUpdate";
            this.buttonUpdate.Size = new System.Drawing.Size(101, 23);
            this.buttonUpdate.TabIndex = 8;
            this.buttonUpdate.Text = "Check for update";
            this.buttonUpdate.UseVisualStyleBackColor = true;
            this.buttonUpdate.Click += new System.EventHandler(this.buttonUpdate_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(188, 160);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(101, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "Reset to defaults";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.buttonResetSettings_Click);
            // 
            // labelUser
            // 
            this.labelUser.AutoSize = true;
            this.labelUser.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelUser.Location = new System.Drawing.Point(187, 92);
            this.labelUser.Name = "labelUser";
            this.labelUser.Size = new System.Drawing.Size(107, 13);
            this.labelUser.TabIndex = 11;
            this.labelUser.Text = "User: <not signed in>";
            // 
            // AboutChnlsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(421, 197);
            this.Controls.Add(this.labelUser);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.buttonUpdate);
            this.Controls.Add(this.labelVersion);
            this.Controls.Add(this.labelServer);
            this.Controls.Add(this.labelSupport);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.labelHeading);
            this.Controls.Add(this.pictureBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutChnlsForm";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "About Email Channels";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label labelHeading;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelSupport;
        private System.Windows.Forms.Label labelServer;
        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.Button buttonUpdate;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label labelUser;
    }
}