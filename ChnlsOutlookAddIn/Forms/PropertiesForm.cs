#region

using System;
using System.Windows.Forms;
using chnls.Service;

#endregion

namespace chnls.Forms
{
    public partial class PropertiesForm : Form
    {
        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        private bool _initialized; // must not be readonly because it stops events from firing during initialization

        public PropertiesForm()
        {
            InitializeComponent();
            var baseUrl = PropertiesService.Instance.BaseUrl;
            radioButtonChnlsIO.Checked = true;
            switch (baseUrl)
            {
                case Constants.UrlChnlsBeta:
                    radioButtonChnlsUS.Checked = true;
                    break;
                case Constants.UrlChnlsDev:
                    radioButtonChnlsDEV.Checked = true;
                    break;
            }
            _initialized = true;
        }

        private void buttonDebug_Click(object sender, EventArgs e)
        {
            PropertiesService.Instance.DebugPanelVisible = true;
        }

        private void radioButtonChnlsIO_CheckedChanged(object sender, EventArgs e)
        {
            if (!_initialized || !((RadioButton) sender).Checked)
                return;

            PropertiesService.Instance.BaseUrl = Constants.UrlChnlsProduction;
        }

        private void radioButtonChnlsUS_CheckedChanged(object sender, EventArgs e)
        {
            if (!_initialized || !((RadioButton) sender).Checked)
                return;

            PropertiesService.Instance.BaseUrl = Constants.UrlChnlsBeta;
        }

        private void radioButtonChnlsDEV_CheckedChanged(object sender, EventArgs e)
        {
            if (!_initialized || !((RadioButton) sender).Checked)
                return;

            PropertiesService.Instance.BaseUrl = Constants.UrlChnlsDev;
        }
    }
}