#region

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Web;
using chnls.Model;
using chnls.Service;
using Microsoft.Office.Interop.Outlook;
using Exception = System.Exception;

#endregion

namespace chnls.Utils
{
    internal class ComposeHelper
    {
        public static string RecipientAddress(Recipient recpient)
        {
            var address = recpient.Address;
            if (String.IsNullOrWhiteSpace(address))
            {
                return recpient.Name;
            }
            return address;
        }

        internal static ChannelInfo GetChannel(Recipient recpient, List<ChannelInfo> channels)
        {
            return EmailHelper.GetChannel(RecipientAddress(recpient), channels);
        }

        public static string GetChannelAddress(Recipient recpient, List<ChannelInfo> channels)
        {
            return GetChannelAddress(RecipientAddress(recpient), channels);
        }

        public static string GetChannelAddress(string address, List<ChannelInfo> channels)
        {
            var channel = EmailHelper.GetChannel(address, channels);
            return null != channel ? channel.channelEmailAddress.address : null;
        }

        public static bool IsChannelAddress(Recipient recpient, List<ChannelInfo> channels)
        {
            var address = recpient.Address;
            if (String.IsNullOrWhiteSpace(address))
            {
                address = recpient.Name;
            }
            return IsChannelAddress(address, channels);
        }

        public static bool IsChannelAddress(string address, List<ChannelInfo> channels)
        {
            return EmailHelper.GetChannel(address, channels) != null;
        }

        public static bool ContainsAddress(Recipients recipients, string address)
        {
            for (var i = 1; i <= recipients.Count; i++) // COM 1 BASED
            {
                var rec = recipients[i]; // COM 1 BASED
                try
                {
                    if (String.Equals(rec.Address, address, StringComparison.CurrentCultureIgnoreCase))
                    {
                        return true;
                    }
                    if (String.Equals(rec.Name, address, StringComparison.CurrentCultureIgnoreCase))
                    {
                        return true;
                    }
                }
                finally
                {
                    if (null != rec)
                    {
                        Marshal.ReleaseComObject(rec);
                    }
                }
            }
            return false;
        }

        public static void PokeMailItemRecipients(MailItem mailItem)
        {
            if (null == mailItem)
            {
                return;
            }
            const string bogusAddress = "hp_tmp";
            var recipients = mailItem.Recipients;
            try
            {
                Recipient rec = null;
                try
                {
                    rec = recipients.Add(bogusAddress);
                    rec.Type = (int) OlMailRecipientType.olBCC;
                    rec.Resolve();
                    for (var i = recipients.Count; i >= 1; i--) // COM 1 BASED
                    {
                        var deleteCheck = recipients[i];
                        try
                        {
                            if (String.Equals(bogusAddress, deleteCheck.Address) ||
                                String.Equals(bogusAddress, deleteCheck.Name))
                            {
                                recipients.Remove(i);
                                return;
                            }
                        }
                        finally
                        {
                            Marshal.ReleaseComObject(deleteCheck);
                        }
                    }
                }
                finally
                {
                    if (null != rec)
                    {
                        Marshal.ReleaseComObject(rec);
                    }
                }
            }
            finally
            {
                Marshal.ReleaseComObject(recipients);
            }
        }

        public static void AddEmailChannel(MailItem mailItem, ChannelInfo channel)
        {
            var channels = new List<ChannelInfo> {channel};
            AddEmailChannels(mailItem, channels);
        }

        public static void AddEmailChannels(MailItem mailItem, ICollection<ChannelInfo> channels)
        {
            Recipients recipients = null;
            try
            {
                recipients = mailItem.Recipients;
                AddRecipients(channels, OlMailRecipientType.olTo, recipients);
            }
            finally
            {
                if (null != recipients)
                {
                    Marshal.ReleaseComObject(recipients);
                }
            }
        }

        private static bool ContainsChannel(Recipients recipients, ChannelInfo channel)
        {
            return ContainsAddress(recipients, channel.channelEmailAddress.address);
        }

        public static void RemoveEmailChannel(MailItem mailItem, ChannelInfo channel)
        {
            Recipients recipients = null;
            try
            {
                recipients = mailItem.Recipients;
                var todelete = new List<int>();
                recipients = mailItem.Recipients;
                for (var i = 1; i <= recipients.Count; i++)
                {
                    Recipient rec = null;
                    try
                    {
                        rec = recipients[i];
                        var recipientAddress = EmailHelper.GetSmtpAddress(rec);
                        if (channel.channelEmailAddress.address.Equals(recipientAddress,
                            StringComparison.InvariantCultureIgnoreCase))
                        {
                            todelete.Insert(0, i);
                        }
                    }
                    finally
                    {
                        if (null != rec)
                        {
                            Marshal.ReleaseComObject(rec);
                        }
                    }
                }
                foreach (var index in todelete)
                {
                    recipients.Remove(index);
                }
            }
            finally
            {
                if (null != recipients)
                {
                    Marshal.ReleaseComObject(recipients);
                }
            }
        }

        private static void AddRecipients(IEnumerable<ChannelInfo> channels, OlMailRecipientType type,
            Recipients recipients)
        {
            foreach (var channel in channels)
            {
                if (ContainsChannel(recipients, channel)) continue;

                Recipient recipient = null;
                try
                {
                    recipient = recipients.Add(channel.channelEmailAddress.address);
                    recipient.Type = (int) type;
                    recipient.Resolve();
                }
                    // ReSharper disable once EmptyGeneralCatchClause
                catch
                {
                }
                finally
                {
                    if (null != recipient)
                    {
                        Marshal.ReleaseComObject(recipient);
                    }
                }
            }
        }

        internal static void MailTo(Uri mailtoUri)
        {
            if (null == mailtoUri ||
                !Uri.UriSchemeMailto.Equals(mailtoUri.Scheme, StringComparison.InvariantCultureIgnoreCase)) return;

            var orig = mailtoUri.OriginalString.Substring(Uri.UriSchemeMailto.Length + 1);
            if (orig.Contains("?"))
            {
                orig = orig.Substring(orig.IndexOf("?", StringComparison.Ordinal));
            }
            if (orig.Contains("#"))
            {
                orig = orig.Substring(orig.IndexOf("#", StringComparison.Ordinal));
            }

            var to = HttpUtility.UrlDecode(orig);
            var query = HttpUtility.ParseQueryString(mailtoUri.Query);
            var subject = query["subject"] ?? query["Subject"];
            var body = query["body"] ?? query["Body"];
            var cc = query["CC"] ?? query["cc"];
            var bcc = query["BCC"] ?? query["bcc"];

            MailTo(to, cc, bcc, subject, body);
        }

        internal static void MailTo(string to = null, string cc = null, string bcc = null, string subject = null,
            string body = null)
        {
            Application application = null;
            MailItem mail = null;
            try
            {
                application = AddinModule.CurrentInstance.OutlookApp.Application;

                // Create a new mail item and set it's subject to be a reply to the message
                mail = (MailItem) application.CreateItem(OlItemType.olMailItem);
                if (!String.IsNullOrWhiteSpace(subject))
                {
                    mail.Subject = subject;
                }

                if (!String.IsNullOrWhiteSpace(to))
                {
                    mail.To = to;
                }

                if (!String.IsNullOrWhiteSpace(cc))
                {
                    mail.CC = cc;
                }

                if (!String.IsNullOrWhiteSpace(bcc))
                {
                    mail.BCC = bcc;
                }

                Recipients msgRecipients = null;
                try
                {
                    // resolve all the recipients so that we can send the message
                    msgRecipients = mail.Recipients;
                    msgRecipients.ResolveAll();
                }
                catch (Exception ex)
                {
                    // the recipients might not be correct, but the user will see this and there will be outlook validation before they send the message.  There really isn't anything else we can do.
                    LoggingService.Error(
                        "Failed to resolve recipients: to:'" + to + "' cc:'" + cc + "' bcc:'" + bcc +
                        "'", ex);
                }
                finally
                {
                    if (null != msgRecipients)
                    {
                        Marshal.ReleaseComObject(msgRecipients);
                        // ReSharper disable once RedundantAssignment
                        msgRecipients = null;
                    }
                }
                if (!String.IsNullOrWhiteSpace(body))
                {
                    mail.Body = body;
                }
                mail.Display();
            }
            finally
            {
                if (null != mail)
                {
                    Marshal.ReleaseComObject(mail);
                    // ReSharper disable once RedundantAssignment
                    mail = null;
                }
                if (null != application)
                {
                    Marshal.ReleaseComObject(application);
                    // ReSharper disable once RedundantAssignment
                    application = null;
                }
            }
        }
    }
}