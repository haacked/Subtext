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
        public KeywordExpander(ObjectRepository repository)
        {
            _repository = repository;
        }

        readonly ObjectRepository _repository;

        public KeywordExpander(IEnumerable<KeyWord> keyWords)
        {
            _keywords = keyWords;
        }

        public IEnumerable<KeyWord> Keywords
        {
            get
            {
                if (_keywords == null)
                {
                    if (_repository != null)
                    {
                        _keywords = _repository.GetKeyWords();
                    }
                }
                return _keywords;
            }
        }

        IEnumerable<KeyWord> _keywords;

        public string Transform(string original)
        {
            return Keywords != null ? Keywords.Aggregate(original, ReplaceFormat) : original;
        }

        /// <summary>
        /// Preforms a forward scan and replace for a given pattern. 
        /// Can specify only to match first fine and if the pattern is CaseSensitive
        /// </summary>
        private static string ReplaceFormat(string source, KeyWord keyword)
        {
            return Scan(source, keyword.Word, keyword.GetFormat, true, keyword.ReplaceFirstTimeOnly,
                        keyword.CaseSensitive);
        }

        private static string Scan(string source, string oldValue, string newValue, bool isFormat, bool onlyFirstMatch,
                                   bool caseSensitive)
        {
            const char tagOpen = '<';
            const char tagClose = '>';
            const string anchorOpen = "<a ";
            const string anchorClose = "</a";

            source += " ";

            bool lastIterMatched = false;

            ScanState state = ScanState.Replace;
            var outputBuffer = new StringBuilder(source.Length);

            var tagstack = new Queue<char>(anchorOpen.Length);

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
                        if (tagOpen == nextChar)
                        {
                            state = ScanState.InTag;
                            break;
                        }
                        if (source.Length - (i + oldValue.Length) > 0)
                        {
                            // peek a head the next target length chunk + 1 boundary char
                            string matchTarget = source.Substring(i, oldValue.Length);

                            if (String.Equals(matchTarget, oldValue,
                                             caseSensitive
                                                 ? StringComparison.Ordinal
                                                 : StringComparison.OrdinalIgnoreCase))
                            {
                                int index = 0 - i;
                                if (index != 0) //Skip if we are at the start of the block
                                {
                                    char prevBeforeMatch = source[(i) - 1];
                                    if (prevBeforeMatch != '>' && prevBeforeMatch != '"' &&
                                       !Char.IsWhiteSpace(prevBeforeMatch))
                                    {
                                        break;
                                    }
                                }

                                // check for word boundary
                                char nextAfterMatch = source[i + oldValue.Length];
                                if (!CharIsWordBoundary(nextAfterMatch))
                                {
                                    break;
                                }

                                // format old with specifier else it's a straight replace
                                if (isFormat)
                                {
                                    outputBuffer.AppendFormat(newValue, oldValue);
                                }
                                else
                                {
                                    outputBuffer.Append(newValue);
                                }

                                // if we're onlyFirstMatch, tack on remainder of source and return
                                if (onlyFirstMatch)
                                {
                                    outputBuffer.Append(source.Substring(i + oldValue.Length,
                                                                               source.Length -
                                                                               (i + oldValue.Length + 1)));
                                    return outputBuffer.ToString();
                                }
                                i += oldValue.Length - 1;

                                lastIterMatched = true;
                                break;
                            }
                        }

                        break;

                    case ScanState.InAnchor:
                        if (anchorClose == new string(tagstack.ToArray()))
                        {
                            state = ScanState.Replace;
                        }
                        break;

                    case ScanState.InTag:
                        if (anchorOpen == new string(tagstack.ToArray()))
                        {
                            state = ScanState.InAnchor;
                        }
                        else if (tagClose == nextChar)
                        {
                            state = ScanState.Replace;
                        }
                        break;

                    default:
                        break;
                }

                if (!lastIterMatched)
                {
                    outputBuffer.Append(nextChar);
                }
                else
                {
                    lastIterMatched = false;
                }
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

        private enum ScanState
        {
            Replace,
            InTag,
            InAnchor
        }
    }
}