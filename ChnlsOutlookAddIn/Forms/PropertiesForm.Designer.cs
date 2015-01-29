namespace chnls.Forms
{
    partial class PropertiesForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PropertiesForm));
            this.label1 = new System.Windows.Forms.Label();
            this.radioButtonChnlsIO = new System.Windows.Forms.RadioButton();
            this.radioButtonChnlsUS = new System.Windows.Forms.RadioButton();
            this.radioButtonChnlsDEV = new System.Windows.Forms.RadioButton();
            this.buttonDebug = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(158, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Email Channels Server";
            // 
            // radioButtonChnlsIO
            // 
            this.radioButtonChnlsIO.AutoSize = true;
            this.radioButtonChnlsIO.Location = new System.Drawing.Point(16, 47);
            this.radioButtonChnlsIO.Name = "radioButtonChnlsIO";
            this.radioButtonChnlsIO.Size = new System.Drawing.Size(121, 17);
            this.radioButtonChnlsIO.TabIndex = 1;
            this.radioButtonChnlsIO.TabStop = true;
            this.radioButtonChnlsIO.Text = "Production (chnls.io)";
            this.radioButtonChnlsIO.UseVisualStyleBackColor = true;
            this.radioButtonChnlsIO.CheckedChanged += new System.EventHandler(this.radioButtonChnlsIO_CheckedChanged);
            // 
            // radioButtonChnlsUS
            // 
            this.radioButtonChnlsUS.AutoSize = true;
            this.radioButtonChnlsUS.Location = new System.Drawing.Point(16, 70);
            this.radioButtonChnlsUS.Name = "radioButtonChnlsUS";
            this.radioButtonChnlsUS.Size = new System.Drawing.Size(95, 17);
            this.radioButtonChnlsUS.TabIndex = 8;
            this.radioButtonChnlsUS.TabStop = true;
            this.radioButtonChnlsUS.Text = "Beta (chnls.us)";
            this.radioButtonChnlsUS.UseVisualStyleBackColor = true;
            this.radioButtonChnlsUS.CheckedChanged += new System.EventHandler(this.radioButtonChnlsUS_CheckedChanged);
            // 
            // radioButtonChnlsDEV
            // 
            this.radioButtonChnlsDEV.AutoSize = true;
            this.radioButtonChnlsDEV.Location = new System.Drawing.Point(16, 93);
            this.radioButtonChnlsDEV.Name = "radioButtonChnlsDEV";
            this.radioButtonChnlsDEV.Size = new System.Drawing.Size(163, 17);
            this.radioButtonChnlsDEV.TabIndex = 9;
            this.radioButtonChnlsDEV.TabStop = true;
            this.radioButtonChnlsDEV.Text = "Development (chnlsdev.com)";
            this.radioButtonChnlsDEV.UseVisualStyleBackColor = true;
            this.radioButtonChnlsDEV.CheckedChanged += new System.EventHandler(this.radioButtonChnlsDEV_CheckedChanged);
            // 
            // buttonDebug
            // 
            this.buttonDebug.Location = new System.Drawing.Point(17, 138);
            this.buttonDebug.Name = "buttonDebug";
            this.buttonDebug.Size = new System.Drawing.Size(75, 23);
            this.buttonDebug.TabIndex = 10;
            this.buttonDebug.Text = "Show debug";
            this.buttonDebug.UseVisualStyleBackColor = true;
            this.buttonDebug.Click += new System.EventHandler(this.buttonDebug_Click);
            // 
            // PropertiesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(446, 173);
            this.Controls.Add(this.buttonDebug);
            this.Controls.Add(this.radioButtonChnlsDEV);
            this.Controls.Add(this.radioButtonChnlsUS);
            this.Controls.Add(this.radioButtonChnlsIO);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PropertiesForm";
            this.Text = "Email Channels Properties";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radioButtonChnlsIO;
        private System.Windows.Forms.RadioButton radioButtonChnlsUS;
        private System.Windows.Forms.RadioButton radioButtonChnlsDEV;
        private System.Windows.Forms.Button buttonDebug;
    }
}