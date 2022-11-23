using TechTalk.SpecFlow;
using INSS.EIIR.QA.Automation.Helpers;
using INSS.EIIR.QA.Automation.Data;
using INSS.EIIR.QA.Automation.TestFramework;
using INSS.EIIR.QA.Automation.Pages;
using OpenQA.Selenium;
using System.Collections.Generic;
using System;

namespace INSS.EIIR.QA.Automation
{
    [Binding]
    public class ATU_82Admin_ViewSubscriberListStepDefinitions : ElementHelper
    {

        [Then(@"I am navigated to the Admin Subscriber List page")]
        public void ThenIAmNavigatedToTheAdminSubscriberListPage()
        {
            AdminViewSubscriberListPage.verifySubscriberListPage();
        }

        [Given(@"I login as an admin user and navigate to view subscriber list page")]
        public void GivenILoginAsAnAdminUserAndNavigateToViewSubscriberListPage()
        {
            WebDriver.Navigate().GoToUrl("https://app-uksouth-sit-eiir-web.azurewebsites.net/Admin");
            WebDriver.FindElement(By.Id("username")).SendKeys(Constants.AdminUserName);
            WebDriver.FindElement(By.Id("password")).SendKeys(Constants.AdminPassword);
            WebDriver.FindElement(By.XPath("//*[@id='main-content']/div/div/form/button")).Click();
            WebDriver.FindElement(By.LinkText("View and update subscribers")).Click();     

        }

        [Then(@"the table is sorted by subscription end date and displays ""([^""]*)"" only")]
        public void ThenTheTableIsSortedBySubscriptionEndDateAndDisplaysOnly(string Subscribers)
        {
            AdminViewSubscriberListPage.VerifySubscribersList(Subscribers);
        }


        [When(@"I click the Active check box")]
        public void WhenIClickTheActiveCheckBox()
        {
            AdminViewSubscriberListPage.ClickActiveCheckbox();
        }

        [Then(@"the Active check box is selected by default")]
        public void ThenTheActiveCheckBoxIsSelectedByDefault()
        {
            AdminViewSubscriberListPage.VerifyActiveCheckBoxIsTicked();
        }


        [When(@"I check the Inactive check box only")]
        public void WhenIClickTheInactiveCheckBox()
        {
            //this will uncheck the active check box
            AdminViewSubscriberListPage.ClickActiveCheckbox();
            //this will check the Inactive check box
            AdminViewSubscriberListPage.ClickInactiveCheckbox();
        }

        [When(@"I click the Home breadcrumb on the Subscriber List page")]
        public void WhenIClickTheHomeBreadcrumbOnTheSubscriberListPage()
        {
            AdminViewSubscriberListPage.clickHomeBreadcrumb();
        }

        [When(@"I check the Active and Inactive check boxes")]
        public void WhenIClickTheActiveAndInactiveCheckBoxes()
        {
            AdminViewSubscriberListPage.ClickInactiveCheckbox();
        }

        [Then(@"I am navigated to the Admin Dashboard page")]
        public void ThenIAmNavigatedToTheAdminDashboardPage()
        {
            AdminLandingPage.verifyAdminLandingPage();
        }




    }
}
