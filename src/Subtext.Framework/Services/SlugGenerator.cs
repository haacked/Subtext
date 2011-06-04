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
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Ninject;
using Subtext.Configuration;
using Subtext.Framework.Components;
using Subtext.Framework.Properties;
using Subtext.Framework.Providers;
using Subtext.Framework.Text;

namespace Subtext.Framework.Services
{
    public class SlugGenerator : ISlugGenerator
    {
        private const string DefaultWordSeparator = "-";

        private static readonly FriendlyUrlSettings DefaultSettings = GetDefaultSettings();
        static readonly Regex TrailingPeriodRegex = new Regex(@"\.+$", RegexOptions.Compiled);
        static readonly Regex WordCharRegex = new Regex(@"[^\w\d\.\- ]+", RegexOptions.Compiled);

        public SlugGenerator(FriendlyUrlSettings slugSettings)
            : this(slugSettings, null)
        {
        }

        [Inject]
        public SlugGenerator(FriendlyUrlSettings slugSettings, ObjectRepository repository)
        {
            SlugSettings = slugSettings ?? DefaultSettings;
            Repository = repository;
        }

        public FriendlyUrlSettings SlugSettings { get; private set; }

        protected ObjectRepository Repository { get; private set; }

        #region ISlugGenerator Members

        public string GetSlugFromTitle(Entry entry)
        {
            if(entry == null)
            {
                throw new ArgumentNullException("entry");
            }
            if(String.IsNullOrEmpty(entry.Title))
            {
                throw new ArgumentException(Resources.Argument_EntryHasNoTitle, "entry");
            }

            string separator = SlugSettings.SeparatingCharacter;
            if(separator != "_" && separator != "." && separator != "-" && separator != string.Empty)
            {
                separator = DefaultWordSeparator;
            }

            string slug = RemoveNonWordCharacters(entry.Title);
            slug = RemoveTrailingPeriods(slug);

            if(SlugSettings.WordCountLimit > 0)
            {
                IEnumerable<string> words = slug.SplitIntoWords().Take(SlugSettings.WordCountLimit);
                IEnumerable<string> encodedWords = words.Select(word => ReplaceUnicodeCharacters(word));
                if(!String.IsNullOrEmpty(separator))
                {
                    slug = String.Join(separator, encodedWords.ToArray());
                    slug = slug.Trim(new[] { separator[0] });
                }
                else
                {
                    //special case for back compati
                    slug = slug.ToPascalCase();
                }
            }

            if(slug.IsNumeric())
            {
                slug = "n_" + slug;
            }
            slug = EnsureUniqueness(slug, SlugSettings.SeparatingCharacter);
            slug = FriendlyUrlSettings.TransformString(slug, SlugSettings.TextTransformation);

            return slug;
        }

        #endregion

        private static FriendlyUrlSettings GetDefaultSettings()
        {
            var config = new NameValueCollection
            {
                {"textTransform", "LowerCase"},
                {"separatingCharacter", DefaultWordSeparator},
                {"limitWordCount", "10"}
            };
            return new FriendlyUrlSettings(config);
        }

        string EnsureUniqueness(string originalSlug, string separator)
        {
            if(Repository == null)
            {
                return originalSlug;
            }
            var suffixFormats = new[]
            {
                string.Empty, "{0}Again", "{0}Yet{0}Again", "{0}And{0}Again", "{0}Once{0}Again", "{0}Once{0}More",
                "{0}To{0}Beat{0}A{0}Dead{0}Horse"
            };
            IEnumerable<string> slugs =
                suffixFormats.Select(s => originalSlug + String.Format(CultureInfo.InvariantCulture, s, separator));
            string uniqueSlug = slugs.First(slug => Repository.GetEntry(slug, false, false) == null);
            return uniqueSlug;
        }

        private static string RemoveNonWordCharacters(string text)
        {
            return WordCharRegex.Replace(text, string.Empty);
        }

        private static string ReplaceUnicodeCharacters(string text)
        {
            string normalized = text.Normalize(NormalizationForm.FormKD);
            Encoding removal = Encoding.GetEncoding(Encoding.ASCII.CodePage,
                                                    new EncoderReplacementFallback(string.Empty),
                                                    new DecoderReplacementFallback(string.Empty));
            byte[] bytes = removal.GetBytes(normalized);

            string encoded = Encoding.ASCII.GetString(bytes);
            if(String.IsNullOrEmpty(encoded))
            {
                return HttpUtility.UrlEncode(text);
            }
            return encoded;
        }

        private static string RemoveTrailingPeriods(string text)
        {
            return TrailingPeriodRegex.Replace(text, string.Empty);
        }
    }
}