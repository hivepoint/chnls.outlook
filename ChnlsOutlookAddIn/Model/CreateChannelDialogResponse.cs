using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace chnls.Model
{
    // ReSharper disable InconsistentNaming
    class CreateChannelDialogResponse
    {
        internal string resultId;
        internal bool success;
        internal ChannelInfo channel;
        internal ChannelGroupInfo group;
    }
    // ReSharper restore InconsistentNaming
}
