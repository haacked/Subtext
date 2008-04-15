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
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using log4net;
using Subtext.Framework.Logging;

namespace Subtext.Configuration
{
	/// <summary>
	/// Encapsulates settings for friendly URL generation.
	/// </summary>
	public sealed class FriendlyUrlSettings
	{
		private readonly static ILog log = new Log();		

		static FriendlyUrlSettings settings = new FriendlyUrlSettings((NameValueCollection) ConfigurationManager.GetSection("FriendlyUrlSettings"));

		public static FriendlyUrlSettings Settings
		{
			get { return settings; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FriendlyUrlSettings"/> class.
		/// </summary>
		/// <param name="config">The config.</param>
		public FriendlyUrlSettings(NameValueCollection config)
		{
			if (config == null)
				return;
			textTransformation = ParseTextTransform(config["textTransform"]);
			separatingCharacter = config["separatingCharacter"];
			string wordCountLimitText = config["limitWordCount"];
			if(!String.IsNullOrEmpty(wordCountLimitText) )
			{
				int.TryParse(wordCountLimitText, out this.wordCountLimit);
			}
			enabled = true;
		}

		public bool Enabled
		{
			get { return this.enabled; }
		}

		private bool enabled = false;

		/// <summary>
		/// The type of transformation to apply on the URL such 
		/// as LowerCase, UpperCase, or None.
		/// </summary>
		public TextTransform TextTransformation
		{
			get { return this.textTransformation; }
		}

		/// <summary>
		/// The character used to separate words in the URL.
		/// </summary>
		public string SeparatingCharacter
		{
			get { return this.separatingCharacter; }
		}

		public int WordCountLimit
		{
			get { return this.wordCountLimit; }
		}

		private int wordCountLimit = 0;

		private string separatingCharacter;

		private TextTransform textTransformation;

		static TextTransform ParseTextTransform(string enumValue)
		{
			if (String.IsNullOrEmpty(enumValue))
				return TextTransform.None;
			try
			{
				return (TextTransform)Enum.Parse(typeof(TextTransform), enumValue);
			}
			catch (FormatException)
			{
				log.Warn("The 'textTransform' setting in the FriendlyUrlSettings section of Web.config has an incorrect value. It should be 'None', 'LowerCase', or 'UpperCase'");
				return TextTransform.None;
			}
		}

		public static string TransformString(string s, TextTransform textTransform)
		{
			switch (textTransform)
			{
				case TextTransform.None:
					break;

				case TextTransform.LowerCase:
					return s.ToLower(CultureInfo.InvariantCulture);

				case TextTransform.UpperCase:
					return s.ToUpper(CultureInfo.InvariantCulture);
			}
			return s;
		}
	}
	
	public enum TextTransform
	{
		None,
		LowerCase,
		UpperCase
	}
}
