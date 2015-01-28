#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using chnls.Model;
using chnls.Service;
using Newtonsoft.Json;

#endregion

namespace chnls.Utils
{
    internal enum ExtensionQueryType
    {
        // ReSharper disable InconsistentNaming
        CHANNELS,
        GROUPS,
        REPLY_MESSAGE_INFO,
        DIALOG_RESULT
        // ReSharper restore InconsistentNaming
    }

    internal enum ExtensionActionType
    {
        // ReSharper disable InconsistentNaming
        AUTHORIZE_URL,
        GO_HOME,
        REFRESH_CHANNELS
        // ReSharper restore InconsistentNaming
    }

    internal class ChnlsBrowserHelper
    {
        internal static void RegisterWithWebClient(HtmlDocument document)
        {
            var code = @"channelsExtensionHelper.registerExtension({'version':'" +
                       typeof (ChnlsBrowserHelper).Assembly.GetName().Version +
                       @"', capabilities:['HANDLES_REPLIES','HANDLES_OPEN_WINDOW','NEEDS_URL_AUTH','HANDLES_CREATE_CHANNEL']});";
            object[] codeString = {code};
            document.InvokeScript("eval", codeString);
        }

        public static void RegisterPopupWithWebClient(HtmlDocument document)
        {
            var code = @"channelsExtensionHelper.registerExtension({'version':'" +
                       typeof (ChnlsBrowserHelper).Assembly.GetName().Version +
                       @"', capabilities:['HANDLES_CLOSE_WINDOW']});";
            object[] codeString = {code};
            document.InvokeScript("eval", codeString);
        }

        internal static void NotifyChannelRefresh(HtmlDocument document)
        {
            PerformAction(document, ExtensionActionType.REFRESH_CHANNELS, "");
        }

        internal static bool PerformAction(HtmlDocument document, ExtensionActionType type, string actionJson)
        {
            if (!IsActionSupported(document, type))
            {
                return false;
            }
            var code = "channelsExtensionHelper.performAction('" + type + "', " + actionJson + ");";
            object[] codeString = {code};
            Debug.WriteLine("type: " + type + " action:" + actionJson);
            document.InvokeScript("eval", codeString);

            return true;
        }

        private static bool IsActionSupported(HtmlDocument document, ExtensionActionType actionType)
        {
            var code = "channelsExtensionHelper.isActionSupported('" + actionType + "');";
            object[] codeString = {code};
            var result = document.InvokeScript("eval", codeString);
            var supported = null != result &&
                            String.Equals("true", (string) result, StringComparison.InvariantCultureIgnoreCase);
            return supported;
        }

        internal static List<ChannelInfo> GetChannels(HtmlDocument document)
        {
            var channelList = GetWebObject<ChannelList>(document, ExtensionQueryType.CHANNELS, "");
            return null != channelList ? channelList.channels : null;
        }

        internal static List<ChannelGroupInfo> GetGroups(HtmlDocument document)
        {
            var groupList = GetWebObject<ChannelGroupList>(document, ExtensionQueryType.GROUPS, "");
            return null != groupList ? groupList.groups : null;
        }

        internal static T GetWebObject<T>(HtmlDocument document, ExtensionQueryType queryType, string key)
        {
            if (!PropertiesService.Instance.Connected)
            {
                return default(T);
            }
            object[] codeString = {@"channelsExtensionHelper.getValue('" + queryType + "', '" + key + "');"};
            if (document == null) return default(T);
            var result = document.InvokeScript("eval", codeString);
            var json = result as string;
            LoggingService.Debug("json:\n" + json);
            return String.IsNullOrWhiteSpace(json) ? default(T) : JsonConvert.DeserializeObject<T>(json);
        }
    }
}