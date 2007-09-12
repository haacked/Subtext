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

namespace Subtext.Framework.Configuration
{
	/// <summary>
	/// Summary description for SkinConfig.
	/// </summary>
	[Serializable]
	public class SkinConfig
	{
		static SkinConfig _defaultSkin = CreateDefaultSkin();

		/// <summary>
		/// Gets the default skin.
		/// </summary>
		/// <returns></returns>
		public static SkinConfig GetDefaultSkin()
		{
			return _defaultSkin;
		}

		/// <summary>
		/// Creates the default skin to be used if none is specified.
		/// </summary>
		/// <returns></returns>
		static SkinConfig CreateDefaultSkin()
		{
			SkinConfig defaultSkin = new SkinConfig();
			defaultSkin.TemplateFolder = "RedBook";
			defaultSkin.SkinStyleSheet = "Blue.css";
			return defaultSkin;
		}

		/// <summary>
		/// This is the skin template folder. Note that multiple "Skins" can 
		/// share the same template folder. The template folder contains the 
		/// *.ascx files for the skins.
		/// </summary>
		public string TemplateFolder
		{
			get{return templateFolder;}
			set{templateFolder = value;}
		}

		private string templateFolder;
		
		/// <summary>
		/// Gets or sets the skin's primary CSS file, if any.  
		/// Some Skins have multiple flavors based on different CSS files.  
		/// For example, Redbook, Bluebook, and Greenbook are all variations 
		/// of the skin Redbook.  They vary by the skin css file.
		/// </summary>
		/// <value>The skin CSS file.</value>
		public string SkinStyleSheet
		{
			get{return skinStyleSheet;}
			set{skinStyleSheet = value;}
		}

		private string skinStyleSheet;

		/// <summary>
		/// This is CSS text that is entered within the admin section.
		/// </summary>
		public string CustomCssText
		{
			get{return customCssText;}
			set{customCssText = value;}
		}
		
		private string customCssText;

		/// <summary>
		/// Returns true if the skin has a skin specific css file 
		/// that is applied after style.css (there is one style.css 
		/// per template folder).
		/// </summary>
		public bool HasStyleSheet
		{
			get
			{
				return SkinStyleSheet != null && SkinStyleSheet.Trim().Length > 0;
			}
		}

		/// <summary>
		/// Returns true if the user specified some custom CSS in the admin section.
		/// </summary>
		public bool HasCustomCssText
		{
			get
			{
				return CustomCssText != null && CustomCssText.Trim().Length > 0;
			}
		}

		/// <summary>
		/// A lookup key for a skin.
		/// </summary>
		public string SkinKey
		{
			get
			{
				if(HasStyleSheet)
				{
					return TemplateFolder + "-" + SkinStyleSheet;
				}
				return TemplateFolder;
			}
		}
	}
}
