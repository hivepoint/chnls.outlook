#region

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using chnls.Service;
using mshtml;

#endregion

namespace chnls.Forms
{
    public partial class WebPopupWindowForm : Form
    {
        private readonly Action<Uri> _callback;
        private readonly WebBrowser _wparent;
        private int _paddingHeight;
        private int _paddingWidth;
        private bool _parentSet;

        public WebPopupWindowForm(WebBrowser wparent, Action<Uri> callback)
        {
            _wparent = wparent;
            _callback = callback;

            InitializeComponent();
            webBrowser.Url = new Uri("about:blank");
            // wait for document will be loaded to do some magic stuff then
            webBrowser.DocumentCompleted += w2_DocumentCompleted;
        }

        public object WebBrowserAx
        {
            get { return webBrowser.ActiveXInstance; }
        }

        private void webBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            UpdateUrl(e.Url);
            var status = "";
            if (IsDisposed)
            {
                status += "disposed ";
            }

            if (!String.IsNullOrWhiteSpace(status))
            {
                status = "[" + status.Trim() + "] ";
            }
            Debug.WriteLine("POP Navigating: " + status + e.Url);
            if (e.Url.AbsolutePath.EndsWith("googleOauthWindow.html"))
            {
                if (null != _callback)
                {
                    Debug.WriteLine("Cancelling: " + e.Url);
                    e.Cancel = true;
                    _callback(e.Url);
                    Close();
                }
            }
        }

        private void UpdateUrl(Uri url)
        {
            if (Uri.UriSchemeHttp.Equals(url.Scheme, StringComparison.InvariantCultureIgnoreCase) ||
                Uri.UriSchemeHttp.Equals(url.Scheme, StringComparison.InvariantCultureIgnoreCase))
            {
                textboxUrl.Text = url.ToString();
            }
        }

        // sets opener for popup window
        // after document in popup window was loaded
        private void w2_DocumentCompleted(object sender, EventArgs e)
        {
            var popup = (WebBrowser) sender;
            SetOpener(_wparent, popup);
        }

        // My most favorite method :)
        // Contains exactly that hack, we're talking about
        private void SetOpener(WebBrowser opener, WebBrowser popup)
        {
            if (!_parentSet)
            {
                _parentSet = true;
                Debug.Assert(popup.Document != null, "popup.Document == null");
                Debug.Assert(opener.Document != null, "opener.Document == null");
                var htmlPopup = popup.Document.Window;
                var htmlOpener = opener.Document.Window;

                // let the dark magic begin

                // actually, WebBrowser control is .NET wrapper around IE COM interfaces
                // we can get a bit closer to them access by getting reference to 
                // "mshtml.IHTMLWindow2" field via Reflection
                Debug.Assert(htmlPopup != null, "htmlPopup == null");
                var fi = htmlPopup.GetType().GetField("htmlWindow2", BindingFlags.Instance | BindingFlags.NonPublic);

                Debug.Assert(fi != null, "fi == null");
                var htmlPopup2 = (IHTMLWindow2) fi.GetValue(htmlPopup);
                var htmlOpener2 = (IHTMLWindow2) fi.GetValue(htmlOpener);

                // opener is set here
                htmlPopup2.window.opener = htmlOpener2.window.self;
            }
        }

        private void textboxUrl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return || e.KeyCode == Keys.Enter)
            {
                try
                {
                    webBrowser.Url = new Uri(textboxUrl.Text);
                }
                    // ReSharper disable once EmptyGeneralCatchClause
                catch
                {
                }
                e.Handled = true;
            }
        }

        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            var description = "POP document complete: " + e.Url;
            if (null != webBrowser.Document)
            {
                description += " [" + webBrowser.Document.Cookie + "]";
            }
            Debug.WriteLine(description);
            LoggingService.Debug(description);
        }

        private void webBrowser_NewWindow(object sender, CancelEventArgs e)
        {
            Debug.WriteLine("POP new window");
        }

        private void webBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            var description = "POP navigated: " + e.Url;
            if (null != webBrowser.Document)
            {
                description += " [" + webBrowser.Document.Cookie + "]";
            }
            Debug.WriteLine(description);
        }

        private void WebPopupWindowForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Debug.WriteLine("POP closing");
            webBrowser.Stop();
        }

        public void LoadUrl(string url)
        {
            webBrowser.Navigate(url);
        }

        private void WebPopupWindowForm_Load(object sender, EventArgs e)
        {
            _paddingHeight = Height - webBrowser.Height;
            _paddingWidth = Width - webBrowser.Width;
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
            Height = height + _paddingHeight;
            Width = width + _paddingWidth;
        }
    }
}