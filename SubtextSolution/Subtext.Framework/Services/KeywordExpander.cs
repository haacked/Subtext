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
using System.Linq;
using System.Text;
using Subtext.Framework.Components;
using Subtext.Framework.Providers;

namespace Subtext.Framework.Services
{
    /// <summary>
    /// Expands keywords into links.
    /// </summary>
    public class KeywordExpander : ITextTransformation
    {
        public KeywordExpander(ObjectProvider repository) : this(GetKeywordsFromRepository(repository)) { 
        }

        public KeywordExpander(IEnumerable<KeyWord> keyWords)
        {
            Keywords = keyWords;
        }

        private static IEnumerable<KeyWord> GetKeywordsFromRepository(ObjectProvider repository) {
            if(repository != null) {
                return repository.GetKeyWords();
            }
            return null;
        }

        public IEnumerable<KeyWord> Keywords
        {
            get;
            private set;
        }

        public string Transform(string original)
        {
            if (Keywords != null) {
                return Keywords.Aggregate(original, (result, keyWord) => ReplaceFormat(result, keyWord));
            }
            return original;
        }

        private enum ScanState { 
            Replace, 
            InTag, 
            InAnchor 
        };

        /// <summary>
        /// Preforms a forward scan and replace for a given pattern. 
        /// Can specify only to match first fine and if the pattern is CaseSensitive
        /// </summary>
        /// <param name="source">Text to search</param>
        /// <param name="oldValue">Pattern to search for</param>
        /// <param name="formatString">Replaced Pattern</param>
        /// <param name="onlyFirstMatch">Match First Only</param>
        /// <returns></returns>
        private static string ReplaceFormat(string source, KeyWord keyword)
        {
            return Scan(source, keyword.Word, keyword.GetFormat, true, keyword.ReplaceFirstTimeOnly, keyword.CaseSensitive);
        }

        private static string Scan(string source, string oldValue, string newValue, bool isFormat, bool onlyFirstMatch, bool caseSensitive)
        {
            const char tagOpen = '<';
            const char tagClose = '>';
            const string anchorOpen = "<a ";
            const string anchorClose = "</a";

            source += " ";

            bool lastIterMatched = false;

            ScanState state = ScanState.Replace;
            StringBuilder outputBuffer = new StringBuilder(source.Length);

            Queue<char> tagstack = new Queue<char>(anchorOpen.Length);

            for (int i = 0; i < source.Length; i++)
            {
                char nextChar = source[i];
                tagstack.Enqueue(nextChar);

                switch (state)
                {
                    case ScanState.Replace:
                        if (anchorOpen == new string(tagstack.ToArray()))
                        {
                            state = ScanState.InAnchor;
                            break;
                        }
                        else
                        {
                            if (tagOpen == nextChar)
                            {
                                state = ScanState.InTag;
                                break;
                            }
                            else
                            {
                                string matchTarget;
                                if (source.Length - (i + oldValue.Length) > 0)
                                {
                                    // peek a head the next target length chunk + 1 boundary char
                                    matchTarget = source.Substring(i, oldValue.Length);

                                    //TODO: Do we want a case insensitive comparison in all cases?
                                    if (String.Equals(matchTarget, oldValue, caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase))
                                    {
                                        int index = 0 - i;
                                        if (index != 0) //Skip if we are at the start of the block
                                        {
                                            char prevBeforeMatch = source[(i) - 1];
                                            if (prevBeforeMatch != '>' && prevBeforeMatch != '"' && !Char.IsWhiteSpace(prevBeforeMatch)) {
                                                break;
                                            }
                                        }

                                        // check for word boundary
                                        char nextAfterMatch = source[i + oldValue.Length];
                                        if (!CharIsWordBoundary(nextAfterMatch))
                                            break;

                                        // format old with specifier else it's a straight replace
                                        if (isFormat)
                                            outputBuffer.AppendFormat(newValue, oldValue);
                                        else
                                            outputBuffer.Append(newValue);

                                        // if we're onlyFirstMatch, tack on remainder of source and return
                                        if (onlyFirstMatch)
                                        {
                                            outputBuffer.AppendFormat(source.Substring(i + oldValue.Length,
                                                source.Length - (i + oldValue.Length + 1)));
                                            return outputBuffer.ToString();
                                        }
                                        else // pop index ahead to end of match and continue
                                            i += oldValue.Length - 1;

                                        lastIterMatched = true;
                                        break;
                                    }
                                }
                            }
                        }

                        break;

                    case ScanState.InAnchor:
                        if (anchorClose == new string(tagstack.ToArray()))
                            state = ScanState.Replace;
                        break;

                    case ScanState.InTag:
                        if (anchorOpen == new string(tagstack.ToArray()))
                            state = ScanState.InAnchor;
                        else if (tagClose == nextChar)
                            state = ScanState.Replace;
                        break;

                    default:
                        break;
                }

                if (!lastIterMatched)
                {
                    outputBuffer.Append(nextChar);
                }
                else
                    lastIterMatched = false;
            }

            outputBuffer.Length--;
            return outputBuffer.ToString();
        }


        // cursory testing for word boundaries. there are still some cracks here for html,
        // e.g., &nbsp; and other boundary entities
        private static bool CharIsWordBoundary(char value)
        {
            switch (value)
            {
                case '_':
                    return false;
                default:
                    return !Char.IsLetterOrDigit(value);
            }
        }
    }
}
