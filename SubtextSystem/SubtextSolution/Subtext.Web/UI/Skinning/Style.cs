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
using System.Xml.Serialization;

namespace Subtext.Web.UI.Skinning
{
	/// <summary>
	/// Declaration of a <code>style</code> element in the skin. Supports the capability 
	/// to specify css stylesheet includes in the <code>head</code> element of the page.
	/// </summary>
	[Serializable]
	public class Style
	{
		private string _href;
		private string _title;
		private string _media;

		/// <summary>
		/// Location of the script. Specified as relative to the skin directory
		/// </summary>
		[XmlAttribute("href")]
		public string Href
		{
			get
			{
				return _href;
			}
			set
			{
				_href = value;
			}	
		}

		/// <summary>
		/// Title of the styesheet.
		/// </summary>
		[XmlAttribute("title")]
		public string Title
		{
			get
			{
				return _title;
			}
			set
			{
				_title = value;
			}	
		}

		/// <summary>
		/// Media for the stylesheet.  Can be a comma delimited list.
		/// </summary>
		/// <remarks>
		/// Allowed media... aural, braille, emboss, handheld, print, projection 
		/// screen, tty, tv
		/// </remarks>
		[XmlAttribute("media")]
		public string Media
		{
			get
			{
				return _media;
			}
			set
			{
				_media = value;
			}	
		}

	}
}
