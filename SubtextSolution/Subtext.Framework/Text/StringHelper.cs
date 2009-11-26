#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Subtext.Framework.Properties;

// adapted from namespace Haack.Text

namespace Subtext.Framework.Text
{
    /// <summary>
    /// Static class with useful string manipulation methods.
    /// </summary>
    public static class StringHelper
    {
        private static readonly Regex NumericRegex = new Regex(@"^\d+$", RegexOptions.Compiled);
        private static readonly Regex SplitWordsRegex = new Regex(@"\W+", RegexOptions.Compiled);

        public static string NullIfEmpty(this string s)
        {
            if(String.IsNullOrEmpty(s))
            {
                return null;
            }
            return s;
        }

        public static string Remove(this string original, string textToRemove, int occurrenceCount,
                                    StringComparison comparison)
        {
            if(!original.Contains(textToRemove, comparison))
            {
                return original;
            }

            string result = original;
            for(int i = 0; i < occurrenceCount; i++)
            {
                result = result.LeftBefore(textToRemove, comparison) + result.RightAfter(textToRemove, comparison);
                if(!result.Contains(textToRemove, comparison))
                {
                    return result;
                }
            }
            return result;
        }

        /// <summary>
        /// Removes any double instances of the specified character. 
        /// So "--" becomes "-" if the character is '-'.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="character">The character.</param>
        /// <returns></returns>
        public static string RemoveDoubleCharacter(this string text, char character)
        {
            if(text == null)
            {
                throw new ArgumentNullException("text");
            }
            if(character == char.MinValue)
            {
                return text;
            }
            var newString = new char[text.Length];
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

        public static IEnumerable<string> SplitIntoWords(this string source)
        {
            return SplitWordsRegex.Split(source.Trim());
        }

        /// <summary>
        /// Converts text to pascal case...
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToPascalCase(this string text)
        {
            if(text == null)
            {
                throw new ArgumentNullException("text");
            }

            if(text.Length == 0)
            {
                return text;
            }

            string[] words = text.Split(' ');
            for(int i = 0; i < words.Length; i++)
            {
                if(words[i].Length > 0)
                {
                    string word = words[i];
                    char firstChar = char.ToUpper(word[0], CultureInfo.InvariantCulture);
                    words[i] = firstChar + word.Substring(1);
                }
            }
            return string.Join(string.Empty, words);
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
        /// <exception cref="ArgumentNullException">Thrown if str is null.</exception>
        public static string Left(this string str, int length)
        {
            if(str == null)
                return null;
            if(length >= str.Length)
            {
                return str;
            }

            return str.Substring(0, length);
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
        public static string Right(this string original, int length)
        {
            if(original == null)
            {
                throw new ArgumentNullException("original");
            }

            if(length < 0)
            {
                throw new ArgumentOutOfRangeException("length", length,
                                                      Resources.ArgumentOutOfRange_LengthMustNotBeNegative);
            }

            if(original.Length == 0 || length == 0)
            {
                return String.Empty;
            }

            if(length >= original.Length)
            {
                return original;
            }

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
        public static string RightAfter(this string original, string search)
        {
            return RightAfter(original, search, StringComparison.InvariantCulture);
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
        public static string RightAfter(this string original, string search, StringComparison comparisonType)
        {
            if(original == null)
            {
                throw new ArgumentNullException("original");
            }

            if(search == null)
            {
                throw new ArgumentNullException("search");
            }

            //Shortcut.
            if(search.Length > original.Length || search.Length == 0)
            {
                return original;
            }

            int searchIndex = original.IndexOf(search, 0, comparisonType);

            if(searchIndex < 0)
            {
                return original;
            }

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
        public static string RightAfterLast(this string original, string search)
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
        public static string RightAfterLast(this string original, string search, int startIndex,
                                            StringComparison comparisonType)
        {
            if(original == null)
            {
                throw new ArgumentNullException("original");
            }
            if(search == null)
            {
                throw new ArgumentNullException("search");
            }

            //Shortcut.
            if(search.Length > original.Length || search.Length == 0)
            {
                return original;
            }

            int searchIndex = original.LastIndexOf(search, startIndex, comparisonType);

            if(searchIndex < 0)
            {
                return original;
            }

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
        public static string LeftBefore(this string original, string search, StringComparison comparisonType)
        {
            if(original == null)
            {
                throw new ArgumentNullException("original");
            }
            if(search == null)
            {
                throw new ArgumentNullException("search");
            }
            //Shortcut.
            if(search.Length > original.Length || search.Length == 0)
            {
                return original;
            }
            int searchIndex = original.IndexOf(search, 0, comparisonType);

            if(searchIndex < 0)
            {
                return original;
            }
            return original.Left(searchIndex);
        }

        /// <summary>
        /// Returns true if the the specified container string contains the 
        /// contained string.
        /// </summary>
        /// <param name="container">Container.</param>
        /// <param name="contained">Contained.</param>
        /// <param name="comparison">Case sensitivity.</param>
        /// <returns></returns>
        public static bool Contains(this string container, string contained, StringComparison comparison)
        {
            return container.IndexOf(contained, comparison) >= 0;
        }

        /// <summary>
        /// Determines whether the specified text is a numeric... or to be 
        /// more precise, if the text is an integer.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>
        /// 	<c>true</c> if the specified text is numeric; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNumeric(this string text)
        {
            return NumericRegex.IsMatch(text);
        }

        public static string MailToEncode(string s)
        {
            return HttpUtility.UrlEncode(HttpUtility.HtmlAttributeEncode(HtmlHelper.RemoveHtml(s).Replace("\"", "'"))).Replace("+", " ");
        }

        public static string MailToBodyEncode(string body)
        {
            return MailToEncode(body.Replace(Environment.NewLine, "%0A"));
        }

        public static string NamedFormat(this string format, object source)
        {
            if(format == null)
            {
                throw new ArgumentNullException("format");
            }
            string[] formattedStrings = (from expression in format.SplitFormat()
                                         select expression.Eval(source)).ToArray();
            return String.Join(string.Empty, formattedStrings);
        }

        private static IEnumerable<ITextExpression> SplitFormat(this string format)
        {
            int exprEndIndex = -1;
            int expStartIndex;

            do
            {
                expStartIndex = format.IndexOfExpressionStart(exprEndIndex + 1);
                if(expStartIndex < 0)
                {
                    //everything after last end brace index.
                    if(exprEndIndex + 1 < format.Length)
                    {
                        yield return new LiteralFormat(
                            format.Substring(exprEndIndex + 1));
                    }
                    break;
                }

                if(expStartIndex - exprEndIndex - 1 > 0)
                {
                    //everything up to next start brace index
                    yield return new LiteralFormat(format.Substring(exprEndIndex + 1
                                                                    , expStartIndex - exprEndIndex - 1));
                }

                int endBraceIndex = format.IndexOfExpressionEnd(expStartIndex + 1);
                if(endBraceIndex < 0)
                {
                    //rest of string, no end brace (could be invalid expression)
                    yield return new FormatExpression(format.Substring(expStartIndex));
                }
                else
                {
                    exprEndIndex = endBraceIndex;
                    //everything from start to end brace.
                    yield return new FormatExpression(format.Substring(expStartIndex
                                                                       , endBraceIndex - expStartIndex + 1));
                }
            } while(expStartIndex > -1);
        }

        static int IndexOfExpressionStart(this string format, int startIndex)
        {
            int index = format.IndexOf('{', startIndex);
            if(index == -1)
            {
                return index;
            }

            //peek ahead.
            if(index + 1 < format.Length)
            {
                char nextChar = format[index + 1];
                if(nextChar == '{')
                {
                    return IndexOfExpressionStart(format, index + 2);
                }
            }

            return index;
        }

        static int IndexOfExpressionEnd(this string format, int startIndex)
        {
            int endBraceIndex = format.IndexOf('}', startIndex);
            if(endBraceIndex == -1)
            {
                return endBraceIndex;
            }
            //start peeking ahead until there are no more braces...
            // }}}}
            int braceCount = 0;
            for(int i = endBraceIndex + 1; i < format.Length; i++)
            {
                if(format[i] == '}')
                {
                    braceCount++;
                }
                else
                {
                    break;
                }
            }
            if(braceCount % 2 == 1)
            {
                return IndexOfExpressionEnd(format, endBraceIndex + braceCount + 1);
            }

            return endBraceIndex;
        }

        /// <summary>
        /// Returns a new String with the last character removed. 
        /// If the string ends with \r\n, both characters are removed.
        /// </summary>
        /// <remarks>
        /// "string\r\n".chop   #=> "string"
        /// "string\n\r".chop   #=> "string\n"
        /// "string\n".chop     #=> "string"
        /// "string".chop       #=> "strin"
        /// "x".chop.chop       #=> ""
        /// </remarks>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Chop(this string text)
        {
            if(String.IsNullOrEmpty(text))
            {
                return text;
            }
            bool chopped = false;
            if(text.EndsWith("\n", StringComparison.Ordinal))
            {
                text = text.Substring(0, text.Length - 1);
                chopped = true;
            }
            if(text.EndsWith("\r", StringComparison.Ordinal))
            {
                text = text.Substring(0, text.Length - 1);
                chopped = true;
            }
            if(!chopped)
            {
                text = text.Substring(0, text.Length - 1);
            }
            return text;
        }

        public static string Chomp(this string text)
        {
            return text.Chomp(null, StringComparison.Ordinal);
        }

        /// <summary>
        /// Returns a new String with the last character removed. 
        /// If the string ends with \r\n, both characters are removed.
        /// </summary>
        /// <remarks>
        /// "hello".chomp            #=> "hello"
        /// "hello\n".chomp          #=> "hello"
        /// "hello\r\n".chomp        #=> "hello"
        /// "hello\n\r".chomp        #=> "hello\n"
        /// "hello\r".chomp          #=> "hello"
        /// "hello \n there".chomp   #=> "hello \n there"
        /// "hello".chomp("llo")     #=> "he"
        /// </remarks>
        public static string Chomp(this string text, string separator, StringComparison comparisonType)
        {
            if(String.IsNullOrEmpty(text))
            {
                return text;
            }

            if(text.EndsWith("\n", StringComparison.Ordinal))
            {
                text = text.Substring(0, text.Length - 1);
            }

            if(text.EndsWith("\r", StringComparison.Ordinal))
            {
                text = text.Substring(0, text.Length - 1);
            }

            if(!String.IsNullOrEmpty(separator))
            {
                if(text.EndsWith(separator, comparisonType))
                {
                    text = text.Substring(0, text.Length - separator.Length);
                }
            }
            return text;
        }

        public static string ToYesNo(this bool value)
        {
            return value ? "Yes" : "No";
        }
    }
}