#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using chnls.Service;

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


        internal static bool IsHivePointUrl(Uri uri)
        {
            return uri.Scheme.Equals("channels", StringComparison.OrdinalIgnoreCase);
        }

        internal static ChannelsRequest GetChnlsRequest(Uri uri)
        {
            if (!IsHivePointUrl(uri)) return null;

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
                    case ChannelsRequestType.CloseWindow:
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
                    case ChannelsRequestType.HandleCreateChannel:
                        return new ChannelsRequestWithGroupAndEmails(type, query["group"], query["emails"]);
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
                        return new ChannelsRequestActionComplete(type, query["action"], success, query["ActionType"], query["response"]);
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

    internal class ChannelsRequestWithGroupAndEmails : ChannelsRequest
    {
        public ChannelsRequestWithGroupAndEmails(ChannelsRequestType type, string group, string emails)
            : base(type)
        {
            Group = group;
            Emails = new List<string>();
            if (!String.IsNullOrWhiteSpace(emails))
            {
                var addresses = emails.Split(',');
                Emails.AddRange(addresses.Where(e => !String.IsNullOrWhiteSpace(e)));
            }
        }

        internal string Group { get; private set; }
        internal List<string> Emails { get; private set; }
    }
    internal class ChannelsRequestActionComplete : ChannelsRequest
    {
        public ChannelsRequestActionComplete(ChannelsRequestType type, string action, bool success, string actionType, string resposne)
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
    internal enum ChannelsRequestType
    {
        ClientLoaded,
        OpenWindow,
        CloseWindow,
        ContentLoaded,
        ChannelListUpdated,
        ChannelUpdated,
        ChannelGroupListUpdated,
        NewItemsAddedToFeed,
        HandleMessageReply,
        ActionComplete,
        UserSignedIn,
        UserSignedOut,
        HandleCreateChannel
    }
}