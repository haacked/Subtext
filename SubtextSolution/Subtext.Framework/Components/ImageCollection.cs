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
using System.Collections.ObjectModel;

namespace Subtext.Framework.Components
{
	/// <summary>
	/// Represents a collection of <see cref="Image">Image</see> Components.
	/// </summary>
	[Serializable]
	public class ImageCollection : Collection<Image>
	{
		private LinkCategory _lc;
		/// <summary>
		/// The link category for this image.
		/// </summary>
		public LinkCategory Category
		{
			get { return _lc; }
			set { _lc = value; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ImageCollection">ImageCollection</see> class.k
		/// </summary>
		public ImageCollection()
			: base()
		{
		}
	}
}

