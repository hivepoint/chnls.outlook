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
        private const string GroupTag = "group";
        private const string MoreTag = "more";
        private readonly List<String> _suggestedChannelIds = new List<string>();
        private HashSet<String> _moredGroups = new HashSet<string>();

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
                var selectedChannels =
                    SelectedChannelIds.Select(
                        channelId => Channels.FirstOrDefault(element => element._id.Equals(channelId)))
                        .Where(e => null != e);
                foreach (var channel in selectedChannels.OrderBy(ChannelHelper.GetNameWithGroup))
                {
                    AddChannel(channel, treeView.Nodes, SelectedChannelTag, true);
                }
                var suggestedChannelIds = SuggestedChannelIds.Where(e => !SelectedChannelIds.Contains(e)).ToList();
                if (suggestedChannelIds.Any())
                {
                    var node = new TreeNode
                    {
                        Text = @"Suggestions",
                        ImageKey = @"stack",
                        ForeColor = SystemColors.GrayText
                    };

                    treeView.Nodes.Add(node);
                    var suggestedChannels = suggestedChannelIds.Select(
                             channelId => Channels.FirstOrDefault(element => element._id.Equals(channelId)))
                             .Where(e => null != e).ToList();
                    foreach (var channel in suggestedChannels.OrderBy(ChannelHelper.GetNameWithGroup))
                    {
                        AddChannel(channel, treeView.Nodes, ChannelTag, true);
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
                var goGroupInfo = new ChannelGroupInfo { _id = "", name = hasGroups ? "Other" : "Channels" };
                var goGroupChannels = Channels.Where(
                    elemenet =>
                        String.IsNullOrWhiteSpace(elemenet.groupId) && !SelectedChannelIds.Contains(elemenet._id) &&
                        !SuggestedChannelIds.Contains(elemenet._id)).ToList();

                AddGroup(treeView.Nodes, goGroupInfo, goGroupChannels);
            }
            finally
            {
                if (treeView.Nodes.Count > 0)
                {
                    treeView.Nodes[0].EnsureVisible();
                }
                treeView.ResumeLayout();
            }
        }

        private bool AddGroup(TreeNodeCollection nodes, ChannelGroupInfo group, List<ChannelInfo> channels)
        {
            var groupNode = new TreeNode
            {
                Text = group.name,
                Name = group._id,
                ImageKey = @"arrow-down",
                ForeColor = SystemColors.GrayText,
                Tag = GroupTag
            };
            nodes.Add(groupNode);
            IEnumerable<ChannelInfo> groupChannels;
            if (_moredGroups.Contains(group._id))
            {
                groupChannels = channels.Where(e => e.activityState != EmbedChannelActivityState.STALE);
            }
            else
            {
                groupChannels = channels.Where(e => e.activityState == EmbedChannelActivityState.RECENT);
            }

            foreach (var channel in groupChannels.OrderBy(e => e.name))
            {
                AddChannel(channel, groupNode.Nodes, ChannelTag, false);
            }

            if (!_moredGroups.Contains(group._id))
            {
                var moreNode = new TreeNode
                {
                    Text = @"more",
                    Name = group._id,
                    ForeColor = SystemColors.GrayText,
                    Tag = MoreTag
                };
                groupNode.Nodes.Add(moreNode);
            }

            groupNode.Expand();
            return true;
        }

        private static void AddChannel(ChannelInfo channel, TreeNodeCollection group, string tag, bool addGroup)
        {
            var text = channel.name;
            if (addGroup)
            {
                text = ChannelHelper.GetNameWithGroup(channel);
            }
            var tooltip = String.IsNullOrWhiteSpace(channel.descr)
                ? ""
                : channel.descr;

            var subscribers = channel.subscribers.Count;
            var watchers = channel.watchers.Count;
            if (subscribers > 0)
            {
                if (!String.IsNullOrWhiteSpace(tooltip))
                {
                    tooltip += "\n";
                }
                tooltip += subscribers + " subscriber" + (subscribers != 1 ? "s" : "");
            }
            if (watchers > 0)
            {
                if (!String.IsNullOrWhiteSpace(tooltip))
                {
                    tooltip += "\n";
                }
                tooltip += watchers + " watching";
            }

            var node = new TreeNode
            {
                Tag = tag,
                Text = text,
                Name = channel._id,
                ImageKey = SelectedChannelTag.Equals(tag) ? @"checkmark2" : null,
                ToolTipText = tooltip
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
                handler(this, new ChannelInfoEventArgs { Channel = channel });
            }
        }

        protected void OnChannelUnselected(ChannelInfo channel)
        {
            LoggingService.Debug("OnChannelUnselected:" + channel.name);
            var handler = ChannelUnselected;
            if (handler != null)
            {
                handler(this, new ChannelInfoEventArgs { Channel = channel });
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
            else if (GroupTag.Equals(e.Node.Tag))
            {
                if (e.Node.IsExpanded && e.Node.Nodes.Count > 0)
                {
                    e.Node.Collapse();
                    e.Node.ImageKey = @"arrow-right";
                }
                else
                {
                    e.Node.Expand();
                    e.Node.ImageKey = @"arrow-down";
                }
            }
            else if (MoreTag.Equals(e.Node.Tag))
            {
                var moreNode = e.Node;
                var groupId = String.IsNullOrWhiteSpace(e.Node.Name) ? "" : e.Node.Name;
                try
                {
                    _moredGroups.Add(groupId);
                    treeView.SuspendLayout();
                    var groupNode = moreNode.Parent;
                    groupNode.Nodes.Clear();
                    var groupChannels = Channels.Where(channel => String.Equals(channel.groupId, groupId) && channel.activityState != EmbedChannelActivityState.STALE);
                    foreach (var channel in groupChannels.OrderBy(channel => channel.name))
                    {
                        AddChannel(channel, groupNode.Nodes, ChannelTag, String.IsNullOrWhiteSpace(groupId));
                    }
                    groupNode.Expand();
                    groupNode.EnsureVisible();
                }
                finally
                {
                    treeView.ResumeLayout();
                }


            }
            e.Cancel = true;
        }

        #endregion
    }
}