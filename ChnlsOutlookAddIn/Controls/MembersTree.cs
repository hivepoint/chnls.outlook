#region

using System.Collections.Generic;
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
            var subscribers = new HashSet<string>();
            var watchers = new HashSet<string>();
            treeView.SuspendLayout();
            treeView.Nodes.Clear();
            try
            {
                foreach (var channel in _selectedChannels)
                {
                    foreach (var association in channel.associations)
                    {
                        if (association.interestLevel != EntityInterestLevel.INTERESTED)
                            continue;
                        switch (association.collection)
                        {
                            case EntityCollection.FOLLOWING:
                                watchers.Add(association.userEmail.ToLowerInvariant());
                                break;
                            case EntityCollection.SUBSCRIPTIONS:
                                subscribers.Add(association.userEmail.ToLowerInvariant());
                                break;
                        }
                    }
                }
                watchers.RemoveWhere(subscribers.Contains);
                AddMembers(@"Subscribers", @"subscribers", subscribers.ToList());
                if (subscribers.Any() && watchers.Any())
                {
                    treeView.Nodes.Add(new TreeNode());
                }
                AddMembers(@"Watchers", @"watchers", watchers.ToList());
            }
            finally
            {
                treeView.ResumeLayout();
            }
        }

        private void AddMembers(string label, string imageKey, List<string> members)
        {
            if (!members.Any()) return;

            members.Sort();
            treeView.Nodes.Add(new TreeNode { Text = label, ImageKey = imageKey });
            foreach (var email in members)
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