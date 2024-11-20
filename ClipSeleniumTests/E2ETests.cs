using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;

namespace ClipSeleniumTests
{
    public class E2ETests : IDisposable
    {
        private IWebDriver _driver;
        public E2ETests()
        {
            new DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser);
            _driver = new ChromeDriver();
        }

        [Fact]
        public void UploadImage_Then_DownloadProcessedImage()
        {
            _driver.Navigate().GoToUrl("https://clip-762c7.web.app/");

            IWebElement uploadButton = _driver.FindElement(By.Id("select-image"));
            uploadButton.Click();

            IWebElement uploadImage = _driver.FindElement(By.Id("fileInput"));

            string imageFilePath = @"gym-review-1.png";
            string fullImagePath = Path.GetFullPath(imageFilePath);

            uploadImage.SendKeys(fullImagePath);

            IWebElement clipImageButton = _driver.FindElement(By.Id("clip-image"));
            clipImageButton.Click();


            // Wait for the processed image to appear on the page
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);
            IWebElement processedImage = _driver.FindElement(By.Id("processed-image"));
            processedImage.Click();

            //Assert

            Assert.True(uploadButton.Displayed);
            Assert.True(clipImageButton.Displayed);
            Assert.True(processedImage.Displayed);
            



        }

        public void Dispose()
        {
            _driver.Quit();
        }
    }
}
