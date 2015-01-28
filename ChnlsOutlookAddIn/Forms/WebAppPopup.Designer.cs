namespace chnls.Forms
{
    partial class WebAppPopup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WebAppPopup));
            this.webBrowser = new chnls.Controls.ClosingWebBrowser();
            this.textboxUrl = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // webBrowser
            // 
            this.webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser.Location = new System.Drawing.Point(0, 20);
            this.webBrowser.MinimumSize = new System.Drawing.Size(320, 320);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.Size = new System.Drawing.Size(784, 541);
            this.webBrowser.TabIndex = 2;
            this.webBrowser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser_DocumentCompleted);
            this.webBrowser.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.webBrowser_Navigating);
            // 
            // textboxUrl
            // 
            this.textboxUrl.Dock = System.Windows.Forms.DockStyle.Top;
            this.textboxUrl.Location = new System.Drawing.Point(0, 0);
            this.textboxUrl.Name = "textboxUrl";
            this.textboxUrl.Size = new System.Drawing.Size(784, 20);
            this.textboxUrl.TabIndex = 3;
            // 
            // WebAppPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.webBrowser);
            this.Controls.Add(this.textboxUrl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(600, 400);
            this.Name = "WebAppPopup";
            this.Text = "@Channels";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WebAppPopup_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.ClosingWebBrowser webBrowser;
        private System.Windows.Forms.TextBox textboxUrl;
    }
}