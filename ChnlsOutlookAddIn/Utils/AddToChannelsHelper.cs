#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using chnls.Forms;
using chnls.Service;
using Microsoft.Office.Interop.Outlook;
using Exception = System.Exception;

#endregion

namespace chnls.Utils
{
    internal class AddToChannelsHelper
    {
        internal static void AddToChannels(List<MailItem> items)
        {
            if (!PropertiesService.Instance.Channels.Any())
            {
                return;
            }

            var shareForm = new AddToChannelForm(items);
            shareForm.Closed += (sender, args) =>
            {
                foreach (var mailItem in items)
                {
                    Marshal.ReleaseComObject(mailItem);
                }
                items.Clear();
            };
            shareForm.AddToChannels = (account, channels) =>
            {
                var items2Send = new List<MailItem>();
                try
                {
                    LoggingService.Debug("Sending to : " +
                                         String.Join(",", channels.Select(e => e.channelEmailAddress.address).ToArray()));
                    foreach (var item in items)
                    {
                        LoggingService.Debug("\t" + item.SenderEmailAddress + " >> " + item.Subject);
                    }
                    PropertiesService.Instance.LastForwardFromAddress = account.SmtpAddress;
                    MailItem mailItem2Send = null;
                    var size = 0;
                    var bodyAddMessage =
                        "<p>Any messages that were not already in Channels.cc will be added to the following channel";
                    if (channels.Count > 0)
                    {
                        bodyAddMessage += "s";
                    }
                    bodyAddMessage += ":\n" + "<ul>\n";
                    bodyAddMessage = channels.Aggregate(bodyAddMessage,
                        (current, channel) => current + ("    <li>@" + channel.name + "</li>\n"));
                    PropertiesService.Instance.AddRecentForwardChannels(channels);
                    bodyAddMessage += "</ul>\n" + "</p>";

                    foreach (var item in items)
                    {
                        if (mailItem2Send == null)
                        {
                            mailItem2Send =
                                AddinModule.CurrentInstance.OutlookApp.Application.CreateItem(OlItemType.olMailItem) as
                                    MailItem;
                            Debug.Assert(mailItem2Send != null, "mailItem != null");
                            mailItem2Send.SendUsingAccount = account;
                            ComposeHelper.AddEmailChannels(mailItem2Send, channels);
                            size = 0;
                        }
                        Attachments attachments = null;
                        try
                        {
                            attachments = mailItem2Send.Attachments;
                            var attachment = attachments.Add(item);
                            if (null != attachment)
                            {
                                Marshal.ReleaseComObject(attachment);
                            }
                        }
                        finally
                        {
                            if (null != attachments)
                            {
                                Marshal.ReleaseComObject(attachments);
                                // ReSharper disable once RedundantAssignment
                                attachments = null;
                            }
                        }
                        size += item.Size;
                        if (size > 10000000)
                        {
                            UpdateToSend(mailItem2Send, bodyAddMessage);
                            items2Send.Add(mailItem2Send);
                            mailItem2Send = null;
                        }
                    }
                    if (mailItem2Send != null)
                    {
                        UpdateToSend(mailItem2Send, bodyAddMessage);
                        items2Send.Add(mailItem2Send);
                    }
                    LoggingService.Debug("Sending " + items2Send.Count + " messages");
                    var failedCount = 0;
                    var sendCount = 0;
                    foreach (var item2Send in items2Send)
                    {
                        sendCount++;
                        var recipients = "";
                        Recipients recipientsList = null;
                        try
                        {
                            recipientsList = item2Send.Recipients;
                            for (var j = 1; j <= recipientsList.Count; j++) // COM 1 BASED
                            {
                                Recipient rec = null;
                                try
                                {
                                    rec = recipientsList[j];
                                    recipients += rec.Address + "; ";
                                }
                                finally
                                {
                                    if (null != rec)
                                    {
                                        Marshal.ReleaseComObject(rec);
                                        // ReSharper disable once RedundantAssignment
                                        rec = null;
                                    }
                                }
                            }
                        }
                        finally
                        {
                            if (null != recipientsList)
                            {
                                Marshal.ReleaseComObject(recipientsList);
                            }
                        }
                        var description = "Sending " + sendCount + " of " + items2Send.Count + ": " + item2Send.Subject +
                                          " to:" + recipients;
                        LoggingService.Debug(description);

                        try
                        {
                            item2Send.Save();
                            item2Send.Send();
                        }
                        catch (Exception ex)
                        {
                            LoggingService.Error("Failed to send: " + description, ex);
                            failedCount++;
                        }
                    }
                    if (failedCount > 0)
                    {
                        MessageBox.Show(@"Some messages have not been sent, they will be in your drafts or outbox");
                    }
                }
                finally
                {
                    foreach (var item in items2Send)
                    {
                        Marshal.ReleaseComObject(item);
                    }
                    items2Send.Clear();
                }
            };
            shareForm.StartPosition = FormStartPosition.CenterParent;

            shareForm.ShowDialog();
        }

        private static void UpdateToSend(MailItem mailItem, string bodyAddMessage)
        {
            var attachments = mailItem.Attachments;
            var count = attachments.Count;
            Marshal.ReleaseComObject(attachments);

            EmailHelper.SetHeader(mailItem, "X-Channels-Populate", "" + count);

            mailItem.Subject = "Forwarded " + count + " message" + (count == 1 ? "" : "s") + " to the hive";
            mailItem.HTMLBody = "<p>" + count + " message" + (count == 1 ? " has" : "s have") +
                                " been forwarded to the hive.</p>\n" + bodyAddMessage;
            mailItem.Save();
        }
    }
}