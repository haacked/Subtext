using System;
using System.Configuration;

namespace Subtext.Web.UI
{
	/// <summary>
	/// Summary description for UIText.
	/// </summary>
	public sealed class UIText
	{
		private UIText()
		{
		}

		public static string PostCollection
		{
			get
			{
				return GetSafeConfig("PostCollection","Post Categories");
			}
		}

		public static string ArticleCollection
		{
			get
			{
				return GetSafeConfig("ArticleCollection","Article Categories");
			}
		}

		public static string ImageCollection
		{
			get
			{
				return GetSafeConfig("ImageCollection","Image Galleries");
			}
		}


		public static string Archives
		{
			get
			{
				return GetSafeConfig("Archives","Archives");
			}
		}

		private static string GetSafeConfig(string name, string defaultValue)
		{
			string text = ConfigurationSettings.AppSettings[name] as string;
			if(text == null)
			{
				return defaultValue;
			}
			return text;
		}
	}
}
