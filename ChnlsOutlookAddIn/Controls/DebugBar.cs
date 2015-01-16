#region

using System;
using System.Windows.Forms;
using chnls.ADXForms;
using chnls.Service;

#endregion

namespace chnls.Controls
{
    /*
     * This control can be exposed at the bottom of the sidebar to allow for some debugging
     */

    public partial class DebugBar : UserControl
    {
        public DebugBar()
        {
            InitializeComponent();
        }

        public ADXOlFormExplorerSidebar Browser { get; set; }


        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            Browser.RefreshBrowser();
        }

        private void buttonGo_Click(object sender, EventArgs e)
        {
            var prompt = new Form {Width = 500, Height = 150, Text = @"URL"};
            var textLabel = new Label {Left = 50, Top = 20, Text = @"URL:"};
            var textBox = new TextBox {Left = 50, Top = 50, Width = 400};
            var confirmation = new Button {Text = @"Ok", Left = 350, Width = 100, Top = 70};

            confirmation.Click += (senderPrompt, ePrompt) => prompt.Close();
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(textBox);
            prompt.ShowDialog();
            Browser.NavigateTo(textBox.Text);
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            PropertiesService.Instance.DebugPanelVisible = false;
        }

        private void btnDump_Click(object sender, EventArgs e)
        {
            Browser.Dump();
        }
    }
}