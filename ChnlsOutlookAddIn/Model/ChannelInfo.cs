#region

using System;
using System.Collections.Generic;

#endregion

namespace chnls.Model
{
    // ReSharper disable InconsistentNaming

    internal enum ChannelState
    {
        PROPOSED,
        PENDING_OWNER,
        PENDING_CONFIRMATION,
        ACTIVE,
        DELETED
    }
    
    [Serializable]
    internal class ChannelInfo
    {
        public string _id;
        public List<string> admins;
        public List<UserEntityAssociation> associations;
        public EmailAddress channelEmailAddress;
        public string channelUrl;
        public long created;
        public string creator;
        public string descr;
        public string groupId;
        public string groupName;
        public long lastUpdated;
        public long mostRecentMessage;
        public string name;
        public List<EmailAddress> permittedAddresses;
        public List<string> permittedDomains;
        public bool privateToGroup;
        public ChannelState state;
    }
}

// ReSharper restore InconsistentNaming