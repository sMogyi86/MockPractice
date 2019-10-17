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
        public void GetIdentityFormated_Should_ReturnTheFormatedIdentity()
        {
            var result = myClient.GetIdentityFormatted();

            Assert.That(result.StartsWith(@"<formatted> "));
            Assert.That(result.Contains(myClient.GetIdentity()));
            Assert.That(result.EndsWith(@" </formatted>"));
        }

        [Test]
        public void GetServiceName_Should_ReturnTheNameOfTheService()
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
        public void GetContentFromatted_Shall_ReturnServiceContent(long id)
        {
            myMockService.Setup(m => m.IsConnected).Returns(true);


        }
    }
}
