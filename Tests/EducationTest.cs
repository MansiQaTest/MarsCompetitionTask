using AventStack.ExtentReports;
using CompetitionTask.Models;
using CompetitionTask.Pages;
using CompetitionTask.Utils;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CompetitionTask.Tests
{
    public class EducationTest : CommonDriver
    {
        Education educationObj;
        LoginPage loginPageObj;
        string addEduFile = @"D:\Mansi-Industryconnect\CompetitionTask\JsonData\EducationData.json";
        List<string> DegreesToDelete; // Initialize this list
        List<string> DegreeToRemove; //

        public EducationTest()
        {
            loginPageObj = new LoginPage();
            educationObj = new Education();
            DegreesToDelete = new List<string>(); // Initialize the list
            DegreeToRemove = new List<string>(); //
        }
        private void RunEducationTest(string testCaseName)
        {
            educationObj.ClickAnyTab("Education");
            test.Log(Status.Info, "Navigated to Education tab");
            test = extent.CreateTest(testCaseName).Info("Test Started");
            try
            {

                List<Models.TestCaseData2> testCases = JsonUtils.ReadJsonData<Models.TestCaseData2>(addEduFile);
                var educationTestCase = testCases.FirstOrDefault(tc => tc.TestCase == testCaseName);

                if (educationTestCase != null)
                {
                    foreach (var item in educationTestCase.Data)
                    {

                        string country = item.Country;
                        string university = item.University;
                        string title = item.Title;
                        string degree = item.Degree;
                        string gradYr = item.GraduationYear;

                        try
                        {
                            educationObj.CreateEducation(country, university, title, degree, gradYr);
                            // Add the degree to the list for cleanup if added successfully
                            DegreesToDelete.Add(degree);
                        }
                        catch (Exception ex)
                        {
                            test.Log(Status.Fail, $"Failed to add education with degree {degree}: {ex.Message}");
                            throw; // Ensure the test fails if the education could not be added
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
        private void RunEditEducationTest(string testCaseName)
        {
            educationObj.ClickAnyTab("Education");
            test.Log(Status.Info, "Navigated to Education tab");

            test = extent.CreateTest(testCaseName).Info("Test Started");
            try
            {
                List<Models.TestCaseData2> testCases = JsonUtils.ReadJsonData<Models.TestCaseData2>(addEduFile);
                var educationTestCase = testCases.FirstOrDefault(tc => tc.TestCase == testCaseName);

                if (educationTestCase != null)
                {
                    foreach (var item in educationTestCase.Data)
                    {
                        string country = item.Country;
                        string university = item.University;
                        string title = item.Title;
                        string degree = item.Degree;
                        string gradYr = item.GraduationYear;
                        try
                        {
                            educationObj.EditEducation(country, university, title, degree, gradYr);
                            // Add the new degree to the list for cleanup if edited successfully
                            DegreesToDelete.Add(degree);
                        }
                        catch (Exception ex)
                        {
                            test.Log(Status.Fail, $"Failed to edit education to degree {degree}: {ex.Message}");
                            throw; // Ensure the test fails if the education could not be edited
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
        private void RunDeleteEducationTest(string testCaseName)
        {
            educationObj.ClickAnyTab("Education");
            test.Log(Status.Info, "Navigated to Education tab");

            test = extent.CreateTest(testCaseName).Info("Test Started");
            try
            {
                List<Models.TestCaseData2> testCases = JsonUtils.ReadJsonData<Models.TestCaseData2>(addEduFile);
                var educationTestCase = testCases.FirstOrDefault(tc => tc.TestCase == testCaseName);

                if (educationTestCase != null)
                {
                    foreach (var item in educationTestCase.Data)
                    {
                        string degree = item.Degree;
                        
                        try
                        {
                            educationObj.DeleteTestData(degree);

                        }
                        catch (Exception ex)
                        {
                            test.Log(Status.Fail, $"Failed to delete education to degree {degree}: {ex.Message}");
                            throw; // Ensure the test fails if the education could not be delete
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

        [Test, Order(1), Description("user can able to Add new Education to the profile")]
        public void AddEducation()
        {
            RunEducationTest("AddEducationData");
            string degree = DegreesToDelete.First();
            try
            {
                string addedEducation = educationObj.GetEducation();
                Console.WriteLine($"Expected Degree: {degree}");
                Console.WriteLine($"Actual Degree: {addedEducation}");
                Assert.That(addedEducation == degree, "Actual Educationname and Expected Educationname do not match");
                if (string.IsNullOrEmpty(addedEducation))
                {
                    test.Log(Status.Fail, $"Test failed with exception: {addedEducation}");
                    Assert.Fail($"Expected Educationname was not added. Actual Educationname: '{addedEducation}'");
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
                educationObj.DeleteTestData(degree);
            }
        }

        [Test, Order(2), Description("user cannot create a education with an empty name")]
        public void AddEducationWithempty()
        {
            try
            {
                RunEducationTest("AddEducationDatawithempty");
                string actualErrorMessage = educationObj.GetErrorMessage();
                Assert.That(actualErrorMessage, Is.EqualTo("Please enter all the fields"), "The expected error message did not appear.");
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

        [Test, Order(3), Description("user cannot create duplicate entries for education data based on existing records")]
        public void AddEducationWithDuplicateEntry()
        {
            RunEducationTest("AddEducationData");
            string degree = DegreesToDelete.First();
            Thread.Sleep(10000);
            try
            {
                RunEducationTest("AddEducationWithDuplicateEntry");
                string actualErrorMessage = educationObj.GetErrorMessage();
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
                educationObj.DeleteTestData(degree);
            }
        }

        [Test, Order(4), Description("User can create multiple education records")]
        public void CreateMultipleData()
        {
            // Run the education test to add multiple records
            RunEducationTest("CreateMultipleData");

            // Extract the expected degrees from the JSON file
            List<string> expectedDegrees = new List<string>();
            List<Models.TestCaseData2> testCases = JsonUtils.ReadJsonData<Models.TestCaseData2>(addEduFile);
            var educationTestCase = testCases.FirstOrDefault(tc => tc.TestCase == "CreateMultipleData");

            if (educationTestCase != null)
            {
                expectedDegrees = educationTestCase.Data.Select(item => item.Degree).ToList();
            }
            else
            {
                test.Log(Status.Fail, "Test case 'CreateMultipleData' not found in JSON data.");
                Assert.Fail("Test case 'CreateMultipleData' not found in JSON data.");
            }

            // Wait for the degrees to be visible
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            var actualDegrees = new List<string>();

            for (int i = 0; i < expectedDegrees.Count; i++)
            {
                // Construct the XPath for each degree dynamically
                string degreeXPath = $"//div[@data-tab='third']//tbody[{i + 1}]/tr/td[4]";

                try
                {
                    // Wait for each degree element to be visible
                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(degreeXPath)));

                    // Get the actual degree text
                    IWebElement degreeElement = driver.FindElement(By.XPath(degreeXPath));
                    actualDegrees.Add(degreeElement.Text.Trim());
                }
                catch (NoSuchElementException)
                {
                    // Log if an element is not found
                    test.Log(Status.Fail, $"Degree element at index {i + 1} not found using XPath: {degreeXPath}");
                    Assert.Fail($"Degree element at index {i + 1} not found.");
                }
                catch (Exception ex)
                {
                    // Log any other exception
                    test.Log(Status.Fail, $"Exception occurred while getting degree element: {ex.Message}");
                    throw;
                }
            }

            // Compare actual degrees with expected degrees
            foreach (var degree in expectedDegrees)
            {
                if (!actualDegrees.Contains(degree))
                {
                    test.Log(Status.Fail, $"Expected degree '{degree}' was not found in the UI. Actual degrees: {string.Join(", ", actualDegrees)}");
                    Assert.Fail($"Expected degree '{degree}' was not found. Actual degrees: {string.Join(", ", actualDegrees)}");
                }
            }

            test.Log(Status.Pass, "All degrees were successfully created and verified.");
            TakeScreenshotWithPngFormat();


            // Perform cleanup after successful verification
            foreach (var degree in expectedDegrees)
            {
                try
                {
                    educationObj.DeleteTestData(degree);
                    test.Log(Status.Info, $"Deleted degree '{degree}' from the UI.");
                }
                catch (Exception ex)
                {
                    test.Log(Status.Fail, $"Failed to delete degree '{degree}': {ex.Message}");
                    // Continue with other deletions even if one fails
                }
            }
        }


        [Test, Order(5), Description("User can create multiple Education records with invalid input")]
        public void CreateEducationDatawithInvalidinput()
        {
            // Run the test to create education data with invalid input
            RunEducationTest("CreateEducationDatawithInvalidinput");
            string degree = DegreesToDelete.First();

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
                    actualErrorMessage = educationObj.GetErrorMessage();
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
                educationObj.DeleteTestData(degree);
            }
        }

        [Test, Order(6), Description("Clear all the data")]
       public void ClearData()
       {
           // RunEducationTest("CreateMultipleData");
            educationObj.CleanEducationData();

            var rows = driver.FindElements(By.CssSelector("div[data-tab='third'] .ui.fixed.table tbody tr"));
            Assert.That(rows.Count == 0, "All records have been successfully deleted.");
        }

        [Test, Order(7), Description("User can edit existing education data")]
        public void EditEducationData()
        {

            RunEducationTest("AddEducationData"); // First, add the education data
            string degree = DegreesToDelete.First();
            string degree2 = string.Empty;

            try
            {
                // Attempt to edit the certificate data
                RunEditEducationTest("EditEducationData");
                degree2 = DegreesToDelete.Last();

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
                        string actualErrorMessage = educationObj.GetErrorMessage();
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
                if (!string.IsNullOrEmpty(degree2))
                {
                    educationObj.DeleteTestData(degree2);
                }
                educationObj.DeleteTestData(degree);
            }
        }

           
        [Test, Order(8), Description("User can edit existing education data with empty")]
        public void EditEducationDatawithempty()
        {           

            RunEducationTest("AddEducationData"); // First, add the education data
            string degree = DegreesToDelete.First();
            string degree2 = null;
            try
            {
                RunEditEducationTest("EditEducationDatawithempty");
                degree2 = DegreesToDelete.Last();
                string actualErrorMessage = educationObj.GetErrorMessage();
                Assert.That(actualErrorMessage, Is.EqualTo("Education information was invalid"), "The expected error message did not appear.");
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
                if (!string.IsNullOrEmpty(degree2))
                {
                    educationObj.DeleteTestData(degree2);
                }
                educationObj.DeleteTestData(degree);
            }
        }

        [Test, Order(9), Description("User can edit existing education data with invalid feild")]
        public void EditEducationDatawithinvalid()
        {
            RunEducationTest("AddEducationData"); // First, add the education data
            string degree = DegreesToDelete.First();
            string degree2 = null;
            
            try
            {
                // Attempt to edit the degree data with invalid values
                RunEditEducationTest("EditEducationDatawithinvalid");
                degree2 = DegreesToDelete.Last();

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
                    string actualErrorMessage = educationObj.GetErrorMessage();
                    Assert.That(actualErrorMessage, Is.EqualTo("Education information was invalid"), "The expected error message did not appear.");
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
                if (!string.IsNullOrEmpty(degree2))
                {
                    educationObj.DeleteTestData(degree2);
                }
                educationObj.DeleteTestData(degree);
            }
        }

        [Test, Order(10), Description("User can detete existing education data")]
        public void DeleteDataWhichisinthelist()
        {
            
            try
            {
                RunEducationTest("AddEducationData");
                // Execute the delete operation
                RunDeleteEducationTest("DeleteDataWhichisinthelist");

                // Verify that the degree is no longer present
                var degreesAfterDeletion = educationObj.GetEducation(); // Assume this method retrieves all degrees from the UI
                foreach (var degree in DegreesToRemove)
                {
                    if (degreesAfterDeletion.Contains(degree))
                    {
                        test.Log(Status.Fail, $"Degree '{degree}' was not deleted successfully.");
                        Assert.Fail($"Degree '{degree}' was not deleted from the UI.");
                    }
                }

                // If all deletions are successful
                test.Log(Status.Pass, "degree were successfully deleted.");
                TakeScreenshotWithPngFormat();
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, $"An unexpected error occurred: {ex.Message}");
                throw; // Ensure the test fails if an exception occurs
            }

        }
        [Test, Order(11), Description("User cannot delete education data which is not in the list")]
        public void DeleteDataWhichIsNotInTheList()
        {
            try
            {
                // Add initial education data
                RunEducationTest("AddEducationData");
                string degree = DegreesToDelete.First();

                // Capture the list of degrees before attempting to delete non-existent data
                var degreesBeforeDeletionAttempt = educationObj.GetEducation(); // Assume this method retrieves all degrees from the UI

                // Attempt to delete non-existent education data
                RunDeleteEducationTest("DeleteDataWhichisnotinthelist");

                // Capture the list of degrees after the deletion attempt
                var degreesAfterDeletionAttempt = educationObj.GetEducation();

                // Verify that the list of degrees remains unchanged
                Assert.That(degreesAfterDeletionAttempt, Is.EquivalentTo(degreesBeforeDeletionAttempt), "The list of degrees should remain unchanged when attempting to delete non-existent data.");
                educationObj.DeleteTestData(degree);
                // Log success and take a screenshot
                test.Log(Status.Pass, "Non-existent education data was not deleted, as expected.");
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
