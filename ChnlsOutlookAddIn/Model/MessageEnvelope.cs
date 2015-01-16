#region

using System.Collections.Generic;

#endregion

namespace chnls.Model
{
    // ReSharper disable InconsistentNaming
    internal class MessageEnvelope
    {
        public List<EmailAddress> cc;
        public EmailAddress from;
        public List<NameValuePair> headers;
        public List<EmailAddress> to;
    }

    // ReSharper restore InconsistentNaming
}