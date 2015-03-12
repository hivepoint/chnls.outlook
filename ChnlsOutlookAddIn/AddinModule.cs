#region

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using AddinExpress.MSO;
using AddinExpress.OL;
using chnls.Forms;
using chnls.Service;
using chnls.Utils;
using Microsoft.Office.Interop.Outlook;
using Application = System.Windows.Forms.Application;

#endregion

namespace chnls
{
    /// <summary>
    ///     Add-in Express Add-in Module
    /// </summary>
    [Guid("45D69442-87C2-47B7-9AF9-10A2EE0AFDDD"), ProgId("ChnlsOutlookAddIn.AddinModule")]
    public partial class AddinModule : ADXAddinModule
    {
        // ReSharper disable InconsistentNaming
        private ADXOlFormsManager adxOlFormsManager;
        private ADXRibbonButton adxRibbonButtonAbout;
        private ADXRibbonButton adxRibbonButtonAddToChannel;
        private ADXRibbonButton adxRibbonButtonFeedback;
        private ADXRibbonGroup adxRibbonGroupChnlsExplorer;
        private ADXRibbonTab adxRibbonTabChnlsExplorer;
        private ADXOlFormsCollectionItem composerSidebar;
        private ADXOlFormsCollectionItem explorerSidebar;
        private ImageList imageListLarge;
        private ImageList imageListSmall;
        private ADXOutlookAppEvents adxOutlookEvents;
        private readonly Timer _updateTimer = new Timer() { Interval = 30000 };
        // ReSharper restore InconsistentNaming

        #region Component Designer generated code

        /// <summary>
        ///     Required by designer
        /// </summary>
        private System.ComponentModel.IContainer components;

        /// <summary>
        ///     Required by designer support - do not modify
        ///     the following method
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddinModule));
            this.adxRibbonTabChnlsExplorer = new AddinExpress.MSO.ADXRibbonTab(this.components);
            this.adxRibbonGroupChnlsExplorer = new AddinExpress.MSO.ADXRibbonGroup(this.components);
            this.adxRibbonButtonAddToChannel = new AddinExpress.MSO.ADXRibbonButton(this.components);
            this.imageListLarge = new System.Windows.Forms.ImageList(this.components);
            this.adxRibbonButtonFeedback = new AddinExpress.MSO.ADXRibbonButton(this.components);
            this.adxRibbonButtonAbout = new AddinExpress.MSO.ADXRibbonButton(this.components);
            this.imageListSmall = new System.Windows.Forms.ImageList(this.components);
            this.adxOlFormsManager = new AddinExpress.OL.ADXOlFormsManager(this.components);
            this.explorerSidebar = new AddinExpress.OL.ADXOlFormsCollectionItem(this.components);
            this.composerSidebar = new AddinExpress.OL.ADXOlFormsCollectionItem(this.components);
            this.adxOutlookEvents = new AddinExpress.MSO.ADXOutlookAppEvents(this.components);
            // 
            // adxRibbonTabChnlsExplorer
            // 
            this.adxRibbonTabChnlsExplorer.Caption = "Chnls";
            this.adxRibbonTabChnlsExplorer.Controls.Add(this.adxRibbonGroupChnlsExplorer);
            this.adxRibbonTabChnlsExplorer.Id = "adxRibbonTab_9427ad8b10104d568799b81e128112aa";
            this.adxRibbonTabChnlsExplorer.IdMso = "TabMail";
            this.adxRibbonTabChnlsExplorer.Ribbons = AddinExpress.MSO.ADXRibbons.msrOutlookExplorer;
            // 
            // adxRibbonGroupChnlsExplorer
            // 
            this.adxRibbonGroupChnlsExplorer.Caption = "Channels.cc";
            this.adxRibbonGroupChnlsExplorer.Controls.Add(this.adxRibbonButtonAddToChannel);
            this.adxRibbonGroupChnlsExplorer.Controls.Add(this.adxRibbonButtonFeedback);
            this.adxRibbonGroupChnlsExplorer.Controls.Add(this.adxRibbonButtonAbout);
            this.adxRibbonGroupChnlsExplorer.Id = "adxRibbonGroup_05815a0dbfb249cc98cf9eddf6046edb";
            this.adxRibbonGroupChnlsExplorer.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.adxRibbonGroupChnlsExplorer.Ribbons = AddinExpress.MSO.ADXRibbons.msrOutlookExplorer;
            // 
            // adxRibbonButtonAddToChannel
            // 
            this.adxRibbonButtonAddToChannel.Caption = "Add to channel";
            this.adxRibbonButtonAddToChannel.Id = "adxRibbonButton_03bec1d14135451aa515a5126c355205";
            this.adxRibbonButtonAddToChannel.Image = 0;
            this.adxRibbonButtonAddToChannel.ImageList = this.imageListLarge;
            this.adxRibbonButtonAddToChannel.ImageTransparentColor = System.Drawing.Color.White;
            this.adxRibbonButtonAddToChannel.Ribbons = AddinExpress.MSO.ADXRibbons.msrOutlookExplorer;
            this.adxRibbonButtonAddToChannel.Size = AddinExpress.MSO.ADXRibbonXControlSize.Large;
            this.adxRibbonButtonAddToChannel.OnClick += new AddinExpress.MSO.ADXRibbonOnAction_EventHandler(this.adxRibbonButtonAddToChannel_OnClick);
            // 
            // imageListLarge
            // 
            this.imageListLarge.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListLarge.ImageStream")));
            this.imageListLarge.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListLarge.Images.SetKeyName(0, "square");
            this.imageListLarge.Images.SetKeyName(1, "square_dark");
            // 
            // adxRibbonButtonFeedback
            // 
            this.adxRibbonButtonFeedback.Caption = "Feedback";
            this.adxRibbonButtonFeedback.Id = "adxRibbonButton_e59bae8d653f401496334f2611f7005e";
            this.adxRibbonButtonFeedback.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.adxRibbonButtonFeedback.Ribbons = AddinExpress.MSO.ADXRibbons.msrOutlookExplorer;
            this.adxRibbonButtonFeedback.OnClick += new AddinExpress.MSO.ADXRibbonOnAction_EventHandler(this.adxRibbonButtonFeedback_OnClick);
            // 
            // adxRibbonButtonAbout
            // 
            this.adxRibbonButtonAbout.Caption = "About";
            this.adxRibbonButtonAbout.Id = "adxRibbonButton_8d578f934fb444cca1a8e169d347d49e";
            this.adxRibbonButtonAbout.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.adxRibbonButtonAbout.Ribbons = AddinExpress.MSO.ADXRibbons.msrOutlookExplorer;
            this.adxRibbonButtonAbout.OnClick += new AddinExpress.MSO.ADXRibbonOnAction_EventHandler(this.adxRibbonButtonAbout_OnClick);
            // 
            // imageListSmall
            // 
            this.imageListSmall.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListSmall.ImageStream")));
            this.imageListSmall.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListSmall.Images.SetKeyName(0, "fav.png");
            // 
            // adxOlFormsManager
            // 
            this.adxOlFormsManager.Items.Add(this.explorerSidebar);
            this.adxOlFormsManager.Items.Add(this.composerSidebar);
            this.adxOlFormsManager.SetOwner(this);
            // 
            // explorerSidebar
            // 
            this.explorerSidebar.AlwaysShowHeader = true;
            this.explorerSidebar.Cached = AddinExpress.OL.ADXOlCachingStrategy.OneInstanceForAllFolders;
            this.explorerSidebar.ExplorerAllowedDropRegions = AddinExpress.OL.ADXOlExplorerAllowedDropRegions.DockRight;
            this.explorerSidebar.ExplorerItemTypes = AddinExpress.OL.ADXOlExplorerItemTypes.olMailItem;
            this.explorerSidebar.ExplorerLayout = AddinExpress.OL.ADXOlExplorerLayout.DockRight;
            this.explorerSidebar.FormClassName = "chnls.ADXForms.ADXOlFormExplorerSidebar";
            this.explorerSidebar.IsHiddenStateAllowed = false;
            this.explorerSidebar.UseOfficeThemeForBackground = true;
            // 
            // composerSidebar
            // 
            this.composerSidebar.Cached = AddinExpress.OL.ADXOlCachingStrategy.None;
            this.composerSidebar.FormClassName = "chnls.ADXForms.ADXOlFormComposeSidebar";
            this.composerSidebar.InspectorAllowedDropRegions = AddinExpress.OL.ADXOlInspectorAllowedDropRegions.RightSubpane;
            this.composerSidebar.InspectorItemTypes = AddinExpress.OL.ADXOlInspectorItemTypes.olMail;
            this.composerSidebar.InspectorLayout = AddinExpress.OL.ADXOlInspectorLayout.RightSubpane;
            this.composerSidebar.InspectorMode = AddinExpress.OL.ADXOlInspectorMode.Compose;
            this.composerSidebar.IsHiddenStateAllowed = false;
            this.composerSidebar.UseOfficeThemeForBackground = true;
            // 
            // adxOutlookEvents
            // 
            this.adxOutlookEvents.ExplorerSelectionChange += new AddinExpress.MSO.ADXOlExplorer_EventHandler(this.adxOutlookEvents_ExplorerSelectionChange);
            // 
            // AddinModule
            // 
            this.AddinName = "ChnlsOutlookAddIn";
            this.SupportedApps = AddinExpress.MSO.ADXOfficeHostApp.ohaOutlook;
            this.AddinInitialize += new AddinExpress.MSO.ADXEvents_EventHandler(this.AddinModule_AddinInitialize);

        }

        #endregion

        #region Add-in Express automatic code

        // Required by Add-in Express - do not modify
        // the methods within this region

        public override IContainer GetContainer()
        {
            if (components == null)
                components = new Container();
            return components;
        }

        [ComRegisterFunction]
        public static void AddinRegister(Type t)
        {
            ADXRegister(t);
        }

        [ComUnregisterFunction]
        public static void AddinUnregister(Type t)
        {
            ADXUnregister(t);
        }

        public override void UninstallControls()
        {
            base.UninstallControls();
        }

        #endregion

        public AddinModule()
        {
            Application.EnableVisualStyles();
            InitializeComponent();
            // Please add any initialization code to the AddinInitialize event handler
        }

        public new static AddinModule CurrentInstance
        {
            get { return ADXAddinModule.CurrentInstance as AddinModule; }
        }

        public _Application OutlookApp
        {
            get { return (HostApplication as _Application); }
        }

        private void adxRibbonButtonAbout_OnClick(object sender, IRibbonControl control, bool pressed)
        {
            new AboutChnlsForm().ShowDialog();
        }

        private void adxOutlookEvents_ExplorerSelectionChange(object sender, object explorer)
        {
            UpdateShareEnablement((Explorer)explorer);
        }



        private void adxRibbonButtonAddToChannel_OnClick(object sender, IRibbonControl control, bool pressed)
        {
            AddToChannels();
        }

        private void UpdateShareEnablement(Explorer explorer)
        {
            var mailItemSelected = explorer == null;

            if (explorer != null)
            {
                Selection selection = null;
                try
                {
                    try
                    {
                        selection = explorer.Selection;
                    }
                    // ReSharper disable once EmptyGeneralCatchClause
                    catch { }
                    if (null != selection)
                    {
                        for (var i = 0; i < selection.Count; i++)
                        {
                            object item = null;
                            try
                            {
                                item = selection[i + 1]; // COM 1 BASED
                                if (item is MailItem)
                                {
                                    mailItemSelected = true;
                                    break;
                                }
                            }
                            finally
                            {
                                if (null != item)
                                {
                                    Marshal.ReleaseComObject(item);
                                    // ReSharper disable once RedundantAssignment
                                    item = null;
                                }
                            }
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    LoggingService.Error("Error getting selection", ex);
                }
                finally
                {
                    if (null != selection)
                    {
                        Marshal.ReleaseComObject(selection);
                        // ReSharper disable once RedundantAssignment
                        selection = null;
                    }
                }
            }
            adxRibbonButtonAddToChannel.Enabled = PropertiesService.Instance.SignedIn && mailItemSelected;
        }

        private void AddinModule_AddinInitialize(object sender, EventArgs e)
        {
            UpdateShareEnablement();
            _updateTimer.Tick += _updateTimer_Tick;
            _updateTimer.Enabled = true;

            InitializeChnls();
        }


        void _updateTimer_Tick(object sender, EventArgs e)
        {
            UpdateShareEnablement();
        }

        private void UpdateShareEnablement()
        {
            Scheduler.RunIfNotScheduled("AddInModule UpdateTimerTick", "Update share enablemebnt", 200, () =>
            {
                var explorer = OutlookApp.ActiveExplorer();
                try
                {
                    UpdateShareEnablement(explorer);
                }
                finally
                {
                    if (null != explorer)
                    {
                        Marshal.ReleaseComObject(explorer);
                    }
                }
            });
        }

        private void adxRibbonButtonFeedback_OnClick(object sender, IRibbonControl control, bool pressed)
        {
            ContactSupport();
        }
    }
}