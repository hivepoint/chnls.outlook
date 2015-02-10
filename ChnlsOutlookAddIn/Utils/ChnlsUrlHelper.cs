#region

using System;
using System.Web;
using chnls.Service;
using Newtonsoft.Json;

#endregion

namespace chnls.Utils
{
    internal class ChnlsUrlHelper
    {
        internal static string GetAppUrl()
        {
            var baseUrl = PropertiesService.Instance.BaseUrl;
            return (String.IsNullOrWhiteSpace(baseUrl)
                ? Constants.UrlChnlsProduction
                : baseUrl) + Constants.UrlSuffix + "&clientVersion=" +
                   typeof(ChnlsUrlHelper).Assembly.GetName().Version;
        }


        internal static bool IsEmailChannelsUrl(Uri uri)
        {
            return uri.Scheme.Equals("channels", StringComparison.OrdinalIgnoreCase);
        }

        internal static ChannelsRequest GetChnlsRequest(Uri uri)
        {
            if (!IsEmailChannelsUrl(uri)) return null;

            try
            {
                var queryString = uri.Query.TrimStart('?');

                var query = HttpUtility.ParseQueryString(queryString);

                var type = (ChannelsRequestType)Enum.Parse(typeof(ChannelsRequestType), uri.Host, true);
                switch (type)
                {
                    case ChannelsRequestType.ClientLoaded:
                    case ChannelsRequestType.ChannelListUpdated:
                    case ChannelsRequestType.ChannelGroupListUpdated:
                    case ChannelsRequestType.UserSignedOut:
                    case ChannelsRequestType.NewItemsAddedToFeed:
                        return new ChannelsRequest(type);
                    case ChannelsRequestType.ContentLoaded:
                        return new ChannelsRequestWithPlace(type, query["place"]);
                    case ChannelsRequestType.OpenWindow:
                        return new ChannelsRequestWithUrl(type, query["value"]);
                    case ChannelsRequestType.HandleMessageReply:
                        return new ChannelsRequestWithId(type, query["id"]);
                    case ChannelsRequestType.ChannelUpdated:
                        return new ChannelsRequestWithId(type, query["id"]);
                    case ChannelsRequestType.UserSignedIn:
                        return new ChannelsRequestWithEmailAndName(type, query["email"], query["name"]);
                    case ChannelsRequestType.ActionComplete:
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
                        return new ChannelsRequestActionComplete(type, query["action"], success, query["ActionType"],
                            query["response"]);
                    case ChannelsRequestType.OpenDialog:
                        return new ChannelRequestOpenDialog(type, query["type"], query["id"], query["value"],
                            query["width"], query["height"]);
                    case ChannelsRequestType.CloseDialog:
                        return new ChannelRequestCloseDialog(type, query["type"], query["id"], query["response"]);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (Exception ex)
            {
                LoggingService.Error("Error handling url:" + uri, ex);
                return null;
            }
        }
    }

    internal class ChannelsRequest
    {
        public ChannelsRequest(ChannelsRequestType type)
        {
            Type = type;
        }

        internal ChannelsRequestType Type { get; private set; }
    }

    internal class ChannelsRequestWithUrl : ChannelsRequest
    {
        public ChannelsRequestWithUrl(ChannelsRequestType type, string url)
            : base(type)
        {
            Url = url;
        }

        internal string Url { get; private set; }
    }

    internal class ChannelsRequestWithId : ChannelsRequest
    {
        public ChannelsRequestWithId(ChannelsRequestType type, string id)
            : base(type)
        {
            Id = id;
        }

        internal string Id { get; private set; }
    }

    internal class ChannelsRequestWithPlace : ChannelsRequest
    {
        public ChannelsRequestWithPlace(ChannelsRequestType type, string palce)
            : base(type)
        {
            Place = palce;
        }

        internal string Place { get; private set; }
    }

    internal class ChannelsRequestWithEmailAndName : ChannelsRequest
    {
        public ChannelsRequestWithEmailAndName(ChannelsRequestType type, string email, string name)
            : base(type)
        {
            Email = email;
            Name = name;
        }

        internal string Email { get; private set; }
        internal string Name { get; private set; }
    }

    internal class ChannelsRequestActionComplete : ChannelsRequest
    {
        public ChannelsRequestActionComplete(ChannelsRequestType type, string action, bool success, string actionType,
            string resposne)
            : base(type)
        {
            Action = action;
            Success = success;
            ActionType = actionType;
            Resposne = resposne;
        }

        internal string Action { get; private set; }
        internal bool Success { get; private set; }
        internal string ActionType { get; private set; }
        internal string Resposne { get; private set; }
    }


    internal class ChannelRequestOpenDialog : ChannelsRequest
    {
        public ChannelRequestOpenDialog(ChannelsRequestType type, string dialogType, string id, string token,
            string suggestedWidth, String suggestedHeight)
            : base(type)
        {
            DialogType = dialogType;
            Id = id;
            Token = token;
            try
            {
                SuggestedHeight = int.Parse(suggestedHeight);
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch (Exception)
            {
                SuggestedHeight = 0;
            }

            try
            {
                SuggestedWidth = int.Parse(suggestedWidth);
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch (Exception)
            {
                SuggestedWidth = 0;
            }
        }

        internal string DialogType { get; private set; }
        internal string Id { get; private set; }
        internal string Token { get; private set; }
        internal int SuggestedWidth { get; private set; }
        internal int SuggestedHeight { get; private set; }

        internal DialogTypeEnum DialogTypeEnum
        {
            get
            {
                return (DialogTypeEnum)Enum.Parse(typeof(DialogTypeEnum), DialogType, true);
            }
        }

    }

    internal class ChannelRequestCloseDialog : ChannelsRequest
    {
        public ChannelRequestCloseDialog(ChannelsRequestType type, string dialogType, string id, string response)
            : base(type)
        {
            DialogType = dialogType;
            Id = id;
            Response = response;
        }

        internal string DialogType { get; private set; }
        internal string Id { get; private set; }
        internal string Response { get; private set; }
        internal DialogTypeEnum DialogTypeEnum
        {
            get
            {
                return (DialogTypeEnum)Enum.Parse(typeof(DialogTypeEnum), DialogType, true);
            }
        }

        internal T GetResponse<T>()
        {
            return String.IsNullOrWhiteSpace(Response) ? default(T) : JsonConvert.DeserializeObject<T>(Response);
        }
    }

    internal enum ChannelsRequestType
    {
        ClientLoaded,
        OpenWindow,
        ContentLoaded,
        ChannelListUpdated,
        ChannelUpdated,
        ChannelGroupListUpdated,
        NewItemsAddedToFeed,
        HandleMessageReply,
        ActionComplete,
        UserSignedIn,
        UserSignedOut,
        OpenDialog,
        CloseDialog
    }
    internal enum DialogTypeEnum
    {
        // ReSharper disable InconsistentNaming
        CREATE_CHANNEL,
        CREATE_GROUP
        // ReSharper restore InconsistentNaming
    }

}