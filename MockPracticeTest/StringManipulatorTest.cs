using System;
using MockPractice;
using NUnit.Framework;

namespace MockPracticeTest
{
    [TestFixture]
    public class StringManipulatorTest
    {
        private readonly StringManipulator myStringManipulator = new StringManipulator();

        [Test]
        public void Transform_NULLParameter_Shall_ThrowArgumentNullException()
            => Assert.Throws<ArgumentNullException>(() => myStringManipulator.Transform(null));

        [TestCase("", "")]
        [TestCase("abcde", "aebcd")]
        public void Transform_Shall_SelectAndGroupVowelsInTheString(string input, string expectedOutput)
            => Assert.AreEqual(expectedOutput, myStringManipulator.Transform(input));
    }
}
