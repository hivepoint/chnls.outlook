#region

using System.Collections.Generic;

#endregion

namespace chnls.Model
{
    internal class SendMessageEnvelopeRequest
    {
        // ReSharper disable InconsistentNaming
        public List<MessageEnvelopeDataV1> envelopes;
        public int reqId;
        // ReSharper restore InconsistentNaming
    }
}