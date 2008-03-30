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

namespace Subtext.Framework.UI.Skinning
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
		/// Script type. Default value is <code>text/javascript</code>
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

		/// <summary>
		/// Whether or not to defer loading the script till page is loaded. 
		/// Be careful which scripts you use this with.
		/// </summary>
		[XmlAttribute]
		public bool Defer
		{
			get { return this.defer; }
			set { this.defer = value; }
		}

		private bool defer;
	}
}
