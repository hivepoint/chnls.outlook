namespace chnls.Model
{
    // ReSharper disable InconsistentNaming

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

    internal enum EntityCollection
    {
        SUBSCRIPTIONS,
        FOLLOWING
    }

    internal enum EntityInterestLevel
    {
        MUST_INCLUDE,
        INTERESTED,
        UNSPECIFIED,
        MUST_EXCLUDE
    }

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