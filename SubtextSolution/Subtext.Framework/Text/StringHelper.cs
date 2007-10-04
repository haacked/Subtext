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
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using Subtext.Framework.Properties;

// adapted from namespace Haack.Text
namespace Subtext.Framework.Text
{
	/// <summary>
	/// Static class with useful string manipulation methods.
	/// </summary>
	public static class StringHelper
	{
		/// <summary>
		/// Removes any double instances of the specified character. 
		/// So "--" becomes "-" if the character is '-'.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="character">The character.</param>
		/// <returns></returns>
		public static string RemoveDoubleCharacter(string text, char character)
		{
			if (text == null)
				throw new ArgumentNullException("text");

			if (character == char.MinValue)
				return text;

			char[] newString = new char[text.Length];
			int i = 0;

			bool lastCharIsOurChar = false;
			foreach(char c in text)
			{
				if(c != character || !lastCharIsOurChar)
				{
					newString[i] = c;
					i++;
				}
				lastCharIsOurChar = (c == character);
			}

			return new string(newString, 0, i);
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
		public static string[] SplitUppercase(string source)
		{
			if (source == null)
				return new string[] { }; //Return empty array.

			if (source.Length == 0)
				return new string[] { "" };

			StringCollection words = new StringCollection();
			int wordStartIndex = 0;

			char[] letters = source.ToCharArray();
			// Skip the first letter. we don't care what case it is.
			for (int i = 1; i < letters.Length; i++)
			{
				if (char.IsUpper(letters[i]))
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
		public static string SplitUppercaseToString(string source)
		{
			return string.Join(" ", SplitUppercase(source));
		}

		/// <summary>
		/// Converts text to pascal case...
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static string PascalCase(string text)
		{
			if (text == null)
				throw new ArgumentNullException("text", Resources.ArgumentNull_String);

			if (text.Length == 0)
				return text;

			string[] words = text.Split(' ');
			for (int i = 0; i < words.Length; i++)
			{
				if (words[i].Length > 0)
				{
					string word = words[i];
					char firstChar = char.ToUpper(word[0], CultureInfo.CurrentUICulture);
					words[i] = firstChar + word.Substring(1);
				}
			}
			return string.Join(string.Empty, words);
		}

		/// <summary>
		/// Returns a string containing a specified number of characters from the left side of a string.
		/// </summary>
		/// <param name="original">Required. String expression from which the leftmost characters are returned.</param>
		/// <param name="length">Required. Integer greater than 0. Numeric expression 
		/// indicating how many characters to return. If 0, a zero-length string ("") 
		/// is returned. If greater than or equal to the number of characters in Str, 
		/// the entire string is returned. If str is null, this returns null.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if length is less than 0</exception>
		/// <exception cref="ArgumentNullException">Thrown if str is null.</exception>
		public static string Left(string original, int length)
		{
			if (original == null)
			{
				throw new ArgumentNullException("original", Resources.ArgumentNull_String);
			}

			if (original.Length == 0)
			{
				throw new ArgumentException(Resources.Argument_StringZeroLength, "original");
			}

			if (length >= original.Length)
			{
				return original;
			}

			return original.Substring(0, length);
		}

		/// <summary>
		/// Returns a string containing a specified number of characters from the right side of a string.
		/// </summary>
		/// <param name="original">Required. String expression from which the rightmost characters are returned.</param>
		/// <param name="length">Required. Integer greater than 0. Numeric expression 
		/// indicating how many characters to return. If 0, a zero-length string ("") 
		/// is returned. If greater than or equal to the number of characters in Str, 
		/// the entire string is returned. If str is null, this returns null.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if length is less than 0</exception>
		/// <exception cref="ArgumentNullException">Thrown if str is null.</exception>
		public static string Right(string original, int length)
		{
			if (original == null)
				throw new ArgumentNullException("original", Resources.ArgumentNull_String);

			if (length < 0)
				throw new ArgumentOutOfRangeException("length", length, Resources.ArgumnetOutOfRange_Length);

			if (original.Length == 0 || length == 0)
				return String.Empty;

			if (length >= original.Length)
				return original;

			return original.Substring(original.Length - length);
		}

		/// <summary>
		/// Returns a string containing every character within a string after the 
		/// first occurrence of another string.
		/// </summary>
		/// <param name="original">Required. String expression from which the rightmost characters are returned.</param>
		/// <param name="search">The string where the end of it marks the 
		/// characters to return.  If the string is not found, the whole string is 
		/// returned.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException">Thrown if str or searchstring is null.</exception>
		public static string RightAfter(string original, string search)
		{
			return RightAfter(original, search, StringComparison.InvariantCultureIgnoreCase);
		}

		/// <summary>
		/// Returns a string containing every character within a string after the 
		/// first occurrence of another string.
		/// </summary>
		/// <param name="original">Required. String expression from which the rightmost characters are returned.</param>
		/// <param name="search">The string where the end of it marks the 
		/// characters to return.  If the string is not found, the whole string is 
		/// returned.</param>
		/// <param name="comparisonType">Determines whether or not to use case sensitive search.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException">Thrown if str or searchstring is null.</exception>
		public static string RightAfter(string original, string search, StringComparison comparisonType)
		{
			if (original == null)
				throw new ArgumentNullException("original", Resources.ArgumentNull_String);

			if (search == null)
				throw new ArgumentNullException("search", Resources.ArgumentNull_String);

			//Shortcut.
			if (search.Length > original.Length || search.Length == 0)
				return original;

			int searchIndex = original.IndexOf(search, 0, comparisonType);

			if (searchIndex < 0)
				return original;

			return Right(original, original.Length - (searchIndex + search.Length));
		}

		/// <summary>
		/// Returns a string containing every character within a string after the 
		/// last occurrence of another string.
		/// </summary>
		/// <param name="original">Required. String expression from which the rightmost characters are returned.</param>
		/// <param name="search">The string where the end of it marks the 
		/// characters to return.  If the string is not found, the whole string is 
		/// returned.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException">Thrown if str or searchstring is null.</exception>
		public static string RightAfterLast(string original, string search)
		{
			return RightAfterLast(original, search, original.Length - 1, StringComparison.InvariantCulture);
		}

		/// <summary>
		/// Returns a string containing every character within a string after the
		/// last occurrence of another string.
		/// </summary>
		/// <param name="original">Required. String expression from which the rightmost characters are returned.</param>
		/// <param name="search">The string where the end of it marks the
		/// characters to return.  If the string is not found, the whole string is
		/// returned.</param>
		/// <param name="startIndex">The start index.</param>
		/// <param name="comparisonType">Determines whether or not to use case sensitive search.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException">Thrown if str or searchstring is null.</exception>
		public static string RightAfterLast(string original, string search, int startIndex, StringComparison comparisonType)
		{
			if (original == null)
				throw new ArgumentNullException("original", "The original string may not be null.");
			if (search == null)
				throw new ArgumentNullException("search", "The searchString string may not be null.");

			//Shortcut.
			if (search.Length > original.Length || search.Length == 0)
				return original;

			int searchIndex = original.LastIndexOf(search, startIndex, comparisonType);

			if (searchIndex < 0)
				return original;

			return Right(original, original.Length - (searchIndex + search.Length));
		}

		/// <summary>
		/// Returns a string containing every character within a string before the 
		/// first occurrence of another string.
		/// </summary>
		/// <param name="str">Required. String expression from which the leftmost characters are returned.</param>
		/// <param name="search">The string where the beginning of it marks the 
		/// characters to return.  If the string is not found, the whole string is 
		/// returned.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException">Thrown if str or searchstring is null.</exception>
		public static string LeftBefore(string str, string search)
		{
			return LeftBefore(str, search, StringComparison.InvariantCulture);
		}

		/// <summary>
		/// Returns a string containing every character within a string before the 
		/// first occurrence of another string.
		/// </summary>
		/// <param name="original">Required. String expression from which the leftmost characters are returned.</param>
		/// <param name="search">The string where the beginning of it marks the 
		/// characters to return.  If the string is not found, the whole string is 
		/// returned.</param>
		/// <param name="comparisonType">Determines whether or not to use case sensitive search.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException">Thrown if str or searchstring is null.</exception>
		public static string LeftBefore(string original, string search, StringComparison comparisonType)
		{
			if (original == null)
			{
				throw new ArgumentNullException("original", Resources.ArgumentNull_String);
			}

			if (search == null)
			{
				throw new ArgumentNullException("search", Resources.ArgumentNull_String);
			}

			//Shortcut.
			if (search.Length > original.Length || search.Length == 0)
			{
				return original;
			}

			int searchIndex = original.IndexOf(search, 0, comparisonType);

			if (searchIndex < 0)
			{
				return original;
			}

			return Left(original, searchIndex);
		}

		/// <summary>
		/// Returns true if the the specified container string contains the 
		/// contained string.
		/// </summary>
		/// <param name="container">Container.</param>
		/// <param name="contained">Contained.</param>
		/// <param name="comparison">Case sensitivity.</param>
		/// <returns></returns>
		public static bool Contains(string container, string contained, StringComparison comparison)
		{
			if (container == null)
			{
				throw new ArgumentNullException("container", Resources.ArgumentNull_String);
			}

			if (contained == null)
			{
				throw new ArgumentNullException("contained", Resources.ArgumentNull_String);
			}

			return container.IndexOf(contained, comparison) >= 0;
		}
		/// <summary>
		/// Returns the EmptyString ("") if the passed in string is either the EmptyString
		/// or is NULL, else it returns the passed in string.
		/// </summary>
		/// <param name="str">The string to be checked</param>
		/// <returns></returns>
		public static String ReturnCheckForNull(string str)
		{
			return (str == null || str.Length == 0) ? string.Empty : str;
		}
		/// <summary>
		/// Returns a NULL if the passed in string is either the EmptyString ("") or 
		/// is NULL, else it returns the passed in string.
		/// </summary>
		/// <param name="str">The string to be checked</param>
		/// <returns></returns>
		public static String ReturnNullForEmpty(string str)
		{
			return (str == null || str.Length == 0) ? null : str;
		}

		/// <summary>
		/// Determines whether the specified text is a numeric... or to be 
		/// more precise, if the text is an integer.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <returns>
		/// 	<c>true</c> if the specified text is numeric; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsNumeric(string text)
		{
			return Regex.IsMatch(text, "^\\d+$");
		}

		/// <summary>
		/// Creates a delimited string using the specified delimiter. 
		/// The Coverter is applied to each item in the enumerable items collection.
		/// </summary>
		/// <param name="delimiter">The delimiter.</param>
		/// <param name="items">The items.</param>
		/// <param name="converter">The converter.</param>
		/// <returns></returns>
		public static string Join<T>(string delimiter, IEnumerable<T> items, Converter<T, string> converter)
		{
			if (delimiter == null)
				throw new ArgumentNullException("delimiter", Resources.ArgumentNull_String);

			if (items == null)
				throw new ArgumentNullException("items", Resources.ArgumentNull_Collection);

			if (converter == null)
				throw new ArgumentNullException("converter", Resources.ArgumentNull_Obj);

			StringBuilder builder = new StringBuilder();
			foreach (T item in items)
			{
				builder.Append(converter(item));
				builder.Append(delimiter);
			}
			if (builder.Length > 0)
				builder.Length = builder.Length - delimiter.Length;

			return builder.ToString();
		}
	}
}