using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
namespace SrtMovie
{
    public class Elements
    {
        public string word = "";
        IWebDriver driver = null;
        public Elements(IWebDriver d)
        {
            driver = d;
        }
        public int countXpath(string s)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(1));
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath(s)));
                int count = driver.FindElements(By.XPath(s)).Count();
                var a = driver.FindElements(By.XPath(s));
                return count;
            }
            catch (NoSuchElementException) { return 0; }
            catch (WebDriverTimeoutException) { return 0; }
            catch (TimeoutException) { return 0; }

        }

        public void ClickXpath(string s)
        {
            var v = driver.FindElements(By.XPath(s));
            foreach (IWebElement IW in v)
            {
                IJavaScriptExecutor executor = (IJavaScriptExecutor)driver;
                executor.ExecuteScript("arguments[0].click();", IW);
            }
        }

        public List<string> GetXpath(string s)
        {
            List<string> result = new List<string>();
            var v = driver.FindElements(By.XPath(s));
            try
            {
                foreach (IWebElement IW in v)
                {
                    result.Add(IW.Text);
                    //MessageBox.Show(IW.Text);
                }
            }
            catch { }
            return result;

        }
        public static List<int> AllIndexesOf(string str, string value)
        {
            if (String.IsNullOrEmpty(value))
                throw new ArgumentException("the string to find may not be empty", "value");
            List<int> indexes = new List<int>();
            for (int index = 0; ; index += value.Length)
            {
                index = str.IndexOf(value, index);
                if (index == -1)
                    return indexes;
                indexes.Add(index);
            }
        }
        public void close()
        {
            driver.Quit();
        }
    }
    interface GoogleTran
    {
        void ShowAllList();
        void tranText2(string text);
        void clearText();
        void speak();
        List<string> readsentence();
        List<Word> readWord();
        List<WordSynonym> readsynonym();
        List<WordDefine> readDefine();
    }
    public class googleTran1 : Elements, GoogleTran
    {
        IWebDriver driver = null;
        public googleTran1(IWebDriver d) : base(d)
        {
            driver = d;
        }
        public void ShowAllList()
        {
            int count = countXpath("//div[contains(@class,'cd-expand-button')]");
            if (count > 0)
            {
                ClickXpath("//div[contains(@class,'cd-expand-button')]");
            }
        }
        public void clearText()
        {
            try
            {
                string xpath = "//div[contains(@class,'clear jfk-button-flat tlid-clear-source-text jfk-button')]";
                int count = driver.FindElements(By.XPath(xpath)).Count();
                IWebElement click1 = driver.FindElement(By.XPath(xpath));
                click1.Click();

            }
            catch (Exception ex)
            {

            }
        }
        public void tranText2(string text)
        {
            word = text;
            string xpath = "//textarea[contains(@id,'source')]";
            IWebElement textareat = driver.FindElement(By.XPath(xpath));
            textareat.SendKeys("a");
            Thread.Sleep(50);
            clearText();
            Thread.Sleep(300);
            IWebElement textarea = driver.FindElement(By.XPath(xpath));
            textarea.SendKeys(text);
            Thread.Sleep(300);
        }
        public void speak()
        {
            string xpath = "//div[contains(@class,'src-tts left-positioned ttsbutton jfk-button-flat source-or-target-footer-button jfk-button')]";
            Thread.Sleep(300);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(1));
            wait.Until(d => d.FindElements(By.XPath(xpath)).Count > 0);
            IWebElement click1 = driver.FindElement(By.XPath(xpath));
            click1.Click();
            string press = click1.GetAttribute("aria-pressed").ToString();
            while (press == "false")
            {
                click1.Click();
                press = click1.GetAttribute("aria-pressed").ToString();
            }
        }
        public List<string> readsentence()
        {
            List<string> a = new List<string>();
            int count = countXpath("//div[contains(@class,'gt-ex-text')]"); // //div[contains(@class,'AZPoqf OvhKBb')]  Qk6AXd
            if (count > 0)
            {
                a = GetXpath("//div[contains(@class,'gt-ex-text')]");
            }
            return a;
        }
        public List<Word> readWord()
        {
            List<Word> Words = new List<Word>();
            List<string> a = new List<string>();
            int count = countXpath("//td/div/span/span[contains(@class,'gt-baf-cell gt-baf-word-clickable')]");
            if (count > 0)
            {
                List<string> aList = GetXpath("//div/span[contains(@class,'gt-cd-pos')]");
                int l = 0;
                int id = 1;
                for (int i = 2; i <= count; i++)
                {
                    a = GetXpath("//tr[" + i.ToString() + "]/td/div/span/span[contains(@class,'gt-baf-cell gt-baf-word-clickable')]");
                    List<string> b = GetXpath("//tr[" + i.ToString() + "]/td/div[contains(@class,'gt-baf-cell gt-baf-translations gt-baf-translations-mobile')]");
                    if (a.Count == 0)
                    {
                        l++;
                    }
                    else if (a[0].Replace(" ", "") == "")
                    {
                        l++;
                    }
                    else if (a.Count > 0)
                    {
                        Word word = new Word();
                        word.a = aList[l];
                        word.id = id;
                        word.define = a[0];
                        word.otherword = b[0];
                        Words.Add(word);
                        id++;
                    }

                }
            }
            return Words;
        }
        public List<WordSynonym> readsynonym()
        {
            List<WordSynonym> WordSynonyms = new List<WordSynonym>();
            List<string> a = new List<string>();
            int count = countXpath("//div[contains(@class,'gt-cd gt-cd-mss')]/div/div[contains(@class,'gt-cd-pos')]");
            if (count > 0)
            {
                List<string> aList = GetXpath("//div[contains(@class,'gt-cd gt-cd-mss')]/div/div[contains(@class,'gt-cd-pos')]");
                bool div = false;
                int countX = countXpath("//ul[1]/div/li/span[contains(@class,'gt-syn-span')]/span[contains(@class,'gt-cd-cl')]");
                if (countX>0)
                {
                    div = true;
                }
                for (int p = 0; p < aList.Count; p++)
                {
                    if (div==true)
                    {
                        a = GetXpath("//ul[" + (p + 1).ToString() + "]/div/li/span[contains(@class,'gt-syn-span')]/span[contains(@class,'gt-cd-cl')]");
                    }
                    else
                    {
                        a = GetXpath("//ul[" + (p + 1).ToString() + "]/li/span[contains(@class,'gt-syn-span')]/span[contains(@class,'gt-cd-cl')]");
                    }
                    int id = 0;
                    foreach (string s in a)
                    {
                        WordSynonym WS = new WordSynonym();
                        WS.id = id + 1;
                        WS.a = aList[p];
                        WS.define = s;
                        if (s.Replace(" ", "").Length > 0)
                        {
                            WordSynonyms.Add(WS);
                        }
                    }
                }
            }
            return WordSynonyms;
        }

        public List<WordDefine> readDefine()
        {
            List<WordDefine> WordDefines = new List<WordDefine>();
            List<string> a = new List<string>();
            int count = countXpath("//div[contains(@class,'gt-cd gt-cd-mmd')]/div/div[contains(@class,'gt-cd-pos')]"); // //div[contains(@class,'Dwvecf')]/div/div[contains(@class,'eIKIse')]
            if (count > 0)
            {
                List<string> aList = GetXpath("//div[contains(@class,'gt-cd gt-cd-mmd')]/div/div[contains(@class,'gt-cd-pos')]");
                int i = 0;
                for (int p = 0; p < aList.Count; p++)
                {
                    a = GetXpath("//div[contains(@class,'gt-def-list')][" + (p + 1).ToString() + "]");// //div[contains(@class,'Dwvecf')]/div[contains(@class,'eqNifb')]["+(p+1).ToString()+"]/div/div[contains(@class,'fw3eif')]
                    int id = 0;
                    string[] items = Regex.Split(a[0], @"[\d]+\r\n");
                    foreach (string item in items)
                    {
                        if (item.Replace(" ", "").Length > 0)
                        {
                            string[] element = Regex.Split(item, @"\r\n");
                            WordDefine WD = new WordDefine();
                            WD.id = id + 1;
                            WD.define = element[0];
                            if (element.Length >= 3)
                            {
                                WD.sentence = element[1];
                            }
                            WD.a = aList[p];
                            id++;
                            WordDefines.Add(WD);
                        }
                    }
                    a.Clear();
                }
            }
            return WordDefines;
        }
        public void close()
        {
            driver.Quit();
        }
    }
    public class googleTran2 : Elements, GoogleTran
    {
        IWebDriver driver = null;
        public googleTran2(IWebDriver d) : base(d)
        {
            driver = d;
        }
        public void ShowAllList()
        {
            int count = countXpath("//div[contains(@class,'VK4HE')]");
            if (count > 0)
            {
                ClickXpath("//div[contains(@class,'VK4HE')]");
            }
        }
        public void clearText()
        {
            try
            {
                string xpath = "//button[contains(@class,'VfPpkd-Bz112c-LgbsSe yHy1rc eT1oJ qiN4Vb GA2I6e')]";
                int count = driver.FindElements(By.XPath(xpath)).Count();
                IWebElement click1 = driver.FindElement(By.XPath(xpath));
                click1.Click();

            }
            catch (Exception ex)
            {

            }
        }
        public void tranText2(string text)
        {
            word = text;
            string xpath = "//textarea[contains(@class,'er8xn')]";
            IWebElement textareat = driver.FindElement(By.XPath(xpath));
            textareat.SendKeys("a");
            Thread.Sleep(50);
            clearText();
            Thread.Sleep(300);
            IWebElement textarea = driver.FindElement(By.XPath(xpath));
            textarea.SendKeys(text);
            Thread.Sleep(300);
        }
        public void speak()
        {

            string xpath = "//button[contains(@class,'VfPpkd-Bz112c-LgbsSe fzRBVc tmJved SSgGrd m0Qfkd')]";
            Thread.Sleep(600);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(1));
            wait.Until(d => d.FindElements(By.XPath(xpath)).Count > 0);
            IWebElement click1 = driver.FindElement(By.XPath(xpath));
            click1.Click();
            string press = click1.GetAttribute("aria-pressed").ToString();
            while (press == "false")
            {
                click1.Click();
                press = click1.GetAttribute("aria-pressed").ToString();
            }
        }
        public List<string> readsentence()
        {
            List<string> a = new List<string>();
            int count = countXpath("//div[contains(@class,'AZPoqf OvhKBb')]");
            if (count > 0)
            {
                a = GetXpath("//div[contains(@class,'AZPoqf OvhKBb')]");
            }
            return a;
        }
        public List<Word> readWord()
        {
            List<Word> Words = new List<Word>();
            List<string> a = new List<string>();
            int count = countXpath("//div[contains(@class,'Dwvecf')]/div/div[contains(@class,'eIKIse')]");
            if (count > 0)
            {
                List<string> aList = GetXpath("//div[contains(@class,'Dwvecf')]/div/div[contains(@class,'eIKIse')]");

                for (int i = 1; i <= count; i++)
                {
                    a = GetXpath("//tbody[" + i.ToString() + "]/tr/th/div/span[contains(@class,'kgnlhe')]");
                    List<string> b = GetXpath("//tbody[" + i.ToString() + "]/tr/td/ul[contains(@class,'FgtVoc OvhKBb')]");
                    for (int l = 0; l < a.Count; l++)
                    {
                        Word word = new Word();
                        word.a = aList[i - 1];
                        word.id = l + 1;
                        word.define = a[l];
                        word.otherword = b[l];
                        Words.Add(word);
                    }
                }
            }
            return Words;
        }
        public List<WordSynonym> readsynonym()
        {
            List<WordSynonym> WordSynonyms = new List<WordSynonym>();
            List<string> a = new List<string>();
            int count = countXpath("//div[contains(@class,'Dwvecf')]/div/div[contains(@class,'KWoJId')]");
            if (count > 0)
            {
                List<string> aList = GetXpath("//div[contains(@class,'Dwvecf')]/div/div[contains(@class,'KWoJId')]");
                for (int p = 0; p < aList.Count; p++)
                {
                    a = GetXpath("//div[contains(@class,'Dwvecf')]/div[contains(@class,'QkhBKc')][" + (p + 1).ToString() + "]/div/div/span");
                    if (a.Count==0)
                    {
                        a = GetXpath("//div[contains(@class,'Dwvecf')]/div[contains(@class,'QkhBKc')][" + (p + 1).ToString() + "]/div/span");
                    }
                    int id = 0;
                    foreach (string s in a)
                    {
                        if (s.Replace(" ", "").Length > 0)
                        {
                            WordSynonym WS = new WordSynonym();
                            WS.id = id + 1;
                            WS.a = aList[p];
                            WS.define = s;
                            WordSynonyms.Add(WS);
                        }
                    }
                }
            }
            return WordSynonyms;
        }

        public List<WordDefine> readDefine()
        {
            List<WordDefine> WordDefines = new List<WordDefine>();
            List<string> a = new List<string>();
            int count = countXpath("//div[contains(@class,'Dwvecf')]/div/div[contains(@class,'eIKIse')]");
            if (count > 0)
            {
                List<string> aList = GetXpath("//div[contains(@class,'Dwvecf')]/div/div[contains(@class,'eIKIse')]");
                List<string> all = GetXpath("//div[contains(@class,'Dwvecf')]");
                for (int p = 0; p < aList.Count; p++)
                {
                    //a = GetXpath("//div[contains(@class,'Dwvecf')]/div[contains(@class,'eqNifb')][" + (p + 1).ToString() + "]/div/div[contains(@class,'fw3eif')]");
                    a = GetXpath("//div[contains(@class,'Dwvecf')]/div[contains(@class,'eqNifb')][" + (p + 1).ToString() + "]");
                    int id = 0;
                    string[] items = Regex.Split(a[0], @"[\d]+\r\n");
                    foreach (string item in items)
                    {
                        if (item.Replace(" ", "").Length > 0)
                        {
                            string[] element = Regex.Split(item, @"\r\n");
                            if (element[0].Replace(" ", "") != "")
                            {
                                WordDefine WD = new WordDefine();
                                WD.id = id + 1;
                                WD.define = element[0];
                                if (element.Length >= 3)
                                {
                                    WD.sentence = element[1];
                                }
                                if (element.Length >= 2)
                                {
                                    WD.a = function.findPastOfSpeech(all[0], element[1], aList);
                                }
                                id++;
                                WordDefines.Add(WD);
                            }

                        }
                    }
                    a.Clear();
                    int counteIKIse = countXpath("//div[contains(@class,'Dwvecf')]/div/div[contains(@class,'eIKIse')]");
                    a = GetXpath("//div[contains(@class,'Dwvecf')]/div[contains(@jsname,'K0TzFf')][" + (p + 1).ToString() + "]");
                    if (a.Count>0)
                    {
                        items = Regex.Split(a[0], @"[\d]+");
                        foreach (string item in items)
                        {
                            if (item.Replace(" ", "").Length > 0)
                            {
                                string[] element = Regex.Split(item, @"\r\n");
                                if (element[0].Replace(" ", "") == "")
                                {
                                    WordDefine WD = new WordDefine();
                                    WD.id = id + 1;
                                    WD.define = element[1];
                                    if (element.Length >= 3)
                                    {
                                        WD.sentence = element[2];
                                    }
                                    WD.a = function.findPastOfSpeech(all[0], element[1], aList);
                                    id++;
                                    WordDefines.Add(WD);
                                }

                            }
                        }
                    }
                    
                    a.Clear();
                }
            }
            return WordDefines;
        }
        public void close()
        {
            driver.Quit();
        }
    }
}
