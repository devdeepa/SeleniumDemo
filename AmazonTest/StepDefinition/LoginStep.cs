using AventStack.ExtentReports;
using DataDrivenProject;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace SpecDemo.StepDefinition
{
    [Binding]
    public class LoginStep
    {
        WebConnector web = new WebConnector();
        

        [Given(@"I go to \""(.*)"" on \""(.*)""")]
        public void GivenIGoToHttpsAmazon_InOn(String url, String browser)
        {
            web.startTest("LoginTest");
            web.OpenBrowser(browser);
            web.Navigate(url);
            web.MouseHover("SignInHover_Id");
            web.TakescreenShot();
           
        }

        [When(@"I click on \""(.*)""")]
        public void WhenIClickOn(string obj)
        {
            web.Click(obj);            
        }

        [When(@"I enter \""(.*)"" as \""(.*)""")]
        public void WhenIEnterAs(string strobj, string strValue)
        {
            Console.WriteLine("Entering into " + strobj + " With Value - " + strValue);
            web.Type(strobj, strValue);
        }

        [Then(@"login should be \""(.*)""")]
        public void ThenLoginShouldBeSuccessful(String strExpectedResult)
        {
           
            bool result = web.isElementPresent("Signout_Xpath");
            String strActualResult = null;
            if (result != true)
                strActualResult = "Failure";
                
            else
                strActualResult = "Sucess";

            Assert.AreEqual(strExpectedResult, strActualResult);
            web.TakescreenShot();
        }

    }
}
