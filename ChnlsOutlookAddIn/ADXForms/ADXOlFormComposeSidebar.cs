#region

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using AddinExpress.OL;
using chnls.Controls;
using chnls.Model;
using chnls.Service;
using chnls.Utils;
using Microsoft.Office.Interop.Outlook;

#endregion

namespace chnls.ADXForms
{
    /*
     * This is the sidebar that appears in the compose email inpsector window
     */
    // ReSharper disable once InconsistentNaming
    public partial class ADXOlFormComposeSidebar : ADXOlForm, ComposeMonitor.IComposeMonitorCallback
    {
        private bool _loadingConent;
        private MailItem _mailItem;
        private ComposeMonitor _monitor;

        public ADXOlFormComposeSidebar()
        {
            InitializeComponent();
            Width = 150;
            PropertiesService.Instance.UserChanged += Instance_UserChanged;
            PropertiesService.Instance.ChannelListChanged += Instance_ChannelListChanged;
            PropertiesService.Instance.GroupListChanged += Instance_GroupListChanged;
            channelTree.ChannelSelected += channelTree_ChannelSelected;
            channelTree.ChannelUnselected += channelTree_ChannelUnselected;
        }

        private List<ChannelInfo> Channels { get; set; }
        private List<ChannelGroupInfo> Groups { get; set; }

        private MailItem MailItem
        {
            get { return _mailItem; }
            set
            {
                if (null != _mailItem)
                {
                    Marshal.ReleaseComObject(_mailItem);
                    _mailItem = null;
                }
                _mailItem = value;
            }
        }

        void ComposeMonitor.IComposeMonitorCallback.OnChannelsChanged(List<ChannelInfo> channels)
        {
            channelTree.SelectedChannels = channels;
            membersTree.SelectedChannels = channels;
        }

        void ComposeMonitor.IComposeMonitorCallback.OnSuggestionsChanged(List<string> suggestedChannels)
        {
            channelTree.SuggestedChannelIds = suggestedChannels;
        }

        private void Instance_GroupListChanged(object sender, EventArgs e)
        {
            OnChannelsChanged();
        }

        private void Instance_ChannelListChanged(object sender, EventArgs e)
        {
            OnChannelsChanged();
        }

        private void OnChannelsChanged()
        {
            Scheduler.RunIfNotScheduled("ComposeSidebarUpdateChannels", "Update channels", 100,
                () =>
                {
                    channelTree.SetState(PropertiesService.Instance.Channels, PropertiesService.Instance.Groups);
                    channelTree_SelectionChanged(null, null);
                });
        }

        private void channelTree_ChannelUnselected(object sender, ChannelTree.ChannelInfoEventArgs e)
        {
            if (null == _monitor) return;
            _monitor.RemoveChannel(e.Channel);
        }

        private void channelTree_ChannelSelected(object sender, ChannelTree.ChannelInfoEventArgs e)
        {
            if (null == _monitor) return;
            _monitor.AddChannel(e.Channel);
        }

        private void Instance_UserChanged(object sender, EventArgs e)
        {
            LoadContent();
        }

        private void channelTree_SelectionChanged(object sender, EventArgs e)
        {
            membersTree.SelectedChannels = channelTree.SelectedChannels;
        }

        private void LoadContent()
        {
            lock (this)
            {
                if (_loadingConent)
                {
                    return;
                }

                _loadingConent = true;
            }
            try
            {
                if (!PropertiesService.Instance.SignedIn)
                {
                    HidePane();
                }
                else
                {
                    HidePane();
                    Inspector inspector = null;
                    try
                    {
                        inspector = InspectorObj as Inspector;
                        if (null == inspector)
                        {
                            HidePane();
                            return;
                        }
                        else
                        {
                            var obj = inspector.CurrentItem;

                            var mi = inspector.CurrentItem as MailItem;
                            if (mi == null)
                            {
                                Marshal.ReleaseComObject(obj);
                            }
                            else
                            {
                                MailItem = mi;
                            }
                        }
                    }
                    finally
                    {
                        if (null != inspector)
                        {
                            Marshal.ReleaseComObject(inspector);
                        }
                    }
                    if (null == MailItem)
                    {
                        HidePane();
                        return;
                    }
                    _monitor = new ComposeMonitor(MailItem, this);
                    UpdateChannels();
                    ShowPane();
                }
            }

            finally
            {
                lock (this)
                {
                    _loadingConent = false;
                }
            }
        }

        private void UpdateChannels()
        {
            Channels = PropertiesService.Instance.Channels;
            Groups = PropertiesService.Instance.Groups;
            channelTree.SetState(Channels, Groups);
        }

        private void HidePane()
        {
            RegionState = ADXRegionState.Hidden;
        }

        private void ShowPane()
        {
            RegionState = ADXRegionState.Normal;
        }

        private void ADXOlComposeHelperForm_ADXBeforeFormShow()
        {
            LoadContent();
        }

        private void ADXOlComposeHelperForm_ADXAfterFormHide(object sender, ADXAfterFormHideEventArgs e)
        {
            Stop();
        }

        private void ADXOlComposeHelperForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Stop();
        }

        private void Stop()
        {
            if (null == _monitor) return;
            _monitor.Stop();
            _monitor = null;
        }
    }
}