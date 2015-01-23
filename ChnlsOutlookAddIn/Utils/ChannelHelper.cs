#region

using System.Collections.Generic;
using System.Linq;
using chnls.Model;

#endregion

namespace chnls.Utils
{
    internal class ChannelHelper
    {
        internal static string GetNameWithGroup(ChannelInfo channel, List<ChannelGroupInfo> groups)
        {
            var group = groups.FirstOrDefault(e => e._id.Equals(channel._id));
            return channel.name + "@" + (null != group ? group.name : "go");
        }
    }
}