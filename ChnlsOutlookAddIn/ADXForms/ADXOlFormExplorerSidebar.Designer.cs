namespace chnls.ADXForms
{
    partial class ADXOlFormExplorerSidebar
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
  
  
        /// <summary>
        /// Clean uppreparation[n++] = " any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }
  
        #region Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelBrowsers = new System.Windows.Forms.Panel();
            this.splash = new chnls.Controls.Splash();
            this.webBrowserMain = new System.Windows.Forms.WebBrowser();
            this.upgradePanel = new chnls.Controls.UpgradePanel();
            this.debugBar = new chnls.Controls.DebugBar();
            this.statusToast = new chnls.Controls.StatusToast();
            this.panelBrowsers.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelBrowsers
            // 
            this.panelBrowsers.Controls.Add(this.splash);
            this.panelBrowsers.Controls.Add(this.webBrowserMain);
            this.panelBrowsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBrowsers.Location = new System.Drawing.Point(0, 29);
            this.panelBrowsers.Name = "panelBrowsers";
            this.panelBrowsers.Size = new System.Drawing.Size(315, 481);
            this.panelBrowsers.TabIndex = 1;
            // 
            // splash
            // 
            this.splash.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splash.Location = new System.Drawing.Point(0, 0);
            this.splash.Name = "splash";
            this.splash.Size = new System.Drawing.Size(315, 481);
            this.splash.TabIndex = 0;
            // 
            // webBrowserMain
            // 
            this.webBrowserMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowserMain.Location = new System.Drawing.Point(0, 0);
            this.webBrowserMain.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowserMain.Name = "webBrowserMain";
            this.webBrowserMain.Size = new System.Drawing.Size(315, 481);
            this.webBrowserMain.TabIndex = 1;
            this.webBrowserMain.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowserMain_DocumentCompleted);
            this.webBrowserMain.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.webBrowserMain_Navigating);
            // 
            // upgradePanel
            // 
            this.upgradePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.upgradePanel.Location = new System.Drawing.Point(0, 0);
            this.upgradePanel.Name = "upgradePanel";
            this.upgradePanel.Size = new System.Drawing.Size(315, 29);
            this.upgradePanel.TabIndex = 0;
            // 
            // debugBar
            // 
            this.debugBar.Browser = null;
            this.debugBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.debugBar.Location = new System.Drawing.Point(0, 562);
            this.debugBar.Margin = new System.Windows.Forms.Padding(2);
            this.debugBar.Name = "debugBar";
            this.debugBar.Size = new System.Drawing.Size(315, 24);
            this.debugBar.TabIndex = 0;
            // 
            // statusToast
            // 
            this.statusToast.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.statusToast.Location = new System.Drawing.Point(0, 510);
            this.statusToast.Name = "statusToast";
            this.statusToast.Size = new System.Drawing.Size(315, 52);
            this.statusToast.TabIndex = 3;
            // 
            // ADXOlFormExplorerSidebar
            // 
            this.ClientSize = new System.Drawing.Size(315, 586);
            this.Controls.Add(this.panelBrowsers);
            this.Controls.Add(this.upgradePanel);
            this.Controls.Add(this.statusToast);
            this.Controls.Add(this.debugBar);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "ADXOlFormExplorerSidebar";
            this.Text = "Email Channels";
            this.Load += new System.EventHandler(this.ADXOlFormExplorerSidebar_Load);
            this.panelBrowsers.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private Controls.UpgradePanel upgradePanel;
        private System.Windows.Forms.Panel panelBrowsers;
        private Controls.DebugBar debugBar;
        private System.Windows.Forms.WebBrowser webBrowserMain;
        private Controls.Splash splash;
        private Controls.StatusToast statusToast;

    }
}
