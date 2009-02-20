using System.Text.RegularExpressions;

namespace Subtext.Framework.Emoticons
{
    public class Emoticon
    {
        private Regex _regex;

        public Emoticon(string emoticonText, string imageTag) {
            EmoticonText = emoticonText;
            ImageTag = imageTag;
            string regexText = Regex.Escape(emoticonText);
            _regex = new Regex(regexText, RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Multiline);
        }

        public string EmoticonText{ 
            get; 
            private set; 
        }

        public string ImageTag { 
            get; 
            set; 
        }

        public string Replace(string text, string appRootUrl) {
            return _regex.Replace(text, string.Format(ImageTag, appRootUrl));
        }
    }
}
