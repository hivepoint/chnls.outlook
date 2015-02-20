#region

using System;
using System.Windows.Forms;

#endregion

namespace chnls.Controls
{
    public partial class UpgradePanel : UserControl
    {
        public UpgradePanel()
        {
            InitializeComponent();
        }

        private void btnUpgrade_Click(object sender, EventArgs e)
        {
            AddinModule.CurrentInstance.CheckForUpdates();
        }
    }
}