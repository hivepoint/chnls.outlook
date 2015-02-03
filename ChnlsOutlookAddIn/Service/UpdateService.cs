#region

using System;
using System.IO;
using System.Xml.Linq;
using chnls.Utils;

#endregion

namespace chnls.Service
{
    partial class UpdateService
    {
        public static UpdateService Instance = new UpdateService();
    }

    partial class UpdateService
    {
        private bool _updateAvailable;

        private UpdateService()
        {
        }

        public String UpdateLocation { private set; get; }

        public bool IsUpdateAvailable
        {
            get { return _updateAvailable; }
            private set
            {
                if (!_updateAvailable && value)
                {
                    OnUpdateAvailable();
                }
                _updateAvailable = value;
            }
        }

        public event EventHandler UpdateAvailable;

        protected void OnUpdateAvailable()
        {
            LoggingService.Info("OnUpdateAvailable");
            var handler = UpdateAvailable;
            if (null != handler)
            {
                handler(this, null);
            }
        }

        public void Start()
        {
            if (!AddinModule.CurrentInstance.IsNetworkDeployed()) return;
            InitUpdateLocation();
            SilentCheck(3);
        }

        private void InitUpdateLocation()
        {
            try
            {
                var rawLocation = GetType().Assembly.Location;
                var directory = Path.GetDirectoryName(rawLocation);
                if (null == directory)
                {
                    LoggingService.Error("No directory at: " + rawLocation);
                    return;
                }
                var installManifestFile = Path.Combine(directory, "appconfig.xml");
                try
                {
                    LoggingService.Info("Loading install manifest: " + installManifestFile);
                    var installManifest = XDocument.Load(installManifestFile);
                    if (null == installManifest.Root)
                    {
                        LoggingService.Error("Empty root: " + installManifestFile);

                        return;
                    }
                    foreach (var element in installManifest.Root.Descendants())
                    {
                        if (!"application".Equals(element.Name.LocalName)) continue;

                        var versionAttr = element.Attribute("providerUrl");
                        if (null != versionAttr)
                        {
                            UpdateLocation = versionAttr.Value;
                        }
                        else
                        {
                            LoggingService.Error("Error loading install manifect, version missing: " +
                                                 installManifestFile);
                        }
                        return;
                    }
                }
                catch (Exception ex)
                {
                    LoggingService.Error("Error loading install manifect: " + installManifestFile, ex);
                }
            }
            finally
            {
                if (String.IsNullOrWhiteSpace(UpdateLocation))
                {
                    UpdateLocation = "https://downloads.emailchannels.com/outlook/";
                    LoggingService.Error("Update location unset, setting to default: '" + UpdateLocation + "'");
                }
            }
        }

        private void SilentCheck(int delaySec)
        {
            if (!IsUpdateAvailable)
            {
                var delayMs = delaySec * 1000;
                if (delayMs <= 0)
                {
                    delayMs = 100;
                }
                Scheduler.Run("Check for update", () =>
                {
                    try
                    {
                        LoggingService.Info("Checking for update: " + UpdateLocation);
                        var manifest = XDocument.Load(UpdateLocation);
                        if (manifest.Root == null)
                        {
                            LoggingService.Error("Update root is null: " + UpdateLocation);
                            return;
                        }

                        foreach (var element in manifest.Root.Descendants())
                        {
                            if (!"assemblyIdentity".Equals(element.Name.LocalName)) continue;

                            var versionAttr = element.Attribute("version");
                            if (null != versionAttr)
                            {
                                var versionStr = versionAttr.Value;
                                var serverVersion = new Version(versionStr);

                                if (GetType().Assembly.GetName().Version < serverVersion)
                                {
                                    LoggingService.Info("Update available: " + serverVersion);
                                    IsUpdateAvailable = true;
                                    OnUpdateAvailable();
                                }
                            }
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        LoggingService.Error("Exception: " + ex);
                    }
                    finally
                    {
                        if (!IsUpdateAvailable)
                        {
                            SilentCheck(60 * 60);
                        }
                    }
                }, delayMs);
            }
        }

        public void CheckAndUpdate()
        {
            AddinModule.CurrentInstance.CheckForUpdates();
        }
    }
}