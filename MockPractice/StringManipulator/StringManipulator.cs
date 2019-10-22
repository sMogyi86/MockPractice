using System;
using System.Linq;

namespace MockPractice
{
    public class StringManipulator
	{
		private bool IsVowel(char c)
		{
			var vowels = new char[] { 'a', 'e', 'i', 'o', 'u' };
			return vowels.Contains(c);
		}

		public string Transform(string s)
		{
            if (s is null)
                throw new ArgumentNullException(nameof(s));

			var x = s.Where(c => IsVowel(c));
			var y = s.Where(c => !IsVowel(c));

			return new string(x.Concat(y).ToArray());
		}
	}
}
