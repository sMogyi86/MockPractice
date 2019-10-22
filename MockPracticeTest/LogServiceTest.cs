using MockPractice;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MockPracticeTest
{
    [TestFixture]
    public class LogServiceTest //http://derpturkey.com/validate-mock-arguments-with-moq/    
    {
        private readonly DateTime myNow = DateTime.Now;
        private LogService myLogService;
        private Mock<ILogger> myLoggerMock;

        private LogLevel mySupportedLogLevel = default(LogLevel);
        private string mySpiedLoggedMessageStr = String.Empty;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var myNowProviderMock = new Mock<INowProvider>();
            myNowProviderMock.Setup(m => m.GetNow()).Returns(myNow);

            myLogService = new LogService(myNowProviderMock.Object);

            myLoggerMock = new Mock<ILogger>();
            myLoggerMock.Setup(m => m.SupportsLogLevel(It.Is<LogLevel>(l => l == mySupportedLogLevel)));
            myLoggerMock.Setup(m => m.Log(It.IsAny<string>(), It.IsAny<LogLevel>()))
                      .Callback<string, LogLevel>((msg, lvl) => { mySpiedLoggedMessageStr = msg; })
                      .Verifiable();

            myLogService.RegisterLogger(myLoggerMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            myLoggerMock.Reset();

            mySupportedLogLevel = default(LogLevel);
            mySpiedLoggedMessageStr = String.Empty;
        }

        [Test]
        public void Ctr_NULLINowProviderParam_Shall_ThrowArgumentNullException()
            => Assert.Throws<ArgumentNullException>(() => new LogService(null));

        [TestCase(LogLevel.Debug, LogLevel.Debug)]
        public void Log_CalledOnlyIfLevelIsSupported(LogLevel logLevelToTry, LogLevel supportedLogLevel)
        {
            mySupportedLogLevel = supportedLogLevel;

            myLogService.Log(nameof(Log_CalledOnlyIfLevelIsSupported), logLevelToTry);

            myLoggerMock.Verify(m => m.Log(It.IsAny<string>(), It.IsAny<LogLevel>()), logLevelToTry.HasFlag(supportedLogLevel) ? Times.Once() : Times.Never());
        }

        //[TestCase]
        //public void Log(string message, LogLevel logLevel)
        //{

        //    myLogService.Log(message, logLevel);

        //    /*
        //    only logs where supported
        //    startswith the date
        //    contains message
        //     */
        //}

        [Test]
        public void RegisterLogger_NULLILoggerParam_Shall_ThrowArgumentNullException()
            => Assert.Throws<ArgumentNullException>(() => myLogService.RegisterLogger(null));

        //[Test]
        //public void Testing()
        //{
        //    string spiedLogMessageStr = null;
        //    LogLevel spiedLogLevel = default(LogLevel);

        //    var loggerMock = new Mock<ILogger>();

        //    loggerMock
        //        .Setup(m => m.SupportsLogLevel(It.IsAny<LogLevel>()))
        //        .Returns(true);
        //    loggerMock
        //        .Setup(m => m.Log(It.IsAny<string>(), It.IsAny<LogLevel>()))
        //        .Callback<string, LogLevel>(
        //        (msg, lvl) =>
        //        {
        //            spiedLogMessageStr = msg;
        //            spiedLogLevel = lvl;
        //        })
        //        .Verifiable();


        //    myLogService.RegisterLogger(loggerMock.Object);

        //    string expectedMessage = "alma";
        //    LogLevel expectedLogLevel = LogLevel.Error;

        //    myLogService.Log(expectedMessage, expectedLogLevel);

        //    Assert.AreEqual(expectedLogLevel, spiedLogLevel);
        //    Assert.IsTrue(spiedLogMessageStr.StartsWith(myNowProviderMock.Object.GetNow().ToString()));
        //    Assert.IsTrue(spiedLogMessageStr.Contains(expectedMessage));
        //}
    }
}