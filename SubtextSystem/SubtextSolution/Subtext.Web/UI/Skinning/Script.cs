using System;
using System.Xml.Serialization;

namespace Subtext.Web.UI.Skinning
{
	/// <summary>
	/// Declaration of a <code>script</code> element in the skin. Supports the capability to specify script includes in the 
	/// <code>head</code> element of the page.
	/// </summary>
	[Serializable]
	public class Script
	{
		private string _type = "text/javascript";
		/// <summary>
		/// Script type. Default value <code>text/javascript</code>
		/// </summary>
		[XmlAttribute]
		public string Type
		{
			get
			{
				return _type;
			}
			set
			{
				_type = value;
			}
		}

		private string _src;
		/// <summary>
		/// Location of the script. Specified as relative to the skin directory
		/// </summary>
		[XmlAttribute]
		public string Src
		{
			get
			{
				return _src;
			}
			set
			{
				_src = value;
			}	
		}
	}
}
