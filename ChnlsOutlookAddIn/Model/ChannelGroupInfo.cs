#region

using System;

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

    [Serializable]
    internal class ChannelGroupInfo
    {
        public string _id;
        public string descr;
        public long lastUpdated;
        public string name;
    }
}

// ReSharper restore InconsistentNaming