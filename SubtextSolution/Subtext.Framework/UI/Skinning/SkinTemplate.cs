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
	/// Summary description for SkinTemplate.
	/// </summary>
	[Serializable]
	public class SkinTemplate
	{
		private string _skin;
		[XmlAttribute]
		public string Skin
		{
			get {return this._skin;}
			set {this._skin = value;}
		}

		private string _secondaryCss;
		
		/// <summary>
		/// Gets or sets the secondary CSS file name.  This is the 
		/// CSS file that differentiates this Skin from another skin 
		/// using the same ASCX files.
		/// </summary>
		/// <value>The secondary CSS.</value>
		[XmlAttribute]
		public string SecondaryCss
		{
			get {return this._secondaryCss;}
			set {this._secondaryCss = value;}
		}

		/// <summary>
		/// Whether or not this skin template has a secondary skin css file.
		/// </summary>
		public bool UseSecondaryCss
		{
			get{return (this.SecondaryCss != null && this.SecondaryCss.Trim().Length > 0);}
		}

		private string _skinID;
		[XmlAttribute]
		public string SkinID
		{
			get {return this._skinID;}
			set {this._skinID = value;}
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

