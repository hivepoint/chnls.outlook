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
            this.labelMembers = new System.Windows.Forms.Label();
            this.channelTree = new chnls.Controls.ChannelTree();
            this.membersTree = new chnls.Controls.MembersTree();
            this.SuspendLayout();
            // 
            // labelChannels
            // 
            this.labelChannels.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelChannels.Location = new System.Drawing.Point(0, 0);
            this.labelChannels.Name = "labelChannels";
            this.labelChannels.Size = new System.Drawing.Size(300, 18);
            this.labelChannels.TabIndex = 1;
            this.labelChannels.Text = "@Channels";
            // 
            // labelMembers
            // 
            this.labelMembers.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labelMembers.Location = new System.Drawing.Point(0, 84);
            this.labelMembers.Name = "labelMembers";
            this.labelMembers.Size = new System.Drawing.Size(300, 16);
            this.labelMembers.TabIndex = 3;
            this.labelMembers.Text = "Members ";
            // 
            // channelTree
            // 
            this.channelTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.channelTree.Location = new System.Drawing.Point(0, 18);
            this.channelTree.Name = "channelTree";
            this.channelTree.Size = new System.Drawing.Size(300, 66);
            this.channelTree.TabIndex = 0;
            this.channelTree.SelectionChanged += new System.EventHandler(this.channelTree_SelectionChanged);
            // 
            // membersTree
            // 
            this.membersTree.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.membersTree.Location = new System.Drawing.Point(0, 100);
            this.membersTree.Name = "membersTree";
            this.membersTree.Size = new System.Drawing.Size(300, 200);
            this.membersTree.TabIndex = 2;
            // 
            // ADXOlFormComposeSidebar
            // 
            this.ClientSize = new System.Drawing.Size(300, 300);
            this.Controls.Add(this.channelTree);
            this.Controls.Add(this.labelMembers);
            this.Controls.Add(this.membersTree);
            this.Controls.Add(this.labelChannels);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "ADXOlFormComposeSidebar";
            this.Text = "ADXOlFormComposeSidebar";
            this.ADXBeforeFormShow += new AddinExpress.OL.ADXOlForm.BeforeFormShow_EventHandler(this.ADXOlComposeHelperForm_ADXBeforeFormShow);
            this.ADXAfterFormHide += new AddinExpress.OL.ADXOlForm.ADXAfterFormHideEventHandler(this.ADXOlComposeHelperForm_ADXAfterFormHide);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ADXOlComposeHelperForm_FormClosing);
            this.ResumeLayout(false);

        }
        #endregion

        private Controls.ChannelTree channelTree;
        private System.Windows.Forms.Label labelChannels;
        private Controls.MembersTree membersTree;
        private System.Windows.Forms.Label labelMembers;
    }
}
