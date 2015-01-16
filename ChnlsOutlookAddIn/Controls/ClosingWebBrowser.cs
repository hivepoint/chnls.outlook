using System.Windows.Forms;

namespace chnls.Controls
{
    class ClosingWebBrowser : WebBrowser
    {

        // Define constants from winuser.h
        // ReSharper disable InconsistentNaming
        private const int WM_PARENTNOTIFY = 0x210;
        private const int WM_DESTROY = 2;
        // ReSharper restore InconsistentNaming


        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_PARENTNOTIFY:
                    if (!DesignMode)
                    {
                        if (m.WParam.ToInt32() == WM_DESTROY)
                        {
                            // new Message { Msg = WM_DESTROY };
                            // Tell whoever cares we are closing
                            var parent = this.Parent as Form;
                            if (parent != null)
                                parent.Close();
                        }
                    }
                    DefWndProc(ref m);
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

    }
}
