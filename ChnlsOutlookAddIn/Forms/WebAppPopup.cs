#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using chnls.Service;
using chnls.Utils;

#endregion

namespace chnls.Forms
{
    public partial class WebAppPopup : Form
    {
        //        private static readonly List<WebAppPopup> Forms = new List<WebAppPopup>();

        private Func<ChannelsRequest, bool> _requestHandler;
        private Action<HtmlDocument> _closeCallback;
        private int _paddingHeight = 0;
        private int _paddingWidth = 0;
        private int _contentHeight = 0;
        private int _contentWidth = 0;

        internal WebAppPopup(Func<ChannelsRequest, bool> requestHandler = null, Action<HtmlDocument> closeCallback = null)
        {
            InitializeComponent();
            _closeCallback = closeCallback;
            _requestHandler = requestHandler;
            Closed += WebAppPopup_Closed;
        }

        private HtmlDocument Document
        {
            get
            {
                Debug.Assert(webBrowser.Document != null, "webBrowser.Document != null");
                return webBrowser.Document;
            }
        }

        internal static WebAppPopup GetInstance(Func<ChannelsRequest, bool> requestHandler = null, Action<HtmlDocument> closeCallback = null)
        {
            //lock (Forms)
            //{
            //    if (Forms.Any())
            //    {
            //        var form = Forms[0];
            //        Forms.RemoveAt(0);
            //        form._closeCallback = closeCallback;
            //        form._requestHandler = requestHandler;
            //        return form;
            //    }
            return new WebAppPopup(requestHandler, closeCallback);
            //}
        }

        private void WebAppPopup_Closed(object sender, EventArgs e)
        {
            _closeCallback = null;
            //lock (Forms)
            //{
            //    GotoBlank();
            //    Forms.Add(this);
            //}
        }

        private void GotoBlank()
        {
            webBrowser.Navigate(ChnlsUrlHelper.GetAppUrl() + "#blank:");
        }

        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
        }

        private void webBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (!ChnlsUrlHelper.IsEmailChannelsUrl(e.Url))
            {
                textboxUrl.Text = e.Url.ToString();
                return;
            }

            e.Cancel = true;
            ProcessRequest(ChnlsUrlHelper.GetChnlsRequest(e.Url));
        }

        private void ProcessRequest(ChannelsRequest request)
        {
            if (null != _requestHandler)
            {
                if (_requestHandler(request))
                {
                    return;
                }
            }
            switch (request.Type)
            {
                case ChannelsRequestType.ClientLoaded:
                    RegisterPopupApp();
                    break;
                case ChannelsRequestType.UserSignedOut:
                    Close();
                    GotoBlank();
                    break;
                case ChannelsRequestType.CloseDialog:
                    ChnlsBrowserHelper.OnDialogClosed(Document, (ChannelRequestCloseDialog)request);
                    Close();
                    GotoBlank();
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
            if (null != _closeCallback)
            {
                _closeCallback(webBrowser.Document);
            }
        }

        internal void NavigateToNonAppPage(string pageAbsolutePath)
        {
            var baseUrl = PropertiesService.Instance.BaseUrl + pageAbsolutePath;
            webBrowser.Navigate(baseUrl);
        }

        private void WebAooWindowForm_Load(object sender, EventArgs e)
        {
            _paddingHeight = Height - webBrowser.Height;
            _paddingWidth = Width - webBrowser.Width;
            UpdateSize();
        }

        public void SetContentSize(int height, int width)
        {

            if (height <= 0)
            {
                height = 400;
            }
            if (width <= 0)
            {
                width = 600;
            }
            if (height < 200)
            {
                height = 150;
            }
            if (width < 200)
            {
                width = 200;
            }
            _contentHeight = height;
            _contentWidth = width;
            if (_paddingHeight > 0 || _paddingWidth > 0)
            {
                UpdateSize();
            }
        }

        private void UpdateSize()
        {
            if (_paddingHeight <= 0 || _paddingWidth <= 0)
                return;

            if (_contentHeight > 0)
            {
                Height = _contentHeight + _paddingHeight;
            }
            if (_contentWidth > 0)
            {
                Width = _contentHeight + _paddingWidth;
            }
        }
    }
}