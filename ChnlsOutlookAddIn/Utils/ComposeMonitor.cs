#region

using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using chnls.Model;
using chnls.Service;
using Microsoft.Office.Interop.Outlook;

#endregion

namespace chnls.Utils
{
    partial class ComposeMonitor
    {
        private readonly IComposeMonitorCallback _callback;
        private readonly ItemEvents_10_SendEventHandler _sendEventHandler;

        private readonly Timer _timer = new Timer();
        private List<string> _suggestedChannels = new List<string>();


        public ComposeMonitor(MailItem mailItem, IComposeMonitorCallback callback)
        {
            _sendEventHandler = OnMailSend;
            _callback = callback;
            _timer.Tick += timer_Tick;
            _timer.Interval = 2000;
            CurrentMailItem = mailItem;
            Channels = PropertiesService.Instance.Channels;
            RefreshParticipants(true);
            UpdateSuggestions();
        }

        private List<ChannelInfo> Channels { get; set; }

        public void Stop()
        {
            _timer.Enabled = false;
            CurrentMailItem = null;
        }

        public void AddChannel(ChannelInfo channel)
        {
            ComposeHelper.PokeMailItemRecipients(CurrentMailItem);
            ComposeHelper.AddEmailChannel(CurrentMailItem, channel);
            RefreshParticipants(true);
        }

        public void RemoveChannel(ChannelInfo channel)
        {
            ComposeHelper.PokeMailItemRecipients(CurrentMailItem);
            ComposeHelper.RemoveEmailChannel(CurrentMailItem, channel);
            RefreshParticipants(true);
        }


        private void OnMailSend(ref bool cancel)
        {
            RefreshParticipants(false);
            PropertiesService.Instance.AddRecentComposeChannels(_currentChannels.Keys);
        }


        private void Update(bool channelsChanged, bool participantsChanged)
        {
            if (channelsChanged)
            {
                _callback.OnChannelsChanged(Channels.ToList());
            }
            if (channelsChanged || participantsChanged)
            {
                UpdateSuggestions();
            }
        }

        private void UpdateSuggestions()
        {
            var holders = new List<SuggestionHolder>();

            foreach (var channel in Channels)
            {
                if (IsStale(channel)) continue;

                var subscribers = new HashSet<string>(Channels.Select(e=>e.channelEmailAddress.address));
                var intersect = _participants.Intersect(subscribers);
                var count = intersect.Count();
                if (count > 0)
                {
                    holders.Add(new SuggestionHolder {ChannelInfo = channel, Score = count});
                }
            }
            holders.Sort((s0, s1) => s0.Score.CompareTo(s1.Score));

            var suggestions = new List<string>();
            const int maxSuggestions = 8;
            for (var i = 0; i < holders.Count && i < maxSuggestions; i++)
            {
                var channel = holders[i].ChannelInfo;
                if (!_currentChannels.ContainsKey(channel._id) && !suggestions.Contains(channel._id))
                {
                    suggestions.Add(channel._id);
                }
            }
            foreach (var recentChannel in PropertiesService.Instance.RecentComposeChannels)
            {
                if (suggestions.Count >= maxSuggestions)
                {
                    break;
                }
                if (!_currentChannels.ContainsKey(recentChannel) && !suggestions.Contains(recentChannel))
                {
                    suggestions.Add(recentChannel);
                }
            }

            if (!suggestions.SequenceEqual(_suggestedChannels))
            {
                _suggestedChannels = suggestions;
                _callback.OnSuggestionsChanged(suggestions);
            }
        }

        private bool IsStale(ChannelInfo channel)
        {
            return false;
        }

        public interface IComposeMonitorCallback
        {
            void OnChannelsChanged(List<ChannelInfo> channels);
            void OnSuggestionsChanged(List<string> suggestedChannelIds);
        }

        private struct SuggestionHolder
        {
            public ChannelInfo ChannelInfo;
            public int Score;
        }
    }
}