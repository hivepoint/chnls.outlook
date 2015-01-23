#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Runtime.InteropServices;
using chnls.Model;
using Microsoft.Office.Interop.Outlook;

#endregion

namespace chnls.Utils
{
    internal class EmailHelper
    {
        public static ChannelInfo GetChannel(string emailAddress, List<ChannelInfo> channels)
        {
            if (String.IsNullOrWhiteSpace(emailAddress)) return null;

            return
                channels.FirstOrDefault(
                    e => e.channelEmailAddress.address.Equals(emailAddress, StringComparison.InvariantCultureIgnoreCase));
        }

        public static bool IsValidEmail(string text)
        {
            if (String.IsNullOrWhiteSpace(text))
            {
                return false;
            }
            try
            {
                var mailAddress = new MailAddress(text);
                return !String.IsNullOrWhiteSpace(mailAddress.Address);
            }
                // ReSharper disable once EmptyGeneralCatchClause
            catch
            {
            }
            return false;
        }

        public static string GetSmtpAddress(Recipient recipient)
        {
            const string prSmtpAddress = "http://schemas.microsoft.com/mapi/proptag/0x39FE001E";
            var pa = recipient.PropertyAccessor;
            try
            {
                var smtpAddress = pa.GetProperty(prSmtpAddress).ToString();
                return IsValidEmail(smtpAddress) ? smtpAddress : null;
            }
            finally
            {
                Marshal.ReleaseComObject(pa);
            }
        }

        public static EmailAddress GetEmailAddress(Recipient recipient)
        {
            var smtpAddress = GetSmtpAddress(recipient);
            return String.IsNullOrWhiteSpace(smtpAddress)
                ? null
                : new EmailAddress {address = smtpAddress, name = recipient.Name};
        }

        internal static void SetHeader(MailItem item, string headerName, string headerValue)
        {
            if (!String.IsNullOrWhiteSpace(headerValue))
            {
                PropertyAccessor propAccessor = null;
                try
                {
                    propAccessor = item.PropertyAccessor;
                    propAccessor.SetProperty(
                        "http://schemas.microsoft.com/mapi/string/{00020386-0000-0000-C000-000000000046}/" + headerName,
                        headerValue);
                }
                finally
                {
                    if (null != propAccessor)
                    {
                        Marshal.ReleaseComObject(propAccessor);
                        // ReSharper disable once RedundantAssignment
                        propAccessor = null;
                    }
                }
            }
        }
    }
}