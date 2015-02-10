#region

using chnls.Forms;
using chnls.Model;
using chnls.Service;

#endregion

namespace chnls.Utils
{
    internal class DialogHelper
    {
        internal static void OpenDialog(ChannelRequestOpenDialog openDialogRequest)
        {
            var form = WebAppPopup.GetInstance(request =>
            {
                switch (request.Type)
                {
                    case ChannelsRequestType.CloseDialog:
                        var closeRequest = (ChannelRequestCloseDialog) request;
                        switch (closeRequest.DialogTypeEnum)
                        {
                            case DialogTypeEnum.CREATE_CHANNEL:
                            case DialogTypeEnum.CREATE_GROUP:
                                var dialogResponse = closeRequest.GetResponse<CreateChannelDialogResponse>();
                                if (null != dialogResponse)
                                {
                                    PropertiesService.Instance.NotifyChannelCreated(dialogResponse.channel,
                                        dialogResponse.group);
                                }
                                break;
                        }
                        break;
                }
                return false;
            }, document =>
            {
                switch (openDialogRequest.DialogTypeEnum)
                {
                    case DialogTypeEnum.CREATE_GROUP:
                    case DialogTypeEnum.CREATE_CHANNEL:
                        PropertiesService.Instance.NotifyChannelRefresh();
                        break;
                }
            });

            form.NavigateFragment(openDialogRequest.Token);
            form.SetContentSize(openDialogRequest.SuggestedHeight, openDialogRequest.SuggestedWidth);
            form.ShowDialog();
        }
    }
}