using Intranet.SeleniumTests.Framework;
using Microsoft.DotNet.PlatformAbstractions;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Remote;
using System;
using Xunit;

namespace Intranet.SeleniumTests
{
    public class SeleniumBasics : IDisposable
    {

        public SeleniumBasics()
        {
            //Setup
            SeleniumDriver.Init(Browser.PhantomJS);
        }

        public void Dispose()
        {
            //Teardown
            SeleniumDriver.Kill();
        }


        [Theory]
        [InlineData("http://www.certaincy.com/", "Certaincy")]
        [InlineData("http://www.google.com", "Google")]
        [InlineData("http://localhost:5000", "Intranet")]
        public void SeleniumCommunicationOK(string url, string keyword)
        {
            SeleniumNavigate.GoToUrl(url);
            Assert.True(SeleniumDriver.Driver.Title.Contains(keyword));
        }
    }
}
