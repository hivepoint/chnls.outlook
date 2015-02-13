#region

using System.Windows.Forms;
using chnls.Forms;
using chnls.Service;

#endregion

namespace chnls.Utils
{
    internal class WelcomeScreenHelper
    {
        internal static void ShowSplash()
        {
            if (PropertiesService.Instance.SplashAlreadyShown) return;

            var wap = new WebAppPopup
            {
                StartPosition = FormStartPosition.CenterParent
            };
            wap.NavigateFragment(Constants.SplashFragment);
            wap.ShowDialog();
            PropertiesService.Instance.SplashAlreadyShown = true;
        }
    }
}