#region

using System.Collections.Generic;

#endregion

namespace chnls.Model
{
    // ReSharper disable InconsistentNaming
    internal class ReplyMessageInfo
    {
        public string _id;
        public List<EmailAddress> bcc = new List<EmailAddress>();
        public List<EmailAddress> cc = new List<EmailAddress>();
        public List<string> channelIds = new List<string>();
        public EmailAddress from;

        public string inReplyToHeader;
        public string messageIdHeader;
        public string normalizedHtmlBody;

        public string normalizedSubject;

        public List<string> referenceMessageIds = new List<string>();
        public EmailAddress sender;

        public long sent;

        public string subject;
        public List<EmailAddress> to = new List<EmailAddress>();
    }

    // ReSharper restore InconsistentNaming
}