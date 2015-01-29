#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using chnls.Model;
using chnls.Service;
using chnls.Utils;
using mshtml;
using Newtonsoft.Json;

#endregion

namespace chnls.ADXForms
{


    // ReSharper disable once InconsistentNaming
    partial class ADXOlFormExplorerSidebar
    {

        private readonly Dictionary<int, Action<CreateChannelResponse>> _createChannelCallbacks =
            new Dictionary<int, Action<CreateChannelResponse>>();

        private readonly Dictionary<int, Action<bool>> _sendMessageCallbacks = new Dictionary<int, Action<bool>>();

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

        private void HandleHivePointUrl(Uri uri)
        {
            LoggingService.Debug("Handle Chnls URI: " + uri);
            var request = ChnlsUrlHelper.GetChnlsRequest(uri);
            switch (request.Type)
            {
                case ChannelsRequestType.ClientLoaded:
                    statusToast.Detail = "Registering add-in";
                    RegisterCallbacks();
                    ChnlsBrowserHelper.RegisterWithWebClient(Document);
                    LoadComplete();
                    break;
                case ChannelsRequestType.OpenWindow:
                    var url = ((ChannelsRequestWithUrl)request).Url;
                    if (!String.IsNullOrWhiteSpace(url) &&
                        String.Compare(url, "about:blank", StringComparison.OrdinalIgnoreCase) != 0)
                    {
                        Debug.WriteLine("NewWindow:" + url);
                        Process.Start(url);
                    }

                    break;
                case ChannelsRequestType.CloseWindow:
                    break;
                case ChannelsRequestType.ContentLoaded:
                    statusToast.HideToast();
                    break;
                case ChannelsRequestType.ChannelUpdated:
                case ChannelsRequestType.ChannelListUpdated:
                    PropertiesService.Instance.ChannelListDirty();
                    break;
                case ChannelsRequestType.ChannelGroupListUpdated:
                    PropertiesService.Instance.ChannelGroupListDirty();
                    break;
                case ChannelsRequestType.ActionComplete:
                    OnActionResponseInt((ChannelsRequestActionComplete)request);
                    break;
                case ChannelsRequestType.NewItemsAddedToFeed:
                    break;
                case ChannelsRequestType.HandleMessageReply:
                    ReplyToMessageId((ChannelsRequestWithId)request);
                    break;
                case ChannelsRequestType.UserSignedIn:
                    PropertiesService.Instance.UserEmail = ((ChannelsRequestWithEmailAndName)request).Email;
                    break;
                case ChannelsRequestType.UserSignedOut:
                    PropertiesService.Instance.UserEmail = null;
                    break;
                case ChannelsRequestType.HandleCreateChannel:
                    var requestWithGroup = (ChannelsRequestWithGroupAndEmails)request;
                    CreateChannelHelper.CreateChannel(requestWithGroup.Group, requestWithGroup.Emails);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
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
                if (!ChnlsBrowserHelper.PerformAction(Document, ExtensionActionType.AUTHORIZE_URL, "{'url':'" + url + "'}"))
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


        private void ReplyToMessageId(ChannelsRequestWithId request)
        {
            var info = ChnlsBrowserHelper.GetWebObject<ReplyMessageInfo>(Document, ExtensionQueryType.REPLY_MESSAGE_INFO,
                request.Id);
            if (null != info)
            {
                ReplyHelper.ReplyTo(info);
            }
        }


        private void OnActionResponseInt(ChannelsRequestActionComplete request)
        {
            OnActionResponseInt(request.ActionType, request.Action, request.Success, request.Resposne);
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
            var callback = _createChannelCallbacks[request.reqId];
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
            var callback = _sendMessageCallbacks[request.reqId];
            if (null == callback) return;
            callback(success);
        }

        private class BrowserObjectDelegate : IBrowserObjectDelegate
        {
            public ADXOlFormExplorerSidebar Broswer { private get; set; }

            public List<ChannelInfo> Channels()
            {
                return ChnlsBrowserHelper.GetChannels(Broswer.Document);
            }

            public List<ChannelGroupInfo> Groups()
            {
                return ChnlsBrowserHelper.GetGroups(Broswer.Document);
            }

            public void NotifyChannelCreated()
            {
                ChnlsBrowserHelper.NotifyChannelRefresh(Broswer.Document);
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