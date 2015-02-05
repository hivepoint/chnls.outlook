#region

using System;
using chnls.Model;

#endregion

namespace chnls.Service
{
    partial class PropertiesService
    {
        public static PropertiesService Instance = new PropertiesService();


        private readonly object _propertiesLock = new object();
        private bool _debugVisible;

        private PropertiesService()
        {
            LoadProperties();
        }

        public bool DebugPanelVisible
        {
            get { return _debugVisible; }
            set
            {
                _debugVisible = value;
                OnDebugVisibleChanged();
            }
        }

        public bool Connected { get; set; }
    }

    partial class PropertiesService
    {
        // User settings
        private ChnlsProperties Properties { get; set; }
        private UserProperties CurrentUserProperties { get; set; }

        public bool SignedIn
        {
            get
            {
                lock (_propertiesLock)
                {
                    return !String.IsNullOrWhiteSpace(Properties.CurrentUser);
                }
            }
        }

        public string BaseUrl
        {
            get { return Properties.BaseUrl; }
            set
            {
                lock (_propertiesLock)
                {
                    if (String.Equals(Properties.BaseUrl, value))
                    {
                        return;
                    }
                    Properties.BaseUrl = value;
                    Properties.CurrentUser = null;
                    Properties.UserProperties.Clear();
                }
                PropertiesDirty();
                OnChannelListChanged();
                OnGroupListChanged();
                OnUserChanged();
                OnBaseUrlChanged();
            }
        }

        public string UserEmail
        {
            get
            {
                lock (_propertiesLock)
                {
                    return SignedIn ? Properties.CurrentUser : null;
                }
            }
            set
            {
                lock (_propertiesLock)
                {
                    if (String.Equals(value, Properties.CurrentUser, StringComparison.InvariantCultureIgnoreCase))
                        return;
                    value = value.ToLowerInvariant();
                    Properties.CurrentUser = value;
                    UpdateCurrentUserProperties();
                    PropertiesDirty();
                    OnUserChanged();
                }
            }
        }

        public bool SplashAlreadyShown
        {
            get { return Properties.SplashAlreadyShown; }
            set
            {
                if (Properties.SplashAlreadyShown == value) return;
                Properties.SplashAlreadyShown = value;
                PropertiesDirty();
            }
        }

        private void UpdateCurrentUserProperties()
        {
            lock (_propertiesLock)
            {
                if (String.IsNullOrWhiteSpace(Properties.CurrentUser))
                {
                    CurrentUserProperties = null;
                }
                else
                {
                    if (!Properties.UserProperties.ContainsKey(Properties.CurrentUser))
                    {
                        CurrentUserProperties = new UserProperties {EmailAddress = Properties.CurrentUser};
                        Properties.UserProperties[Properties.CurrentUser] = CurrentUserProperties;
                    }
                    else
                    {
                        CurrentUserProperties = Properties.UserProperties[Properties.CurrentUser];
                    }
                }
            }
        }

        internal void ResetToDefaults()
        {
            lock (_propertiesLock)
            {
                Properties = new ChnlsProperties();
            }
            PropertiesDirty();
            OnChannelListChanged();
            OnGroupListChanged();
            OnUserChanged();
            OnBaseUrlChanged();
        }

        public event EventHandler BaseUrlChanged;
        public event EventHandler UserChanged;
        public event EventHandler DebugVisibleChanged;

        #region events

        protected void OnUserChanged()
        {
            LoggingService.Debug("OnUserChanged");
            var handler = UserChanged;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }
        }

        protected void OnBaseUrlChanged()
        {
            LoggingService.Debug("OnBaseUrlChanged");
            var handler = BaseUrlChanged;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }
        }

        protected void OnDebugVisibleChanged()
        {
            LoggingService.Debug("OnDebugVisibleChanged");
            var handler = DebugVisibleChanged;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }
        }

        #endregion
    }

    public class OfflineException : Exception
    {
    }

    public class NotSignedInException : Exception
    {
    }
}