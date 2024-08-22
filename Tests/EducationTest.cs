using AventStack.ExtentReports;
using CompetitionTask.Models;
using CompetitionTask.Pages;
using CompetitionTask.Utils;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Newtonsoft.Json;
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
        List<string> EducationDataToCleanUp;
        List<string> jsonDataFile;
               

        public EducationTest()
        {
            loginPageObj = new LoginPage();
            educationObj = new Education();
            EducationDataToCleanUp = new List<string>(); 
            
        }
        private void RunEducationTest(string jsonDataFile)
        {
            educationObj.ClickAnyTab("Education");
            test.Log(Status.Info, "Navigated to Education tab");
            
            try
            {

                List<EducationModel> educationData = JsonUtils.ReadJsonData<EducationModel>(jsonDataFile);
                    foreach (var item in educationData)
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
                         EducationDataToCleanUp.Add(degree);
                        }
                        catch (Exception ex)
                        {
                            test.Log(Status.Fail, $"Failed to add education with degree {degree}: {ex.Message}");
                            throw; // Ensure the test fails if the education could not be added
                        }

                    }
            }
            catch (Exception e)
            {
                test.Log(Status.Fail, e.ToString());
                throw;
            }
        }
        private void RunEditEducationTest(string jsonDataFile)
        {
            educationObj.ClickAnyTab("Education");
            test.Log(Status.Info, "Navigated to Education tab");


            try
            {
                List<EducationModel> educationData = JsonUtils.ReadJsonData<EducationModel>(jsonDataFile);
                foreach (var item in educationData)
                {
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
                            EducationDataToCleanUp.Add(degree);
                        }
                        catch (Exception ex)
                        {
                            test.Log(Status.Fail, $"Failed to edit education to degree {degree}: {ex.Message}");
                            throw; // Ensure the test fails if the education could not be edited
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
        private void RunDeleteEducationTest(string jsonDataFile)
        {
            educationObj.ClickAnyTab("Education");
            test.Log(Status.Info, "Navigated to Education tab");

            
            try
            {
                List<EducationModel> educationData = JsonUtils.ReadJsonData<EducationModel>(jsonDataFile);
                foreach (var item in educationData)
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
            catch (Exception e)
            {
                test.Log(Status.Fail, e.ToString());
                throw;
            }
        }

        [Test, Order(1), Description("user can able to Add new Education to the profile")]
        public void AddEducation()
        {
            RunEducationTest(@"D:\Mansi-Industryconnect\CompetitionTask\JsonData\AddEducation.json");
            string degree = EducationDataToCleanUp.First();
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
                CommonDriver.EducationDataToCleanUp.Add(degree);
            }
        }

        [Test, Order(2), Description("user cannot create a education with an empty name")]
        public void AddEducationWithempty()
        {
            try
            {
                RunEducationTest(@"D:\Mansi-Industryconnect\CompetitionTask\JsonData\AddEducationDatawithempty.json");

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
            RunEducationTest(@"D:\Mansi-Industryconnect\CompetitionTask\JsonData\AddEducation.json");

            string degree = EducationDataToCleanUp.First();

            try
            {
                RunEducationTest(@"D:\Mansi-Industryconnect\CompetitionTask\JsonData\AddEducationWithDuplicateEntry.json");
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
                
                CommonDriver.EducationDataToCleanUp.Add(degree);
            }
        }

        [Test, Order(4), Description("User can create multiple education records")]
        public void CreateMultipleData()
        {
            
            RunEducationTest(@"D:\Mansi-Industryconnect\CompetitionTask\JsonData\CreateEduMultipleData.json");

            List<EducationModel> degrees = JsonConvert.DeserializeObject<List<EducationModel>>(File.ReadAllText(@"D:\Mansi-Industryconnect\CompetitionTask\JsonData\CreateEduMultipleData.json"));
            List<string> expectedDegrees = degrees.Select(c => c.Degree).ToList();

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

            foreach (var degree in expectedDegrees)
            {
                try
                {
                    CommonDriver.EducationDataToCleanUp.Add(degree);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }


        [Test, Order(5), Description("User can create Education records with invalid input")]
        public void CreateEducationDatawithInvalidinput()
        {
            // Run the test to create education data with invalid input
            RunEducationTest(@"D:\Mansi-Industryconnect\CompetitionTask\JsonData\CreateEducationDatawithInvalidinput.json");
            // string degree = string.Empty;
            string degree = EducationDataToCleanUp.Last();

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
                CommonDriver.EducationDataToCleanUp.Add(degree);}
            }

     

        [Test, Order(6), Description("User can edit existing education data")]
        public void EditEducationData()
        {

            RunEducationTest(@"D:\Mansi-Industryconnect\CompetitionTask\JsonData\AddEducation.json"); // First, add the education data
            string degree = EducationDataToCleanUp.First();
            string degree2 = string.Empty;

            try
            {
                // Attempt to edit the certificate data
                RunEditEducationTest(@"D:\Mansi-Industryconnect\CompetitionTask\JsonData\EditEducationData.json");
                degree2 = EducationDataToCleanUp.LastOrDefault();

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
                try
                {
                    if (!string.IsNullOrEmpty(degree2))
                    {

                        CommonDriver.EducationDataToCleanUp.Add(degree2);

                    }
                    if (!string.IsNullOrEmpty(degree))
                    {
                        CommonDriver.EducationDataToCleanUp.Add(degree);

                    }
                }
                catch (Exception ex)
                {
                    test.Log(Status.Fail, $"Failed to add certificate name for cleanup: {ex.Message}");
                }
            }
        }


        [Test, Order(7), Description("User can edit existing education data with empty")]
        public void EditEducationDatawithempty()
        {

            RunEducationTest(@"D:\Mansi-Industryconnect\CompetitionTask\JsonData\AddEducation.json"); // First, add the education data
            string degree = EducationDataToCleanUp.First();
            string degree2 = null;
            try
            {
                // Run the edit test with empty data
                RunEditEducationTest(@"D:\Mansi-Industryconnect\CompetitionTask\JsonData\EditEducationDatawithempty.json");

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
                        string actualErrorMessage = educationObj.GetErrorMessage();
                        Assert.That(actualErrorMessage, Is.EqualTo("Please enter all the fields"), "The expected error message did not appear.");
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
                    if (!string.IsNullOrEmpty(degree2))
                    {

                        CommonDriver.EducationDataToCleanUp.Add(degree2);


                    }
                    if (!string.IsNullOrEmpty(degree))
                    {
                        CommonDriver.EducationDataToCleanUp.Add(degree);

                    }
                }
                catch (Exception cleanupEx)
                {
                    test.Log(Status.Fail, $"Failed to add certificate name during cleanup: {cleanupEx.Message}");
                }
            }
        }

        [Test, Order(8), Description("User can edit existing education data with invalid feild")]
        public void EditEducationDatawithinvalid()
        {
            RunEducationTest(@"D:\Mansi-Industryconnect\CompetitionTask\JsonData\AddEducation.json"); // First, add the education data
            string degree = EducationDataToCleanUp.First();
            string degree2 = string.Empty;

            try
            {
                // Attempt to edit the degree data with invalid values
                RunEditEducationTest(@"D:\Mansi-Industryconnect\CompetitionTask\JsonData\EditEducationDatawithinvalid.json");
                degree2 = EducationDataToCleanUp.Last();

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
                try
                {
                    if (!string.IsNullOrEmpty(degree2))
                    {

                        CommonDriver.EducationDataToCleanUp.Add(degree2);

                    }
                    if (!string.IsNullOrEmpty(degree))
                    {
                        CommonDriver.EducationDataToCleanUp.Add(degree);

                    }
                }

                catch (Exception cleanupEx)

                {
                    test.Log(Status.Fail, $"Failed to add certificate name during cleanup: {cleanupEx.Message}");
                }
            }
        }

        [Test, Order(9), Description("User can detete existing education data")]
        public void DeleteDataWhichisinthelist()
        {
            
            try
            {
                RunEducationTest(@"D:\Mansi-Industryconnect\CompetitionTask\JsonData\AddEducation.json");
                // Execute the delete operation
                RunDeleteEducationTest(@"D:\Mansi-Industryconnect\CompetitionTask\JsonData\DeleteEduDataWhichisinthelist.json");

                // Verify that the degree is no longer present
                var degreesAfterDeletion = educationObj.GetEducation(); 
                foreach (var degree in CommonDriver.EducationDataToCleanUp)
                {
                    if (degreesAfterDeletion.Contains(degree))
                    {
                        test.Log(Status.Fail, $"Degree '{degree}' was not deleted successfully.");
                        Assert.Fail($"Degree '{degree}' was not deleted from the UI.");
                    }
                }

                test.Log(Status.Pass, "degree were successfully deleted.");
                TakeScreenshotWithPngFormat();
                EducationDataToCleanUp.Clear();
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, $"An unexpected error occurred: {ex.Message}");
                throw; // Ensure the test fails if an exception occurs
            }
            finally
            {
                if (CommonDriver.EducationDataToCleanUp == null || !CommonDriver.EducationDataToCleanUp.Any())
                {
                    test.Log(Status.Info, "No data to clean up.");
                }
                else
                {
                    try
                    {
                        foreach (var degree in CommonDriver.EducationDataToCleanUp)
                        {
                            test.Log(Status.Info, $"Attempting to delete degree: {degree}");
                        }
                    }
                    catch (Exception cleanupEx)
                    {
                        test.Log(Status.Fail, $"Failed during cleanup operation: {cleanupEx.Message}");
                    }
                }
            }


        }
        [Test, Order(10), Description("User cannot delete education data which is not in the list")]
        public void DeleteDataWhichIsNotInTheList()
        {
            try
            {
                // Add initial education data
                RunEducationTest(@"D:\Mansi-Industryconnect\CompetitionTask\JsonData\AddEducation.json");
                string degree = EducationDataToCleanUp.First();

                // Capture the list of degrees before attempting to delete non-existent data
                var degreesBeforeDeletionAttempt = educationObj.GetEducation(); // Assume this method retrieves all degrees from the UI

                // Attempt to delete non-existent education data
                RunDeleteEducationTest(@"D:\Mansi-Industryconnect\CompetitionTask\JsonData\DeleteEduDataWhichisnotinthelist.json");

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
