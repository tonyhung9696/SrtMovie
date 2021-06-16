using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace SrtMovie
{
    public class controlWeb
    {
        private string _url = "https://translate.google.com/?hl=zh-TW&tab=wT#view=home&op=translate&sl=auto&tl=zh-TW&text=";
        IWebDriver driver = null;
        public bool elementshow = true;
        internal GoogleTran g;
        public controlWeb()
        {
            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            //service.HideCommandPromptWindow = true;
            var driverOptions = new ChromeOptions();
            //driverOptions.AddArgument("headless");
            driver = new ChromeDriver(service, driverOptions);
            driver.Navigate().GoToUrl(_url);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(10000);
            check();
        }

        public void check()
        {
            try
            {
                var a = driver.FindElement(By.XPath("//textarea[contains(@id,'source')]"));
                g = new googleTran1(driver);
            }
            catch (Exception ex)
            {
                g = new googleTran2(driver);
                elementshow = false;
            }
        }
        

        public void close()
        {
            driver.Close();
            driver.Quit();
            driver.Dispose();
        }
    }
}
