using MockPractice;
using Moq;
using NUnit.Framework;
using System;

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
            var nowProviderMock = new Mock<INowProvider>();
            nowProviderMock.Setup(m => m.GetNow()).Returns(myNow);

            myLogService = new LogService(nowProviderMock.Object);

            myLoggerMock = new Mock<ILogger>();
            myLogService.RegisterLogger(myLoggerMock.Object);
        }

        [SetUp]
        public void SetUp()
        {
            myLoggerMock
                .Setup(m => m.SupportsLogLevel(It.IsAny<LogLevel>()))
                .Returns<LogLevel>(lvl => lvl.HasFlag(mySupportedLogLevel));
            myLoggerMock
                .Setup(m => m.Log(It.IsAny<string>(), It.IsAny<LogLevel>()))
                .Callback<string, LogLevel>((msg, lvl) => { mySpiedLoggedMessageStr = msg; });
                //.Verifiable();
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
        [TestCase(LogLevel.Error, LogLevel.Debug)]
        [TestCase(LogLevel.Info, LogLevel.Debug)]
        public void Log_Shall_BeCalledOnlyIfLevelIsSupported(LogLevel logLevelToTry, LogLevel supportedLogLevel)
        {
            mySupportedLogLevel = supportedLogLevel;

            myLogService.Log(nameof(Log_Shall_BeCalledOnlyIfLevelIsSupported), logLevelToTry);

            myLoggerMock.Verify(m => m.Log(It.IsAny<string>(), It.IsAny<LogLevel>()), logLevelToTry.HasFlag(supportedLogLevel) ? Times.Once() : Times.Never());
        }

        [Test]
        public void Log_Shall_FormatTheRecordWithDate()
        {
            mySupportedLogLevel = LogLevel.Error;
            var messageToLog = nameof(Log_Shall_FormatTheRecordWithDate);

            myLogService.Log(messageToLog, mySupportedLogLevel);

            Assert.IsTrue(mySpiedLoggedMessageStr.StartsWith(myNow.ToString()));
            Assert.IsTrue(mySpiedLoggedMessageStr.EndsWith(messageToLog));
        }

        [Test]
        public void RegisterLogger_NULLILoggerParam_Shall_ThrowArgumentNullException()
            => Assert.Throws<ArgumentNullException>(() => myLogService.RegisterLogger(null));
    }
}