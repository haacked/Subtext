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
using System.Text;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework.Components;
using Subtext.Framework.Providers;

namespace Subtext.Framework.Util
{
	public static class Keywords
	{
		#region Readers/Writers

		private enum ScanState : byte { Replace, InTag, InAnchor };

		public static string Replace(string source, string oldValue, string newValue)
		{
			return Scan(source, oldValue, newValue, false, false);
		}

		public static string Replace(string source, string oldValue, string newValue, bool onlyFirstMatch)
		{
			return Scan(source, oldValue, newValue, false, onlyFirstMatch);
		}

		/// <summary>
		/// Preforms a forward scan and replace for a given pattern. 
		/// Replaces all finds and preforms a case sensitive search
		/// </summary>
		/// <param name="source">Text to search</param>
		/// <param name="oldValue">Pattern to search for</param>
		/// <param name="formatString">Replaced Pattern</param>
		/// <returns></returns>
		public static string ReplaceFormat(string source, string oldValue, string formatString)
		{
			return Scan(source, oldValue, formatString, true, false);
		}

		/// <summary>
		/// Preforms a forward scan and replace for a given pattern. 
		/// Can specify only to match first fine and if the pattern is CaseSensitive
		/// </summary>
		/// <param name="source">Text to search</param>
		/// <param name="oldValue">Pattern to search for</param>
		/// <param name="formatString">Replaced Pattern</param>
		/// <param name="onlyFirstMatch">Match First Only</param>
		/// <returns></returns>
		public static string ReplaceFormat(string source, string oldValue, string formatString, bool onlyFirstMatch)
		{
			return Scan(source, oldValue, formatString, true, onlyFirstMatch);
		}

		private static string Scan(string source, string oldValue, string newValue, bool isFormat, bool onlyFirstMatch)
		{
			const char tagOpen = '<';
			const char tagClose = '>';
			const string anchorOpen = "<a ";
			const string anchorClose = "</a";

			source += " ";

			bool lastIterMatched = false;

			ScanState state = ScanState.Replace;
			StringBuilder outputBuffer = new StringBuilder(source.Length);

			CharQueue tagstack =
				new CharQueue(anchorOpen.Length >= anchorClose.Length ? anchorOpen.Length : anchorClose.Length);

			for (int i = 0; i < source.Length; i++)
			{
				char nextChar = source[i];
				tagstack.Enqueue(nextChar);

				switch (state)
				{
					case ScanState.Replace:
						if (anchorOpen == tagstack.ToString(anchorOpen.Length))
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
								if (source.Length - (i + tagstack.Length + oldValue.Length) > 0)
								{
									// peek a head the next target length chunk + 1 boundary char
									string matchTarget = source.Substring(i + tagstack.Length, oldValue.Length);

									//TODO: Do we want a case insensitive comparison in all cases?
									if (String.Equals(matchTarget, oldValue, StringComparison.InvariantCultureIgnoreCase))
									//if (matchTarget == oldValue)
									{
										int index = tagstack.Length - i;
										if (index != 0) //Skip if we are at the start of the block
										{
											char prevBeforeMatch = source[(i + tagstack.Length) - 1];
											if (prevBeforeMatch != '>' && prevBeforeMatch != '"' && !Char.IsWhiteSpace(prevBeforeMatch))
											{
												break;
											}
										}

										// check for word boundary
										char nextAfterMatch = source[i + tagstack.Length + oldValue.Length];
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
						if (anchorClose == tagstack.ToString(anchorClose.Length))
							state = ScanState.Replace;
						break;

					case ScanState.InTag:
						if (anchorOpen == tagstack.ToString(anchorOpen.Length))
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

			return outputBuffer.ToString().Trim();
		}


		// cursory testing for word boundaries. there are still some cracks here for html,
		// e.g., &nbsp; and other boundary entities
		private static bool CharIsWordBoundary(char value)
		{
			switch (value)
			{
				case '_':
					return false;
				//				case '<' :
				//					return false;
				default:
					return !Char.IsLetterOrDigit(value);
			}
		}


		#endregion

		#region Data

		public static void Format(Entry entry)
		{
			ICollection<KeyWord> kwc = GetKeywords();
			if (kwc != null && kwc.Count > 0)
			{
				foreach (KeyWord keyword in kwc)
				{
					entry.Body = ReplaceFormat(entry.Body, keyword.Word, keyword.GetFormat, keyword.ReplaceFirstTimeOnly);
				}
			}
		}

		public static KeyWord GetKeyword(int id)
		{
			return ObjectProvider.Instance().GetKeyword(id);
		}

		public static ICollection<KeyWord> GetKeywords()
		{
			return ObjectProvider.Instance().GetKeywords();
		}

		public static IPagedCollection<KeyWord> GetPagedKeywords(int pageIndex, int pageSize)
		{
			return ObjectProvider.Instance().GetKeywordsByPage(pageIndex, pageSize);
		}

		/// <summary>
		/// Updates the keyword.
		/// </summary>
		/// <param name="kw">The kw.</param>
		public static void UpdateKeyword(KeyWord kw)
		{
			ObjectProvider.Instance().UpdateKeyword(kw);
		}

		/// <summary>
		/// Creates the keyword.
		/// </summary>
		/// <param name="kw">The kw.</param>
		/// <returns></returns>
		public static int CreateKeyword(KeyWord kw)
		{
			return ObjectProvider.Instance().InsertKeyword(kw);
		}

		/// <summary>
		/// Deletes the key word.
		/// </summary>
		/// <param name="KeyWordID">The key word ID.</param>
		public static void DeleteKeyword(int KeyWordID)
		{
			ObjectProvider.Instance().DeleteKeyword(KeyWordID);
		}

		#endregion
	}


	
}
