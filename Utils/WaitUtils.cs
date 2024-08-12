using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompetitionTask.Utils
{
    public class WaitUtils
    {
        
            private static readonly string XPATH = "XPath";
            private static readonly string ID = "Id";
            private static readonly string CSS_SELECTOR = "CssSelector";
            private static readonly string NAME = "Name";

            public static void WaitToBeVisible(IWebDriver driver, string locatorType, string locatorValue, int seconds)
            {
            // WebDriverWait webDriverWait = new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));

                if (locatorType == XPATH)
                {
                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(locatorValue)));
                }
                if (locatorType == ID)
                {
                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id(locatorValue)));
                }
                if (locatorType == CSS_SELECTOR)
                {
                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector(locatorValue)));
                }
                if (locatorType == NAME)
                {
                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Name(locatorValue)));
                }
            }

            public static void WaitToBeClickable(IWebDriver driver, string locatorType, string locatorValue, int seconds)
            {
            WebDriverWait wait  = new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));

                if (locatorType == XPATH)
                {
                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath(locatorValue)));
                }
                if (locatorType == ID)
                {
                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id(locatorValue)));
                }
                if (locatorType == CSS_SELECTOR)
                {
                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector(locatorValue)));
                }
                if (locatorType == NAME)
                {
                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Name(locatorValue)));
                }
            }

            public static void WaitToExist(IWebDriver webDriver, string locatorType, string locatorValue, int seconds)
            {
            WebDriverWait wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(seconds));

                if (locatorType == XPATH)
                {
                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath(locatorValue)));
                }
                if (locatorType == ID)
                {
                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id(locatorValue)));
                }
                if (locatorType == CSS_SELECTOR)
                {
                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.CssSelector(locatorValue)));
                }
                if (locatorType == NAME)
                {
                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Name(locatorValue)));
                }
            }
        }
    }

