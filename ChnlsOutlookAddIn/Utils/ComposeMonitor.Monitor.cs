#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using chnls.Model;
using chnls.Service;
using Microsoft.Office.Interop.Outlook;
using Exception = System.Exception;

#endregion

namespace chnls.Utils
{
    partial class ComposeMonitor
    {
        private Dictionary<string, ChannelInfo> _currentChannels = new Dictionary<string, ChannelInfo>();

        private MailItem _mailItem;
        private SortedSet<string> _participants = new SortedSet<string>();

        public SortedSet<string> Participants
        {
            get { return new SortedSet<string>(_participants); }
        }

        private MailItem CurrentMailItem
        {
            get { return _mailItem; }
            set
            {
                if (value == _mailItem) return;

                if (_mailItem != null)
                {
                    _currentChannels.Clear();
                    _participants.Clear();
                    _timer.Enabled = false;

                    _mailItem.PropertyChange -= mailItem_PropertyChange;
                    _mailItem.CustomPropertyChange -= mailItem_CustomPropertyChange;
                    _mailItem.BeforeCheckNames -= mailItem_BeforeCheckNames;
                    ((ItemEvents_10_Event) _mailItem).Send -= _sendEventHandler;
                    _mailItem = null;
                }
                _mailItem = value;
                if (_mailItem != null)
                {
                    _currentChannels.Clear();
                    _participants.Clear();
                    object parent = null;
                    Recipients recipients = null;
                    try
                    {
                        parent = _mailItem.Parent;
                        recipients = _mailItem.Recipients;
                    }
                    finally
                    {
                        if (null != recipients)
                        {
                            Marshal.ReleaseComObject(recipients);
                            recipients = null;
                        }
                        if (null != parent)
                        {
                            Marshal.ReleaseComObject(parent);
                            parent = null;
                        }
                    }
                    _mailItem.PropertyChange += mailItem_PropertyChange;
                    _mailItem.CustomPropertyChange += mailItem_CustomPropertyChange;
                    _mailItem.BeforeCheckNames += mailItem_BeforeCheckNames;
                    ((ItemEvents_10_Event) _mailItem).Send += _sendEventHandler;
                    _timer.Enabled = true;
                    RefreshParticipants(true);
                }
            }
        }


        /*
         * Refresh the toggle button state
         */

        public void RefreshParticipants(bool fireUpdates)
        {
            try
            {
                if (!_timer.Enabled) return;

                Recipients recipients = null;
                try
                {
                    recipients = CurrentMailItem.Recipients;


                    var channels = new Dictionary<string, ChannelInfo>();
                    var participants = new SortedSet<string>();

                    for (var i = 0; i < recipients.Count; i++)
                    {
                        Recipient rec = null;
                        try
                        {
                            rec = recipients[i + 1]; // COM 1 BASED

                            var channel = ComposeHelper.GetChannel(rec, Channels);
                            if (null != channel)
                            {
                                channels[channel._id] = channel;
                            }
                            else
                            {
                                var recipientAddress = ComposeHelper.RecipientAddress(rec);
                                if (!String.IsNullOrWhiteSpace(recipientAddress))
                                {
                                    participants.Add(recipientAddress.ToLower());
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
                    var channelsChanged = _currentChannels.Keys.Except(channels.Keys).Any() || channels.Keys.Except(_currentChannels.Keys).Any();
                    var participantsChanged = !_participants.SequenceEqual(participants);
                    if (channelsChanged)
                    {
                        _currentChannels = channels;
                    }
                    if (participantsChanged)
                    {
                        _participants = participants;
                    }
                    if (fireUpdates)
                    {
                        Update(channelsChanged, participantsChanged);
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
            catch (Exception ex)
            {
                _timer.Enabled = false;
                LoggingService.Error("Error updating message", ex);
            }
        }


        private void mailItem_BeforeCheckNames(ref bool cancel)
        {
            RefreshParticipants(true);
        }

        private void mailItem_CustomPropertyChange(string name)
        {
            RefreshParticipants(true);
        }

        private void mailItem_PropertyChange(string name)
        {
            RefreshParticipants(true);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            RefreshParticipants(true);
        }
    }
}