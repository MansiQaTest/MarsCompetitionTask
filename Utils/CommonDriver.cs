using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using CompetitionTask.Pages;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.IO;


namespace CompetitionTask.Utils
{
    public class CommonDriver
    {
        CommonDriver commonDriverObj;
        LoginPage loginPageObj;
        Education educationObj;
        Certificate certificateObj;

        public static IWebDriver driver;
        public static ExtentReports extent;
        public static ExtentTest test;
        public static List<string> DegreesToDelete { get; set; } = new List<string>();
        public static List<string> CertificatenamesToDelete { get; set; } = new List<string>();




        [OneTimeSetUp]
        public void ExtentReportSetup()
        {
            try
            {
                
                var sparkReporter = new ExtentSparkReporter(@"D:\Mansi-Industryconnect\CompetitionTask\Reports\extentReport.html");
                extent = new ExtentReports();
                extent.AttachReporter(sparkReporter);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        [SetUp]
        public void Initialization()
        {
            // Initialize WebDriver (Chrome in this case)

            
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("http://localhost:5000/Home");
            loginPageObj = new LoginPage();
            educationObj = new Education();
            certificateObj = new Certificate();

            var testName = TestContext.CurrentContext.Test.Name;
            test = extent.CreateTest(testName);

            loginPageObj.LoginAction(driver);
            test.Log(Status.Info, "Login successful");
        }

        [TearDown]
        public void Cleanup()
        {
            try
            {
                // Take a screenshot on failure
                if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
                {
                    TakeScreenshotWithPngFormat();
                }
                foreach (var certificatename in CertificatenamesToDelete)
                {
                    try
                    {
                        certificateObj.DeleteTestData(certificatename);
                        test.Log(Status.Info, $"Deleted certificate name '{certificatename}' from the UI.");
                    }
                    catch (Exception cleanupEx)
                    {
                        test.Log(Status.Fail, $"Failed to delete certificate name during cleanup: {cleanupEx.Message}");
                    }
                }
                foreach (var degree in DegreesToDelete)
                {
                    try
                    {
                        educationObj.DeleteTestData(degree);
                        test.Log(Status.Info, $"Deleted certificate name '{degree}' from the UI.");
                    }
                    catch (Exception cleanupEx)
                    {
                        test.Log(Status.Fail, $"Failed to delete certificate name during cleanup: {cleanupEx.Message}");
                    }
                }

                // Quit the WebDriver after each test if desired
                if (driver != null)
                {
                    driver.Quit();
                    driver.Dispose();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred during cleanup: {e.Message}");
            }
           
        }

        public static void TakeScreenshotWithPngFormat()
        {
            try
            {
                Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                screenshot.SaveAsFile(@"D:\Mansi-Industryconnect\CompetitionTask\ScreenShot\Screenshot." + System.Drawing.Imaging.ImageFormat.Png);
                

                // Add screenshot to ExtentReports
              
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred while taking screenshot: {e.Message}");
            }
        }

        [OneTimeTearDown]
        public void TeardownReport()
        {
            try
            {
                // Cleanup test data

                extent.Flush();

            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred during final teardown: {e.Message}");
            }
            
        }
    }
}