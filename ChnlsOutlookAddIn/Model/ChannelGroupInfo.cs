#region

using System.Collections.Generic;

#endregion

namespace chnls.Model
{
    // ReSharper disable InconsistentNaming

    public enum ChannelGroupState
    {
        PROPOSED,
        PENDING_OWNER,
        PENDING_CONFIRMATION,
        ACTIVE,
        DELETED
    }

    internal class ChannelGroupInfo
    {
        public string _id;
        public List<string> admins;
        public long created;
        public string creator;
        public string descr;
        public long lastUpdated;
        public bool limitChannelCreators;
        public string name;
        public List<string> permittedChannelAddrs;
        public List<string> permittedChannelCreatorAddrs;
        public List<string> permittedChannelCreatorDomains;
        public List<string> permittedChannelDomains;
        public ChannelGroupState state;
    }
}

// ReSharper restore InconsistentNaming