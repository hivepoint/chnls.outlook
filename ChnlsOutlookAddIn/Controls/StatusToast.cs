#region

using System;
using System.Windows.Forms;

#endregion

namespace chnls.Controls
{
    public partial class StatusToast : UserControl
    {
        private readonly Timer timer = new Timer();

        public StatusToast()
        {
            InitializeComponent();
            lblMessage.Text = "";
            lblDetail.Text = "";
            timer.Tick += timer_Tick;
        }

        public string Detail
        {
            set { lblDetail.Text = value; }
        }

        private void timer_Tick(object sender, EventArgs e)
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
                timer.Interval = timeout_seconds*1000;
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
    }
}