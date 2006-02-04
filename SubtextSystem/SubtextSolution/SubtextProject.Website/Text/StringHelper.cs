#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at SourceForge at http://sourceforge.net/projects/subtext
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Collections.Specialized;
using System.Globalization;

// adapted from namespace Haack.Text
namespace SubtextProject.Website.Text
{
	/// <summary>
	/// Static class with useful string manipulation methods.
	/// </summary>
	public sealed class StringHelper
	{
		private StringHelper()
		{
		}

		/// <summary>
		/// Compares the strings and returns true if they are equal.
		/// </summary>
		/// <param name="source">Source.</param>
		/// <param name="compared">Compared.</param>
		/// <param name="ignoreCase"></param>
		/// <returns></returns>
		public static bool AreEqual(string source, string compared, bool ignoreCase)
		{
			return string.Compare(source, compared, ignoreCase, CultureInfo.InvariantCulture) == 0;
		}

		/// <summary>
		/// Compares the strings and returns true if they are equal, ignoring case.
		/// </summary>
		/// <param name="source">Source.</param>
		/// <param name="compared">Compared.</param>
		/// <returns></returns>
		public static bool AreEqualIgnoringCase(string source, string compared)
		{
			return AreEqual(source, compared, true);
		}

		/// <summary>
		/// Parses a camel cased or pascal cased string and returns an array 
		/// of the words within the string.
		/// </summary>
		/// <example>
		/// The string "PascalCasing" will return an array with two 
		/// elements, "Pascal" and "Casing".
		/// </example>
		/// <param name="source"></param>
		/// <returns></returns>
		public static string[] SplitUpperCase(string source)
		{
			if(source == null)
				return new string[] {}; //Return empty array.

			if(source.Length == 0)
				return new string[] {""};

			StringCollection words = new StringCollection();
			int wordStartIndex = 0;

			char[] letters = source.ToCharArray();
			// Skip the first letter. we don't care what case it is.
			for(int i = 1; i < letters.Length; i++)
			{
				if(char.IsUpper(letters[i]))
				{
					//Grab everything before the current index.
					words.Add(new String(letters, wordStartIndex, i - wordStartIndex));
					wordStartIndex = i;
				}
			}
			//We need to have the last word.
			words.Add(new String(letters, wordStartIndex, letters.Length - wordStartIndex)); 

			//Copy to a string array.
			string[] wordArray = new string[words.Count];
			words.CopyTo(wordArray, 0);
			return wordArray;
		}

		/// <summary>
		/// Parses a camel cased or pascal cased string and returns a new 
		/// string with spaces between the words in the string.
		/// </summary>
		/// <example>
		/// The string "PascalCasing" will return an array with two 
		/// elements, "Pascal" and "Casing".
		/// </example>
		/// <param name="source"></param>
		/// <returns></returns>
		public static string SplitUpperCaseToString(string source)
		{
			return string.Join(" ", SplitUpperCase(source));
		}

		/// <summary>
		/// Returns a string containing a specified number of characters from the left side of a string.
		/// </summary>
		/// <param name="str">Required. String expression from which the leftmost characters are returned.</param>
		/// <param name="length">Required. Integer greater than 0. Numeric expression 
		/// indicating how many characters to return. If 0, a zero-length string ("") 
		/// is returned. If greater than or equal to the number of characters in Str, 
		/// the entire string is returned. If str is null, this returns null.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if length is less than 0</exception>
		/// <exception cref="NullReferenceException">Thrown if str is null.</exception>
		public static string Left(string str, int length)
		{
			if(length >= str.Length)
				return str;

			return str.Substring(0, length);
		}

		/// <summary>
		/// Returns a string containing a specified number of characters from the right side of a string.
		/// </summary>
		/// <param name="str">Required. String expression from which the rightmost characters are returned.</param>
		/// <param name="length">Required. Integer greater than 0. Numeric expression 
		/// indicating how many characters to return. If 0, a zero-length string ("") 
		/// is returned. If greater than or equal to the number of characters in Str, 
		/// the entire string is returned. If str is null, this returns null.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if length is less than 0</exception>
		/// <exception cref="NullReferenceException">Thrown if str is null.</exception>
		public static string Right(string str, int length)
		{
			if(str == null)
				throw new NullReferenceException("Right cannot be evaluated on a null string.");

			if(length < 0)
				throw new ArgumentOutOfRangeException("length", length, "Length must not be negative.");
			
			if(str.Length == 0 || length == 0)
				return String.Empty;

			if(length >= str.Length)
				return str;

			return str.Substring(str.Length - length);
		}

		/// <summary>
		/// Returns a string containing every character within a string after the 
		/// first occurrence of another string.
		/// </summary>
		/// <param name="str">Required. String expression from which the rightmost characters are returned.</param>
		/// <param name="searchString">The string where the end of it marks the 
		/// characters to return.  If the string is not found, the whole string is 
		/// returned.</param>
		/// <returns></returns>
		/// <exception cref="NullReferenceException">Thrown if str or searchstring is null.</exception>
		public static string RightAfter(string str, string searchString)
		{
			return RightAfter(str, searchString, true);
		}

		/// <summary>
		/// Returns a string containing every character within a string after the 
		/// first occurrence of another string.
		/// </summary>
		/// <param name="str">Required. String expression from which the rightmost characters are returned.</param>
		/// <param name="searchString">The string where the end of it marks the 
		/// characters to return.  If the string is not found, the whole string is 
		/// returned.</param>
		/// <param name="caseSensitive">Default true: If true, uses case sensitive search.</param>
		/// <returns></returns>
		/// <exception cref="NullReferenceException">Thrown if str or searchstring is null.</exception>
		public static string RightAfter(string str, string searchString, bool caseSensitive)
		{
			if(searchString == null)
				throw new NullReferenceException("Search string may not be null.");

			//Shortcut.
			if(searchString.Length > str.Length || searchString.Length == 0)
				return str;

			int searchIndex;

			if(caseSensitive)
				searchIndex = str.IndexOf(searchString, 0);
			else
				searchIndex = str.ToUpper(CultureInfo.InvariantCulture).IndexOf(searchString.ToUpper(CultureInfo.InvariantCulture), 0);
			
			if(searchIndex < 0)
				return str;

			return Right(str, str.Length - (searchIndex + searchString.Length));
		}

		/// <summary>
		/// Returns a string containing every character within a string before the 
		/// first occurrence of another string.
		/// </summary>
		/// <param name="str">Required. String expression from which the leftmost characters are returned.</param>
		/// <param name="searchString">The string where the beginning of it marks the 
		/// characters to return.  If the string is not found, the whole string is 
		/// returned.</param>
		/// <returns></returns>
		/// <exception cref="NullReferenceException">Thrown if str or searchstring is null.</exception>
		public static string LeftBefore(string str, string searchString)
		{
			return LeftBefore(str, searchString, true);
		}

		/// <summary>
		/// Returns a string containing every character within a string before the 
		/// first occurrence of another string.
		/// </summary>
		/// <param name="str">Required. String expression from which the leftmost characters are returned.</param>
		/// <param name="searchString">The string where the beginning of it marks the 
		/// characters to return.  If the string is not found, the whole string is 
		/// returned.</param>
		/// <param name="caseSensitive">Default true: If true, uses case sensitive search.</param>
		/// <returns></returns>
		/// <exception cref="NullReferenceException">Thrown if str or searchstring is null.</exception>
		public static string LeftBefore(string str, string searchString, bool caseSensitive)
		{
			if(searchString == null)
				throw new NullReferenceException("Search string may not be null.");

			//Shortcut.
			if(searchString.Length > str.Length || searchString.Length == 0)
				return str;

			int searchIndex;
			if(caseSensitive)
				searchIndex = str.IndexOf(searchString, 0);
			else
				searchIndex = str.ToUpper(CultureInfo.InvariantCulture).IndexOf(searchString.ToUpper(CultureInfo.InvariantCulture), 0);

			if(searchIndex < 0)
				return str;

			return Left(str, searchIndex);
		}

		/// <summary>
		/// Returns true if the specified string to be searched starts with 
		/// the specified prefix in a culturally invariant manner.
		/// </summary>
		/// <param name="searched">The string to check its start.</param>
		/// <param name="prefix">The string to search for at the beginning of the searched string.</param>
		/// <param name="ignoreCase">Ignore case.</param>
		/// <returns></returns>
		public static bool StartsWith(string searched, string prefix, bool ignoreCase)
		{
			if(searched == null)
				throw new NullReferenceException("The searched string may not be null.");

			// If we're not ignoring the case, use the built in function. 
			// That's what it's there for.
			if(!ignoreCase)
				return searched.StartsWith(prefix);

			if(prefix == null)
				throw new NullReferenceException("The prefix string may not be null.");

			if(prefix.Length > searched.Length)
				return false;

			string prefixSizedString = Left(searched, prefix.Length);
			return AreEqual(prefixSizedString, prefix, true);
		}

		/// <summary>
		/// Returns true if the specified string to be searched ends with 
		/// the specified prefix in a culturally invariant manner.
		/// </summary>
		/// <param name="searched">The string to check its end.</param>
		/// <param name="suffix">The string to search for at the end of the searched string.</param>
		/// <param name="ignoreCase">Ignore case.</param>
		/// <returns></returns>
		public static bool EndsWith(string searched, string suffix, bool ignoreCase)
		{
			if(searched == null)
				throw new NullReferenceException("The searched string may not be null.");

			if(!ignoreCase)
				return searched.EndsWith(suffix);

			if(suffix == null)
				throw new NullReferenceException("The prefix string may not be null.");

			if(suffix.Length > searched.Length)
				return false;

			string suffixSizedString = Right(searched, suffix.Length);
			return AreEqual(suffixSizedString, suffix, true);
		}

		/// <summary>
		/// Returns the index of the first string within the second.
		/// </summary>
		/// <param name="container">Container.</param>
		/// <param name="contained">Contained.</param>
		/// <param name="caseSensitive">Case sensitive.</param>
		/// <returns></returns>
		public static int IndexOf(string container, string contained, bool caseSensitive)
		{
			if(caseSensitive)
				return container.IndexOf(contained);
			else
				return container.ToUpper(CultureInfo.InvariantCulture).IndexOf(contained.ToUpper(CultureInfo.InvariantCulture));
		}

	}
}
