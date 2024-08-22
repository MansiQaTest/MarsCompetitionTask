using CompetitionTask.Models;
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
    public class LoginPage : CommonDriver
    {
        private IWebElement signinButton => driver.FindElement(By.XPath("//a[text()='Sign In']"));
        private IWebElement Username => driver.FindElement(By.XPath("//input[@name='email']"));
        private IWebElement Password => driver.FindElement(By.XPath("//input[@name='password']"));
        private IWebElement loginButton => driver.FindElement(By.XPath("//button[text()='Login']"));

        private string e_signin = "//a[text()='Sign In']";

        public void LoginAction(IWebDriver driver) 
        {
            WaitUtils.WaitToBeVisible(driver, "XPath", "//a[text()='Sign In']", 100);
            signinButton.Click();
           
            string loginFile = @"D:\Mansi-Industryconnect\CompetitionTask\JsonData\login.json";
            List<Models.TestCaseData> testCases = JsonUtils.ReadJsonData<Models.TestCaseData>(loginFile);


            foreach (var testCase in testCases)
            {
                if (testCase.TestCase == "LoginData")
                {
                    var loginData = testCase.Data;
                    string email = loginData.Email;
                    string password = loginData.Password;

                    Username.SendKeys(email);
                    Password.SendKeys(password);
                    loginButton.Click();
                    Thread.Sleep(1000);
                    break;
                }
            }


            }
        public void VerifyLoggedInUser()
        {
            
            WaitUtils.WaitToBeVisible(driver, "XPath", "//span[contains(text(),'Hi')]", 100);
            IWebElement checkUser = driver.FindElement(By.XPath("//span[contains(text(),'Hi')]"));
            Console.WriteLine(checkUser.Text);
            if (checkUser.Text == "Hi Mansi")
            {

                Console.WriteLine("Logged in");
            }
            else
            {
                Console.WriteLine("Not Logged in");

            }
        }

    }
}
