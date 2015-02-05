using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace chnls.Model
{
    // ReSharper disable InconsistentNaming
    class CreateChannelDialogResponse
    {
        public string resultId;
        public bool success;
        public ChannelInfo channel;
        public ChannelGroupInfo group;
    }
    // ReSharper restore InconsistentNaming
}
