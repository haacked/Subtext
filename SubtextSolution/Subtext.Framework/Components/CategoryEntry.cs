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
using Subtext.Extensibility;

namespace Subtext.Framework.Components
{
	/// <summary>
	/// Summary description for CategoryEntry.
	/// </summary>
	public class CategoryEntry : Entry
	{
		/// <summary>
		/// Creates a new <see cref="CategoryEntry"/> instance.
		/// </summary>
		public CategoryEntry() : base(PostType.None)
		{}

	    /// <summary>
	    /// Returns the categories for this entry.
	    /// </summary>
		public StringCollection Categories
		{
			get {return this.categories;}
		}

        private StringCollection categories = new StringCollection();
	}
}

