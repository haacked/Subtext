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
	public static class KeyWords
	{
		public static KeyWord GetKeyWord(int KeyWordID)
		{
			return ObjectProvider.Instance().GetKeyWord(KeyWordID);
		}

        public static ICollection<KeyWord> GetKeyWords()
		{
			return ObjectProvider.Instance().GetKeyWords();
		}

        public static IPagedCollection<KeyWord> GetPagedKeyWords(int pageIndex, int pageSize)
		{
			return ObjectProvider.Instance().GetPagedKeyWords(pageIndex, pageSize);
		}

		public static void UpdateKeyWord(KeyWord kw)
		{
			ObjectProvider.Instance().UpdateKeyWord(kw);
		}

		public static int CreateKeyWord(KeyWord kw)
		{
			return ObjectProvider.Instance().InsertKeyWord(kw);
		}

		public static bool DeleteKeyWord(int KeyWordID)
		{
			return ObjectProvider.Instance().DeleteKeyWord(KeyWordID);
		}
	}
}
