using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;


namespace chnls.Controls
{
    /*
     * http://stackoverflow.com/questions/10362988/treeview-flickering
     */
    class BufferedTreeView : TreeView
    {
        protected override void OnHandleCreated(EventArgs e)
        {
            // ReSharper disable once RedundantThisQualifier
            SendMessage(this.Handle, TVM_SETEXTENDEDSTYLE, (IntPtr)TVS_EX_DOUBLEBUFFER, (IntPtr)TVS_EX_DOUBLEBUFFER);
            base.OnHandleCreated(e);
        }
        // Pinvoke:
        // ReSharper disable InconsistentNaming
        private const int TVM_SETEXTENDEDSTYLE = 0x1100 + 44;
        // ReSharper disable once UnusedMember.Local
        private const int TVM_GETEXTENDEDSTYLE = 0x1100 + 45;
        private const int TVS_EX_DOUBLEBUFFER = 0x0004;
        // ReSharper restore InconsistentNaming
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);
    }
}
