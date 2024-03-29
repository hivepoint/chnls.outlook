#region

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using AddinExpress.OL;
using chnls.Forms;
using chnls.Properties;
using chnls.Service;
using chnls.Utils;
using WebBrowserCom = SHDocVw.WebBrowser;

#endregion

namespace chnls.ADXForms
{
    // ReSharper disable once InconsistentNaming
    public partial class ADXOlFormExplorerSidebar : ADXOlForm
    {
        private WebBrowserCom _wbComMain;

        public ADXOlFormExplorerSidebar()
        {
            PropertiesService.Instance.Connected = false;
            InitializeComponent();

            webBrowserMain.Url = new Uri("about:blank");
            Text = "Channels.cc";
            Icon = Resources.favIcon;

            debugBar.Browser = this;
            debugBar.Visible = PropertiesService.Instance.DebugPanelVisible;
            PropertiesService.Instance.DebugVisibleChanged +=
                (sender, e) => { debugBar.Visible = PropertiesService.Instance.DebugPanelVisible; };
            UpdateService.Instance.UpdateAvailable += (s, e) => { upgradePanel.Visible = true; };
            upgradePanel.Visible = UpdateService.Instance.IsUpdateAvailable;
        }

        private void ADXOlFormExplorerSidebar_Load(object sender, EventArgs e)
        {
            InitializeConnection();
            InitializeChnlsNavigation();
        }

        private void ADXOlFormExplorerSidebar_ADXAfterFormShow()
        {
            if (null != _wbComMain)
            {
                _wbComMain = (WebBrowserCom)webBrowserMain.ActiveXInstance;
                _wbComMain.NewWindow3 += WbComMainNewWindow3;
            }
        }

        private void ADXOlFormExplorerSidebar_ADXAfterFormHide(object sender, ADXAfterFormHideEventArgs e)
        {
//            if (null == _wbComMain) return;
//            Marshal.ReleaseComObject(_wbComMain);
//            _wbComMain = null;
        }

        private void WbComMainNewWindow3(ref object ppDisp, ref bool cancel, uint dwFlags, string bstrUrlContext,
            string bstrUrl)
        {
            var uri = new Uri(bstrUrl);
            if (bstrUrl == "about:blank" || IsNotCurrentLoadIteration())
            {
                cancel = true;
                return;
            }
            var description = "Navigating new window: " + uri;
            LoggingService.Debug(description);
            if (IsOauth(uri))
            {
                var pwf = new WebPopupWindowForm(webBrowserMain, url => GoogleOAuthReturn(url.Fragment))
                {
                    StartPosition = FormStartPosition.CenterParent
                };
                if (null != ppDisp)
                {
                   // Marshal.ReleaseComObject(ppDisp);
                }
                ppDisp = pwf.WebBrowserAx;

                Scheduler.Run(description, () =>
                {
                    if (IsNotCurrentLoadIteration())
                    {
                        // this isn't the current load itertion, ignore
                        return;
                    }
                    pwf.ShowDialog();
                }, 1);
            }
            else
            {
                cancel = true;
                Scheduler.Run(description, () =>
                {
                    if (IsNotCurrentLoadIteration())
                    {
                        // this isn't the current load itertion, ignore
                        return;
                    }
                    if (ChnlsUrlHelper.IsEmailChannelsUrl(uri))
                    {
                        HandleEmailChannelsUrl(uri);
                    }
                    else if (IsMailTo(uri))
                    {
                        HandleMailTo(uri);
                    }
                    else
                    {
                        AuthorizeUrl(uri);
                    }
                }, 10);
            }
        }

        private void HandleMailTo(Uri uri)
        {
            ComposeHelper.MailTo(uri);
        }

        private static bool IsMailTo(Uri uri)
        {
            return Uri.UriSchemeMailto.Equals(uri.Scheme, StringComparison.InvariantCultureIgnoreCase);
        }

        private void webBrowserMain_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            var description = "Navigating: " + e.Url;
            if (null != webBrowserMain.Document)
            {
                description += " [" + webBrowserMain.Document.Cookie + "]";
            }
            Debug.WriteLine(description);
            LoggingService.Debug(description);

            if (ChnlsUrlHelper.IsEmailChannelsUrl(e.Url))
            {
                Scheduler.Run(description,
                    () =>
                    {
                        if (IsNotCurrentLoadIteration())
                        {
                            // this isn't the current load itertion, ignore
                            return;
                        }
                        HandleEmailChannelsUrl(e.Url);
                    }, 10);
                e.Cancel = true;
            }
            else if (IsMailTo(e.Url))
            {
                HandleMailTo(e.Url);
                e.Cancel = true;
            }
            else if (IsIeErrorUrl(e.Url))
            {
                HandleInvalidIeVersion();
            }
        }

        private static bool IsOauth(Uri url)
        {
            return url.PathAndQuery.ToLower().Contains("/r/client/google_oauth/v1/request");
        }

        private void HandleInvalidIeVersion()
        {
            LoggingService.Debug("Invalide IE Version");
            statusToast.Show("Invalid IE version", null, 15);
        }

        private static bool IsIeErrorUrl(Uri uri)
        {
            return uri.ToString().ToLower().Contains(Constants.UrlIeVersionProblemString);
        }
    }
}