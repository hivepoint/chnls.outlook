#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using chnls.Model;
using chnls.Properties;
using chnls.Service;
using chnls.Utils;
using Microsoft.Office.Interop.Outlook;
using Application = Microsoft.Office.Interop.Outlook.Application;

#endregion

namespace chnls.Forms
{
    public partial class AddToChannelForm : Form
    {
        public AddToChannelForm(List<MailItem> mailItems)
        {
            InitializeComponent();
            ResizeColumns();
            Text = @"Send " + mailItems.Count + @" message" + (mailItems.Count == 1 ? "" : "s") + @" to Email Channels";
            PropertiesService.Instance.ChannelListChanged += Instance_ChannelListChanged;
            PropertiesService.Instance.GroupListChanged += Instance_ChannelListChanged;
            UpdateChannels(50);
            channelTree.SuggestedChannelIds = PropertiesService.Instance.RecentForwardChannels;
            UpdateMessageList(mailItems);
            UpdateAccounts();
            UpdateShareButton();
        }

        void Instance_ChannelListChanged(object sender, EventArgs e)
        {
            UpdateChannels(200);
        }

        private void UpdateChannels(int delay)
        {
            Scheduler.RunIfNotScheduled("AddToChannelForm:UpdateChannels", "UpdateChannels", delay, () =>
            {
                Channels = PropertiesService.Instance.Channels;
                Groups = PropertiesService.Instance.Groups;
                if (null == Channels || !Channels.Any())
                {
                    MessageBox.Show(@"Error: No channels available. Please ensure you are signed in and you have channels.");
                    Close();
                }
            });
        }

        private List<ChannelGroupInfo> Groups { get; set; }

        private List<ChannelInfo> Channels { get; set; }

        internal Action<Account, List<ChannelInfo>> AddToChannels { private get; set; }

        public override sealed string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }


        private void UpdateMessageList(IEnumerable<MailItem> mailItems)
        {
            foreach (var mailItem in mailItems)
            {
                var from = mailItem.SenderName;
                if (String.IsNullOrWhiteSpace(from))
                {
                    from = mailItem.SenderEmailAddress;
                }
                listBoxMessages.Items.Add(new ListViewItem(new[] { from, mailItem.Subject }));
            }
        }

        private void UpdateAccounts()
        {
            Application application = null;
            NameSpace session = null;
            Accounts accounts = null;
            try
            {
                application = AddinModule.CurrentInstance.OutlookApp.Application;
                session = application.Session;
                accounts = session.Accounts;
                for (var i = 1; i <= accounts.Count; i++) // COM 1 BASED
                {
                    var account = accounts[i];
                    if (String.IsNullOrWhiteSpace(account.SmtpAddress)) continue;

                    var accountComboItem = new AccountComboItem { Account = account };
                    comboBoxFrom.Items.Add(accountComboItem);
                    if (comboBoxFrom.Items.Count == 1 ||
                        String.Equals(PropertiesService.Instance.LastForwardFromAddress, account.SmtpAddress))
                    {
                        comboBoxFrom.SelectedItem = accountComboItem;
                    }
                }
            }
            finally
            {
                if (null != accounts)
                {
                    Marshal.ReleaseComObject(accounts);
                    accounts = null;
                }
                if (null != session)
                {
                    Marshal.ReleaseComObject(session);
                    session = null;
                }
                if (null != application)
                {
                    Marshal.ReleaseComObject(application);
                    application = null;
                }
            }
            labelFrom.Visible = comboBoxFrom.Items.Count > 0;
            comboBoxFrom.Visible = comboBoxFrom.Items.Count > 0;
        }

        private void channelTree_SelectionChanged(object sender, EventArgs e)
        {
            UpdateShareButton();
        }

        private void comboBoxFrom_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void UpdateShareButton()
        {
            var selectedChannels = channelTree.SelectedChannels;
            var channelsSelected = selectedChannels.Any();
            btnShare.Enabled = channelsSelected;
            lblChannels.Text = channelsSelected
                ? string.Join(", ", selectedChannels.Select(e => e.name).ToArray())
                : Resources.AddToChannelForm_UpdateChannelList_please_select_a_channel;
        }

        private void ResizeColumns()
        {
            var width = listBoxMessages.ClientSize.Width;
            var from = (int)(width * 0.25);
            var subject = width - from;

            listBoxMessages.Columns[0].Width = from;
            listBoxMessages.Columns[1].Width = subject;
        }

        public new void Close()
        {
            foreach (var cbitem in comboBoxFrom.Items)
            {
                var item = (AccountComboItem)cbitem;
                Marshal.ReleaseComObject(item.Account);
                item.Account = null;
            }
            comboBoxFrom.Items.Clear();

            base.Close();
        }

        private void btnShare_Click(object sender, EventArgs e)
        {
            var selectedChannels = channelTree.SelectedChannels;

            if (!selectedChannels.Any()) return;

            AddToChannels(((AccountComboItem)comboBoxFrom.SelectedItem).Account, selectedChannels);
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private class AccountComboItem
        {
            internal Account Account;

            public override string ToString()
            {
                return Account.SmtpAddress;
            }
        }
    }
}