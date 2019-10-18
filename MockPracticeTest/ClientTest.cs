using MockPractice;
using Moq;
using NUnit.Framework;
using System;

namespace MockPracticeTest
{
    [TestFixture]
    public class ClientTest
    {
        private Mock<IService> myMockService;
        private Mock<IContentFormatter> myMockContainerFormatter;
        private Client myClient;

        [SetUp]
        public void SetUp()
        {
            myMockService = new Mock<IService>();
            myMockContainerFormatter = new Mock<IContentFormatter>();
            myClient = new Client(myMockService.Object, myMockContainerFormatter.Object);
        }

        [Test]
        public void Ctr_NULLparameter_ShallThrrow_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Client(null, myMockContainerFormatter.Object));
            Assert.Throws<ArgumentNullException>(() => new Client(myMockService.Object, null));
        }

        [Test]
        public void GetIdentity_ShallReturn_2()
            => Assert.That("2" == myClient.GetIdentity());

        [Test]
        public void GetIdentityFormated_ShallReturnTheFormatedIdentity()
        {
            var result = myClient.GetIdentityFormatted();

            Assert.That(result.StartsWith(@"<formatted> "));
            Assert.That(result.Contains(myClient.GetIdentity()));
            Assert.That(result.EndsWith(@" </formatted>"));
        }

        [Test]
        public void GetServiceName_ShallReturnTheNameOfTheService()
        {
            var serviceName = "name of the service";
            myMockService.SetupGet(s => s.Name).Returns(serviceName);

            var result = myClient.GetServiceName();

            Assert.AreEqual(serviceName, result);
            myMockService.VerifyGet(s => s.Name, Times.Once);
        }

        [Test]
        public void Dispose_Shall_CallServiceDispose()
        {
            myClient.Dispose();
            myMockService.Verify(m => m.Dispose(), Times.Once);
        }

        [Test]
        public void GetContent_ShallReturn_ServiceContent()
        {
            var expectedResult = "expectedResult";
            myMockService.Setup(m => m.IsConnected).Returns(false);
            myMockService.Setup(m => m.GetContent(It.IsAny<long>())).Returns(expectedResult);

            var result = myClient.GetContent(0);

            myMockService.Verify(m => m.Connect(), Times.Once);
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void GetContentFromatted_ShallReturn_FormattedServiceContent()
        {
            var inputString = "inputString";
            myMockService.Setup(m => m.IsConnected).Returns(true);
            myMockService.Setup(m => m.GetContent(It.IsAny<long>())).Returns(inputString);
            var format = "format";
            myMockContainerFormatter.Setup(m => m.Format(It.IsAny<string>())).Returns<string>(s => format + s);

            var result = myClient.GetContentFormatted(0);

            myMockService.Verify(m => m.Connect(), Times.Never);
            StringAssert.StartsWith(format, result);
            StringAssert.EndsWith(inputString, result);
        }
    }
}