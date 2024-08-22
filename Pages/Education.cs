using CompetitionTask.Utils;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompetitionTask.Pages
{
    public class Education : CommonDriver
    {
        private IWebElement tabOption => driver.FindElement(By.XPath("//div[@class = 'ui top attached tabular menu']/a[3]"));
        private IWebElement addNewEduBtn => driver.FindElement(By.XPath(e_addedubutton));
        private IWebElement addInstName => driver.FindElement(By.XPath(e_addinstName));
        private IWebElement addDegree => driver.FindElement(By.XPath("//input[contains(@placeholder, 'Degree')]"));
        private IWebElement addCountry => driver.FindElement(By.XPath("//div[@data-tab='third']//select[@name='country']"));
        private IWebElement addTitle => driver.FindElement(By.XPath("//div[@data-tab='third']//select[@name='title']"));
        private IWebElement addYrOfGrdtn => driver.FindElement(By.XPath("//div[@data-tab='third']//select[@name='yearOfGraduation']"));
        private IWebElement addEduButton => driver.FindElement(By.XPath(e_AddButton));
        private IWebElement errormessage => driver.FindElement(By.XPath(e_errormessage));
        private IWebElement successmessage => driver.FindElement(By.XPath(e_successmessage));
        private IWebElement educationdata => driver.FindElement(By.XPath(e_educationdata));
        private IWebElement buttonDelete => driver.FindElement(By.XPath(e_deletebutton));
        private IWebElement editLanguageButton => driver.FindElement(By.XPath(e_editLanguageButton));
        private IWebElement buttonUpdate => driver.FindElement(By.XPath(e_updateButton));
        private IWebElement educationTable => driver.FindElement(By.XPath("//div[@data-tab='third']//tbody"));
        private IWebElement cancelButton => driver.FindElement(By.XPath(e_cancelButton));




        private string e_addedubutton = "//div[@data-tab='third']//div[@class ='ui teal button ']";
        private string e_addinstName = "//input[contains(@placeholder, 'College/University')]";
        private string e_AddButton = "//input[@value='Add']";
        private string e_waitForTab = "//div[@class = 'ui top attached tabular menu']/a[3]";
        private string e_errormessage = "//div[@class='ns-box ns-growl ns-effect-jelly ns-type-error ns-show']";
        private string e_successmessage = "//div[@class='ns-box ns-growl ns-effect-jelly ns-type-success ns-show']";
        private string e_educationdata = "//div[@data-tab='third']//tbody[last()]/tr/td[4]";
        private string e_deletebutton = "//div[@data-tab='third']//tbody[last()]/tr[1]/td[6]/span[2]/i[1]";
        private string e_editLanguageButton = "//div[@data-tab='third']//tbody[last()]//tr[1]/td[6]/span[1]/i[1]";
        private string e_updateButton = "//input[@value='Update']";
        private string e_cancelButton = "//input[@value='Cancel']";




        public void CreateEducation(string country, string university, string title, string degree, string gradYr)
        {
            
            WaitUtils.WaitToBeClickable(driver, "XPath", e_addedubutton, 10);

            addNewEduBtn.Click();

            WaitUtils.WaitToBeClickable(driver, "XPath", e_addinstName, 10);
            addInstName.SendKeys(university);

            var selectCountryDropdown = new SelectElement(addCountry);
            selectCountryDropdown.SelectByValue(country);

            var selecttitleDropdown = new SelectElement(addTitle);
            selecttitleDropdown.SelectByValue(title);

            addDegree.SendKeys(degree);

            var selectYearDropdown = new SelectElement(addYrOfGrdtn);
            selectYearDropdown.SelectByValue(gradYr);

            WaitUtils.WaitToBeClickable(driver, "XPath", e_AddButton, 10);
            addEduButton.Click();
            Thread.Sleep(1000);
            bool isErrorDisplayed = driver.FindElements(By.XPath("//div[@class='ns-box ns-growl ns-effect-jelly ns-type-error ns-show']")).Count > 0;

            if (isErrorDisplayed)
            {
                // If an error is displayed, click the 'Cancel' button
                WaitUtils.WaitToBeClickable(driver, "XPath", e_cancelButton, 10);
                cancelButton.Click();
                Console.WriteLine("An error occurred while adding education. Cancelled the operation.");
            }
            else
            {
                Console.WriteLine("Education added successfully.");
            }

        }



        public void ClickAnyTab(string tab)
        {
            //Wait for tabs to be visible
            WaitUtils.WaitToBeVisible(driver, "XPath", e_waitForTab, 3);

            //Click on specified tab
            tabOption.Click();
        }
        public string GetErrorMessage()
        {
            WaitUtils.WaitToBeVisible(driver, "XPath", e_errormessage, 1);
            return errormessage.Text;
            
        }
        public string GetSuccessMessage()
        {
            WaitUtils.WaitToBeVisible(driver, "XPath", e_successmessage, 10);
            return successmessage.Text;
        }

        public string GetEducation()
        {

            try
            {

                WaitUtils.WaitToBeVisible(driver, "XPath", e_educationdata, 10);
                string educationText = educationdata.Text;
                return educationdata.Text;
            }
            catch (Exception)
            {
                return "locator not found";
            }
        }

        public string GetScreenshot()
        {
            var file = ((ITakesScreenshot)driver).GetScreenshot();
            var img = file.AsBase64EncodedString;
            return img;
        }

        public void EditEducation(string country, string university, string title, string degree, string gradYr)
        {

            WaitUtils.WaitToBeClickable(driver, "XPath", e_editLanguageButton, 20);
            editLanguageButton.Click();

            WaitUtils.WaitToBeClickable(driver, "XPath", e_addinstName, 10);
            addInstName.Clear();
            addInstName.SendKeys(university);

            var selectCountryDropdown = new SelectElement(addCountry);
            selectCountryDropdown.SelectByValue(country);

            var selectTitleDropdown = new SelectElement(addTitle);
            selectTitleDropdown.SelectByValue(title);

            addDegree.Clear();
            addDegree.SendKeys(degree);

            var selectYearDropdown = new SelectElement(addYrOfGrdtn);
            selectYearDropdown.SelectByValue(gradYr);

            WaitUtils.WaitToBeClickable(driver, "XPath", e_updateButton, 10);
            buttonUpdate.Click();

            //WaitForPopupToDisappear();

            cancelButton.Click();

        }
    
            public void DeleteTestData(string degree)
            {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
            bool isDegreeFound = false;
            while (true)
            {

                try
                {
                    // Navigate to the Education tab if not already there
                    //  ClickAnyTab("Education");

                    // Wait for the table with education data to be visible
                    WaitUtils.WaitToBeVisible(driver, "XPath", "//div[@data-tab='third']//tbody", 10);

                    // Find the row that contains the test data
                    var rows = driver.FindElements(By.XPath("//div[@data-tab='third']//tbody/tr"));
                    foreach (var row in rows)
                    {
                        var degreeCell = row.FindElement(By.XPath("./td[4]"));
                        if (degreeCell.Text == degree)
                        {
                            isDegreeFound = true;

                            // Click the delete button in the same row as the test data
                            var deleteButton = row.FindElement(By.XPath("//div[@data-tab='third']//tbody[last()]/tr/td[6]/span[2]/i"));
                            deleteButton.Click();

                            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(By.XPath("./td[4][text()='" + degree + "']")));

                            Thread.Sleep(1000);
                            //Console.WriteLine($"Deleted test data with degree: {degree}");
                            break;
                        }
                    }
                    if (!isDegreeFound)
                    {
                        break;
                    }
                }
                catch (NoSuchElementException)
                {
                    // No more delete buttons found, break the loop
                    break;
                }
                catch (WebDriverTimeoutException)
                {
                    // Delete button not found within wait time, break the loop
                    break;
                }
            }

        }
        public void CleanEducationData()
        {
            ClickAnyTab("Education");

            while (true)
            {
                try
                {

                    //wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[3]/div/div[2]/div/table/tbody[last()]/tr/td[3]/span[2]/i")));
                    WaitUtils.WaitToBeVisible(driver, "XPath", e_deletebutton, 10);
                    // Find the delete button for the last record
                    buttonDelete.Click();
                    Thread.Sleep(6000);
                }
                catch (NoSuchElementException)
                {
                    // Break the loop if no more delete buttons are found
                    break;
                }
                catch (WebDriverTimeoutException)
                {
                    // Break the loop if the delete button is not found within the wait time
                    break;
                }
            }
        }

            private void WaitForPopupToDisappear()
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                try
                {
                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//div[@class='ns-box-inner']")));
                }
                catch (WebDriverTimeoutException)
                {
                    // Handle the case where the popup does not disappear within the timeout
                    Assert.Fail("The popup did not disappear within the expected time.");
                }
            }
        }
    
}
