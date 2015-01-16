#region

using System;
using System.Collections.Generic;
using System.Linq;
using chnls.Model;

#endregion

namespace chnls.Service
{
    partial class PropertiesService
    {
        public List<ChannelInfo> Channels
        {
            get
            {
                lock (Properties)
                {
                    AssertSignedIn();
                    return new List<ChannelInfo>(CurrentUserProperties.Channels);
                }
            }
        }

        // Channel and group properties
        public void AddRecentComposeChannels(List<string> channelEmails)
        {
            if (channelEmails.Count == 0)
            {
                return;
            }
            lock (Properties)
            {
                if (!SignedIn)
                {
                    return;
                }
                var channels = GetChannelInfoForEmails(channelEmails);
                CurrentUserProperties.RecentComposeChannels.RemoveAll(
                    e => channels.Any(channel => e.Equals(channel._id)));
                CurrentUserProperties.RecentComposeChannels.InsertRange(0,
                    channels.Select(channel => channel._id).ToList());
            }
            PropertiesDirty();
            OnRecentComposeChannelListChanged();
        }

        public void AddRecentForwardChannels(List<string> channelEmails)
        {
            if (channelEmails.Count == 0)
            {
                return;
            }
            lock (Properties)
            {
                if (!SignedIn)
                {
                    return;
                }
                var channels = GetChannelInfoForEmails(channelEmails);
                CurrentUserProperties.RecentComposeChannels.RemoveAll(
                    e => channels.Any(channel => e.Equals(channel._id)));
                CurrentUserProperties.RecentComposeChannels.InsertRange(0,
                    channels.Select(channel => channel._id).ToList());
            }
            PropertiesDirty();
        }

        public ChannelInfo GetChannelInfoForEmail(string emailAddress)
        {
            lock (Properties)
            {
                emailAddress = emailAddress.Trim();
                return null == CurrentUserProperties
                    ? null
                    : CurrentUserProperties.Channels.FirstOrDefault(
                        channel =>
                            channel.channelEmailAddress.Equals(emailAddress, StringComparison.InvariantCultureIgnoreCase));
            }
        }

        public List<ChannelInfo> GetChannelInfoForEmails(List<string> emailAddressList)
        {
            var channels = new List<ChannelInfo>();
            lock (Properties)
            {
                if (null == CurrentUserProperties) return channels;
                channels.AddRange(emailAddressList.Select(GetChannelInfoForEmail).Where(channel => null != channel));
            }
            return channels;
        }

        public void UpdateChannels(List<ChannelInfo> channels)
        {
            lock (Properties)
            {
                if (null == CurrentUserProperties)
                {
                    return;
                }
                CurrentUserProperties.Channels.Clear();
                CurrentUserProperties.Channels.AddRange(channels);
            }
            PropertiesDirty();
            OnChannelListChanged();
        }

        public void UpdateGroups(List<ChannelGroupInfo> groups)
        {
            lock (Properties)
            {
                if (null == CurrentUserProperties)
                {
                    return;
                }
                CurrentUserProperties.Groups.Clear();
                CurrentUserProperties.Groups.AddRange(groups);
            }
            PropertiesDirty();
            OnGroupListChanged();
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