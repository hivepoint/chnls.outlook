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
        public EmailAddress from;
        public string messageIdHeader;
        public string normalizedHtmlBody;
        public string normalizedSubject;
        public string replyPrefix;
        public EmailAddress sender;
        public long sent;
        public string subject;
        public string threadId;
        public List<EmailAddress> to = new List<EmailAddress>();
    }

    // ReSharper restore InconsistentNaming
}