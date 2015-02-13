#region

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
    }
}