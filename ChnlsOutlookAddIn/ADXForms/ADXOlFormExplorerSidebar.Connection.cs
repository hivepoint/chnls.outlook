#region

using System;
using System.IO;
using System.Windows.Forms;
using AddinExpress.OL;
using chnls.Service;
using chnls.Utils;

#endregion

namespace chnls.ADXForms
{
    // ReSharper disable once InconsistentNaming
    partial class ADXOlFormExplorerSidebar
    {
        private const string ServerConnectionMessage = "Error connecting to server.";

        private readonly object _iterationLock = new object();
        private readonly Timer _timerLoad = new Timer();
        private readonly Timer _timerReconnect = new Timer();
        private string _baseUrl;
        private int _currentIteration;
        private int _iterationCounter;
        private int _reconnectCountdown = 30;
        private bool _reconnectPending;

        private void InitializeConnection()
        {
            _timerReconnect.Tick += timerReconnect_Tick;
            _timerLoad.Interval = 45000;
            _timerLoad.Tick += timerLoad_Tick;
            ADXPostMessageReceived += ADXOlForm1_ADXPostMessageReceived;
            ADXAfterFormShow += ADXOlForm1_ADXAfterFormShow;
            PropertiesService.Instance.BaseUrlChanged += Instance_BaseUrlChanged;
        }

        private void ADXOlForm1_ADXAfterFormShow()
        {
            ADXPostMessage(IntPtr.Zero, IntPtr.Zero);
        }

        private void ADXOlForm1_ADXPostMessageReceived(object sender, ADXPostMessageReceivedEventArgs e)
        {
            UpdateBaseUrl();
        }

        private void Instance_BaseUrlChanged(object sender, EventArgs e)
        {
            UpdateBaseUrl();
        }

        private void UpdateBaseUrl()
        {
            var updatedUrl = ChnlsUrlHelper.GetAppUrl();
            if (_baseUrl == updatedUrl) return;

            _baseUrl = updatedUrl;
            Reconnect(0);
        }

        private void LoadComplete()
        {
            LoggingService.Debug("Load complete");
            _timerLoad.Enabled = false;
            _timerReconnect.Enabled = false;
            statusToast.Detail = "Connecting to hive";
            splash.Visible = false;
            PropertiesService.Instance.Connected = true;
        }


        private void timerLoad_Tick(object sender, EventArgs e)
        {
            if (!_timerLoad.Enabled) return;

            LoggingService.Info("Reconnect: " + _baseUrl);
            Reconnect(5);
            _timerLoad.Enabled = false;
        }

        private bool IsNotCurrentLoadIteration()
        {
            var doesntMatch = false;
            try
            {
                lock (_iterationLock)
                {
                    doesntMatch = _currentIteration != _iterationCounter;
                    return doesntMatch;
                }
            }
            finally
            {
                if (doesntMatch)
                {
                    LoggingService.Warn("Ignoring call because we are refreshing page");
                }
            }
        }

        private void Reconnect(int delay)
        {
            if (_reconnectPending && delay > 0)
            {
                return;
            }
            LoggingService.Info("Reconnect: " + _baseUrl);
            _reconnectPending = true;
            PropertiesService.Instance.Connected = false;
            lock (_iterationLock)
            {
                _iterationCounter++;
            }
            _reconnectCountdown = delay;
            if (delay <= 0)
            {
                _timerReconnect.Interval = 10;
            }
            else
            {
                _timerReconnect.Interval = 1000;
            }
            _timerReconnect.Enabled = false;
            _timerReconnect.Enabled = true;
        }

        private void ConnectNow(bool force)
        {
            if (!_reconnectPending && !force) return;

            statusToast.Show("Connecting", null, 0);
            splash.Visible = true;
            _reconnectCountdown = 0;
            LoggingService.Debug("Retrying now...");
            statusToast.Detail = "Retrying now...";
            lock (_iterationLock)
            {
                _iterationCounter++;
                _currentIteration = _iterationCounter;
            }
            _reconnectPending = false;
            _timerReconnect.Enabled = false;
            webBrowserMain.Stop();
            webBrowserMain.Navigate("about:blank");
            webBrowserMain.Stop();
            LoggingService.Info("Navigating to: " + _baseUrl);
            webBrowserMain.Navigate(_baseUrl);
            _timerLoad.Enabled = false;
            _timerLoad.Enabled = true;
        }

        private void timerReconnect_Tick(object sender, EventArgs e)
        {
            if (_reconnectPending)
            {
                if (_reconnectCountdown == 0)
                {
                    ConnectNow(false);
                }
                else
                {
                    var msg = ServerConnectionMessage + " Retrying in " + _reconnectCountdown + " second";
                    if (_reconnectCountdown > 1)
                    {
                        msg += "s";
                    }
                    msg += "...";
                    statusToast.Show(msg, null, 0);
                    LoggingService.Debug(msg);
                    _reconnectCountdown--;
                }
            }
        }

        private void webBrowserMain_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            statusToast.Detail = "Loading application";
            var description = "DocumentCompleted: " + e.Url;
            if (null != webBrowserMain.Document)
            {
                description += "[" + webBrowserMain.Document.Cookie + "]";
            }
            LoggingService.Debug(description);
        }
    }

    // ReSharper disable once InconsistentNaming
    partial class ADXOlFormExplorerSidebar
    {
        internal void ClearLocalCookies()
        {
            webBrowserMain.Stop();
            webBrowserMain.Navigate(
                "javascript:void((function(){var a,b,c,e,f;f=0;a=document.cookie.split('; ');for(e=0;e<a.length&&a[e];e++){f++;for(b='.'+location.host;b;b=b.replace(/^(?:%5C.|[^%5C.]+)/,'')){for(c=location.pathname;c;c=c.replace(/.$/,'')){document.cookie=(a[e]+'; domain='+b+'; path='+c+'; expires='+new Date((new Date()).getTime()-1e11).toGMTString());}}}})())");
            webBrowserMain.Refresh();
        }

        internal void RefreshBrowser()
        {
            webBrowserMain.Refresh();
            LoggingService.Info("Refresh Browser: " + _baseUrl);
        }

        internal void Dump()
        {
            var cookies = Document.Cookie;
            LoggingService.Debug("Cookies: " + cookies);

            var theCookies = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Cookies));
            foreach (var cookie in theCookies)
            {
                try
                {
                    var reader = File.OpenText(cookie);

                    var text = "";
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        text += line + "\r\n";
                    }
                    reader.Close();
                    if (text.Trim().Length > 0 && text.ToLower().Contains("hivepoint"))
                    {
                        LoggingService.Debug("Cookie: " + cookie + ": " + text);
                    }
                }
                catch (Exception ex)
                {
                    LoggingService.Error("Error reading: " + cookie, ex);
                }
            }
        }

        internal void NavigateTo(string urlString)
        {
            webBrowserMain.Navigate(urlString);
            LoggingService.Info("Navigate to: " + urlString);
        }
    }
}