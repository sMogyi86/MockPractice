using MockPractice;
using NUnit.Framework;

namespace MockPracticeTest
{
    [TestFixture]
    public class StringManipulatorTest
    {
        private readonly StringManipulator myStringManipulator = new StringManipulator();

        [TestCase("", "")]
        [TestCase("abcde", "aebcd")]
        public void Transform_Shall_SelectAndGroupVowelsInTheString(string input, string expectedOutput)
            => Assert.AreEqual(expectedOutput, myStringManipulator.Transform(input));
    }
}
