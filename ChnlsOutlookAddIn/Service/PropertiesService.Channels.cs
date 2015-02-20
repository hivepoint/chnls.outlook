#region

using System;
using System.Collections.Generic;
using System.Linq;
using chnls.Model;
using chnls.Utils;

#endregion

namespace chnls.Service
{
    internal interface IBrowserObjectDelegate
    {
        List<ChannelInfo> Channels();
        List<ChannelGroupInfo> Groups();

        void NotifyChannelRefresh();

        void GotoChannel(string p);
        void NotifyDialogClosed(ChannelRequestCloseDialog request);
    }

    partial class PropertiesService
    {
        private bool ChannelsDirty { get; set; }
        private bool ChannelGroupsDirty { get; set; }
        public IBrowserObjectDelegate BrowserObjectDelegate { set; private get; }


        public List<ChannelInfo> Channels
        {
            get
            {
                lock (_propertiesLock)
                {
                    if (!SignedIn)
                    {
                        return new List<ChannelInfo>();
                    }
                    if (!ChannelsDirty)
                    {
                        return new List<ChannelInfo>(CurrentUserProperties.Channels);
                    }
                }
                var updateChannels = BrowserObjectDelegate.Channels();
                lock (_propertiesLock)
                {
                    if (null != updateChannels)
                    {
                        UpdateChannels(updateChannels);
                    }
                    return new List<ChannelInfo>(CurrentUserProperties.Channels);
                }
            }
        }

        public List<ChannelGroupInfo> Groups
        {
            get
            {
                lock (_propertiesLock)
                {
                    if (!SignedIn)
                    {
                        return new List<ChannelGroupInfo>();
                    }
                    if (!ChannelGroupsDirty)
                    {
                        return new List<ChannelGroupInfo>(CurrentUserProperties.Groups);
                    }
                }
                var updateGroups = BrowserObjectDelegate.Groups();
                lock (_propertiesLock)
                {
                    if (null != updateGroups)
                    {
                        UpdateGroups(updateGroups);
                    }
                    return new List<ChannelGroupInfo>(CurrentUserProperties.Groups);
                }
            }
        }

        public List<String> RecentComposeChannels
        {
            get
            {
                lock (_propertiesLock)
                {
                    return !SignedIn
                        ? new List<string>()
                        : new List<String>(CurrentUserProperties.RecentComposeChannels);
                }
            }
        }

        public List<String> RecentForwardChannels
        {
            get
            {
                lock (_propertiesLock)
                {
                    return !SignedIn
                        ? new List<string>()
                        : new List<String>(CurrentUserProperties.RecentForwardChannels);
                }
            }
        }

        public string LastForwardFromAddress
        {
            get
            {
                lock (_propertiesLock)
                {
                    return CurrentUserProperties != null ? CurrentUserProperties.LastForwardFromAddress : null;
                }
            }
            set
            {
                lock (_propertiesLock)
                {
                    if (CurrentUserProperties == null ||
                        String.Equals(CurrentUserProperties.LastForwardFromAddress, value))
                    {
                        return;
                    }
                    CurrentUserProperties.LastForwardFromAddress = value;
                    PropertiesDirty();
                }
            }
        }

        public void ChannelListDirty()
        {
            lock (_propertiesLock)
            {
                ChannelsDirty = true;
            }
            OnChannelListChanged();
        }

        public void ChannelGroupListDirty()
        {
            lock (_propertiesLock)
            {
                ChannelGroupsDirty = true;
            }
            OnGroupListChanged();
        }

        // Channel and group properties
        public void AddRecentComposeChannels(IEnumerable<string> channelEmails)
        {
            var list = channelEmails.ToList();
            if (!list.Any())
            {
                return;
            }
            lock (_propertiesLock)
            {
                if (!SignedIn)
                {
                    return;
                }
                var channels = GetChannelInfoForEmails(list);
                CurrentUserProperties.RecentComposeChannels.RemoveAll(
                    e => channels.Any(channel => e.Equals(channel._id)));
                CurrentUserProperties.RecentComposeChannels.InsertRange(0,
                    channels.Select(channel => channel._id).ToList());
            }
            PropertiesDirty();
            OnRecentComposeChannelListChanged();
        }

        public void AddRecentForwardChannels(List<ChannelInfo> channels)
        {
            if (!channels.Any())
            {
                return;
            }
            lock (_propertiesLock)
            {
                if (!SignedIn)
                {
                    return;
                }

                CurrentUserProperties.RecentForwardChannels.RemoveAll(
                    e => channels.Any(channel => e.Equals(channel._id)));
                CurrentUserProperties.RecentForwardChannels.InsertRange(0,
                    channels.Select(channel => channel._id).ToList());
            }
            PropertiesDirty();
        }

        public ChannelInfo GetChannelInfoForEmail(string emailAddress)
        {
            lock (_propertiesLock)
            {
                emailAddress = emailAddress.Trim();
                return null == CurrentUserProperties
                    ? null
                    : CurrentUserProperties.Channels.FirstOrDefault(
                        channel =>
                            channel.channelEmailAddress.address.Equals(emailAddress,
                                StringComparison.InvariantCultureIgnoreCase));
            }
        }

        public List<ChannelInfo> GetChannelInfoForEmails(IEnumerable<string> emailAddressList)
        {
            var channels = new List<ChannelInfo>();
            lock (_propertiesLock)
            {
                if (null == CurrentUserProperties) return channels;
                channels.AddRange(emailAddressList.Select(GetChannelInfoForEmail).Where(channel => null != channel));
            }
            return channels;
        }

        public void UpdateChannels(List<ChannelInfo> channels)
        {
            lock (_propertiesLock)
            {
                if (null == CurrentUserProperties)
                {
                    return;
                }
                CurrentUserProperties.Channels.Clear();
                CurrentUserProperties.Channels.AddRange(channels);
                ChannelsDirty = false;
            }
            PropertiesDirty();
            OnChannelListChanged();
        }

        public void UpdateGroups(List<ChannelGroupInfo> groups)
        {
            lock (_propertiesLock)
            {
                if (null == CurrentUserProperties)
                {
                    return;
                }
                CurrentUserProperties.Groups.Clear();
                CurrentUserProperties.Groups.AddRange(groups);
                ChannelGroupsDirty = false;
            }
            PropertiesDirty();
            OnGroupListChanged();
        }

        public void NotifyDialogClosed(ChannelRequestCloseDialog request)
        {
            BrowserObjectDelegate.NotifyDialogClosed(request);
        }

        internal void NotifyChannelRefresh()
        {
            BrowserObjectDelegate.NotifyChannelRefresh();
        }

        internal void NotifyChannelCreated(ChannelInfo channel, ChannelGroupInfo group)
        {
            BrowserObjectDelegate.NotifyChannelRefresh();
            lock (_propertiesLock)
            {
                if (null == CurrentUserProperties)
                {
                    return;
                }
                if (null != channel)
                {
                    CurrentUserProperties.Channels.RemoveAll(e => e._id.Equals(channel._id));
                    CurrentUserProperties.Channels.Add(channel);
                }
                if (null != group)
                {
                    CurrentUserProperties.Groups.RemoveAll(e => e._id.Equals(group._id));
                    CurrentUserProperties.Groups.Add(group);
                }
            }
            PropertiesDirty();
            if (null != channel)
            {
                OnChannelListChanged();
            }
            if (null != group)
            {
                OnGroupListChanged();
            }
        }

        public event EventHandler ChannelListChanged;
        public event EventHandler GroupListChanged;
        public event EventHandler RecentComposeChannelListChanged;

        #region events

        protected void OnChannelListChanged()
        {
            LoggingService.Debug("OnChannelListChanged");
            var handler = ChannelListChanged;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }
        }

        protected void OnGroupListChanged()
        {
            LoggingService.Debug("OnGroupListChanged");
            var handler = GroupListChanged;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }
        }

        protected void OnRecentComposeChannelListChanged()
        {
            LoggingService.Debug("OnRecentComposeChannelListChanged");
            var handler = RecentComposeChannelListChanged;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }
        }

        #endregion
    }
}