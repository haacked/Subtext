using System;
using System.Globalization;
using System.Text;
using Subtext.Framework.Components;
using Subtext.Framework.Providers;

namespace Subtext.Framework.Util
{
	public class KeyWords
	{
		#region Readers/Writers

		private enum ScanState : byte { Replace, InTag, InAnchor };

		public static string Replace(string source, string oldValue, string newValue)
		{
			return Scan(source, oldValue, newValue, false, false,true);
		}

		public static string Replace(string source, string oldValue, string newValue, bool onlyFirstMatch, bool CaseSensitive)
		{
			return Scan(source, oldValue, newValue, false, onlyFirstMatch,CaseSensitive);
		}

		/// <summary>
		/// Preforms a forward scan and replace for a given pattern. Replaces all finds and preforms a case sensitive search
		/// </summary>
		/// <param name="source">Text to search</param>
		/// <param name="oldValue">Pattern to search for</param>
		/// <param name="formatString">Replaced Pattern</param>
		/// <returns></returns>
		public static string ReplaceFormat(string source, string oldValue, string formatString)
		{
			return Scan(source, oldValue, formatString, true, false,true);
		}

		/// <summary>
		/// Preforms a forward scan and replace for a given pattern. Can specify only to match first fine and if the pattern is CaseSensitive
		/// </summary>
		/// <param name="source">Text to search</param>
		/// <param name="oldValue">Pattern to search for</param>
		/// <param name="formatString">Replaced Pattern</param>
		/// <param name="onlyFirstMatch">Match First Only</param>
		/// <param name="CaseSensitive">Is CaseSensitive</param>
		/// <returns></returns>
		public static string ReplaceFormat(string source, string oldValue, string formatString, bool onlyFirstMatch, bool CaseSensitive)
		{
			return Scan(source, oldValue, formatString, true, onlyFirstMatch, CaseSensitive);
		}

		private static string Scan(string source, string oldValue, string newValue, bool isFormat, bool onlyFirstMatch, bool CaseSensitive)
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
								string matchTarget;
								if (source.Length - (i + tagstack.Length + oldValue.Length) > 0)
								{
									// peek a head the next target length chunk + 1 boundary char
									matchTarget = source.Substring(i + tagstack.Length, oldValue.Length);
									//Do we want a case insesitive comparison?
									if(string.Compare(matchTarget,oldValue,!CaseSensitive) == 0)
									//if (matchTarget == oldValue)
									{
										int index= tagstack.Length - i;
										if(index != 0) //Skip if we are at the start of the block
										{
											char prevBeforeMatch = source[(i + tagstack.Length)-1];
											if(prevBeforeMatch != '>' && prevBeforeMatch != '"' && !Char.IsWhiteSpace(prevBeforeMatch))
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
				case '_' :
					return false;
					//				case '<' :
					//					return false;
				default:
					return !Char.IsLetterOrDigit(value);
			}
		}
	

		#endregion

		#region Data

		public static void Format(ref Entry entry)
		{
			KeyWordCollection kwc = GetKeyWords();
			if(kwc != null && kwc.Count > 0)
			{
				KeyWord kw = null;
				for(int i =0; i<kwc.Count;i++)
				{
					kw = kwc[i];
					entry.Body = ReplaceFormat(entry.Body,kw.Word,kw.GetFormat,kw.ReplaceFirstTimeOnly,kw.CaseSensitive);
				}
			}
		}

		public static KeyWord GetKeyWord(int KeyWordID)
		{
			return DTOProvider.Instance().GetKeyWord(KeyWordID);
		}

		public static KeyWordCollection GetKeyWords()
		{
			return DTOProvider.Instance().GetKeyWords();
		}

		public static PagedKeyWordCollection GetPagedKeyWords(int pageIndex, int pageSize,bool sortDescending)
		{
			return DTOProvider.Instance().GetPagedKeyWords(pageIndex,pageSize,sortDescending);
		}

		public static void UpdateKeyWord(KeyWord kw)
		{
			DTOProvider.Instance().UpdateKeyWord(kw);
		}

		public static int InsertKeyWord(KeyWord kw)
		{
			return DTOProvider.Instance().InsertKeyWord(kw);
		}

		public static bool DeleteKeyWord(int KeyWordID)
		{
			return DTOProvider.Instance().DeleteKeyWord(KeyWordID);
		}

		#endregion
	}


	#region CharQueue
	internal class CharQueue : object
	{
		private char[] _list;
		private int _charCount = 0;

		protected CharQueue() {}
		public CharQueue(int length)
		{
			_list = new char[length];
		}

		public char this[int i]
		{
			get { return _list[i]; }
			set { _list[i] = value; }
		}

		public void Enqueue(char x)
		{
			for (int i = 0; i < _list.Length; i++)
			{
				if (i < _list.Length - 1)
					_list[i] = _list[i + 1];
				else
					_list[i] = x;
			}				
		}

		public int Length
		{
			get { return _charCount; }
		}

		public char Dequeue()
		{
			char result = _list[0];

			char[] compacted = new char[_list.Length - 1];
			_list.CopyTo(compacted, 1);				
			_list = compacted;

			return result;
		}

		public bool Holds(string value)
		{
			return Holds(value, true);
		}

		public bool Holds(string value, bool ignoreCase)
		{
			return (0 == String.Compare(value, this.ToString(), ignoreCase, 
			                            CultureInfo.InvariantCulture));					
		}

		public override string ToString()
		{
			return new string(_list);
		}

		public string ToString(int length)
		{
			if (length != _list.Length)
			{	
				char[] results = new char[length];
				_list.CopyTo(results,0);
				return new string(results);
			}
			else
				return this.ToString();
		}
	

		#endregion

	}
}
