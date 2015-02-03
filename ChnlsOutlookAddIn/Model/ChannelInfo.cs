#region

using System;
using System.Collections.Generic;

#endregion

namespace chnls.Model
{
    // ReSharper disable InconsistentNaming
    [Serializable]
    internal enum EmbedChannelActivityState

    {
        None,
        RECENT,
        ACTIVE,
        STALE
    }

    [Serializable]
    internal class ChannelInfo
    {
        public string _id;
        public EmbedChannelActivityState activityState;
        public EmailAddress channelEmailAddress;
        public string descr;
        public string groupId;
        public string groupName;
        public string name;
        public List<EmailAddress> subscribers;
        public List<EmailAddress> watchers;
    }
}

// ReSharper restore InconsistentNaming