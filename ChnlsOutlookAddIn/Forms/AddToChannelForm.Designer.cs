namespace chnls.Forms
{
    partial class AddToChannelForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddToChannelForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.channelTree = new chnls.Controls.ChannelTree();
            this.comboBoxFrom = new System.Windows.Forms.ComboBox();
            this.labelFrom = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.listBoxMessages = new System.Windows.Forms.ListView();
            this.columnHeaderFrom = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderSubject = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblChannels = new System.Windows.Forms.Label();
            this.btnShare = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.channelTree);
            this.panel1.Controls.Add(this.comboBoxFrom);
            this.panel1.Controls.Add(this.labelFrom);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.listBoxMessages);
            this.panel1.Controls.Add(this.lblChannels);
            this.panel1.Location = new System.Drawing.Point(7, 7);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(580, 349);
            this.panel1.TabIndex = 22;
            // 
            // channelTree
            // 
            this.channelTree.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.channelTree.Location = new System.Drawing.Point(6, 23);
            this.channelTree.Name = "channelTree";
            this.channelTree.Size = new System.Drawing.Size(139, 271);
            this.channelTree.TabIndex = 25;
            this.channelTree.SelectionChanged += new System.EventHandler(this.channelTree_SelectionChanged);
            // 
            // comboBoxFrom
            // 
            this.comboBoxFrom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxFrom.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.comboBoxFrom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFrom.FormattingEnabled = true;
            this.comboBoxFrom.Location = new System.Drawing.Point(117, 321);
            this.comboBoxFrom.Name = "comboBoxFrom";
            this.comboBoxFrom.Size = new System.Drawing.Size(408, 21);
            this.comboBoxFrom.TabIndex = 24;
            this.comboBoxFrom.SelectedIndexChanged += new System.EventHandler(this.comboBoxFrom_SelectedIndexChanged);
            // 
            // labelFrom
            // 
            this.labelFrom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelFrom.AutoSize = true;
            this.labelFrom.Location = new System.Drawing.Point(71, 325);
            this.labelFrom.Name = "labelFrom";
            this.labelFrom.Size = new System.Drawing.Size(33, 13);
            this.labelFrom.TabIndex = 23;
            this.labelFrom.Text = "From:";
            this.labelFrom.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(57, 302);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 22;
            this.label1.Text = "Send to:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Channels";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(148, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Selected messages";
            // 
            // listBoxMessages
            // 
            this.listBoxMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxMessages.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderFrom,
            this.columnHeaderSubject});
            this.listBoxMessages.FullRowSelect = true;
            this.listBoxMessages.GridLines = true;
            this.listBoxMessages.Location = new System.Drawing.Point(151, 23);
            this.listBoxMessages.MultiSelect = false;
            this.listBoxMessages.Name = "listBoxMessages";
            this.listBoxMessages.Size = new System.Drawing.Size(424, 271);
            this.listBoxMessages.TabIndex = 17;
            this.listBoxMessages.UseCompatibleStateImageBehavior = false;
            this.listBoxMessages.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderFrom
            // 
            this.columnHeaderFrom.Text = "From";
            this.columnHeaderFrom.Width = 87;
            // 
            // columnHeaderSubject
            // 
            this.columnHeaderSubject.Text = "Subject";
            this.columnHeaderSubject.Width = 317;
            // 
            // lblChannels
            // 
            this.lblChannels.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblChannels.AutoEllipsis = true;
            this.lblChannels.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblChannels.Location = new System.Drawing.Point(114, 302);
            this.lblChannels.Name = "lblChannels";
            this.lblChannels.Size = new System.Drawing.Size(461, 13);
            this.lblChannels.TabIndex = 19;
            this.lblChannels.Text = "label4";
            // 
            // btnShare
            // 
            this.btnShare.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShare.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnShare.Location = new System.Drawing.Point(431, 362);
            this.btnShare.Name = "btnShare";
            this.btnShare.Size = new System.Drawing.Size(75, 23);
            this.btnShare.TabIndex = 24;
            this.btnShare.Text = "Send";
            this.btnShare.UseVisualStyleBackColor = true;
            this.btnShare.Click += new System.EventHandler(this.btnShare_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(512, 362);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 25;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // AddToChannelForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(594, 391);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnShare);
            this.Controls.Add(this.btnCancel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AddToChannelForm";
            this.Text = "Add to Email Channels";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.AddToChannelForm_FormClosed);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox comboBoxFrom;
        private System.Windows.Forms.Label labelFrom;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListView listBoxMessages;
        private System.Windows.Forms.ColumnHeader columnHeaderFrom;
        private System.Windows.Forms.ColumnHeader columnHeaderSubject;
        private System.Windows.Forms.Label lblChannels;
        private System.Windows.Forms.Button btnShare;
        private System.Windows.Forms.Button btnCancel;
        private Controls.ChannelTree channelTree;
    }
}