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
	}
}
