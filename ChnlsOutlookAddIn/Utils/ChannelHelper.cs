#region

using System.Collections.Generic;
using System.Linq;
using chnls.Model;

#endregion

namespace chnls.Utils
{
    internal class ChannelHelper
    {
        internal static string GetNameWithGroup(ChannelInfo channel)
        {
            return (string.IsNullOrWhiteSpace(channel.groupName) ? "" : (channel.groupName + "/")) + channel.name;
        }

        internal static int GetInterestMemberCount(ChannelInfo channel, EntityCollection collection)
        {
            if (null == channel)
            {
                return 0;
            }
            var members = new HashSet<string>();
            foreach (var address in channel.subscribers.Select(e => e.address))
            {
                members.Add(address.ToLowerInvariant());
            }
            foreach (var address in channel.watchers.Select(e => e.address))
            {
                members.Add(address.ToLowerInvariant());
            }
            return members.Count();
        }
    }
}