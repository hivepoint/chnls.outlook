#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using chnls.Model;
using chnls.Service;
using Newtonsoft.Json;

#endregion

namespace chnls.Utils
{
    /*
     * This class is a place for all browser logic for dealing with the chnls web service.
     * 
     * For example, all javascript actions that are performed on the browser should be done through this helper and all chnls url messages should be processed here too.
     */

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
                       @"', capabilities:['HANDLES_MESSAGE_REPLY','HANDLES_OPEN_WINDOW','NEEDS_URL_AUTH','HANDLES_CREATE_CHANNEL']});";
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
            if (null == channelList || null == channelList.channels)
            {
                return null;
            }
            return
                channelList.channels.Select(ValidateAndCorrectChannel)
                    .Where(verifiedChannel => null != verifiedChannel)
                    .ToList();
        }

        private static ChannelInfo ValidateAndCorrectChannel(ChannelInfo channelInfo)
        {
            if (null == channelInfo.channelEmailAddress
                || String.IsNullOrWhiteSpace(channelInfo.name)
                || String.IsNullOrWhiteSpace(channelInfo._id)
                || (!String.IsNullOrWhiteSpace(channelInfo.groupId) && String.IsNullOrWhiteSpace(channelInfo.groupName))
                || (String.IsNullOrWhiteSpace(channelInfo.groupId) && !String.IsNullOrWhiteSpace(channelInfo.groupName)))
            {
                return null;
            }
            if (EmbedChannelActivityState.None == channelInfo.activityState)
            {
                channelInfo.activityState = EmbedChannelActivityState.ACTIVE;
            }

            if (null == channelInfo.subscribers)
            {
                channelInfo.subscribers = new List<EmailAddress>();
            }
            channelInfo.subscribers.RemoveAll(e => null == e || String.IsNullOrWhiteSpace(e.address));
            if (null == channelInfo.watchers)
            {
                channelInfo.watchers = new List<EmailAddress>();
            }
            channelInfo.watchers.RemoveAll(e => null == e || String.IsNullOrWhiteSpace(e.address));
            return channelInfo;
        }

        internal static List<ChannelGroupInfo> GetGroups(HtmlDocument document)
        {
            var groupList = GetWebObject<ChannelGroupList>(document, ExtensionQueryType.GROUPS, "");
            if (null == groupList || null == groupList.groups)
            {
                return null;
            }
            return
                groupList.groups.Select(ValidateAndCorrectChannelGroup)
                    .Where(validatedGroup => null != validatedGroup)
                    .ToList();
        }

        private static ChannelGroupInfo ValidateAndCorrectChannelGroup(ChannelGroupInfo groupInfo)
        {
            if (String.IsNullOrWhiteSpace(groupInfo._id)
                || String.IsNullOrWhiteSpace(groupInfo.name))
            {
                return null;
            }
            return groupInfo;
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