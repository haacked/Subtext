#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
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
        private readonly static ILog Log = new Log();

        public static readonly FriendlyUrlSettings Settings =
            new FriendlyUrlSettings((NameValueCollection)ConfigurationManager.GetSection("FriendlyUrlSettings"));

        /// <summary>
        /// Initializes a new instance of the <see cref="FriendlyUrlSettings"/> class.
        /// </summary>
        /// <param name="config">The config.</param>
        public FriendlyUrlSettings(NameValueCollection config)
        {
            if (config == null)
            {
                return;
            }
            TextTransformation = ParseTextTransform(config["textTransform"]);
            SeparatingCharacter = config["separatingCharacter"];
            string wordCountLimitText = config["limitWordCount"];
            if (!String.IsNullOrEmpty(wordCountLimitText))
            {
                int wordCountLimit;
                int.TryParse(wordCountLimitText, out wordCountLimit);
                WordCountLimit = wordCountLimit;
            }
            Enabled = true;
        }

        public bool Enabled { get; private set; }

        /// <summary>
        /// The type of transformation to apply on the URL such 
        /// as LowerCase, UpperCase, or None.
        /// </summary>
        public TextTransform TextTransformation { get; private set; }

        /// <summary>
        /// The character used to separate words in the URL.
        /// </summary>
        public string SeparatingCharacter { get; private set; }

        public int WordCountLimit { get; private set; }

        static TextTransform ParseTextTransform(string enumValue)
        {
            if (String.IsNullOrEmpty(enumValue))
            {
                return TextTransform.None;
            }
            try
            {
                return (TextTransform)Enum.Parse(typeof(TextTransform), enumValue);
            }
            catch (FormatException)
            {
                Log.Warn(
                    "The 'textTransform' setting in the FriendlyUrlSettings section of Web.config has an incorrect value. It should be 'None', 'LowerCase', or 'UpperCase'");
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