using NUnit.Framework;
using OpenQA.Selenium;
using INSS.EIIR.QA.Automation.Helpers;
using System;
using System.Collections.Generic;
using INSS.EIIR.QA.Automation.Data;

namespace INSS.EIIR.QA.Automation.Pages
{
    internal class AdminViewSubscriberListPage : ElementHelper
    {
        private static string expectedPageUrl { get; } = "https://app-uksouth-sit-eiir-web.azurewebsites.net/admin/subscribers";
        private static string expectedPageTitle { get; } = "Subscribers - Individual Insolvency Register";
        private static string expectedPageHeader { get; } = "Subscribers";
        private static By pageHeaderElement { get; } = By.XPath("//*[@id='main-content']//h1");
        private static By activeCheckbox { get; } = By.Id("Active");
        private static By inactiveCheckbox { get; } = By.Id("InActive");
        private static By homeBreadcrumb { get; } = By.LinkText("Home");
        public static int Loopcount;
        private static List<object[]> result;

        public static void verifySubscriberListPage()
        {
            Assert.IsTrue(WebDriver.Url.Contains(expectedPageUrl));
            Assert.AreEqual(expectedPageTitle, WebDriver.Title);
            Assert.AreEqual(expectedPageHeader, WebDriver.FindElement(pageHeaderElement).Text);
        }

        public static void clickHomeBreadcrumb()
        {
            ClickElement(homeBreadcrumb);
        }

        public static void ClickActiveCheckbox()
        {
            ClickElement(activeCheckbox);
        }

        public static void ClickInactiveCheckbox()
        {
            ClickElement(inactiveCheckbox);
        }

        public static void VerifyActiveCheckBoxIsTicked()
        {
            Boolean isChecked;
            isChecked = WebDriver.FindElement(activeCheckbox).Selected;
            Assert.IsTrue(isChecked);
        }

        public static void VerifySubscribersList(string Subscribers)
        {

            switch (Subscribers)
            {
                case "Active subscribers":
                    result = SqlQueries.GetAllActiveSubscribersListFromDb();
                    break;
                case "Inactive subscribers":
                    result = SqlQueries.GetAllInactiveSubscribersListFromDb();
                    break;
                case "All subscribers":
                    result = SqlQueries.GetAllSubscribersListFromDb();
                    break;
            }

            //List<object[]> result = SqlQueries.GetAllSubscribersListFromDb();

            //Set value of how many times to loop (max 10 as only 10 records at a time can be displayed per page)
            //If the number of records returned in the SQL query is less than 10, set loopcount to that number minus 1 because the foor loop iterates
            //from 0. Otherwise set the loopcount to 9 (which equates to 10 due to for loop starting count from 0)

            if (result.Count > 10)
            {
                Loopcount = 9;
            }
            else
            {
                Loopcount = result.Count - 1;
            }


            for (int i = 0; i <= Loopcount; i++)
            {
                string subID = Convert.ToString(result[i][0]);
                string CompName = Convert.ToString(result[i][1]);
                string SubEnd = Convert.ToString(result[i][2]);

                int x = i + 1;
                string SubscriberIDXpath = "//table/tbody/tr[" + x + "]/td[1]";
                string CompanyNameXpath = "//table/tbody/tr[" + x + "]/td[2]";
                string SubscriptionEndDateXpath = "//table/tbody/tr[" + x + "]/td[3]";

                string SubscriberID = WebDriver.FindElement(By.XPath(SubscriberIDXpath)).Text;
                string CompanyName = WebDriver.FindElement(By.XPath(CompanyNameXpath)).Text;
                string SubscriptionEndDate = WebDriver.FindElement(By.XPath(SubscriptionEndDateXpath)).Text;

                DateTime dateTime12 = DateTime.Parse(SubEnd);
                DateTime dateTime13 = DateTime.Parse(SubscriptionEndDate);

                Assert.AreEqual(subID, SubscriberID);
                Assert.AreEqual(CompName, CompanyName);
                Assert.AreEqual(dateTime12, dateTime13);

            }
        }



        public static void ClickSubscriberLink(string SubscriberName)
        {
            IList<IWebElement> all = WebDriver.FindElements(By.XPath("//table/tbody//td[2]/a"));

            String[] allText = new String[all.Count];
            int i = 0;
            foreach (IWebElement element in all)
            {

                allText[i] = element.Text;
                string Test = allText[i];
       
                i = i + 1;

                if (Test == SubscriberName)
                {
                    String ExpectedXpath = "//*[@id='main-content']/div/div[2]/table/tbody/tr[" + i + "]/td[2]/a";
                    WebDriver.FindElement(By.XPath(ExpectedXpath)).Click();
                    break;
                }
            }
        }
    }
}


