#region

using System;
using System.Net.Mail;

#endregion

namespace chnls.Model
{
    // ReSharper disable InconsistentNaming
    [Serializable]
    internal class EmailAddress
    {
        public string address;
        public string name;

        public MailAddress MailAddress
        {
            get { return new MailAddress(address, name); }
        }
    }
}

// ReSharper restore InconsistentNaming