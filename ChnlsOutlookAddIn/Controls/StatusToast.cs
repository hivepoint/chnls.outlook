using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace chnls.Controls
{
    public partial class StatusToast : UserControl
    {
        private Timer timer = new Timer();
        public StatusToast()
        {
            InitializeComponent();
            lblMessage.Text = "";
            lblDetail.Text = "";
            timer.Tick += timer_Tick;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (timer.Enabled)
            {
                timer.Enabled = false;
                HideToast();
            }
        }

        public void Show(string message, string detail, int timeout_seconds)
        {
            if (String.IsNullOrWhiteSpace(message))
            {
                return;
            }
            lblMessage.Text = message;
            lblDetail.Text = detail;
            if (timeout_seconds > 5)
            {
                timer.Interval = timeout_seconds * 1000;
                timer.Enabled = false;
                timer.Enabled = true;
            }
            else
            {
                timer.Enabled = false;
            }
        }

        public void HideToast()
        {
            Visible = false;
        }

        private void labelSupport_Click(object sender, EventArgs e)
        {
            AddinModule.CurrentInstance.ContactSupport();
        }

        public string Detail { set { lblDetail.Text = value; } }
    }
}
