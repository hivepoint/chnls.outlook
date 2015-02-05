#region

using System;
using System.Windows.Forms;
using chnls.Service;
using chnls.Utils;

#endregion

namespace chnls.Forms
{
    public partial class WebAppPopup : Form
    {
        private readonly Action<HtmlDocument> _callback;

        internal WebAppPopup(Action<HtmlDocument> callback)
        {
            InitializeComponent();
            _callback = callback;
        }

        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
        }

        private void webBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (!ChnlsUrlHelper.IsEmailChannelsUrl(e.Url)) return;

            e.Cancel = true;
            ProcessRequest(ChnlsUrlHelper.GetChnlsRequest(e.Url));
        }

        private void ProcessRequest(ChannelsRequest request)
        {
            switch (request.Type)
            {
                    case ChannelsRequestType.ClientLoaded:
                    RegisterPopupApp();
                    break;
                case ChannelsRequestType.UserSignedOut:
                    Close();
                    break;
                case ChannelsRequestType.CloseWindow:
                    Close();
                    break;
            }
        }

        private void RegisterPopupApp()
        {
            ChnlsBrowserHelper.RegisterPopupWithWebClient(webBrowser.Document);
        }

        public void NavigateFragment(string fragment)
        {
            if (!fragment.StartsWith("#"))
            {
                fragment = "#" + fragment;
            }
            var baseUrl = ChnlsUrlHelper.GetAppUrl() + fragment;
            webBrowser.Navigate(baseUrl);
        }

        private void WebAppPopup_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (null != _callback)
            {
                _callback(webBrowser.Document);
            }
        }

        internal void NavigateToNonAppPage(string pageAbsolutePath)
        {
            var baseUrl = PropertiesService.Instance.BaseUrl + pageAbsolutePath;
            webBrowser.Navigate(baseUrl);
        }
    }
}