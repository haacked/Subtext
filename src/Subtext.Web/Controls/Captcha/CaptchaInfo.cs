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
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Text;
using System.Globalization;
using System.Text;

namespace Subtext.Web.Controls.Captcha
{
    [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Captcha")]
    [Serializable]
    public struct CaptchaInfo
    {
        private const string DefaultValidRandomTextChars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";

        private const string GoodFontList =
            "arial; arial black; comic sans ms; courier new; estrangelo edessa; franklin gothic medium; georgia; lucida console; lucida sans unicode; mangal; microsoft sans serif; palatino linotype; sylfaen; tahoma; times new roman; trebuchet ms; verdana;";

        private static readonly Random Random = new Random();
        public DateTime DateGenerated;
        public string FontFamily;
        public int Height;
        private int _randomTextLength;
        private string _text;
        private string _validRandomTextChars;
        public CaptchaImage.FontWarpFactor WarpFactor;
        public int Width;

        /// <summary>
        /// Initializes a new instance of the <see cref="CaptchaInfo"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        public CaptchaInfo(string text)
        {
            Width = 180;
            Height = 50;
            _randomTextLength = 5;
            WarpFactor = CaptchaImage.FontWarpFactor.Low;
            FontFamily = string.Empty;
            _text = text;
            _validRandomTextChars = DefaultValidRandomTextChars;
            DateGenerated = DateTime.UtcNow;
            FontFamily = RandomFontFamily();
        }

        /// <summary>
        /// A string of valid characters to use in the Captcha text.  
        /// A random character will be selected from this string for 
        /// each character.
        /// </summary>
        public string TextChars
        {
            get { return _validRandomTextChars ?? DefaultValidRandomTextChars; }
            set { _validRandomTextChars = value; }
        }

        /// <summary>
        /// Gets or sets the text to render.
        /// </summary>
        /// <value>The text.</value>
        public string Text
        {
            get
            {
                if (String.IsNullOrEmpty(_text))
                {
                    _text = GenerateRandomText();
                }
                return _text;
            }
            set { _text = value; }
        }

        /// <summary>
        /// Number of characters to use in the CAPTCHA test.
        /// </summary>
        /// <value>The length of the text.</value>
        public int TextLength
        {
            get
            {
                if (_randomTextLength <= 0)
                {
                    _randomTextLength = 4;
                }
                return _randomTextLength;
            }
            set
            {
                _randomTextLength = value;
                _text = GenerateRandomText();
            }
        }

        /// <summary>
        /// Returns a random font family name.
        /// </summary>
        /// <returns></returns>
        private static string RandomFontFamily()
        {
            var collection1 = new InstalledFontCollection();
            FontFamily[] familyArray1 = collection1.Families;
            string fontFamily = "bogus";
            while (GoodFontList.IndexOf(fontFamily, StringComparison.OrdinalIgnoreCase) == -1)
            {
                fontFamily = familyArray1[Random.Next(0, collection1.Families.Length)].Name.ToLowerInvariant();
            }
            return fontFamily;
        }

        /// <summary>
        /// Returns a base 64 encrypted serialized representation of this object.
        /// </summary>
        /// <returns></returns>
        public string ToEncryptedString()
        {
            if (Width == 0)
            {
                Width = 180;
            }

            if (Height == 0)
            {
                Height = 50;
            }

            return CaptchaBase.EncryptString(ToString());
        }

        /// <summary>
        /// Reconstructs an instance of this type from an encrypted serialized string.
        /// </summary>
        /// <param name="encrypted"></param>
        public static CaptchaInfo FromEncryptedString(string encrypted)
        {
            string decrypted = CaptchaBase.DecryptString(encrypted);
            string[] values = decrypted.Split('|');

            var info = new CaptchaInfo
            {
                Width = int.Parse(values[0], CultureInfo.InvariantCulture),
                Height = int.Parse(values[1], CultureInfo.InvariantCulture),
                WarpFactor = (CaptchaImage.FontWarpFactor)Enum.Parse(typeof(CaptchaImage.FontWarpFactor), values[2]),
                FontFamily = values[3],
                Text = values[4],
                DateGenerated = DateTime.ParseExact(values[5], "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture)
            };
            return info;
        }

        private string GenerateRandomText()
        {
            var builder = new StringBuilder();
            int length = TextChars.Length;
            for (int i = 0; i < TextLength; i++)
            {
                builder.Append(TextChars.Substring(Random.Next(length), 1));
            }
            DateGenerated = DateTime.UtcNow;
            return builder.ToString();
        }

        /// <summary>
        /// Returns the fully qualified type name of this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> containing a fully qualified type name.
        /// </returns>
        public override string ToString()
        {
            return String.Format(CultureInfo.InvariantCulture, "{0}|{1}|{2}|{3}|{4}|{5}"
                                 , Width
                                 , Height
                                 , WarpFactor
                                 , FontFamily
                                 , Text
                                 , DateGenerated.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture));
        }
    }
}
