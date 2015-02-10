using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;
using chnls.Model;
using chnls.Service;
using Microsoft.Office.Interop.Outlook;
using Exception = System.Exception;

namespace chnls.Utils
{
    class ReplyHelper
    {
        /*
        * Create a new message that is a reply to a message in Email Channels. 
        * 
        * The message contents to reply to are provided by the parameter
        *     ReplyMessageInfo replyTo
        * 
        */

        internal static void ReplyTo(ReplyMessageInfo replyTo)
        {
            if (null == replyTo) return;

            Application application = null;
            MailItem mail = null;
            try
            {
                application = AddinModule.CurrentInstance.OutlookApp.Application;

                // Create a new mail item and set it's subject to be a reply to the message
                mail = (MailItem)application.CreateItem(OlItemType.olMailItem);
                mail.Subject = "Re: " + replyTo.normalizedSubject;
                // the normalized subject doesn't have any RE or FWD

                // Make sure the orginal sender is in the to field
                // Add recipient using display name, alias, or smtp address
                var tos = new List<EmailAddress>(replyTo.to);
                var ccs = new List<EmailAddress>(replyTo.cc);
                if (null != replyTo.from)
                {
                    if (!ContainsAddress(tos, replyTo.from.address))
                    {
                        tos.Insert(0, replyTo.from);
                    }
                    ccs.RemoveAll(
                        item => item.address.Equals(replyTo.from.address, StringComparison.OrdinalIgnoreCase));
                }
                else if (null != replyTo.sender)
                {
                    if (!ContainsAddress(tos, replyTo.sender.address))
                    {
                        tos.Insert(0, replyTo.sender);
                    }
                    ccs.RemoveAll(
                        item => item.address.Equals(replyTo.sender.address, StringComparison.OrdinalIgnoreCase));
                }

                // Remove the current user from the recipients
                RemoveCurrentUser(tos);
                RemoveCurrentUser(ccs);

                EnsureChannels(tos, ccs, replyTo.channelIds);

                mail.To = GetAddressString(tos);
                mail.CC = GetAddressString(ccs);
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
                        "Failed to resolve recipients: " + GetRecipientString(msgRecipients), ex);
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
                EmailHelper.SetHeader(mail, "In-Reply-To", replyTo.messageIdHeader);

                var html = GetQuotedHtmlBody(replyTo);

                mail.HTMLBody = html.ToString();
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

        private static void EnsureChannels(IEnumerable<EmailAddress> tos, ICollection<EmailAddress> ccs, ICollection<string> channelIds)
        {
            var recipients = new HashSet<String>();
            foreach (var address in tos.Select(e=>e.address))
            {
                recipients.Add(address.ToLowerInvariant());
            }
            foreach (var address in ccs.Select(e => e.address))
            {
                recipients.Add(address.ToLowerInvariant());
            }
            var channels = PropertiesService.Instance.Channels.Where(e => channelIds.Contains(e._id));
            var missingChannels = channels.Where(e => !recipients.Contains(e.channelEmailAddress.address.ToLowerInvariant()));
            foreach (var missingChannel in missingChannels)
            {
                ccs.Add(missingChannel.channelEmailAddress);
            }
        }

        private static StringBuilder GetQuotedHtmlBody(ReplyMessageInfo replyTo)
        {
            var html = new StringBuilder();
            html.Append(OutlookEmailHeaderConst.EMAIL_HEADER);
            html.Append("<body>");

            html.Append("<div><p>&nbsp;</p></div>\n");

            html.Append("<div style='border:none;border-left:solid #e1e1e1 1.0pt;padding:0in 0in 0in 4.0pt'>");
            html.Append("<div>");
            html.Append("<div style='border:none;border-top:solid #e1e1e1 1.0pt;padding:3.0pt 0in 0in 0in'>");
            if (null != replyTo.from)
            {
                var fromString = HttpUtility.HtmlEncode(replyTo.from.MailAddress.ToString());
                html.Append(
                    "<p class=MsoNormal><a name=\"_MailOriginal\"><b><span style='font-size:10.0pt;font-family:\"Tahoma\",\"sans-serif\";mso-fareast-font-family:\"Times New Roman\"'>From:</span></b></a><span style='mso-bookmark:_MailOriginal'><span style='font-size:10.0pt;font-family:\"Tahoma\",\"sans-serif\";mso-fareast-font-family:\"Times New Roman\"'> " +
                    fromString + "<br>");
            }
            else if (null != replyTo.sender)
            {
                var senderString = HttpUtility.HtmlEncode(replyTo.sender.MailAddress.ToString());
                html.Append(
                    "<p class=MsoNormal><a name=\"_MailOriginal\"><b><span style='font-size:10.0pt;font-family:\"Tahoma\",\"sans-serif\";mso-fareast-font-family:\"Times New Roman\"'>Sender:</span></b></a><span style='mso-bookmark:_MailOriginal'><span style='font-size:10.0pt;font-family:\"Tahoma\",\"sans-serif\";mso-fareast-font-family:\"Times New Roman\"'> " +
                    senderString + "<br>");
            }
            html.Append("<b>Sent:</b> " + DateUtil.DateTimeFrom1970(replyTo.sent) + "<br>");
            if (replyTo.to.Count > 0)
            {
                html.Append("<b>To:</b> " + HttpUtility.HtmlEncode(GetAddressString(replyTo.to)) + "<br/>");
            }
            if (replyTo.cc.Count > 0)
            {
                html.Append("<b>Cc:</b> " + HttpUtility.HtmlEncode(GetAddressString(replyTo.cc)) + "<br/>");
            }
            html.Append("<b>Subject:</b> " + HttpUtility.HtmlEncode(replyTo.subject) + "<br/>");
            html.Append("<o:p></o:p></span></span></p></div></div>");
            html.Append("<span style='mso-bookmark:_MailOriginal'>");
            html.Append(replyTo.normalizedHtmlBody);

            html.Append("</span>");
            html.Append("</div>");
            html.Append("</body>");
            html.Append("</html>");
            return html;
        }

        private static string GetRecipientString(Recipients recipients)
        {
            var recipientStr = "";
            if (null == recipients)
            {
                recipientStr = "<Unknown recipients>";
            }
            else
            {
                for (var i = 0; i < recipients.Count; i++)
                {
                    Recipient recipient = null;
                    try
                    {
                        recipient = recipients[i + 1]; // COM 1 BASED
                        if (!String.IsNullOrWhiteSpace(recipientStr))
                        {
                            recipientStr += "; ";
                        }
                        recipientStr += recipient.Address;
                    }
                    finally
                    {
                        if (null != recipient)
                        {
                            Marshal.ReleaseComObject(recipient);
                            // ReSharper disable once RedundantAssignment
                            recipient = null;
                        }
                    }
                }
            }
            return recipientStr;
        }


        private static bool ContainsAddress(IEnumerable<EmailAddress> list, string emailAddress)
        {
            return list.Any(em => String.Equals(emailAddress, em.address, StringComparison.OrdinalIgnoreCase));
        }

        private static string GetAddressString(IEnumerable<EmailAddress> list)
        {
            var sb = new StringBuilder();
            foreach (var emailAddress in list)
            {
                if (sb.Length > 0)
                {
                    sb.Append("; ");
                }
                sb.Append(emailAddress.MailAddress);
            }
            return sb.ToString();
        }

        private static void RemoveCurrentUser(List<EmailAddress> addresses)
        {
            Application application = null;
            NameSpace session = null;
            Accounts accounts = null;
            try
            {
                application = AddinModule.CurrentInstance.OutlookApp.Application;
                session = application.Session;
                accounts = session.Accounts;
                for (var i = 0; i < accounts.Count; i++)
                {
                    Account account = null;
                    try
                    {
                        account = accounts[i + 1]; // COM 1 BASED
                        if (!String.IsNullOrWhiteSpace(account.SmtpAddress))
                        {
                            addresses.RemoveAll(item => item.address.ToLower() == account.SmtpAddress.ToLower());
                        }
                    }
                    finally
                    {
                        if (null != account)
                        {
                            Marshal.ReleaseComObject(account);
                            // ReSharper disable once RedundantAssignment
                            account = null;
                        }
                    }
                }
            }
            finally
            {
                if (null != accounts)
                {
                    Marshal.ReleaseComObject(accounts);
                    // ReSharper disable once RedundantAssignment
                    accounts = null;
                }
                if (null != session)
                {
                    Marshal.ReleaseComObject(session);
                    // ReSharper disable once RedundantAssignment
                    session = null;
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
