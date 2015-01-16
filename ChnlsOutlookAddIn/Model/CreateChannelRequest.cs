#region

using System.Collections.Generic;

#endregion

namespace chnls.Model
{
    // ReSharper disable InconsistentNaming
    public class CreateChannelRequest
    {
        public string channelName;
        public int reqId;
        public List<string> subscribers;
    }

    // ReSharper restore InconsistentNaming
}