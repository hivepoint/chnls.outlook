#region

using System.Collections.Generic;
using System.Linq;
using AddinExpress.MSO;
using chnls.Model;

#endregion

namespace chnls.Utils
{
    internal class ChannelHelper
    {
        internal static string GetNameWithGroup(ChannelInfo channel, List<ChannelGroupInfo> groups)
        {
            var group = groups.FirstOrDefault(e => e._id.Equals(channel._id));
            return (null != group ? (group.name + "/") : "") + channel.name;
        }

        internal static int GetInterestMemberCount(ChannelInfo channel, EntityCollection collection)
        {
            if (null == channel || null==channel.associations)
            {
                return 0;
            }
            return channel.associations.Count(e => e.collection == collection&& e.interestLevel==EntityInterestLevel.INTERESTED);
        }
    }
}