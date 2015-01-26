#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using chnls.Service;
using chnls.Utils;
using Microsoft.Office.Interop.Outlook;
using Exception = System.Exception;

#endregion

namespace chnls
{
    public partial class AddinModule
    {
        internal void ContactSupport()
        {
            Process.Start(Constants.UrlSupport);
        }

        internal void GotoChnlsServer()
        {
            Process.Start(PropertiesService.Instance.BaseUrl);
        }

        private void InitializeChnls()
        {
            PropertiesService.Instance.ChannelListChanged += Instance_ChannelListChanged;
            PropertiesService.Instance.UserChanged += Instance_UserChanged;
            UpdateService.Instance.Start();
        }

        private void Instance_UserChanged(object sender, EventArgs e)
        {
            UpdateShareEnablement();
        }

        private void Instance_ChannelListChanged(object sender, EventArgs e)
        {
            UpdateShareEnablement();
        }

        private void AddToChannels()
        {
            var items = GetSelectedMailItems();
            if (!items.Any())
            {
                return;
            }

            AddToChannelsHelper.AddToChannels(items);
        }

        private List<MailItem> GetSelectedMailItems()
        {
            var items = new List<MailItem>();
            Selection selection = null;
            var explorer = OutlookApp.ActiveExplorer();

            try
            {
                try
                {
                    selection = explorer.Selection;
                }
                    // ReSharper disable once EmptyGeneralCatchClause
                catch
                {
                }
                if (null != selection)
                {
                    for (var i = 0; i < selection.Count; i++)
                    {
                        object item = null;
                        try
                        {
                            item = selection[i + 1]; // COM 1 BASED
                            if (item is MailItem)
                            {
                                items.Add((MailItem) item);
                                item = null;
                            }
                        }
                        finally
                        {
                            if (null != item)
                            {
                                Marshal.ReleaseComObject(item);
                                // ReSharper disable once RedundantAssignment
                                item = null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingService.Error("Error getting selection", ex);
            }
            finally
            {
                if (null != selection)
                {
                    Marshal.ReleaseComObject(selection);
                    // ReSharper disable once RedundantAssignment
                    selection = null;
                }
                if (null != explorer)
                {
                    Marshal.ReleaseComObject(explorer);
                    // ReSharper disable once RedundantAssignment
                    explorer = null;
                }
            }
            return items;
        }
    }
}