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
using System.Globalization;
using System.Xml.Serialization;

namespace Subtext.Framework.UI.Skinning
{
	/// <summary>
	/// Summary description for SkinTemplate.
	/// </summary>
	[Serializable]
	public class SkinTemplate
	{
		/// <summary>
		/// This is the folder that contains the template files (*.ascx) 
		/// for the current skin.
		/// </summary>
		[XmlAttribute]
		public string TemplateFolder
		{
			get {return this.templateFolder;}
			set {this.templateFolder = value;}
		}

		private string templateFolder;

		/// <summary>
		/// Gets or sets the stylesheet for this Skin.  Remember, 
		/// every skin template folder should include a "style.css" 
		/// file that is rendered by default.
		/// </summary>
		/// <remarks>
		/// This property makes it possible to have multiple skins 
		/// use the same template folder.
		/// </remarks>
		/// <value>The secondary CSS.</value>
		[XmlAttribute]
		public string StyleSheet
		{
			get {return this.styleSheet;}
			set {this.styleSheet = value;}
		}

		private string styleSheet;

		/// <summary>
		/// Whether or not this skin template has a secondary skin css file.
		/// </summary>
		[XmlIgnore]
		public bool HasSkinStylesheet
		{
			get{return (this.StyleSheet != null && this.StyleSheet.Trim().Length > 0);}
		}

		/// <summary>
		/// Gets the name of the skin as will be displayed in the 
		/// drop-down list in the admin section.
		/// </summary>
		[XmlAttribute]
		public string Name
		{
			get {return this.name;}
			set {this.name = value;}
		}

		private string name;
		
		/// <summary>
		/// A key representing this particular skin.  A Skin 
		/// is really a combination of the TemplateFolder and 
		/// the Stylesheet specified.
		/// </summary>
		[XmlIgnore]
		public string SkinKey
		{
			get
			{
				return (this.TemplateFolder + (this.StyleSheet != null && this.StyleSheet.Length > 0 ? "-" + this.StyleSheet : string.Empty)).ToUpper(CultureInfo.InvariantCulture);
			}
		}

		private Script[] _scripts;
		/// <summary>
		/// Collection of <code>script</code> elements, declared for the skin.
		/// </summary>
		[XmlArray("Scripts")]
		public Script[] Scripts
		{
			get
			{
				return _scripts;
			}
			set
			{
				_scripts = value;
			}
		}

		private Style[] _styles;
		/// <summary>
		/// Collection of stylesheet elements declared for the skin.
		/// </summary>
		[XmlArray("Styles")]
		public Style[] Styles
		{
			get
			{
				return _styles;
			}
			set
			{
				_styles = value;
			}
		}
		
	}
}

