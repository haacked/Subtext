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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using Subtext.Configuration;
using Subtext.Framework.Components;
using Subtext.Framework.Properties;
using Subtext.Framework.Text;
using System.Text;
using System.Web;
using Subtext.Framework.Providers;

namespace Subtext.Framework.Services
{
    public class SlugGenerator : ISlugGenerator
    {
        private const string DefaultWordSeparator = "-";

        private static FriendlyUrlSettings _defaultSettings = GetDefaultSettings();
        private static FriendlyUrlSettings GetDefaultSettings() {
            var config = new NameValueCollection();
            config.Add("textTransform", "LowerCase");
            config.Add("separatingCharacter", DefaultWordSeparator);
            config.Add("limitWordCount", "10");
            return new FriendlyUrlSettings(config);
        }

        public SlugGenerator(FriendlyUrlSettings slugSettings) : this(slugSettings, null) {
        }

        public SlugGenerator(FriendlyUrlSettings slugSettings, ObjectProvider repository)
        {
            SlugSettings = slugSettings ?? _defaultSettings;
            Repository = repository;
        }

        public FriendlyUrlSettings SlugSettings {
            get;
            private set;
        }

        protected ObjectProvider Repository {
            get;
            private set;
        }

        public string GetSlugFromTitle(Entry entry)
        {
            if (entry == null) {
                throw new ArgumentNullException("entry");
            }
            if (String.IsNullOrEmpty(entry.Title)) {
                throw new ArgumentException(Resources.ArgumentNull_String, "title");
            }

            string separator = SlugSettings.SeparatingCharacter;
            if (separator != "_" && separator != "." && separator != "-" && separator != string.Empty) {
                separator = DefaultWordSeparator;
            }

            string slug = RemoveNonWordCharacters(entry.Title);
            slug = RemoveTrailingPeriods(slug);

            if (SlugSettings.WordCountLimit > 0) {
                IEnumerable<string> words = slug.SplitIntoWords().Take(SlugSettings.WordCountLimit);
                var encodedWords = words.Select(word => ReplaceUnicodeCharacters(word));
                if (!String.IsNullOrEmpty(separator)) {
                    slug = String.Join(separator, encodedWords.ToArray());
                    slug = slug.Trim(new char[] { SlugSettings.SeparatingCharacter[0] });
                }
                else {
                    //special case for back compati
                    slug = slug.ToPascalCase();
                }
            }

            if (slug.IsNumeric()) {
                slug = "n_" + slug;
            }
            slug = EnsureUniqueness(slug, SlugSettings.SeparatingCharacter);
            slug = FriendlyUrlSettings.TransformString(slug, SlugSettings.TextTransformation);
            
            return slug;
        }

        string EnsureUniqueness(string originalSlug, string separator) {
            if (Repository == null) { 
                return originalSlug;
            }
            string[] suffixFormats = new[] { 
                string.Empty, "{0}Again", "{0}Yet{0}Again", "{0}And{0}Again", "{0}Once{0}Again", "{0}Once{0}More", "{0}To{0}Beat{0}A{0}Dead{0}Horse" };
            var slugs = suffixFormats.Select(s => originalSlug + String.Format(s, separator));
            string uniqueSlug = slugs.First(slug => Repository.GetEntry(slug, false, false) == null);
            return uniqueSlug;
        }

        static Regex _wordCharRegex = new Regex(@"[^\w\d\.\- ]+", RegexOptions.Compiled);
        
        private static string RemoveNonWordCharacters(string text) {
            return _wordCharRegex.Replace(text, string.Empty);
        }

        private static string ReplaceSpacesWithSeparator(string text, char wordSeparator)
        {
            if (wordSeparator == char.MinValue) {
                //Special case if we are just removing spaces.
                return text.ToPascalCase();
            } 
            else {
                return text.Replace(' ', wordSeparator);
            }
        }

        private static string ReplaceUnicodeCharacters(string text)
        {
            string normalized = text.Normalize(NormalizationForm.FormKD);
            Encoding removal = Encoding.GetEncoding(Encoding.ASCII.CodePage, 
                new EncoderReplacementFallback(string.Empty), 
                new DecoderReplacementFallback(string.Empty));
            byte[] bytes = removal.GetBytes(normalized);
            
            string encoded = Encoding.ASCII.GetString(bytes);
            if (String.IsNullOrEmpty(encoded)) {
                return HttpUtility.UrlEncode(text);
            }
            return encoded;
        }

        static Regex _trailingPeriodRegex = new Regex(@"\.+$", RegexOptions.Compiled);
        private static string RemoveTrailingPeriods(string text)
        {
            return _trailingPeriodRegex.Replace(text, string.Empty);
        }
    }
}
