#region

using System.Collections.Generic;
using System.ComponentModel;

#endregion

namespace chnls.Model
{
    internal class ChnlsProperties
    {
        private readonly Dictionary<string, UserProperties> _userProperties = new Dictionary<string, UserProperties>();

        public ChnlsProperties()
        {
            BaseUrl = Constants.UrlChnlsProduction;
        }

        public Dictionary<string, UserProperties> UserProperties
        {
            get { return _userProperties; }
        }

        public string CurrentUser { get; set; }

        [DefaultValue(Constants.UrlChnlsProduction)]
        public string BaseUrl { get; set; }

        [DefaultValue(false)]
        public bool SplashAlreadyShown { get; set; }
    }
}