using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AventStack.ExtentReports;
using DataDrivenProject;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Interactions;
using TechTalk.SpecFlow;

namespace SpecDemo
{
    public class WebConnector
    {
        IWebDriver webDriver = null;
        ExtentReports rep = ExtentManager.getInstance();
        ExtentTest test;

        public void Info(String msg)
        {
            test.Log(Status.Info, msg);
        }

        public void startTest(string testName)
        {
            test = rep.CreateTest(testName);
        }
        public void OpenBrowser(String browserType)
        {
            Info("Opening the browser");

            if (browserType.Equals("Mozilla"))
            {
                FirefoxDriverService driverService = FirefoxDriverService.CreateDefaultService(@"D:\Setups\geckodriver-v0.27.0-win64\");
                driverService.FirefoxBinaryPath = @"C:\Program Files (x86)\Mozilla Firefox\firefox.exe";
                webDriver = new FirefoxDriver(driverService);
                webDriver.Manage().Window.Maximize();
            }
            else if (browserType.Equals("Chrome"))
            {
                webDriver = new ChromeDriver("");
            }
            else if (browserType.Equals("IE"))
            {
                webDriver = new InternetExplorerDriver("");
            }
            webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
        }

        public void Navigate(String url)
        {
            Info("Navigating To" + url);
            webDriver.Url = ConfigurationManager.AppSettings[url];
            Console.WriteLine(webDriver.Url);
        }

        public void Click(String xPathExp)
        {
            Info("Clicking on " + xPathExp);
            getElement(xPathExp).Click();
            //test.Log(Status.Info, "Clicked successfully on " + xPathExp);
           
        }

        public void Type(String xPathExpKey, String Data)
        {
            Info("Typing on" + xPathExpKey);
            getElement(xPathExpKey).SendKeys(Data);
        }

        public IWebElement getElement(string locatorKey)
        {
            IWebElement e = null;
            try
            {
                if (locatorKey.EndsWith("_Xpath"))
                {
                    e = webDriver.FindElement(By.XPath(ConfigurationManager.AppSettings[locatorKey]));
                }
                else if (locatorKey.EndsWith("_Id"))
                {
                    e = webDriver.FindElement(By.Id(ConfigurationManager.AppSettings[locatorKey]));
                }
                else if (locatorKey.EndsWith("_name"))
                {
                    e = webDriver.FindElement(By.Name(ConfigurationManager.AppSettings[locatorKey]));
                }
                else if (locatorKey.EndsWith("_LinkText"))
                {
                    e = webDriver.FindElement(By.LinkText(ConfigurationManager.AppSettings[locatorKey]));
                }
                else if (locatorKey.EndsWith("_Class"))
                {
                    e = webDriver.FindElement(By.ClassName(ConfigurationManager.AppSettings[locatorKey]));
                }
                else
                {
                    //reportFailure("Locator is not correct" + locatorKey);
                    //Assert.Fail("Locator is not correct" + locatorKey);
                }
            }
            catch (Exception ex)
            {
                //reportFailure(ex.Message);
                //Assert.Fail("Fail the Test " + ex.Message);
            }

            return e;
        }

        public bool isElementPresent(string obj)
        {
            int Count = webDriver.FindElements(By.XPath(ConfigurationManager.AppSettings[obj])).Count;
            if (Count == 1)
                return true;
            else
                return false;
        }

        public void MouseHover(String locatorKey)
        {
            Info("Moving to Element "+ locatorKey);
            Actions act = new Actions(webDriver);
            act.MoveToElement(getElement(locatorKey));
            act.Build();
            act.Perform();          
        }

        public void TakescreenShot()
        {
            string screenshotfile = DateTime.Now.ToString().Replace("/", "_").Replace(":", "_").Replace(" ", "_") + ".png";
            ITakesScreenshot sc = webDriver as ITakesScreenshot;
            Screenshot screenshot = sc.GetScreenshot();
            String strFilepath = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory);
            strFilepath = Directory.GetParent(Directory.GetParent(strFilepath).FullName).FullName;
            string screenshotpath = strFilepath + "\\Screenshots\\" + screenshotfile;
            screenshot.SaveAsFile(strFilepath + "\\Screenshots\\" + screenshotfile, ScreenshotImageFormat.Png);
            test.Log(Status.Info, "Screenshot -", MediaEntityBuilder.CreateScreenCaptureFromPath(screenshotpath).Build());            
        }

        public void ReportPass(String msg)
        {
            test.Log(Status.Pass, msg);
        }

        public void ReportFailure(String msg)
        {
            test.Log(Status.Fail, msg);
            TakescreenShot();
            Assert.Fail();
        }

    

        [AfterScenario]
        public void quit()
        {
            if (rep != null)
                rep.Flush();
        }
    }
}
