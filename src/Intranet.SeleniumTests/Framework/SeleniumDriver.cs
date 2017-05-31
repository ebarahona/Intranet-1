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
        public static IWebDriver driver;

        public static void Init(Browser browser)
        {
            switch(browser)
            {
                case Browser.PhantomJS:
                    {
                        driver = new PhantomJSDriver();
                        break;
                    }
                case Browser.FireFox:
                    {
                        driver = new FirefoxDriver();
                        break;
                    }
            }
        }

        public static void Kill()
        {
            driver.Quit();
        }

    }
}
