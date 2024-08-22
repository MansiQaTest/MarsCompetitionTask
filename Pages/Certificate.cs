using CompetitionTask.Utils;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompetitionTask.Pages
{
    public class Certificate : CommonDriver
    {
        private IWebElement tabOption => driver.FindElement(By.XPath("//div[@class = 'ui top attached tabular menu']/a[4]"));
        private IWebElement addNewCerBtn => driver.FindElement(By.XPath(e_addedubutton));
        private IWebElement addCertName => driver.FindElement(By.XPath(e_addCertName));
        private IWebElement addCertFrom => driver.FindElement(By.XPath(e_addCertFrom));
        private IWebElement addYrOfCert => driver.FindElement(By.XPath("//div[@data-tab='fourth']//select[@name='certificationYear']"));
        private IWebElement addCertButton => driver.FindElement(By.XPath(e_AddButton));
        private IWebElement errormessage => driver.FindElement(By.XPath(e_errormessage));
        private IWebElement successmessage => driver.FindElement(By.XPath(e_successmessage));
        private IWebElement Certificatedata => driver.FindElement(By.XPath(e_certificatedata));
        private IWebElement buttonDelete => driver.FindElement(By.XPath(e_deletebutton));
        private IWebElement editCertButton => driver.FindElement(By.XPath(e_editCertButton));
        private IWebElement buttonUpdate => driver.FindElement(By.XPath(e_updateButton));
        private IWebElement certificateTable => driver.FindElement(By.XPath("//div[@data-tab='fourth']//tbody"));
        private IWebElement cancelButton => driver.FindElement(By.XPath(e_cancelButton));





        private string e_addedubutton = "//div[@data-tab='fourth']//div[@class ='ui teal button ']";
        private string e_addCertName = "//input[contains(@placeholder, 'Certificate or Award')]";
        private string e_addCertFrom = "//input[contains(@placeholder, 'Certified From (e.g. Adobe)')]";
        private string e_AddButton = "//input[@value='Add']";
        private string e_waitForTab = "//div[@class = 'ui top attached tabular menu']/a[4]";
        private string e_errormessage = "//div[@class='ns-box ns-growl ns-effect-jelly ns-type-error ns-show']";
        private string e_successmessage = "//div[@class='ns-box ns-growl ns-effect-jelly ns-type-success ns-show']";
        private string e_certificatedata = "//div[@data-tab='fourth']//tbody[last()]/tr/td[1]";
        private string e_deletebutton = "//div[@data-tab='fourth']//tbody[last()]/tr[1]/td[4]/span[2]/i";
        private string e_editCertButton = "//div[@data-tab='fourth']//tbody[last()]//tr[1]/td[4]/span[1]/i";
        private string e_updateButton = "//input[@value='Update']";
        private string e_cancelButton = "//input[@value='Cancel']";




        public void Createcertificate(string certificatename, string certificatefrom, string certificationYear)
        {
            WaitUtils.WaitToBeClickable(driver, "XPath", e_addedubutton, 10);

            addNewCerBtn.Click();

            WaitUtils.WaitToBeClickable(driver, "XPath", e_addCertName, 10);
            addCertName.SendKeys(certificatename);

            WaitUtils.WaitToBeClickable(driver, "XPath", e_addCertFrom, 10);
            addCertFrom.SendKeys(certificatefrom);

            var selectYearDropdown = new SelectElement(addYrOfCert);
            selectYearDropdown.SelectByValue(certificationYear);

            WaitUtils.WaitToBeClickable(driver, "XPath", e_AddButton, 10);

            addCertButton.Click();
            Thread.Sleep(1000);
            bool isErrorDisplayed = driver.FindElements(By.XPath("//div[@class='ns-box ns-growl ns-effect-jelly ns-type-error ns-show']")).Count > 0;

            if (isErrorDisplayed)
            {
                // If an error is displayed, click the 'Cancel' button
                WaitUtils.WaitToBeClickable(driver, "XPath", e_cancelButton, 10);
                cancelButton.Click();
                Console.WriteLine("An error occurred while adding certificate. Cancelled the operation.");
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

        public string GetCertificate()
        {

            try
            {

                WaitUtils.WaitToBeVisible(driver, "XPath", e_certificatedata, 10);
                string certificateText = Certificatedata.Text;
                return Certificatedata.Text;
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

        public void Editcertificate(string certificatename, string certificatefrom, string certificationYear)
        {

            WaitUtils.WaitToBeClickable(driver, "XPath", e_editCertButton, 20);
            editCertButton.Click();



            WaitUtils.WaitToBeClickable(driver, "XPath", e_addCertName, 10);
            addCertName.Clear();
            addCertName.SendKeys(certificatename);

            WaitUtils.WaitToBeClickable(driver, "XPath", e_addCertFrom, 10);
            addCertFrom.Clear();
            addCertFrom.SendKeys(certificatefrom);

            var selectYearDropdown = new SelectElement(addYrOfCert);
            selectYearDropdown.SelectByValue(certificationYear);

            WaitUtils.WaitToBeClickable(driver, "XPath", e_updateButton, 10);
            buttonUpdate.Click();



        }

        public void DeleteTestData(string certificatename)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
            bool isCertificateFound = false;
            while (true)
            {

                try
                {


                    // Wait for the table with certificate data to be visible
                    WaitUtils.WaitToBeVisible(driver, "XPath", "//div[@data-tab='fourth']//tbody", 10);

                    // Find the row that contains the test data
                    var rows = driver.FindElements(By.XPath("//div[@data-tab='fourth']//tbody/tr"));
                    foreach (var row in rows)
                    {
                        var certificatenameCell = row.FindElement(By.XPath("./td[1]"));
                        if (certificatenameCell.Text == certificatename)
                        {
                            isCertificateFound = true;

                            // Click the delete button in the same row as the test data
                            var deleteButton = row.FindElement(By.XPath("//div[@data-tab='fourth']//tbody[last()]/tr[1]/td[4]/span[2]/i"));
                            deleteButton.Click();
                            Thread.Sleep(2000);
                            //Console.WriteLine($"Deleted test data with certificatename: {certificatename}");
                            break;
                        }
                    }
                    if (!isCertificateFound)
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
        public void CleancertificateData()
        {
            ClickAnyTab("Certificates");

            while (true)
            {
                try
                {

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
    }
}
