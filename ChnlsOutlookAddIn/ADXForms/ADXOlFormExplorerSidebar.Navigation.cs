#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Web;
using System.Windows.Forms;
using chnls.Model;
using chnls.Service;
using mshtml;
using Newtonsoft.Json;

#endregion

namespace chnls.ADXForms
{
    internal enum ExtensionQueryType
    {
        // ReSharper disable InconsistentNaming
        CHANNELS,
        GROUPS,
        REPLY_MESSAGE_INFO
        // ReSharper restore InconsistentNaming
    }

    // ReSharper disable once InconsistentNaming
    partial class ADXOlFormExplorerSidebar
    {
        private const string ActionCreateChannel = "createChannel";
        private static int _requestCount;


        private readonly Dictionary<int, Action<CreateChannelResponse>> createChannelCallbacks =
            new Dictionary<int, Action<CreateChannelResponse>>();

        private readonly Dictionary<int, Action<bool>> sendMessageCallbacks = new Dictionary<int, Action<bool>>();

        private HtmlDocument Document
        {
            get
            {
                Debug.Assert(webBrowserMain.Document != null, "webBrowserMain.Document != null");
                return webBrowserMain.Document;
            }
        }

        private void InitializeChnlsNavigation()
        {
            PropertiesService.Instance.BrowserObjectDelegate = new BrowserObjectDelegate { Broswer = this };
        }

        private static bool IsHivePointUrl(Uri uri)
        {
            return uri.Scheme.Equals("channels", StringComparison.OrdinalIgnoreCase);
        }

        private void HandleHivePointUrl(Uri uri)
        {
            var queryString = uri.Query.TrimStart('?');

            var query = HttpUtility.ParseQueryString(queryString);
            LoggingService.Debug("Handle Chnls URI: " + uri);
            switch (uri.Host.ToLower())
            {
                case "clientloaded":
                    statusToast.Detail = "Registering add-in";
                    RegisterCallbacks();
                    RegisterWithWebClient();
                    LoadComplete();
                    break;
                case "contentloaded":
                    statusToast.HideToast();
                    break;
                case "replyto":
                    ReplyToMessageId(query["id"]);
                    break;
                case "openwindow":
                    var url = query["value"];
                    if (!String.IsNullOrWhiteSpace(url) &&
                        String.Compare(url, "about:blank", StringComparison.OrdinalIgnoreCase) != 0)
                    {
                        Debug.WriteLine("NewWindow:" + url);
                        Process.Start(url);
                    }
                    break;
                case "channellistupdated":
                    PropertiesService.Instance.ChannelListDirty();
                    break;
                case "channelgrouplistupdated":
                    PropertiesService.Instance.ChannelGroupListDirty();
                    break;
                case "actioncomplete":
                    var success = false;
                    try
                    {
                        success = Boolean.Parse(query["success"]);
                    }
                    catch (ArgumentNullException)
                    {
                    }
                    catch (FormatException)
                    {
                    }
                    OnActionResponseInt(query["value"], query["action"], success, query["response"]);
                    break;
                case "usersignedin":
                    PropertiesService.Instance.UserEmail = query["email"];
                    break;
                case "usersignedout":
                    PropertiesService.Instance.UserEmail = null;
                    break;
            }
        }

        private void GoogleOAuthReturn(String hash)
        {
            if (null == hash)
            {
                hash = "";
            }
            var code = "window.googleOauth.__doFinish('" + hash + "');";
            object[] codeString = { code };
            Document.InvokeScript("eval", codeString);
        }

        public void AuthorizeUrl(Uri url)
        {
            LoggingService.Debug("Authorize1: " + url);
            if (url.ToString().ToLower().StartsWith(PropertiesService.Instance.BaseUrl.ToLower() + "ui/svc"))
            {
                // if it is a url that is a hivepoint url, get it authorized
                if (!PerformAction("AUTHORIZE_URL", "{'url':'" + url + "'}"))
                {
                    // authorize url is not supported, so just open the url
                    Process.Start(url.ToString());
                }
            }
            else
            {
                Process.Start(url.ToString());
            }
        }

        private void RegisterCallbacks()
        {
            webBrowserMain.ObjectForScripting = new ScriptManager(this);

            var head = Document.GetElementsByTagName("head")[0];
            var scriptEl = Document.CreateElement("script");
            Debug.Assert(scriptEl != null, "scriptEl != null");
            var element = (IHTMLScriptElement)scriptEl.DomElement;
            element.text =
                "function ChnlsNativeOnActionComplete(actionType, actionRequest, success, actionResponse){"
                +
                "window.external.OnActionResponse(actionType, JSON.stringify(actionRequest), success, JSON.stringify(actionResponse));"
                + "}";

            head.AppendChild(scriptEl);
        }

        // This nested class must be ComVisible for the JavaScript to be able to call it.

        private void RegisterWithWebClient()
        {
            var code = @"channelsExtensionHelper.registerExtension({'version':'" + GetType().Assembly.GetName().Version +
                       @"', capabilities:['HANDLES_REPLIES','HANDLES_OPEN_WINDOW','NEEDS_URL_AUTH','HANDLES_CREATE_CHANNEL']});";
            object[] codeString = { code };
            Document.InvokeScript("eval", codeString);
        }

        private void ReplyToMessageId(string messageId)
        {
            var info = GetWebObject<ReplyMessageInfo>(ExtensionQueryType.REPLY_MESSAGE_INFO, messageId);
            if (null != info)
            {
                //ComposeService.Instance.ReplyTo(info);
            }
        }

        private T GetWebObject<T>(ExtensionQueryType queryType, string key)
        {
            if (!PropertiesService.Instance.Connected)
            {
                return default(T);
            }
            object[] codeString = { @"channelsExtensionHelper.getValue('" + queryType + "', '" + key + "');" };
            if (webBrowserMain.Document == null) return default(T);
            var result = webBrowserMain.Document.InvokeScript("eval", codeString);
            var json = result as string;
            LoggingService.Debug("json:\n" + json);
            return String.IsNullOrWhiteSpace(json) ? default(T) : JsonConvert.DeserializeObject<T>(json);
        }

        private bool PerformAction(string type, string actionJson)
        {
            if (!IsActionSupported(type))
            {
                return false;
            }
            var code = "channelsExtensionHelper.performAction('" + type + "', " + actionJson + ");";
            object[] codeString = { code };
            Debug.WriteLine("type: " + type + " action:" + actionJson);
            Document.InvokeScript("eval", codeString);

            return true;
        }

        private bool IsActionSupported(string actionType)
        {
            var code = "channelsExtensionHelper.isActionSupported('" + actionType + "');";
            object[] codeString = { code };
            var result = Document.InvokeScript("eval", codeString);
            var supported = null != result &&
                            String.Equals("true", (string)result, StringComparison.InvariantCultureIgnoreCase);
            return supported;
        }


        private void OnActionResponseInt(string actionType, string actionRequest, bool success, string actionResponse)
        {
            Debug.Print("OnActionResponse: " + actionType + " " + actionRequest + " " + actionResponse);
            switch (actionType.ToUpper())
            {
                case "CREATE_CHANNEL":
                    OnCreateChannel(actionRequest, actionResponse);
                    break;
                case "UPLOAD_ENVELOPES":
                    OnUploadEnvelopesChannel(actionRequest, success);
                    break;
            }
        }

        private void OnCreateChannel(string actionRequest, string actionResponse)
        {
            if (String.IsNullOrWhiteSpace(actionRequest))
            {
                return;
            }
            var request = JsonConvert.DeserializeObject<CreateChannelRequest>(actionRequest);
            if (null == request)
            {
                return;
            }
            var callback = createChannelCallbacks[request.reqId];
            if (null == callback) return;
            CreateChannelResponse response = null;
            if (!String.IsNullOrWhiteSpace(actionResponse))
            {
                response = JsonConvert.DeserializeObject<CreateChannelResponse>(actionResponse);
            }
            callback(response);
        }

        private void OnUploadEnvelopesChannel(string actionRequest, bool success)
        {
            if (String.IsNullOrWhiteSpace(actionRequest))
            {
                return;
            }
            var request = JsonConvert.DeserializeObject<SendMessageEnvelopeRequest>(actionRequest);
            if (null == request)
            {
                return;
            }
            var callback = sendMessageCallbacks[request.reqId];
            if (null == callback) return;
            callback(success);
        }

        private class BrowserObjectDelegate : IBrowserObjectDelegate
        {
            public ADXOlFormExplorerSidebar Broswer { private get; set; }

            public List<ChannelInfo> Channels()
            {
                var channelList = Broswer.GetWebObject<ChannelList>(ExtensionQueryType.CHANNELS, "");
                return null != channelList ? channelList.channels : null;
            }

            public List<ChannelGroupInfo> Groups()
            {
                var groupList = Broswer.GetWebObject<ChannelGroupList>(ExtensionQueryType.GROUPS, "");
                return null != groupList ? groupList.groups : null;
            }
        }

        [ComVisible(true)]
        public class ScriptManager
        {
            // Variable to store the form of type Form1.
            private readonly ADXOlFormExplorerSidebar _webbrowser;

            // Constructor.
            public ScriptManager(ADXOlFormExplorerSidebar webbrowser)
            {
                // Save the form so it can be referenced later.
                _webbrowser = webbrowser;
            }

            // This method can be called from JavaScript.
            public void OnActionResponse(string actionType, string actionRequest, bool success, string actionResponse)
            {
                // Call a method on the form.
                _webbrowser.OnActionResponseInt(actionType, actionRequest, success, actionResponse);
            }
        }
    }
}