#region

using System;
using System.Windows.Forms;
using chnls.Service;
using chnls.Utils;

#endregion

namespace chnls.Forms
{
    public partial class AboutChnlsForm : Form
    {
        public AboutChnlsForm()
        {
            InitializeComponent();

            RefreshContent();
            PropertiesService.Instance.BaseUrlChanged += OnBaseUrlChanged;
        }

        private void OnBaseUrlChanged(object sender, EventArgs e)
        {
            RefreshContent();
        }

        private void RefreshContent()
        {
            labelServer.Text = @"Server: " + PropertiesService.Instance.BaseUrl;
            var userEmail = PropertiesService.Instance.UserEmail;
            if (String.IsNullOrWhiteSpace(userEmail))
            {
                userEmail = "<not signed in>";
            }
            labelUser.Text = @"User: " + userEmail;
            labelSupport.Text = @"Feedback: " + Constants.SupportEmailAddress;
            labelVersion.Text = @"Version: " + GetType().Assembly.GetName().Version;
        }

        private static void OptionsClick()
        {
            new PropertiesForm().ShowDialog();
        }

        private void labelSupport_Click(object sender, EventArgs e)
        {
            Close();
            Scheduler.Run("Send support email", () => AddinModule.CurrentInstance.ContactSupport(), 10);
        }

        private void labelHeading_Click(object sender, EventArgs e)
        {
            if ((((int) ModifierKeys) & ((int) Keys.Control)) > 0 && (((int) ModifierKeys) & ((int) Keys.Shift)) > 0)
            {
                OptionsClick();
            }
        }

        private void labelServer_Click(object sender, EventArgs e)
        {
            AddinModule.CurrentInstance.GotoChnlsServer();
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            UpdateService.Instance.CheckAndUpdate();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if ((((int) ModifierKeys) & ((int) Keys.Control)) > 0 && (((int) ModifierKeys) & ((int) Keys.Shift)) > 0)
            {
                OptionsClick();
            }
        }

        private void buttonResetSettings_Click(object sender, EventArgs e)
        {
            PropertiesService.Instance.ResetToDefaults();
            MessageBox.Show(@"Please restart Outlook to complete the reset");
            Close();
        }
    }
}