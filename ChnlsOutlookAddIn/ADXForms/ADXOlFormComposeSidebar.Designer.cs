using chnls.Utils;

namespace chnls.ADXForms
{
    partial class ADXOlFormComposeSidebar : ComposeMonitor.IComposeMonitorCallback
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
            this.labelChannels = new System.Windows.Forms.Label();
            this.panelHR = new System.Windows.Forms.Panel();
            this.panelSpacer = new System.Windows.Forms.Panel();
            this.channelTree = new chnls.Controls.ChannelTree();
            this.membersTree = new chnls.Controls.MembersTree();
            this.panelMembers = new System.Windows.Forms.Panel();
            this.panelMembers.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelChannels
            // 
            this.labelChannels.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelChannels.Location = new System.Drawing.Point(0, 0);
            this.labelChannels.Name = "labelChannels";
            this.labelChannels.Size = new System.Drawing.Size(300, 18);
            this.labelChannels.TabIndex = 1;
            this.labelChannels.Text = "Email Channels";
            // 
            // panelHR
            // 
            this.panelHR.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panelHR.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHR.Location = new System.Drawing.Point(0, 0);
            this.panelHR.Name = "panelHR";
            this.panelHR.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.panelHR.Size = new System.Drawing.Size(300, 1);
            this.panelHR.TabIndex = 3;
            // 
            // panelSpacer
            // 
            this.panelSpacer.BackColor = System.Drawing.SystemColors.Control;
            this.panelSpacer.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSpacer.Location = new System.Drawing.Point(0, 1);
            this.panelSpacer.Name = "panelSpacer";
            this.panelSpacer.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.panelSpacer.Size = new System.Drawing.Size(300, 3);
            this.panelSpacer.TabIndex = 4;
            // 
            // channelTree
            // 
            this.channelTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.channelTree.Location = new System.Drawing.Point(0, 18);
            this.channelTree.Name = "channelTree";
            this.channelTree.Size = new System.Drawing.Size(300, 182);
            this.channelTree.TabIndex = 0;
            this.channelTree.SelectionChanged += new System.EventHandler(this.channelTree_SelectionChanged);
            // 
            // membersTree
            // 
            this.membersTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.membersTree.Location = new System.Drawing.Point(0, 4);
            this.membersTree.Margin = new System.Windows.Forms.Padding(3, 8, 3, 3);
            this.membersTree.Name = "membersTree";
            this.membersTree.Size = new System.Drawing.Size(300, 91);
            this.membersTree.TabIndex = 2;
            // 
            // panelMembers
            // 
            this.panelMembers.BackColor = System.Drawing.SystemColors.Control;
            this.panelMembers.Controls.Add(this.membersTree);
            this.panelMembers.Controls.Add(this.panelSpacer);
            this.panelMembers.Controls.Add(this.panelHR);
            this.panelMembers.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelMembers.Location = new System.Drawing.Point(0, 200);
            this.panelMembers.Name = "panelMembers";
            this.panelMembers.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.panelMembers.Size = new System.Drawing.Size(300, 100);
            this.panelMembers.TabIndex = 5;
            // 
            // ADXOlFormComposeSidebar
            // 
            this.ClientSize = new System.Drawing.Size(300, 300);
            this.Controls.Add(this.channelTree);
            this.Controls.Add(this.panelMembers);
            this.Controls.Add(this.labelChannels);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "ADXOlFormComposeSidebar";
            this.Text = "ADXOlFormComposeSidebar";
            this.ADXBeforeFormShow += new AddinExpress.OL.ADXOlForm.BeforeFormShow_EventHandler(this.ADXOlComposeHelperForm_ADXBeforeFormShow);
            this.ADXAfterFormShow += new AddinExpress.OL.ADXOlForm.AfterFormShow_EventHandler(this.ADXOlFormComposeSidebar_ADXAfterFormShow);
            this.ADXAfterFormHide += new AddinExpress.OL.ADXOlForm.ADXAfterFormHideEventHandler(this.ADXOlComposeHelperForm_ADXAfterFormHide);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ADXOlComposeHelperForm_FormClosing);
            this.Resize += new System.EventHandler(this.ADXOlFormComposeSidebar_Resize);
            this.panelMembers.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private Controls.ChannelTree channelTree;
        private System.Windows.Forms.Label labelChannels;
        private Controls.MembersTree membersTree;
        private System.Windows.Forms.Panel panelHR;
        private System.Windows.Forms.Panel panelSpacer;
        private System.Windows.Forms.Panel panelMembers;
    }
}
