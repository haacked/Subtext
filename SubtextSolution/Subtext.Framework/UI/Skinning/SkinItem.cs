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
using System.Web.Caching;
using System.Xml.Serialization;

namespace Subtext.Framework.UI.Skinning
{
	/// <summary>
	/// Summary description for SkinItem.
	/// </summary>
	[Serializable]
	public class SkinItem
	{
		[XmlAttribute]
		public bool UseSpacer = false;
		[XmlAttribute]
		public string ControlName;
		[XmlAttribute]
		public string ParentControl;
		[XmlAttribute]
		public CacheKey CacheKey = CacheKey.None;
		[XmlAttribute]
		public int CacheTime = 0;
		[XmlAttribute]
		public CacheItemPriority CacheItemPriority = CacheItemPriority.Default;
	}
}

