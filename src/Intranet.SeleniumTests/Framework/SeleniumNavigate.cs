using System;
using System.Collections.Generic;
using System.Text;

namespace Intranet.SeleniumTests.Framework
{
    class SeleniumNavigate
    {
        public static void GoToUrl(string url)
        {
            SeleniumDriver.Driver.Navigate().GoToUrl(url);
        }

        public static void GoBack()
        {
            SeleniumDriver.Driver.Navigate().Back();
        }

        public static void GoForward()
        {
            SeleniumDriver.Driver.Navigate().Forward();
        }

        public static void Refresh()
        {
            SeleniumDriver.Driver.Navigate().Refresh();
        }
    }
}
