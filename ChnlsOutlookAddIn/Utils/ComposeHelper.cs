#region

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using chnls.Model;
using Microsoft.Office.Interop.Outlook;

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
                        rec = null;
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
                    for (var i = 1; i <= recipients.Count; i++)
                    {
                        var deleteCheck = recipients[i]; // COM 1 BASED
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
                            deleteCheck = null;
                        }
                    }
                }
                finally
                {
                    if (null != rec)
                    {
                        Marshal.ReleaseComObject(rec);
                        rec = null;
                    }
                }
            }
            finally
            {
                Marshal.ReleaseComObject(recipients);
                recipients = null;
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
                    recipients = null;
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
                            rec = null;
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
                    recipients = null;
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
                        recipient = null;
                    }
                }
            }
        }
    }
}