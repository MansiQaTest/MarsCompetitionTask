using AventStack.ExtentReports;
using CompetitionTask.Models;
using CompetitionTask.Pages;
using CompetitionTask.Utils;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CompetitionTask.Tests
{
    public class CertificateTest : CommonDriver
    {
        Certificate CertificateObj;
        LoginPage loginPageObj;
        string addCertFile = @"D:\Mansi-Industryconnect\CompetitionTask\JsonData\CertificateData.json";
        List<string> CertificatenamesToDelete; // Initialize this list
        List<string> CertificateToRemove; //

        public CertificateTest()
        {
            loginPageObj = new LoginPage();
            CertificateObj = new Certificate();
            CertificatenamesToDelete = new List<string>(); // Initialize the list
            CertificateToRemove = new List<string>(); //
        }

        private void RunCertificateTest(string testCaseName)
        {
            CertificateObj.ClickAnyTab("Certifications");
            test.Log(Status.Info, "Navigated to Certificate tab");
            test = extent.CreateTest(testCaseName).Info("Test Started");
            try
            {

                List<Models.TestCaseData4> testCases = JsonUtils.ReadJsonData<Models.TestCaseData4>(addCertFile);
                var CertificateTestCase = testCases.FirstOrDefault(tc => tc.TestCase == testCaseName);

                if (CertificateTestCase != null)
                {
                    foreach (var item in CertificateTestCase.Data)
                    {

                        string certificatename = item.CertificationName;
                        string certificatefrom = item.CertificationFrom;
                        string certificationYear = item.CertificationYear;

                        try
                        {
                            CertificateObj.Createcertificate(certificatename, certificatefrom, certificationYear);
                            // Add the certificatename to the list for cleanup if added successfully
                           CertificatenamesToDelete.Add(certificatename);
                        }
                        catch (Exception ex)
                        {
                            test.Log(Status.Fail, $"Failed to add Certificate with certificatename {certificatename}: {ex.Message}");
                            throw; // Ensure the test fails if the Certificate could not be added
                        }

                    }
                }
                else
                {
                    throw new Exception($"Test case {testCaseName} not found in JSON data.");
                }
            }
            catch (Exception e)
            {
                test.Log(Status.Fail, e.ToString());
                throw;
            }
        }
        private void RunEditCertificateTest(string testCaseName)
        {
            CertificateObj.ClickAnyTab("Certifications");
            test.Log(Status.Info, "Navigated to Certificate tab");
            test = extent.CreateTest(testCaseName).Info("Test Started");
            try
            {
                List<Models.TestCaseData4> testCases = JsonUtils.ReadJsonData<Models.TestCaseData4>(addCertFile);
                var CertificateTestCase = testCases.FirstOrDefault(tc => tc.TestCase == testCaseName);

                if (CertificateTestCase != null)
                {
                    foreach (var item in CertificateTestCase.Data)
                    {
                        string certificatename = item.CertificationName;
                        string certificatefrom = item.CertificationFrom;
                        string certificationYear = item.CertificationYear;
                        try
                        {
                            CertificateObj.Editcertificate(certificatename, certificatefrom, certificationYear);
                            // Add the certificatename to the list for cleanup if added successfully
                            CertificatenamesToDelete.Add(certificatename);
                        }
                        catch (Exception ex)
                        {
                            test.Log(Status.Fail, $"Failed to edit Certificate to certificatename {certificatename}: {ex.Message}");
                            throw; // Ensure the test fails if the Certificate could not be edited
                        }
                    }
                }
                else
                {
                    throw new Exception($"Test case {testCaseName} not found in JSON data.");
                }
            }
            catch (Exception e)
            {
                test.Log(Status.Fail, e.ToString());
                throw;
            }
        }
        private void RunDeleteCertificateTest(string testCaseName)
        {
            CertificateObj.ClickAnyTab("Certifications");
            test.Log(Status.Info, "Navigated to Certificate tab");

            test = extent.CreateTest(testCaseName).Info("Test Started");
            try
            {
                List<Models.TestCaseData4> testCases = JsonUtils.ReadJsonData<Models.TestCaseData4>(addCertFile);
                var CertificateTestCase = testCases.FirstOrDefault(tc => tc.TestCase == testCaseName);

                if (CertificateTestCase != null)
                {
                    foreach (var item in CertificateTestCase.Data)
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
                else
                {
                    throw new Exception($"Test case {testCaseName} not found in JSON data.");
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
            RunCertificateTest("AddCertificateData");
            string certificatename = CertificatenamesToDelete.First();
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
                // Clean up test data
                CertificateObj.DeleteTestData(certificatename);
            }
        }

        [Test, Order(2), Description("user cannot create a Certificate with an empty name")]
        public void AddCertificateWithempty()
        {
            try
            {
                RunCertificateTest("AddCertificateDatawithempty");
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
            RunCertificateTest("AddCertificateData");
            string certificatename = CertificatenamesToDelete.First();
            Thread.Sleep(10000);
            try
            {
                RunCertificateTest("AddCertificateWithDuplicateEntry");
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
                // Clean up test data
                CertificateObj.DeleteTestData(certificatename);
            }

        }


        [Test, Order(4), Description("User can create multiple Certificate records")]
        public void CreateMultipleData()
        {
            // Run the Certificate test to add multiple records
            RunCertificateTest("CreateMultipleData");

            // Extract the expected degrees from the JSON file
            List<string> expectedCerificateName = new List<string>();
            List<Models.TestCaseData4> testCases = JsonUtils.ReadJsonData<Models.TestCaseData4>(addCertFile);
            var CertificateTestCase = testCases.FirstOrDefault(tc => tc.TestCase == "CreateMultipleData");

            if (CertificateTestCase != null)
            {
                expectedCerificateName = CertificateTestCase.Data.Select(item => item.CertificationName).ToList();
            }
            else
            {
                test.Log(Status.Fail, "Test case 'CreateMultipleData' not found in JSON data.");
                Assert.Fail("Test case 'CreateMultipleData' not found in JSON data.");
            }

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


            // Perform cleanup after successful verification
            foreach (var certificatename in expectedCerificateName)
            {
                try
                {
                    CertificateObj.DeleteTestData(certificatename);
                    test.Log(Status.Info, $"Deleted certificatename '{certificatename}' from the UI.");
                }
                catch (Exception ex)
                {
                    test.Log(Status.Fail, $"Failed to delete certificatename '{certificatename}': {ex.Message}");
                    // Continue with other deletions even if one fails
                }
            }
        }


        [Test, Order(5), Description("User can create multiple Certificate records with invalid input")]
        public void CreateCertificateDatawithInvalidinput()
        {
            // Run the test to create certificate data with invalid input
            RunCertificateTest("CreateCertificateDatawithInvalidinput");
            string certificatename = CertificatenamesToDelete.First();

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
                // Clean up test data
                CertificateObj.DeleteTestData(certificatename);
            }
        }

        [Test, Order(6), Description("Clear all the data")]
        public void ClearData()
        {
           // RunCertificateTest("CreateMultipleData");
            CertificateObj.CleancertificateData();

            var rows = driver.FindElements(By.CssSelector("div[data-tab='third'] .ui.fixed.table tbody tr"));
            Assert.That(rows.Count == 0, "All records have been successfully deleted.");
        }

        [Test, Order(7), Description("User can edit existing Certificate data")]
        public void EditCertificateData()
        {
            // First, add the Certificate data
            RunCertificateTest("AddCertificateData");
            string certificatename = CertificatenamesToDelete.First();
            string certificatename2 = string.Empty;

            try
            {
                // Attempt to edit the certificate data
                RunEditCertificateTest("EditCertificateData");
                certificatename2 = CertificatenamesToDelete.Last();

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
                // Clean up test data
                if (!string.IsNullOrEmpty(certificatename2))
                {
                    CertificateObj.DeleteTestData(certificatename2);
                }
                CertificateObj.DeleteTestData(certificatename);
            }
        }

        [Test, Order(8), Description("User can edit existing Certificate data with empty")]
        public void EditCertificateDatawithempty()
        {
            // First, add the Certificate data
            RunCertificateTest("AddCertificateData");
            string certificatename = CertificatenamesToDelete.First();
            string certificatename2 = null;
            try
            {
                RunCertificateTest("EditCertificateDatawithempty");
                certificatename2 = CertificatenamesToDelete.Last();
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
            finally
            {
                if (!string.IsNullOrEmpty(certificatename2))
                {
                    CertificateObj.DeleteTestData(certificatename2);
                }
                CertificateObj.DeleteTestData(certificatename);
            }
        }


        [Test, Order(9), Description("User can edit existing Certificate data with invalid feild")]
        public void EditCertificateDatawithinvalid()
        {
            RunCertificateTest("AddCertificateData");
            string certificatename = CertificatenamesToDelete.First();
            string certificatename2 = null;
            // Assuming the new certificate is the last one added
            try
            {
                // Attempt to edit the certificate data with empty values
                RunEditCertificateTest("EditCertificateDatawithinvalid");
                certificatename2 = CertificatenamesToDelete.Last();

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
                // Clean up test data
                if (!string.IsNullOrEmpty(certificatename2))
                {
                    CertificateObj.DeleteTestData(certificatename2);
                }
                CertificateObj.DeleteTestData(certificatename);
            }
        }
        [Test, Order(10), Description("User can detete existing Certificate data")]
        public void DeleteDataWhichisinthelist()
        {

            try
            {
                RunCertificateTest("AddCertificateData");
                // Execute the delete operation
                RunDeleteCertificateTest("DeleteDataWhichisinthelist");

                // Verify that the certificate is no longer present
                var certificatesAfterDeletion = CertificateObj.GetCertificate(); // Assume this method retrieves all certificate from the UI
                foreach (var certificatename in CertificatenamesToDelete)
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
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, $"An unexpected error occurred: {ex.Message}");
                throw; // Ensure the test fails if an exception occurs
            }

        }
        [Test, Order(11), Description("User cannot delete Certificate data which is not in the list")]
        public void DeleteDataWhichIsNotInTheList()
        {
            try
            {
                // Add initial Certificate data
                RunCertificateTest("AddCertificateData");
                string certificatename = CertificatenamesToDelete.First();

                // Capture the list of certificate before attempting to delete non-existent data
                var certificatesBeforeDeletionAttempt = CertificateObj.GetCertificate(); // Assume this method retrieves all certificate from the UI

                // Attempt to delete non-existent Certificate data
                RunDeleteCertificateTest("DeleteDataWhichisnotinthelist");

                // Capture the list of certificate after the deletion attempt
                var certificatesAfterDeletionAttempt = CertificateObj.GetCertificate();

                // Verify that the list of certificate remains unchanged
                Assert.That(certificatesAfterDeletionAttempt, Is.EquivalentTo(certificatesBeforeDeletionAttempt), "The list of ce` should remain unchanged when attempting to delete non-existent data.");
                CertificateObj.DeleteTestData(certificatename);
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
