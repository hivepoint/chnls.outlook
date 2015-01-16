namespace chnls.Controls
{
    partial class StatusToast
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
            this.labelSupport = new System.Windows.Forms.Label();
            this.lblDetail = new System.Windows.Forms.Label();
            this.lblMessage = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelSupport
            // 
            this.labelSupport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelSupport.AutoSize = true;
            this.labelSupport.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelSupport.Location = new System.Drawing.Point(269, 27);
            this.labelSupport.Name = "labelSupport";
            this.labelSupport.Padding = new System.Windows.Forms.Padding(4);
            this.labelSupport.Size = new System.Drawing.Size(63, 21);
            this.labelSupport.TabIndex = 5;
            this.labelSupport.Text = "Feedback";
            this.labelSupport.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelSupport.Click += new System.EventHandler(this.labelSupport_Click);
            // 
            // lblDetail
            // 
            this.lblDetail.AutoEllipsis = true;
            this.lblDetail.Location = new System.Drawing.Point(0, 28);
            this.lblDetail.Name = "lblDetail";
            this.lblDetail.Size = new System.Drawing.Size(263, 21);
            this.lblDetail.TabIndex = 4;
            this.lblDetail.Text = "label1";
            this.lblDetail.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblMessage
            // 
            this.lblMessage.AutoEllipsis = true;
            this.lblMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.Location = new System.Drawing.Point(0, 0);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(327, 27);
            this.lblMessage.TabIndex = 3;
            this.lblMessage.Text = "label1";
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // StatusToast
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelSupport);
            this.Controls.Add(this.lblDetail);
            this.Controls.Add(this.lblMessage);
            this.Name = "StatusToast";
            this.Size = new System.Drawing.Size(327, 52);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelSupport;
        private System.Windows.Forms.Label lblDetail;
        private System.Windows.Forms.Label lblMessage;

    }
}
