#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using chnls.Model;
using chnls.Service;
using chnls.Utils;

#endregion

namespace chnls.Controls
{
    partial class ChannelTree : UserControl
    {
        private const string ChannelTag = "channel";
        private const string SelectedChannelTag = "channel_selected";
        private readonly List<String> _suggestedChannelIds = new List<string>();

        private List<ChannelInfo> _channels;
        private bool _dirty;
        private List<ChannelGroupInfo> _groups;

        public ChannelTree()
        {
            InitializeComponent();
            SelectedChannelIds = new HashSet<string>();
            Channels = PropertiesService.Instance.Channels;
            Groups = PropertiesService.Instance.Groups;
            UpdateTree();
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<string> SelectedChannelAddresses
        {
            get
            {
                var ret = new List<string>();
                foreach (var channelId in SelectedChannelIds)
                {
                    var channel = Channels.FirstOrDefault(e => e._id.Equals(channelId));
                    if (null != channel)
                    {
                        ret.Add(channel.channelEmailAddress.address);
                    }
                }
                return ret;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<ChannelInfo> SelectedChannels
        {
            get { return Channels.Where(e => SelectedChannelIds.Contains(e._id)).ToList(); }
            set
            {
                if (!SelectedChannelIds.Except(value.Select(e => e._id)).Any() &&
                    value.All(e => _suggestedChannelIds.Contains(e._id))) return;

                SelectedChannelIds.Clear();
                foreach (var channel in value)
                {
                    SelectedChannelIds.Add(channel._id);
                }
                UpdateTree();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<String> SuggestedChannelIds
        {
            private get { return _suggestedChannelIds.ToList(); }
            set
            {
                if (!_suggestedChannelIds.Except(value).Any() && !value.Except(_suggestedChannelIds).Any()) return;

                _suggestedChannelIds.Clear();
                _suggestedChannelIds.AddRange(value);
                UpdateTree();
            }
        }

        private List<ChannelInfo> Channels
        {
            get { return _channels; }
            set
            {
                _channels = value;
                _channels.Sort((c0, c1) => String.Compare(c0.name, c1.name, StringComparison.InvariantCultureIgnoreCase));
            }
        }

        private List<ChannelGroupInfo> Groups
        {
            get { return _groups; }
            set
            {
                _groups = value;
                _groups.Sort((c0, c1) => String.Compare(c0.name, c1.name, StringComparison.InvariantCultureIgnoreCase));
            }
        }

        private HashSet<string> SelectedChannelIds { get; set; }

        public void UpdateSelectedChannels(List<ChannelInfo> selectedChannels)
        {
            SelectedChannels = selectedChannels;
        }

        public void SetState(List<ChannelInfo> channels, List<ChannelGroupInfo> groups)
        {
            Channels = channels;
            Groups = groups;
            UpdateTree();
        }

        public void SetSelectedAddresses(List<string> selectedAddresses)
        {
            SelectedChannelIds.Clear();
            foreach (var address in selectedAddresses)
            {
                foreach (var channelInfo in Channels)
                {
                    if (String.Equals(channelInfo.channelEmailAddress.address, address,
                        StringComparison.InvariantCultureIgnoreCase))
                    {
                        SelectedChannelIds.Add(channelInfo._id);
                    }
                }
            }
            UpdateTree();
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
            treeView.SuspendLayout();
            treeView.Nodes.Clear();
            try
            {
                foreach (var channelId in SelectedChannelIds)
                {
                    var selectedChannel = Channels.FirstOrDefault(element => element._id.Equals(channelId));
                    if (null != selectedChannel)
                    {
                        AddChannel(selectedChannel, treeView.Nodes, SelectedChannelTag, true);
                    }
                }
                var suggestedChannels = SuggestedChannelIds.Where(e => !SelectedChannelIds.Contains(e)).ToList();
                if (suggestedChannels.Any())
                {
                    var node = new TreeNode
                    {
                        Text = @"Suggestions",
                        ImageKey = @"stack",
                        ForeColor = SystemColors.GrayText
                    };

                    treeView.Nodes.Add(node);
                    foreach (var channelId in suggestedChannels)
                    {
                        var recentChannel = Channels.FirstOrDefault(element => element._id.Equals(channelId));
                        if (null != recentChannel)
                        {
                            AddChannel(recentChannel, treeView.Nodes, ChannelTag, true);
                        }
                    }
                }
                var hasGroups = false;
                foreach (var group in Groups)
                {
                    var groupInfo = group;
                    var channels = Channels.Where(
                        elemenet =>
                            elemenet.groupId.Equals(groupInfo._id) && !SelectedChannelIds.Contains(elemenet._id) &&
                            !SuggestedChannelIds.Contains(elemenet._id)).ToList();

                    hasGroups |= AddGroup(treeView.Nodes, group, channels);
                }
                var goGroupInfo = new ChannelGroupInfo {_id = null, name = hasGroups ? "Other" : "Channels"};
                var goGroupChannels = Channels.Where(
                    elemenet =>
                        String.IsNullOrWhiteSpace(elemenet.groupId) && !SelectedChannelIds.Contains(elemenet._id) &&
                        !SuggestedChannelIds.Contains(elemenet._id)).ToList();

                AddGroup(treeView.Nodes, goGroupInfo, goGroupChannels);
            }
            finally
            {
                treeView.ResumeLayout();
            }
        }

        private bool AddGroup(TreeNodeCollection nodes, ChannelGroupInfo group, List<ChannelInfo> channels)
        {
            var node = new TreeNode
            {
                Text = group.name,
                Name = group._id,
                ImageKey = @"stack",
                ForeColor = SystemColors.GrayText
            };
            nodes.Add(node);
            foreach (var channel in channels)
            {
                AddChannel(channel, node.Nodes, ChannelTag, false);
            }
            node.Expand();
            return true;
        }

        private void AddChannel(ChannelInfo channel, TreeNodeCollection group, string tag, bool addGroup)
        {
            var text = channel.name;
            if (addGroup)
            {
                var channelGroup = Groups.FirstOrDefault(element => element._id.Equals(channel.groupId));
                text += "@" + (null != channelGroup ? channelGroup.name : "go");
            }

            var node = new TreeNode
            {
                Tag = tag,
                Text = text,
                Name = channel._id,
                ImageKey = SelectedChannelTag.Equals(tag) ? @"checkmark2" : null
            };
            group.Add(node);
        }

        public event EventHandler SelectionChanged;
        public event EventHandler<ChannelInfoEventArgs> ChannelSelected;
        public event EventHandler<ChannelInfoEventArgs> ChannelUnselected;

        #region events

        protected void OnSelectedChannelsChanged()
        {
            LoggingService.Debug("OnSelectedChannelsChanged");
            var handler = SelectionChanged;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }
        }

        protected void OnChannelSelected(ChannelInfo channel)
        {
            LoggingService.Debug("OnChannelSelected:" + channel.name);
            var handler = ChannelSelected;
            if (handler != null)
            {
                handler(this, new ChannelInfoEventArgs {Channel = channel});
            }
        }

        protected void OnChannelUnselected(ChannelInfo channel)
        {
            LoggingService.Debug("OnChannelUnselected:" + channel.name);
            var handler = ChannelUnselected;
            if (handler != null)
            {
                handler(this, new ChannelInfoEventArgs {Channel = channel});
            }
        }

        public class ChannelInfoEventArgs : EventArgs
        {
            public ChannelInfo Channel { get; set; }
        }

        #endregion

        #region treeview events

        private void treeView_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (SelectedChannelTag.Equals(e.Node.Tag))
            {
                SelectedChannelIds.Remove(e.Node.Name);
                UpdateTreeNow();
                OnChannelUnselected(Channels.First(c => c._id.Equals(e.Node.Name)));
                OnSelectedChannelsChanged();
            }
            else if (ChannelTag.Equals(e.Node.Tag))
            {
                SelectedChannelIds.Add(e.Node.Name);
                UpdateTreeNow();
                OnChannelSelected(Channels.First(c => c._id.Equals(e.Node.Name)));
                OnSelectedChannelsChanged();
            }
            else
            {
                if (e.Node.IsExpanded)
                {
                    e.Node.Collapse();
                }
                else
                {
                    e.Node.Expand();
                }
            }
            e.Cancel = true;
        }

        #endregion
    }
}