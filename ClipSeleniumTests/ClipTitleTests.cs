using OpenQA.Selenium.Chrome;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;

namespace ClipSeleniumTests
{
    public class ClipTitleTests
    {
        [Fact]
        public void DisplayCorrectTitle_On_NavigateToHomePage()
        {
            new DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser);
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("--headless");
            
            using var driver = new ChromeDriver(chromeOptions);

            driver.Navigate().GoToUrl("https://clip-762c7.web.app/");
            
            Assert.Equal("Clip", driver.Title);
        }
        
    }
}
