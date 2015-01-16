using chnls.Controls;

namespace chnls.Forms
{
    partial class WebPopupWindowForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WebPopupWindowForm));
            this.textboxUrl = new System.Windows.Forms.TextBox();
            this.webBrowser = new ClosingWebBrowser();
            this.SuspendLayout();
            // 
            // textboxUrl
            // 
            this.textboxUrl.Dock = System.Windows.Forms.DockStyle.Top;
            this.textboxUrl.Location = new System.Drawing.Point(0, 0);
            this.textboxUrl.Name = "textboxUrl";
            this.textboxUrl.Size = new System.Drawing.Size(564, 20);
            this.textboxUrl.TabIndex = 1;
            this.textboxUrl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textboxUrl_KeyDown);
            // 
            // webBrowser
            // 
            this.webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser.Location = new System.Drawing.Point(0, 20);
            this.webBrowser.MinimumSize = new System.Drawing.Size(320, 320);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.Size = new System.Drawing.Size(564, 501);
            this.webBrowser.TabIndex = 0;
            this.webBrowser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser_DocumentCompleted);
            this.webBrowser.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.webBrowser_Navigated);
            this.webBrowser.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.webBrowser_Navigating);
            this.webBrowser.NewWindow += new System.ComponentModel.CancelEventHandler(this.webBrowser_NewWindow);
            // 
            // WebPopupWindowForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(564, 521);
            this.Controls.Add(this.webBrowser);
            this.Controls.Add(this.textboxUrl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WebPopupWindowForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "HivePoint";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WebPopupWindowForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ClosingWebBrowser webBrowser;
        private System.Windows.Forms.TextBox textboxUrl;
    }
}