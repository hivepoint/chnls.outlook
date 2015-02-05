﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using chnls.Forms;
using chnls.Service;

namespace chnls.Utils
{
    class WelcomeScreenHelper
    {
        internal static void ShowSplash()
        {
            if (PropertiesService.Instance.SplashAlreadyShown) return;

            var wap = new WebAppPopup((document) =>
            {

            })
            {
                StartPosition = FormStartPosition.CenterParent
            };
            wap.NavigateToNonAppPage("/splash/outlook");
            wap.ShowDialog();
            PropertiesService.Instance.SplashAlreadyShown = true;
        }
    }
}