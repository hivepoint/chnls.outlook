#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using chnls.Forms;
using chnls.Model;
using chnls.Service;

#endregion

namespace chnls.Utils
{
    internal class CreateChannelHelper
    {
        internal static void CreateChannel(string groupId, List<string> emails, bool gotoChannel)
        {
            const string reqId = @"CreateChannel";
            var form = new WebAppPopup((document =>
            {
                var result = ChnlsBrowserHelper.GetWebObject<CreateChannelDialogResponse>(document,
                    ExtensionQueryType.DIALOG_RESULT,
                    reqId);
                if (null == result || !result.success)
                {
                    return;
                }
                PropertiesService.Instance.NotifyChannelCreated(result.channel, result.group, gotoChannel);
            }))
            {
                StartPosition = FormStartPosition.CenterParent
            };
            var fragment = "new_channel:";
            if (!String.IsNullOrWhiteSpace(groupId))
            {
                fragment += "group:" + groupId + ";";
            }
            if (emails.Any())
            {
                fragment += "emails:" + String.Join(",", emails) + ";";
            }
            fragment += "req:" + reqId;
            form.NavigateFragment(fragment);
            form.ShowDialog();
        }
    }
}