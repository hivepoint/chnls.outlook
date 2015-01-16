#region

using System;
using chnls.Model;

#endregion

namespace chnls.Service
{
    partial class PropertiesService
    {
        public static PropertiesService Instance = new PropertiesService();
        public bool DebugPanelVisible { get; set; }
        public bool Connected { get; set; }

        private PropertiesService()
        {
            LoadProperties();
        }
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
                lock (Properties)
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
                lock (Properties)
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

        public bool IsSignedIn
        {
            get { return String.IsNullOrWhiteSpace(UserEmail); }
        }

        public string UserEmail
        {
            get
            {
                lock (Properties)
                {
                    return SignedIn ? Properties.CurrentUser : null;
                }
            }
            set
            {
                lock (Properties)
                {
                    if (String.Equals(value, Properties.CurrentUser, StringComparison.InvariantCultureIgnoreCase))
                        return;
                    value = value.ToLowerInvariant();
                    Properties.CurrentUser = value;
                    if (Properties.UserProperties.ContainsKey(value))
                    {
                        CurrentUserProperties = new UserProperties { EmailAddress = value };
                        Properties.UserProperties[value] = CurrentUserProperties;
                    }
                    else
                    {
                        CurrentUserProperties = Properties.UserProperties[value];
                    }
                    PropertiesDirty();
                    OnUserChanged();
                }
            }
        }

        public void AssertSignedIn()
        {
            if (!SignedIn)
            {
                throw new NotSignedInException();
            }
        }

        internal void ResetToDefaults()
        {
            lock (Properties)
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