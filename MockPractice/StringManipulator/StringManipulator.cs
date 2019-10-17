using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
			var x = s.Where(c => IsVowel(c));
			var y = s.Where(c => !IsVowel(c));

			return new string(x.Concat(y).ToArray());
		}
	}
}
