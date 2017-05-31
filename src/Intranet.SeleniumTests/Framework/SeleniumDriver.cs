using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.PhantomJS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Intranet.SeleniumTests.Framework
{
    public enum Browser
    {
        PhantomJS,
        FireFox
    }

    public class SeleniumDriver
    {
        public static IWebDriver Driver;

        public static void Init(Browser browser)
        {
            switch(browser)
            {
                case Browser.PhantomJS:
                    {
                        Driver = new PhantomJSDriver();
                        break;
                    }
                case Browser.FireFox:
                    {
                        Driver = new FirefoxDriver();
                        break;
                    }
            }
        }

        public static void Kill()
        {
            Driver.Quit();
        }

    }
}
