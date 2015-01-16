namespace chnls.Controls
{
    partial class DebugBar
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.buttonGo = new System.Windows.Forms.Button();
            this.buttonSplash = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.btnDump = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Location = new System.Drawing.Point(2, 2);
            this.buttonRefresh.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(23, 19);
            this.buttonRefresh.TabIndex = 0;
            this.buttonRefresh.Text = "R";
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // buttonGo
            // 
            this.buttonGo.Location = new System.Drawing.Point(29, 2);
            this.buttonGo.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.buttonGo.Name = "buttonGo";
            this.buttonGo.Size = new System.Drawing.Size(31, 19);
            this.buttonGo.TabIndex = 1;
            this.buttonGo.Text = "Go";
            this.buttonGo.UseVisualStyleBackColor = true;
            this.buttonGo.Click += new System.EventHandler(this.buttonGo_Click);
            // 
            // buttonSplash
            // 
            this.buttonSplash.Location = new System.Drawing.Point(64, 2);
            this.buttonSplash.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.buttonSplash.Name = "buttonSplash";
            this.buttonSplash.Size = new System.Drawing.Size(26, 19);
            this.buttonSplash.TabIndex = 2;
            this.buttonSplash.Text = "S";
            this.buttonSplash.UseVisualStyleBackColor = true;
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.Location = new System.Drawing.Point(188, 2);
            this.buttonClose.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(21, 19);
            this.buttonClose.TabIndex = 3;
            this.buttonClose.Text = "X";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // btnDump
            // 
            this.btnDump.Location = new System.Drawing.Point(94, 2);
            this.btnDump.Margin = new System.Windows.Forms.Padding(2);
            this.btnDump.Name = "btnDump";
            this.btnDump.Size = new System.Drawing.Size(26, 19);
            this.btnDump.TabIndex = 4;
            this.btnDump.Text = "D";
            this.btnDump.UseVisualStyleBackColor = true;
            this.btnDump.Click += new System.EventHandler(this.btnDump_Click);
            // 
            // DebugBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnDump);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonSplash);
            this.Controls.Add(this.buttonGo);
            this.Controls.Add(this.buttonRefresh);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "DebugBar";
            this.Size = new System.Drawing.Size(212, 24);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonRefresh;
        private System.Windows.Forms.Button buttonGo;
        private System.Windows.Forms.Button buttonSplash;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button btnDump;
    }
}
