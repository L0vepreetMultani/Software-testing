using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using System.Text;



///<summary>
/// Lovepreet singh, 000848567
/// Automated testing for testing admin credentials, creating user, deleting user, the city directory on Microsoft Edge and Chrome 
/// </summary>

namespace Assignment5_DotNet6
{
    [TestClass]
    public class SeleniumTestCases
    {
        private static IWebDriver driver;
        private StringBuilder verificationErrors;
        private bool acceptNextAlert = true;
        private const string BROWSER = "EDGE";

        private const string DRIVER_LOCATION = @"C:\DRIVERS";
        private const string FIREFOX_BIN_LOCATION = @"C:\Program Files\Mozilla Firefox\firefox.exe";

        [ClassInitialize]
        public static void InitializeClass(TestContext testContext)
        {
            // FIREFOX
            if (BROWSER == "FIREFOX")
            {
                FirefoxDriverService service = FirefoxDriverService.CreateDefaultService(DRIVER_LOCATION);
                // Note that the line below needs to be the full exe Name not just the path
                // service.FirefoxBinaryPath = FIREFOX_BIN_LOCATION;
                // service.Host = "::1";
                driver = new FirefoxDriver(service);   
            }
            //CHROME
            else if (BROWSER == "CHROME")
                driver = new ChromeDriver(DRIVER_LOCATION);  
            //EDGE
            else if (BROWSER == "EDGE")
                driver = new EdgeDriver(DRIVER_LOCATION);


        }

        [ClassCleanup]
        public static void CleanupClass()
        {
            try
            {
                driver.Close();
                driver.Dispose();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
        }

        [TestInitialize]
        public void InitializeTest()
        {
            verificationErrors = new StringBuilder();
        }

        [TestCleanup]
        public void CleanupTest()
        {
            Assert.AreEqual("", verificationErrors.ToString());
        }

        [TestMethod]
        public void RunAllTests()
        {

            TestLoginAdmin(); //testing admin login 

            TheCreateUserTest(); //testing creating the user 

            TheDeleteUserTest(); //testing deleting the user that was created

            // Directory Testing - 4 Cities
            TheCityDirectoryTest("Hamilton {7}");  //testing if Hamilton is returning 7 results
            TheCityDirectoryTest("Sydney {1}"); //testing if Sydney is returning 1 result 
            TheCityDirectoryTest("Mississauga {11}"); //testing if Mississauga is returning 11 results 
            TheCityDirectoryTest("Woodbridge {2}"); //testing if Woodbridge is returning 2 results
        }


        /// <summary>
        /// Testing if login works correctly for admin credentials
        /// </summary>
        public void TestLoginAdmin()
        {
            driver.Navigate().GoToUrl("https://csunix.mohawkcollege.ca/tooltime/comp10066/A3/login.php");
            //driver.FindElement(By.Id("username")).Click();
            //driver.FindElement(By.Id("username")).Clear();
            driver.FindElement(By.Id("username")).SendKeys("admin");
            //driver.FindElement(By.Name("password")).Clear();
            driver.FindElement(By.Name("password")).SendKeys("adminP6ss");
            driver.FindElement(By.Name("Submit")).Click();
            driver.FindElement(By.Id("loginname")).Click();
            try
            {
                Assert.AreEqual("User: admin", driver.FindElement(By.Id("loginname")).Text);
            }
            catch (Exception e)
            {
                verificationErrors.Append(e.Message);
            }
            try
            {
                Assert.AreEqual("User Admin", driver.FindElement(By.LinkText("User Admin")).Text);
            }
            catch (Exception e)
            {
                verificationErrors.Append(e.Message);
            }
            driver.FindElement(By.LinkText("Logout")).Click();
            driver.FindElement(By.Id("loginname")).Click();
            try
            {
                Assert.AreEqual("Not Logged In", driver.FindElement(By.Id("loginname")).Text);
            }
            catch (Exception e)
            {
                verificationErrors.Append(e.Message);
            }
        }

        /// <summary>
        /// Test that creates the user with last name and last 3 digits of student id
        /// </summary>
        public void TheCreateUserTest()
        {
            driver.Navigate().GoToUrl("https://csunix.mohawkcollege.ca/tooltime/comp10066/A3/login.php");
            driver.FindElement(By.Id("username")).Click();
            driver.FindElement(By.Id("username")).Clear();
            driver.FindElement(By.Id("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).Clear();
            driver.FindElement(By.Name("password")).SendKeys("adminP6ss");
            driver.FindElement(By.Name("searchByPC")).Submit();
            driver.Navigate().GoToUrl("https://csunix.mohawkcollege.ca/tooltime/comp10066/A3/membernews.php");
            driver.FindElement(By.LinkText("User Admin")).Click();
            driver.Navigate().GoToUrl("https://csunix.mohawkcollege.ca/tooltime/comp10066/A3/adminuser.php");
            driver.FindElement(By.Id("username")).Click();
            driver.FindElement(By.Id("username")).Clear();
            driver.FindElement(By.Id("username")).SendKeys("lovey112");
            driver.FindElement(By.Id("password")).Clear();
            driver.FindElement(By.Id("password")).SendKeys("singh111");
            driver.FindElement(By.Name("activate")).Click();
            driver.FindElement(By.XPath("//form[@id='form1']/table/tbody/tr[5]/td/input[2]")).Click();
            driver.FindElement(By.Name("Add New Member")).Click();
            driver.FindElement(By.XPath("//div[@id='body']/div/div")).Click();
            try
            {
                Assert.AreEqual("Record successfully inserted.", driver.FindElement(By.XPath("//div[@id='body']/div/div")).Text);
            }
            catch (Exception e)
            {
                verificationErrors.Append(e.Message);
            }
            try
            {
                Assert.IsNotNull(driver.FindElement(By.XPath("//td[@id='lovey112']"))); //changed the XPath according to Lab video on canvas
            }
            catch (Exception e)
            {
                verificationErrors.Append(e.Message);
            }
            driver.FindElement(By.XPath("//td[@id='lovey112']/a/img")).Click();
            driver.FindElement(By.LinkText("here")).Click();
            driver.FindElement(By.LinkText("Logout")).Click();
            driver.Navigate().GoToUrl("https://csunix.mohawkcollege.ca/tooltime/comp10066/A3/login.php");
        }


        /// <summary>
        /// Test that deletes the user
        /// </summary>
        public void TheDeleteUserTest()
        {
            driver.Navigate().GoToUrl("https://csunix.mohawkcollege.ca/tooltime/comp10066/A3/login.php");
            driver.FindElement(By.Id("username")).Click();
            driver.FindElement(By.Id("username")).Clear();
            driver.FindElement(By.Id("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).Clear();
            driver.FindElement(By.Name("password")).SendKeys("adminP6ss");
            driver.FindElement(By.Name("searchByPC")).Submit();
            driver.Navigate().GoToUrl("https://csunix.mohawkcollege.ca/tooltime/comp10066/A3/membernews.php");
            driver.FindElement(By.LinkText("User Admin")).Click();
            driver.Navigate().GoToUrl("https://csunix.mohawkcollege.ca/tooltime/comp10066/A3/adminuser.php");
            driver.FindElement(By.Id("username")).Click();
            driver.FindElement(By.Id("username")).Clear();
            driver.FindElement(By.Id("username")).SendKeys("lovey112");
            driver.FindElement(By.Id("password")).Clear();
            driver.FindElement(By.Id("password")).SendKeys("singh111");
            driver.FindElement(By.Name("activate")).Click();
            driver.FindElement(By.XPath("//form[@id='form1']/table/tbody/tr[5]/td/input[2]")).Click();
            driver.FindElement(By.Name("Add New Member")).Click();
            try
            {
                Assert.AreEqual("COMP10066 Testing Automation Lab", driver.Title);
            }
            catch (Exception e)
            {
                verificationErrors.Append(e.Message);
            }
            try
            {
                Assert.AreEqual("Record successfully inserted.", driver.FindElement(By.XPath("//div[@id='body']/div/div")).Text);
            }
            catch (Exception e)
            {
                verificationErrors.Append(e.Message);
            }
            driver.FindElement(By.XPath("//td[@id='lovey112']/a/img")).Click();
            driver.FindElement(By.LinkText("here")).Click();
            try
            {
                Assert.AreEqual("User Minhas565 was successfully deleted.", driver.FindElement(By.XPath("//div[@id='body']/div/div")).Text);
            }
            catch (Exception e)
            {
                verificationErrors.Append(e.Message);
            }
            driver.FindElement(By.LinkText("Logout")).Click();
            driver.Navigate().GoToUrl("https://csunix.mohawkcollege.ca/tooltime/comp10066/A3/login.php");
        }
        /// <summary>
        /// Test that makes sure the city directory is working and returning the correct number of results
        /// </summary>
        /// <param name="city"> String that represents the city</param>
        public void TheCityDirectoryTest(String city)
        {
            driver.Navigate().GoToUrl("https://csunix.mohawkcollege.ca/tooltime/comp10066/A3/login.php");
            driver.FindElement(By.Id("username")).Click();
            driver.FindElement(By.Id("username")).Clear();
            driver.FindElement(By.Id("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).Clear();
            driver.FindElement(By.Name("password")).SendKeys("adminP6ss");
            driver.FindElement(By.Name("searchByPC")).Submit();
            driver.Navigate().GoToUrl("https://csunix.mohawkcollege.ca/tooltime/comp10066/A3/membernews.php");
            driver.FindElement(By.LinkText("Directory")).Click();
            driver.Navigate().GoToUrl("https://csunix.mohawkcollege.ca/tooltime/comp10066/A3/memberdirectory.php");
            driver.FindElement(By.Id("city")).Click();
            new SelectElement(driver.FindElement(By.Id("city"))).SelectByText(city);
            driver.FindElement(By.Name("submit")).Click();

            string myString = (city.Split(' '))[0];
            int result = city.Length; //gets the length of the city
            int count = city[result - 2];
            for (int i = 1; i <= count; i++) //looks through all the results returned whenever a city is searched
            {
                try
                {


                    String path = "//ul[@class='companylist']/li[3]"; //ensuring each result is returning the correct city in the third row in the right class
                    IWebElement element = driver.FindElement(By.XPath(path)); //finding it by the XPath
                    String response = element.Text;
                    Assert.IsTrue(response.Contains(myString));

                }

                catch (Exception e)
                {
                    verificationErrors.Append(e.Message);
                }
            }

            driver.FindElement(By.LinkText("Logout")).Click();
            driver.Navigate().GoToUrl("https://csunix.mohawkcollege.ca/tooltime/comp10066/A3/login.php");
        }

        private bool IsElementPresent(By by)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        private bool IsAlertPresent()
        {
            try
            {
                driver.SwitchTo().Alert();
                return true;
            }
            catch (NoAlertPresentException)
            {
                return false;
            }
        }

        private string CloseAlertAndGetItsText()
        {
            try
            {
                IAlert alert = driver.SwitchTo().Alert();
                string alertText = alert.Text;
                if (acceptNextAlert)
                {
                    alert.Accept();
                }
                else
                {
                    alert.Dismiss();
                }
                return alertText;
            }
            finally
            {
                acceptNextAlert = true;
            }
        }
    }
}
