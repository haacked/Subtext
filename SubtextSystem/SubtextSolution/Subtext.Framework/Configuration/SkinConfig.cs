using System;

namespace Subtext.Framework.Configuration
{
	/// <summary>
	/// Summary description for SkinConfig.
	/// </summary>
	[Serializable]
	public class SkinConfig
	{
		public SkinConfig()
		{
			//
			// TODO: Add constructor logic here
			//
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

		//Subject to change. Do not rely on this for anything
		[System.Obsolete("This property is just a temporary placed holder. Very likely to go away in the next version",false)]
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
