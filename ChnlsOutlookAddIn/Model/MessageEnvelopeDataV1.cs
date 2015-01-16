#region

using System.Collections.Generic;

#endregion

namespace chnls.Model
{
    internal class MessageEnvelopeDataV1
    {
        // ReSharper disable InconsistentNaming
        public List<EmailAddress> cc;
        public EmailAddress from;
        public List<NameValuePair> headers;
        public string messageIdHeader;
        public long sent;
        public string subject;
        public List<EmailAddress> to;
        // ReSharper restore InconsistentNaming
    }
}