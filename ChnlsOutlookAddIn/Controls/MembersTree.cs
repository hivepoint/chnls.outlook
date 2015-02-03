#region

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using chnls.Model;
using chnls.Utils;

#endregion

namespace chnls.Controls
{
    public partial class MembersTree : UserControl
    {
        private bool _dirty;
        private List<ChannelInfo> _selectedChannels;

        public MembersTree()
        {
            InitializeComponent();
        }


        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal List<ChannelInfo> SelectedChannels
        {
            set
            {
                _selectedChannels = value;
                UpdateTree();
            }
        }


        private void UpdateTree()
        {
            lock (this)
            {
                if (_dirty) return;

                _dirty = true;
                Scheduler.Run("Update Tree", UpdateTreeNow, 300);
            }
        }

        private void UpdateTreeNow()
        {
            lock (this)
            {
                _dirty = false;
            }
            var subscribers = new Dictionary<string,EmailAddress>();
            var watchers = new Dictionary<string, EmailAddress>();
            treeView.SuspendLayout();
            treeView.Nodes.Clear();
            try
            {
                foreach (var channel in _selectedChannels)
                {
                    foreach (var subscriber in channel.subscribers)
                    {
                        subscribers[subscriber.address.ToLowerInvariant()] = subscriber;
                    }
                    foreach (var watcher in channel.watchers)
                    {
                        watchers[watcher.address.ToLowerInvariant()] = watcher;
                    }
                }
                foreach (var address in subscribers.Keys)
                {
                    watchers.Remove(address);
                }
                AddMembers(@"Subscribers", @"subscribers", subscribers.Values);
                if (subscribers.Any() && watchers.Any())
                {
                    treeView.Nodes.Add(new TreeNode());
                }
                AddMembers(@"Watchers", @"watchers", watchers.Values);
            }
            finally
            {
                treeView.ResumeLayout();
            }
        }

        private void AddMembers(string label, string imageKey, ICollection<EmailAddress> members)
        {
            if (!members.Any()) return;
            var displayValues = members.Select(e => e.MailAddress.DisplayName).ToList();
            displayValues.Sort();
            treeView.Nodes.Add(new TreeNode { Text = label, ImageKey = imageKey });
            foreach (var email in displayValues)
            {
                treeView.Nodes.Add(new TreeNode { Text = email });
            }
        }

        #region event handles

        private void treeViewMembers_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            e.Cancel = true;
        }

        #endregion
    }
}