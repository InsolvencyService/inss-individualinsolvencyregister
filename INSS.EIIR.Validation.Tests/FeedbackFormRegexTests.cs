using INSS.EIIR.Models.FeedbackModels;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace INSS.EIIR.Validation.Tests
{
    [SetUpFixture]
    public class SetupTrace
    {
        [OneTimeSetUp]
        public void StartTest()
        {
            Trace.Listeners.Add(new ConsoleTraceListener());
        }

        [OneTimeTearDown]
        public void EndTest()
        {
            Trace.Flush();
        }
    }

    public class FeedbackFormRegexTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [TestCase("This is a test message")]
        [TestCase("Message to test")]
        [TestCase("Another test message")]
        public void MessageValidation_PassValidMessage_ReturnPass(string message)
        {
            // Arrange
            var createCaseFeedbackModel = new CreateCaseFeedback
            {
                Message = message,
                ReporterEmailAddress = "test@testing.com",
                ReporterFullname = "Steve Smith"
            };
            var messageToValidate = createCaseFeedbackModel.Message;
            string pattern = @"(^[\s'-.a-zA-Z0-9]*$)";

            var timer = new Stopwatch();

            // Act
            timer.Start();
            Match match = Regex.Match(messageToValidate, pattern, RegexOptions.IgnoreCase);
            timer.Stop();
            var elapsedTime = timer.Elapsed;

            // Assert
            Assert.IsTrue(match.Success);
            Console.WriteLine($"Total milliseconds for MESSAGE regex test {elapsedTime.TotalMilliseconds}");
        }

        [Test]
        [TestCase("test123@testing.com")]
        [TestCase("test123@testing.co.uk")]
        [TestCase("test.testing@testing.org")]
        [TestCase("email@example.com")]
        [TestCase("firstname.lastname@example.com")]
        [TestCase("email@subdomain.example.com")]
        [TestCase("firstname+lastname@example.com")]
        [TestCase("email@123.123.123.123")]
        [TestCase("email@[123.123.123.123]")]
        [TestCase(@"""email""@example.com")]
        [TestCase("1234567890@example.com")]
        [TestCase("email@example-one.com")]
        [TestCase("_______@example.com")]
        [TestCase("email@example.name")]
        [TestCase("email@example.museum")]
        [TestCase("email@example.co.jp")]
        [TestCase("firstname-lastname@example.com")]
        public void EmailAddressValidation_PassValidEmail_ReturnPass(string emailAddress)
        {
            // Arrange
            var createCaseFeedbackModel = new CreateCaseFeedback
            {
                Message = "Some feed back message",
                ReporterEmailAddress = emailAddress,
                ReporterFullname = "Steve Smith"
            };
            var emailAddressToValidate = createCaseFeedbackModel.ReporterEmailAddress;
            string pattern = @"(?:[a-z0-9!#$%&'*+\/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+\/=?^_`{|}~-]+)*|(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*)@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])"; // Average after all run 1.5 - 2.3

            // Act
            Match match = Regex.Match(emailAddressToValidate, pattern, RegexOptions.IgnoreCase);

            // Assert
            Assert.IsTrue(match.Success);
        }

        [Test]
        [TestCase("Steve Smith")]
        [TestCase("Mike Green")]
        [TestCase("Julie Jones")]
        public void ReporterNameValidation_PassValidReporterName_ReturnPass(string reporterName)
        {
            // Arrange
            var createCaseFeedbackModel = new CreateCaseFeedback
            {
                Message = "Some feed back message",
                ReporterEmailAddress = "test@testing.com",
                ReporterFullname = reporterName
            };
            var reporterNameToValidate = createCaseFeedbackModel.ReporterFullname;
            string pattern = @"(^[\s'-.a-zA-Z0-9]*$)";

            // Act
            Match match = Regex.Match(reporterNameToValidate, pattern, RegexOptions.IgnoreCase);

            // Assert
            Assert.IsTrue(match.Success);
        }
    }
}