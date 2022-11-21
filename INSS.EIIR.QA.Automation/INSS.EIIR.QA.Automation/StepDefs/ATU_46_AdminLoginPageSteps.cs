using TechTalk.SpecFlow;
using INSS.EIIR.QA.Automation.Helpers;
using INSS.EIIR.QA.Automation.TestFramework;
using INSS.EIIR.QA.Automation.Pages;



namespace INSS.EIIR.QA.Automation.StepDefs
{
    [Binding]
    public class ATU_46_AdminLoginPageSteps : ElementHelper
    {
        [Given(@"I navigate to the Admin Login page")]
        public void GivenINavigateToTheAdminLoginPage()
        {
            WebDriver.Manage().Window.Maximize();
            AdminLoginPage.navigateToAdminLoginPage();
        }

        [Then(@"I am navigated Admin Login page")]
        public void ThenIAmNavigatedAdminLoginPage()
        {
            AdminLoginPage.verifyAdminLoginPage();
        }

        [When(@"I enter a password only and press the Sign in button")]
        public void WhenIEnterAPasswordOnlyAndPressTheSignInButton()
        {
            AdminLoginPage.enterPasswordOnly();
        }

        [When(@"I enter a username only and press the Sign in button")]
        public void WhenIEnterAUsernameOnlyAndPressTheSignInButton()
        {
            AdminLoginPage.enterUsernameOnly();
        }

        [When(@"I don't enter a password or username and press the Sign in button")]
        public void WhenIDontEnterAPasswordOrUsernameAndPressTheSignInButton()
        {
            AdminLoginPage.clearTextFields();
        }

        [Then(@"I am shown the enter username error message")]
        public void ThenIAmShownTheEnterUsernameErrorMessage()
        {
            AdminLoginPage.verifyNullUsernameErrorMessage();
        }

        [Then(@"I am shown the enter password error message")]
        public void ThenIAmShownTheEnterPasswordErrorMessage()
        {
            AdminLoginPage.verifyNullPasswordErrorMessage();
        }

        [Then(@"I am shown the enter username and enter password error messages")]
        public void ThenIAmShownTheEnterUsernameAndEnterPasswordErrorMessages()
        {
            AdminLoginPage.verifyNullPasswordAndUsernameErrorMessage();
        }

        [When(@"I enter an invalid username and password combination and I press the Sign in button")]
        public void WhenIEnterAnInvalidUsernameAndPasswordCombinationAndIPressTheSignInButton()
        {
            AdminLoginPage.enterInvalidLoginDetails();
        }

        [Then(@"I am shown the Your login details are incorrect error message")]
        public void ThenIAmShownTheYourLoginDetailsAreIncorrectErrorMessage()
        {
            AdminLoginPage.verifyInvalidLoginDetailsErrorMessage();
        }















    }
}
