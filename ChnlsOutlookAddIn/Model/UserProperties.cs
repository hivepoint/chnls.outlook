#region

using System.Collections.Generic;

#endregion

namespace chnls.Model
{
    internal class UserProperties
    {
        public UserProperties()
        {
            RecentComposeChannels = new List<string>();
            RecentForwardChannels = new List<string>();
            Channels = new List<ChannelInfo>();
            Groups = new List<ChannelGroupInfo>();
        }

        public string EmailAddress { get; set; }

        public List<string> RecentComposeChannels { get; set; }

        public List<string> RecentForwardChannels { get; set; }

        public List<ChannelInfo> Channels { get; set; }

        public List<ChannelGroupInfo> Groups { get; set; }
    }
}