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

        public static IWebDriver driver;
        public static ExtentReports extent;
        public static ExtentTest test;
        public static List<string> DegreesToRemove { get; set; } = new List<string>();



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
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred during cleanup: {e.Message}");
            }
            finally
            {
                // Clean up WebDriver
                if (driver != null)
                {
                    driver.Quit();
                    driver.Dispose();
                }
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
            extent.Flush();
        }
    }
}