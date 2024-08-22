using AventStack.ExtentReports;
using CompetitionTask.Models;
using CompetitionTask.Pages;
using CompetitionTask.Utils;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Newtonsoft.Json;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CompetitionTask.Tests
{
    [TestFixture]
    public class CertificateTest : CommonDriver
    {
        Certificate CertificateObj;
        LoginPage loginPageObj;
        List<string> CertificateDataToCleanUp;
        List<string> jsonDataFile;

        public CertificateTest()
        {
            loginPageObj = new LoginPage();
            CertificateObj = new Certificate();
            CertificateDataToCleanUp = new List<string>();
        }

        private void RunCertificateTest(string jsonDataFile)
        {
            
            CertificateObj.ClickAnyTab("Certifications");
            test.Log(Status.Info, "Navigated to Certificate tab");

            try
            {
                List<CertificationModel> CertificateData = JsonUtils.ReadJsonData<CertificationModel>(jsonDataFile);

                foreach (var item in CertificateData)
                {

                    string certificatename = item.CertificationName;
                    string certificatefrom = item.CertificationFrom;
                    string certificationYear = item.CertificationYear;

                    try
                    {
                        CertificateObj.Createcertificate(certificatename, certificatefrom, certificationYear);
                        // Add the certificatename to the list for cleanup if added successfully
                        CertificateDataToCleanUp.Add(certificatename);
                    }
                    catch (Exception ex)
                    {
                        test.Log(Status.Fail, $"Failed to add Certificate with certificatename {certificatename}: {ex.Message}");
                        throw; // Ensure the test fails if the Certificate could not be added
                    }

                }
            }
            catch (Exception e)
            {
                test.Log(Status.Fail, e.ToString());
                throw;
            }
        }
        private void RunEditCertificateTest(string jsonDataFile)
        {
            CertificateObj.ClickAnyTab("Certifications");
            test.Log(Status.Info, "Navigated to Certificate tab");
            try
            {
                List<CertificationModel> CertificateData = JsonUtils.ReadJsonData<CertificationModel>(jsonDataFile);

                foreach (var item in CertificateData)
                {
                    {
                        string certificatename = item.CertificationName;
                        string certificatefrom = item.CertificationFrom;
                        string certificationYear = item.CertificationYear;
                        try
                        {
                            CertificateObj.Editcertificate(certificatename, certificatefrom, certificationYear);
                            // Add the certificatename to the list for cleanup if added successfully
                            CertificateDataToCleanUp.Add(certificatename);
                        }
                        catch (Exception ex)
                        {
                            test.Log(Status.Fail, $"Failed to edit Certificate to certificatename {certificatename}: {ex.Message}");
                            throw; // Ensure the test fails if the Certificate could not be edited
                        }
                    }
                }
            }
            catch (Exception e)
            {
                test.Log(Status.Fail, e.ToString());
                throw;
            }
        }
        private void RunDeleteCertificateTest(string jsonDataFile)
        {
           
            CertificateObj.ClickAnyTab("Certifications");
            test.Log(Status.Info, "Navigated to Certificate tab");

            try
            {
                List<CertificationModel> CertificateData = JsonUtils.ReadJsonData<CertificationModel>(jsonDataFile);

                foreach (var item in CertificateData) 
                { 
                    string certificatename = item.CertificationName;

                        try
                        {
                            CertificateObj.DeleteTestData(certificatename);

                        }
                        catch (Exception ex)
                        {
                            test.Log(Status.Fail, $"Failed to delete Certificate to degree {certificatename}: {ex.Message}");
                            throw; // Ensure the test fails if the Certificate could not be delete
                        }
                    
                }
            }
            catch (Exception e)
            {
                test.Log(Status.Fail, e.ToString());
                throw;
            }
        }
     

        [Test, Order(1), Description("user can able to Add new Certificate to the profile")]
        public void AddCertificate()
        {
            RunCertificateTest(@"D:\Mansi-Industryconnect\CompetitionTask\JsonData\AddCertificate.json");
            
            string certificatename = CertificateDataToCleanUp.First();
            try
            {
                string addedCertificate = CertificateObj.GetCertificate();
                Console.WriteLine($"Expected Degree: {certificatename}");
                Console.WriteLine($"Actual Degree: {addedCertificate}");
                Assert.That(addedCertificate == certificatename, "Actual certificatename and Expected certificatename do not match");
                if (string.IsNullOrEmpty(addedCertificate))
                {
                    test.Log(Status.Fail, $"Test failed with exception: {addedCertificate}");
                    Assert.Fail($"Expected certificatename was not added. Actual certificatename: '{addedCertificate}'");
                }
                else
                {
                    test.Log(Status.Pass, "Test passed successfully");
                    TakeScreenshotWithPngFormat();
                }
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, $"An unexpected error occurred: {ex.Message}");
                throw;
            }
            finally
            {
                CommonDriver.CertificateDataToCleanUp.Add(certificatename);

            }
        }

        [Test, Order(2), Description("user cannot create a Certificate with an empty name")]
        public void AddCertificateWithempty()
        {
            try
            {
               
                RunCertificateTest(@"D:\Mansi-Industryconnect\CompetitionTask\JsonData\AddCertificateDatawithempty.json");
             
                string actualErrorMessage = CertificateObj.GetErrorMessage();
                Assert.That(actualErrorMessage, Is.EqualTo("Please enter Certification Name, Certification From and Certification Year"), "The expected error message did not appear.");
                test.Log(Status.Pass, "Test passed successfully");
                TakeScreenshotWithPngFormat();
            }
            catch (WebDriverTimeoutException ex)
            {
                test.Log(Status.Fail, $"Timed out waiting for the error message: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, $"An unexpected error occurred: {ex.Message}");
                throw;
            }
        }

        [Test, Order(3), Description("user cannot create duplicate entries for Certificate data based on existing records")]
        public void AddCertificateWithDuplicateEntry()
        {

            RunCertificateTest(@"D:\Mansi-Industryconnect\CompetitionTask\JsonData\AddCertificate.json");
            string certificatename = CertificateDataToCleanUp.First();
            Thread.Sleep(10000);
            try
            {

                RunCertificateTest(@"D:\Mansi-Industryconnect\CompetitionTask\JsonData\AddCertificateWithDuplicateEntry.json");

                string actualErrorMessage = CertificateObj.GetErrorMessage();
                Assert.That(actualErrorMessage, Is.EqualTo("This information is already exist."), "The expected error message did not appear.");
                if (string.IsNullOrEmpty(actualErrorMessage))
                {
                    test.Log(Status.Fail, $"Test failed with exception: {actualErrorMessage}");
                    Assert.Fail($"Expected message was not same. Actual message: '{actualErrorMessage}'");
                }
                else
                {
                    test.Log(Status.Pass, "Test passed successfully");
                    TakeScreenshotWithPngFormat();
                }
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, $"An unexpected error occurred: {ex.Message}");
                throw;
            }
            finally
            {
                CommonDriver.CertificateDataToCleanUp.Add(certificatename); 
            }

        }


        [Test, Order(4), Description("User can create multiple Certificate records")]
        public void CreateMultipleData()
        {
          

            RunCertificateTest(@"D:\Mansi-Industryconnect\CompetitionTask\JsonData\CreateMultipleCertificateData.json");

            List<CertificationModel> certificates = JsonConvert.DeserializeObject<List<CertificationModel>>(File.ReadAllText(@"D:\Mansi-Industryconnect\CompetitionTask\JsonData\CreateMultipleCertificateData.json"));
            List<string> expectedCerificateName = certificates.Select(c => c.CertificationName).ToList();
            // Wait for the degrees to be visible
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            var actualCertificateName = new List<string>();

            for (int i = 0; i < expectedCerificateName.Count; i++)
            {
                // Construct the XPath for each certificatename dynamically
                string certificatenameXPath = $"//div[@data-tab='fourth']//tbody[{i + 1}]/tr/td[1]";

                try
                {
                    // Wait for each degree element to be visible
                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(certificatenameXPath)));

                    // Get the actual degree text
                    IWebElement certificatenameElement = driver.FindElement(By.XPath(certificatenameXPath));
                    actualCertificateName.Add(certificatenameElement.Text.Trim());
                }
                catch (NoSuchElementException)
                {
                    // Log if an element is not found
                    test.Log(Status.Fail, $"CertificationName element at index {i + 1} not found using XPath: {certificatenameXPath}");
                    Assert.Fail($"CertificationName element at index {i + 1} not found.");
                }
                catch (Exception ex)
                {
                    // Log any other exception
                    test.Log(Status.Fail, $"Exception occurred while getting certificatename element: {ex.Message}");
                    throw;
                }
            }

            // Compare actual degrees with expected degrees
            foreach (var certificatename in expectedCerificateName)
            {
                if (!actualCertificateName.Contains(certificatename))
                {
                    test.Log(Status.Fail, $"Expected certificatename '{certificatename}' was not found in the UI. Actual certificatename: {string.Join(", ", actualCertificateName)}");
                    Assert.Fail($"Expected certificatename '{certificatename}' was not found. Actual certificatenames: {string.Join(", ", actualCertificateName)}");
                }
            }

            test.Log(Status.Pass, "All certificate were successfully created and verified.");
            TakeScreenshotWithPngFormat();


            
            foreach (var certificatename in expectedCerificateName)
            {
                try
                {
                    CommonDriver.CertificateDataToCleanUp.Add(certificatename);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }


        [Test, Order(5), Description("User can create Certificate records with invalid input")]
        public void CreateCertificateDatawithInvalidinput()
        {
            // Run the test to create certificate data with invalid input
            RunCertificateTest(@"D:\Mansi-Industryconnect\CompetitionTask\JsonData\CreateCertificateDatawithInvalidinput.json");
            string certificatename = CertificateObj.GetCertificate();

            try
            {
                // Wait for the messages to appear
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                // Check if a success message is displayed
                try
                {
                    if (wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//div[@class='ns-box ns-growl ns-effect-jelly ns-type-success ns-show']"))) != null)
                    {
                        Assert.Fail("Success message displayed instead of error message.");
                    }
                }
                catch (WebDriverTimeoutException)
                {
                    // Expected outcome, as we are looking for an error message
                }

                // Wait for the error message to be visible
                string actualErrorMessage = string.Empty;
                try
                {
                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//div[@class='ns-box ns-growl ns-effect-jelly ns-type-error ns-show']")));
                    actualErrorMessage = CertificateObj.GetErrorMessage();
                }
                catch (WebDriverTimeoutException)
                {
                    Assert.Fail("Error message not displayed within the timeout period.");
                }

                // Assert the error message
                Assert.That(actualErrorMessage, Is.EqualTo("Please enter Valid data"), "The expected error message did not appear.");

                test.Log(Status.Pass, "Test passed successfully, error message is displayed.");
                TakeScreenshotWithPngFormat();

               
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, $"An unexpected error occurred: {ex.Message}");
                throw;
            }
           finally
            {
                CommonDriver.CertificateDataToCleanUp.Add(certificatename);
           }
        }

        [Test, Order(6), Description("User can edit existing Certificate data")]
        public void EditCertificateData()
        {
            // Run the test to add a certificate
            RunCertificateTest(@"D:\Mansi-Industryconnect\CompetitionTask\JsonData\AddCertificate.json");

            // Get the first certificate name that was added
            string certificatename = CertificateDataToCleanUp.FirstOrDefault();
            string certificatename2 = string.Empty;

            try
            {
                // Run the test to edit the certificate
                RunEditCertificateTest(@"D:\Mansi-Industryconnect\CompetitionTask\JsonData\EditCertificateData.json");

                // Get the last certificate name after editing
                certificatename2 = CertificateDataToCleanUp.LastOrDefault();

                // Wait for the messages to appear
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                try
                {
                    // Check if a success message is displayed
                    var successElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//div[@class='ns-box ns-growl ns-effect-jelly ns-type-success ns-show']")));
                    if (successElement != null)
                    {
                        // Assert the success message is displayed
                        string successMessage = successElement.Text;
                        Assert.That(successMessage, Is.Not.Empty, "Success message should not be empty.");
                        test.Log(Status.Pass, "Success message displayed as expected.");
                        TakeScreenshotWithPngFormat();
                        return; // Exit early since the test should pass
                    }
                }
                catch (WebDriverTimeoutException)
                {
                    // Expected outcome if success message is not displayed
                }

                try
                {
                    // Check if an error message is displayed
                    var errorElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//div[@class='ns-box ns-growl ns-effect-jelly ns-type-error ns-show']")));
                    if (errorElement != null)
                    {
                        // Retrieve and assert the error message
                        string actualErrorMessage = CertificateObj.GetErrorMessage();
                        Assert.Fail($"Error message displayed: {actualErrorMessage}. Test failed as success was expected.");
                    }
                }
                catch (WebDriverTimeoutException)
                {
                    // Expected outcome if error message is not displayed
                }

                // If neither message is found
                test.Log(Status.Fail, "Neither success nor error message was displayed as expected.");
                Assert.Fail("Neither success nor error message was displayed as expected.");
            }
            catch (WebDriverTimeoutException ex)
            {
                test.Log(Status.Fail, $"Timed out waiting for the message: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, $"An unexpected error occurred: {ex.Message}");
                throw;
            }
            finally
            {
                
                try
                {
                    if (!string.IsNullOrEmpty(certificatename2))
                    {
                       
                        CommonDriver.CertificateDataToCleanUp.Add(certificatename2);

                    }
                    if (!string.IsNullOrEmpty(certificatename))
                    {
                        CommonDriver.CertificateDataToCleanUp.Add(certificatename);
                        
                    }
                }
                catch (Exception ex)
                {
                    test.Log(Status.Fail, $"Failed to add certificate name for cleanup: {ex.Message}");
                }
            }
        }


        [Test, Order(7), Description("User can edit existing Certificate data with empty")]
        public void EditCertificateDatawithempty()
        {
            // First, add the Certificate data
            RunCertificateTest(@"D:\Mansi-Industryconnect\CompetitionTask\JsonData\AddCertificate.json");

            string certificatename = CertificateDataToCleanUp.FirstOrDefault();
            string certificatename2 = null;

            try
            {
                // Run the edit test with empty data
                RunEditCertificateTest(@"D:\Mansi-Industryconnect\CompetitionTask\JsonData\EditCertificateDatawithempty.json");

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                try
                {
                    // Check if a success message is displayed
                    var successElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//div[@class='ns-box ns-growl ns-effect-jelly ns-type-success ns-show']")));
                    if (successElement != null)
                    {
                        Assert.Fail("Success message displayed instead of error message.");
                    }
                }
                catch (WebDriverTimeoutException)
                {
                    // Expected outcome if success message is not displayed
                }

                try
                {
                    // Wait for the error message to be visible
                    var errorElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//div[@class='ns-box ns-growl ns-effect-jelly ns-type-error ns-show']")));
                    if (errorElement != null)
                    {
                        // Retrieve and validate the error message
                        string actualErrorMessage = CertificateObj.GetErrorMessage();
                        Assert.That(actualErrorMessage, Is.EqualTo("Please enter Certification Name, Certification From and Certification Year"), "The expected error message did not appear.");
                        test.Log(Status.Pass, "Test passed successfully, error message is displayed.");
                        TakeScreenshotWithPngFormat();
                    }
                }
                catch (WebDriverTimeoutException)
                {
                    Assert.Fail("Error message not displayed within the timeout period.");
                }
            }
            catch (Exception ex)
            {
                // Log any other unexpected exception
                test.Log(Status.Fail, $"An unexpected error occurred: {ex.Message}");
                TakeScreenshotWithPngFormat();
                throw;
            }
            finally
            {
                
                try
                {
                    if (!string.IsNullOrEmpty(certificatename2))
                    {
                       
                        CommonDriver.CertificateDataToCleanUp.Add(certificatename2);

                        
                    }
                    if (!string.IsNullOrEmpty(certificatename))
                    {
                        CommonDriver.CertificateDataToCleanUp.Add(certificatename);

                    }
                }
                catch (Exception cleanupEx)
                {
                    test.Log(Status.Fail, $"Failed to add certificate name during cleanup: {cleanupEx.Message}");
                }
            }
        }


        [Test, Order(8), Description("User can edit existing Certificate data with invalid feild")]
        public void EditCertificateDatawithinvalid()
        {

            RunCertificateTest(@"D:\Mansi-Industryconnect\CompetitionTask\JsonData\AddCertificate.json");


            string certificatename = CertificateDataToCleanUp.First();
            string certificatename2 = string.Empty;
            // Assuming the new certificate is the last one added
            try
            {
                // Attempt to edit the certificate data with empty values

                RunEditCertificateTest(@"D:\Mansi-Industryconnect\CompetitionTask\JsonData\EditCertificateDatawithinvalid.json");

                certificatename2 = CertificateDataToCleanUp.Last();

                // Wait for the messages to appear
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                try
                {
                    // Check if a success message is displayed
                    if (wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//div[@class='ns-box ns-growl ns-effect-jelly ns-type-success ns-show']"))) != null)
                    {
                        Assert.Fail("Success message displayed instead of error message.");
                    }
                }
                catch (WebDriverTimeoutException)
                {
                    // Expected outcome, as we are looking for an error message
                }

                try
                {
                    // Wait for the error message to be visible
                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//div[@class='ns-box ns-growl ns-effect-jelly ns-type-error ns-show']")));
                    // Retrieve and validate the error message
                    string actualErrorMessage = CertificateObj.GetErrorMessage();
                    Assert.That(actualErrorMessage, Is.EqualTo("Certificate information was invalid"), "The expected error message did not appear.");
                    test.Log(Status.Pass, "Test passed successfully, error message is displayed.");
                }
                catch (WebDriverTimeoutException)
                {
                    Assert.Fail("Error message not displayed within the timeout period.");
                }

                TakeScreenshotWithPngFormat();
            }
            catch (WebDriverTimeoutException ex)
            {
                test.Log(Status.Fail, $"Timed out waiting for the error message: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, $"An unexpected error occurred: {ex.Message}");
                throw;
            }
            finally
            {
               
                try
                {
                    if (!string.IsNullOrEmpty(certificatename2))
                    {
                        
                        CommonDriver.CertificateDataToCleanUp.Add(certificatename2);

                    }
                    if (!string.IsNullOrEmpty(certificatename))
                    {
                        CommonDriver.CertificateDataToCleanUp.Add(certificatename);

                    }
                }
                catch (Exception cleanupEx)
                {
                    test.Log(Status.Fail, $"Failed to add certificate name during cleanup: {cleanupEx.Message}");
                }
            }
        }
        [Test, Order(9), Description("User can detete existing Certificate data")]
        public void DeleteDataWhichisinthelist()
        {

            try
            {
                RunCertificateTest(@"D:\Mansi-Industryconnect\CompetitionTask\JsonData\AddCertificate.json");


                // Execute the delete operation

                RunDeleteCertificateTest(@"D:\Mansi-Industryconnect\CompetitionTask\JsonData\DeleteCertDataWhichisinthelist.json");
                

                // Verify that the certificate is no longer present
                var certificatesAfterDeletion = CertificateObj.GetCertificate(); // Assume this method retrieves all certificate from the UI
                foreach (var certificatename in CommonDriver.CertificateDataToCleanUp)
                {
                    if (certificatesAfterDeletion.Contains(certificatename))
                    {
                        test.Log(Status.Fail, $"CertificationName '{certificatename}' was not deleted successfully.");
                        Assert.Fail($"CertificationName '{certificatename}' was not deleted from the UI.");
                    }
                }

                // If all deletions are successful
                test.Log(Status.Pass, "certificate were successfully deleted.");
                TakeScreenshotWithPngFormat();
                CertificateDataToCleanUp.Clear();

            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, $"An unexpected error occurred: {ex.Message}");
                throw; // Ensure the test fails if an exception occurs
            }
            finally
            {

                // If there is no data to clean up, simply log that there's nothing to clean
                if (CommonDriver.CertificateDataToCleanUp == null || !CommonDriver.CertificateDataToCleanUp.Any())
                {
                    test.Log(Status.Info, "No data to clean up.");
                }
                else
                {
                    try
                    {
                        // Perform cleanup operation if there are items to clean up
                        foreach (var certificatename in CommonDriver.CertificateDataToCleanUp)
                        {
                            // Call a method to delete the specific degree if necessary
                            // For example:
                            // DeleteDegree(degree);
                            // Make sure the method handles cases where the degree does not exist

                            // Optionally log each deletion
                            test.Log(Status.Info, $"Attempting to delete degree: {certificatename}");
                        }
                    }
                    catch (Exception cleanupEx)
                    {
                        test.Log(Status.Fail, $"Failed during cleanup operation: {cleanupEx.Message}");
                    }
                }
            }

        }
        [Test, Order(10), Description("User cannot delete Certificate data which is not in the list")]
        public void DeleteDataWhichIsNotInTheList()
        {
            try
            {
                // Add initial Certificate data


                RunCertificateTest(@"D:\Mansi-Industryconnect\CompetitionTask\JsonData\AddCertificate.json");


                string certificatename = CertificateDataToCleanUp.First();

                // Capture the list of certificate before attempting to delete non-existent data
                var certificatesBeforeDeletionAttempt = CertificateObj.GetCertificate(); // Assume this method retrieves all certificate from the UI

                // Attempt to delete non-existent Certificate data


                RunDeleteCertificateTest(@"D:\Mansi-Industryconnect\CompetitionTask\JsonData\DeleteCertDataWhichisnotinthelist.json");

                // Capture the list of certificate after the deletion attempt
                var certificatesAfterDeletionAttempt = CertificateObj.GetCertificate();

                // Verify that the list of certificate remains unchanged
                Assert.That(certificatesAfterDeletionAttempt, Is.EquivalentTo(certificatesBeforeDeletionAttempt), "The list of ce` should remain unchanged when attempting to delete non-existent data.");
                CommonDriver.CertificateDataToCleanUp.Add(certificatename);

                // Log success and take a screenshot
                test.Log(Status.Pass, "Non-existent Certificate data was not deleted, as expected.");
                TakeScreenshotWithPngFormat();
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, $"An unexpected error occurred: {ex.Message}");
                throw; // Ensure the test fails if an exception occurs
            }
        }
    }
}
