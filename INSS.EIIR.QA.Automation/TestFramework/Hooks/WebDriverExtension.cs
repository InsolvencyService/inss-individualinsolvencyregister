using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace TestFramework.Hooks

{
    public static class WebDriverExtension
    {
        private static string _screenShotDirectory;
        private static readonly Random random;
        private static readonly TimeSpan implicitTimeout;
        private static readonly int timeout;

        public static IWebElement FindElement(this IWebDriver driver, By by, int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
                wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
                return wait.Until(drv => drv.FindElement(by));
            }
            return driver.FindElement(by);
        }


        private static void setImplicitTimeout(this IWebDriver driver, int timeout)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(timeout);
        }

        private static void switchOffImplicitWaiting(this IWebDriver driver)
        {
            setImplicitTimeout(driver, 0);
        }

        private static void resetImplicitTimeOut(this IWebDriver driver)
        {
            setImplicitTimeout(driver, timeout);
        }

        public static void ClearAndSendKeys(this IWebDriver driver, By identifier, string text, int? waitingTime = null)
        {
            driver.FindElement(identifier, 5).Clear();
            driver.FindElement(identifier).SendKeys(text);
            if (waitingTime != null)
            {
                Thread.Sleep(Convert.ToInt16(waitingTime));
            }
            driver.FindElement(identifier).SendKeys(Keys.Tab);
        }

        public static void SendKeys(this IWebDriver driver, By identifier, string text, int? waitingTime = null)
        {
            driver.FindElement(identifier, 5).SendKeys(text);
            if (waitingTime != null)
            {
                Thread.Sleep(Convert.ToInt16(waitingTime));
            }
            driver.FindElement(identifier).SendKeys(Keys.Tab);
        }

        public static void Click(this IWebDriver driver, By identifier)
        {
            driver.FindElement(identifier, 5).Click();
        }

        internal static string GetUrl(this IWebDriver driver)
        {
            return driver.Url.Trim();
        }

        public static void FocusAndClick(this IWebDriver driver, By identifier)
        {
            IWebElement element = driver.FindElement(identifier, 5);
            Actions act = new Actions(driver);
            act.MoveToElement(element).Click().Build().Perform();
        }

        internal static void Focus(this IWebDriver driver, By identifier = null, IWebElement webElement = null)
        {
            if (webElement != null)
            {
                Actions act = new Actions(driver);
                act.MoveToElement(webElement).Build().Perform();
            }
            else
            {
                IWebElement element = driver.FindElement(identifier, 5);
                Actions act = new Actions(driver);
                act.MoveToElement(element).Build().Perform();
            }
        }

        internal static void ScrollIntoView(this IWebDriver driver, By identifier)
        {
            IWebElement element = driver.FindElement(identifier, 5);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(false);", element);
        }

        internal static bool SelectOptionByText(this IWebDriver driver, By identifier, string text)
        {
            bool elementToSelectExist = false;
            try
            {
                SelectElement select = new SelectElement(driver.FindElement(identifier, 5));
                Thread.Sleep(1000);
                select.SelectByText(text);
                elementToSelectExist = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return elementToSelectExist;
        }

        internal static void SelectOptionByValue(this IWebDriver driver, By identifier, string text)
        {
            SelectElement select = new SelectElement(driver.FindElement(identifier, 5));
            Thread.Sleep(1000);
            select.SelectByValue(text);
        }

        internal static string GetElementText(this IWebDriver driver, By identifier)
        {
            return driver.FindElement(identifier, 5).Text.Trim();
        }

        internal static void CloseCurrentWindow(this IWebDriver driver)
        {
            try
            {
                driver.Close();
                driver.Navigate().Refresh();
            }
            catch (Exception)
            {
                Console.WriteLine("Current Window has already been closed");
            }
        }

        internal static IList<IWebElement> FindElements(this IWebDriver driver, By Identifier)
        {
            return driver.FindElements(Identifier);
        }

        internal static IList<IWebElement> FindElements(this IWebDriver driver, By baseIdentifier, By subIdentifier)
        {
            return driver.FindElement(baseIdentifier).FindElements(subIdentifier);
        }

        internal static IWebElement FindElement(this IWebDriver driver, By Identifier)
        {
            return driver.FindElement(Identifier);
        }
        internal static ICollection<IWebElement> GetElementsList(this IWebDriver driver, By identifier)
        {
            ICollection<IWebElement> elementsInList = driver.FindElements(identifier);
            return elementsInList;
        }

        private static string GetText(IWebElement element)
        {

            return element.GetAttribute("value").Trim();

        }

        internal static List<string> GetTextFromAllElementsMatching(this IWebDriver driver, By identifier)
        {
            List<string> result = new List<string>();

            foreach (IWebElement element in driver.FindElements(identifier))
            {
                string elementText = GetText(element);
                result.Add(elementText);
            }

            return result;
        }

        internal static bool ElementIsNotDisplayed(this IWebDriver driver, By identifier)
        {
            return !driver.FindElement(identifier, 5).Displayed;
        }


        internal static bool ElementContainsText(this IWebDriver driver, By identifier, string expectedText)
        {
            List<string> values = GetTextFromAllElementsMatching(driver, identifier);
            foreach (string value in values)
            {
                if (value.ToLower().Contains(expectedText.ToLower()))
                {
                    return true;
                }
            }

            return false;
        }

        internal static bool ElementDoesNotContainText(this IWebDriver driver, By identifier, string expectedText)
        {
            List<string> values = GetTextFromAllElementsMatching(driver, identifier);
            foreach (string value in values)
            {
                if (value.ToLower().Contains(expectedText.ToLower()))
                {
                    return false;
                }
            }
            return true;
        }

        internal static bool ElementIsDisplayed(this IWebDriver driver, By identifier)
        {
            if (driver.FindElement(identifier, 5).Displayed)
            {
                return driver.FindElement(identifier).Displayed;
            }
            return false;
        }

        internal static bool ElementDisplayed(this IWebDriver driver, By identifier)
        {
            IWebElement element = driver.FindElement(identifier, 5);

            return element.Displayed;
        }


        internal static bool ElementExists(this IWebDriver driver, By identifier)
        {
            return ElementExists(driver, identifier, implicitTimeout);
        }

        internal static bool ElementExists(this IWebDriver driver, By identifier, TimeSpan timeout)
        {
            bool result = false;
            try
            {
                switchOffImplicitWaiting(driver);
                WebDriverWait wait = new WebDriverWait(driver, timeout);
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(identifier));
                result = true;
            }
            catch (WebDriverTimeoutException)
            {
                result = false;
            }
            finally
            {
                resetImplicitTimeOut(driver);
            }
            return result;
        }

        internal static bool ElementDoesNotExist(this IWebDriver driver, By identifier, TimeSpan timeout)
        {
            WebDriverWait wait = new WebDriverWait(driver, timeout);
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(identifier));
            bool result = true;
            try
            {
                Console.WriteLine("Element could not be found");

            }
            catch (WebDriverTimeoutException)
            {
                switchOffImplicitWaiting(driver);
            }
            finally
            {
                resetImplicitTimeOut(driver);
            }

            return result;
        }

        internal static bool WaitForElementToBeVisible(this IWebDriver driver, By identifier)
        {
            return WaitForElementToBeVisible(driver, identifier, timeout);
        }

        internal static bool WaitForElementToBeVisible(this IWebDriver driver, By identifier, int timeout)
        {
            bool elementVisible = false;

            try
            {
                switchOffImplicitWaiting(driver);
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(identifier));
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(identifier));

                elementVisible = true;
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("Element is not visible");

            }
            finally
            {
                resetImplicitTimeOut(driver);
            }
            return elementVisible;
        }

        internal static void WaitForElementNotToBeVisible(this IWebDriver driver, By identifier)
        {
            WaitForElementNotToBeVisible(driver, identifier, timeout);
        }

        internal static void WaitForElementNotToBeVisible(this IWebDriver driver, By identifier, int timeout)
        {
            try
            {
                switchOffImplicitWaiting(driver);
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(identifier));
            }
            catch (WebDriverTimeoutException)
            {

            }
            finally
            {
                resetImplicitTimeOut(driver);
            }
        }
        internal static void WaitForElementToContainText(this IWebDriver driver, By identifier, string expectedText)
        {
            WaitForElementToContainText(driver, identifier, expectedText, timeout);
        }

        internal static void WaitForElementToContainText(this IWebDriver driver, By identifier, string expectedText, int timeout)
        {
            try
            {
                switchOffImplicitWaiting(driver);
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
                wait.Until(elementContainsText(driver, identifier, expectedText));
            }
            catch (WebDriverTimeoutException)
            {

            }
            finally
            {
                resetImplicitTimeOut(driver);
            }
        }
        private static Func<IWebDriver, bool> elementContainsText(this IWebDriver driver, By identifier, String expectedText)
        {
            return (d) =>
            {
                bool result = false;
                try
                {
                    result = GetText(driver.FindElement(identifier, 5)).Contains(expectedText);
                }
                catch (StaleElementReferenceException)
                {

                }

                return result;
            };
        }
        internal static void HandleAlert(this IWebDriver driver)
        {
            try
            {
                IAlert alert = driver.SwitchTo().Alert();

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.AlertIsPresent());
                Thread.Sleep(TimeSpan.FromSeconds(5));
                alert.Accept();
            }
            catch (Exception)
            {
                Console.WriteLine("Continue with test execution");
            }
        }

        internal static void HandleConfirmation(this IWebDriver driver)
        {
            try
            {
                IAlert alert = driver.SwitchTo().Alert();

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.AlertIsPresent());
                Thread.Sleep(TimeSpan.FromSeconds(5));
                alert.Dismiss();
            }
            catch (Exception)
            {
                Console.WriteLine("Continue with test execution");
            }
        }
        internal static bool ElementIsEnabled(this IWebDriver driver, By identifier)
        {
            return driver.FindElement(identifier, 5).Enabled;
        }

        internal static void SwitchToIFrame(this IWebDriver driver, string frameId)
        {
            if (ElementExists(driver, By.Id(frameId)))
            {
                NavigateToFrame(driver, By.Id(frameId));
            }
        }
        internal static bool NavigateToFrame(this IWebDriver driver, By identifier)
        {
            bool navigationSuccessful = false;
            try
            {
                driver.SwitchTo().Frame(driver.FindElement(identifier, 5));
                navigationSuccessful = true;
            }
            catch (Exception)
            {
                Console.WriteLine("Navigation to the frame was not successful");
            }

            return navigationSuccessful;
        }

        internal static void SwitchBackToDefaultContent(this IWebDriver driver)
        {
            driver.SwitchTo().DefaultContent();
        }

        internal static void RefreshWindow(this IWebDriver driver)
        {
            driver.Navigate().Refresh();
        }

        internal static void SwitchToDefaultWindow(this IWebDriver driver)
        {
            string currentWindow = driver.CurrentWindowHandle;
            driver.SwitchTo().Window(currentWindow);
        }

        internal static string SwitchToLastWindow(this IWebDriver driver)
        {
            driver.SwitchTo().Window(driver.WindowHandles.Last());
            driver.Manage().Window.Maximize();

            return driver.Title;
        }

        internal static void AcceptConfirmWindow(this IWebDriver driver)
        {
            driver.SwitchTo().Alert().Accept();
        }

        internal static void WaitForPageToLoad(this IWebDriver driver, int second)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(second));
            wait.Until(driver1 => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
        }

        public static bool ElementIsSelected(this IWebDriver driver, By identifier)
        {
            IWebElement element = driver.FindElement(identifier, 5);
            return element.Selected;
        }

        public static bool ElementIsClickable(this IWebDriver driver, By identifier)
        {
            IWebElement element = driver.FindElement(identifier, 5);
            return element.Enabled;
        }

        public static void SubmitFocus(this IWebDriver driver, By identifier)
        {
            Actions action = new Actions(driver);
            IWebElement element = driver.FindElement(identifier, 5);
            action.MoveToElement(element);
            Click(driver, identifier);
            action.Perform();
        }

        public static void PressTabKey(this IWebDriver driver, By identifier)
        {
            IWebElement element = driver.FindElement(identifier, 5);
            element.SendKeys(Keys.Tab);
        }

        public static void PressEnterKey(this IWebDriver driver, By identifier)
        {
            IWebElement element = driver.FindElement(identifier, 5);
            element.SendKeys(Keys.Enter);
        }

        public static void EnterTextInTextAreas(this IWebDriver driver, By identifier, string text, TimeSpan timeout)
        {
            IWebElement element = driver.FindElement(identifier, 5);
            IList<IWebElement> elements = element.FindElements(identifier);

            foreach (IWebElement ele in elements)
            {
                ele.Clear();
                ele.SendKeys(text);
            }
        }

        public static string ChooseFromIdenticalElements(this IWebDriver driver, By identifier, TimeSpan timeout)
        {
            IWebElement element = driver.FindElement(identifier, 5);
            IList<IWebElement> elements = element.FindElements(identifier);

            var option = elements[random.Next(elements.ToArray().Length)];
            string text = option.Text;
            Console.WriteLine("The text in the selected element is: [{0}]", text);
            option.Click();

            return text;
        }

        public static void ClickSpecificLinkFromIdenticalElements(this IWebDriver driver, By identifier, string textOflinkToClick, TimeSpan timeout)
        {
            IWebElement element = driver.FindElement(identifier, 5);
            IList<IWebElement> links = element.FindElements(identifier);

            foreach (var link in links)
            {
                if (link.Text.Contains(textOflinkToClick))
                {
                    link.Click();
                    break;
                }
            }
        }
        public static void ClickTab(this IWebDriver driver, By tabLocator)
        {
            WaitForElementToBeVisible(driver, tabLocator);
            Click(driver, tabLocator);
        }

    }
}
