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

		static SkinConfig CreateDefaultSkin()
		{
			SkinConfig defaultSkin = new SkinConfig();
			defaultSkin.SkinName = "RedBook";
			defaultSkin.SkinCssFile = "Blue.css";
			return defaultSkin;
		}

		private string _skinName;
		public string SkinName
		{
			get{return _skinName;}
			set{_skinName = value;}
		}

		private string _skinCssFile;
		public string SkinCssFile
		{
			get{return _skinCssFile;}
			set{_skinCssFile = value;}
		}

		private string _skinCssText;
		public string SkinCssText
		{
			get{return _skinCssText;}
			set{_skinCssText = value;}
		}

		public bool HasSecondaryFile
		{
			get
			{
				return SkinCssFile != null && SkinCssFile.Trim().Length > 0;
			}
		}

		public bool HasSecondaryText
		{
			get
			{
				return SkinCssText != null && SkinCssText.Trim().Length > 0;
			}
		}

		//TODO: Subject to change. Do not rely on this for anything
		public string SkinID
		{
			get
			{
				if(this.HasSecondaryFile)
				{
					return SkinName + "-" + SkinCssFile;
				}
				return SkinName;
			}
		}
	}
}
