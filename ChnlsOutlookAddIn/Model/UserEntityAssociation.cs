#region

using System;

#endregion

namespace chnls.Model
{
    // ReSharper disable InconsistentNaming

    [Serializable]
    internal enum EntityType
    {
        CHANNEL_GROUP,
        CHANNEL,
        EMAIL_ADDRESS,
        TAG,
        FILE,
        MESSAGE,
        COLLECTION
    }

    [Serializable]
    internal enum EntityCollection
    {
        SUBSCRIPTIONS,
        FOLLOWING
    }

    [Serializable]
    internal enum EntityInterestLevel
    {
        MUST_INCLUDE,
        INTERESTED,
        UNSPECIFIED,
        MUST_EXCLUDE
    }

    [Serializable]
    internal class UserEntityAssociation
    {
        public string _id;
        public EntityCollection collection;
        public string entityName;
        public EntityType entityType;
        public EntityInterestLevel interestLevel;
        public string userEmail;
    }

// ReSharper restore InconsistentNaming
}