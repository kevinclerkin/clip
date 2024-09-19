using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace ClipSeleniumTests
{
    public class E2ETest : IDisposable
    {
        private IWebDriver _driver;

        public E2ETest()
        {
            _driver = new ChromeDriver();
            _driver.Manage().Window.Maximize();
        }
        
        [Fact]
        public void DisplayCorrectTitle_On_NavigateToHomePage()
        {
            _driver.Navigate().GoToUrl("https://clip-762c7.web.app/");

            Assert.Equal("Clip", _driver.Title);

        }

        public void Dispose()
        {
            _driver.Quit();
        }
        
    }
}
