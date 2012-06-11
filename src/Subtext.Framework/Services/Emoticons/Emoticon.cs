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

using System.Globalization;
using System.Text.RegularExpressions;

namespace Subtext.Framework.Emoticons
{
    public class Emoticon
    {
        private readonly Regex _regex;

        public Emoticon(string emoticonText, string imageTag)
        {
            EmoticonText = emoticonText;
            ImageTag = imageTag;
            string regexText = Regex.Escape(emoticonText);
            _regex = new Regex(regexText, RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Multiline);
        }

        public string EmoticonText { get; private set; }

        public string ImageTag { get; set; }

        public string Replace(string text, string appRootUrl)
        {
            return _regex.Replace(text, string.Format(CultureInfo.InvariantCulture, ImageTag, appRootUrl));
        }
    }
}