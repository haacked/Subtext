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
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;
using Sgml;
using Subtext.Framework.Configuration;
using Subtext.Framework.Properties;
using Subtext.Framework.Services;
using Velocit.RegularExpressions;

namespace Subtext.Framework.Text
{
    /// <summary>
    /// Static class used for parsing, formatting, and validating HTML.
    /// </summary>
    public static class HtmlHelper
    {
        static readonly Regex AnchorRegex = new Regex(@"<a(\s+\w+\s*=\s*(?:""[^""]*?""|'[^']*?')(?!\w))+\s*>.*?</a>",
                                                       RegexOptions.IgnoreCase | RegexOptions.Singleline |
                                                       RegexOptions.Compiled);

        static readonly Regex HrefRegex = new Regex(@"\s+href\s*=\s*(""(?<url>[^""]*?)""|'(?<url>[^']*?)')",
                                                     RegexOptions.IgnoreCase | RegexOptions.Singleline |
                                                     RegexOptions.Compiled);

        static readonly Regex RelRegex = new Regex(@"\s+rel\s*=\s*(""[^""]*?\btag\b.*?""|'[^']*?\btag\b.*?')",
                                                    RegexOptions.IgnoreCase | RegexOptions.Singleline |
                                                    RegexOptions.Compiled);

        /// <summary>
        /// Replaces the host in the given url with the new host.
        /// </summary>
        /// <param name="originalUrl"></param>
        /// <param name="newHost"></param>
        /// <returns></returns>
        public static string ReplaceHost(string originalUrl, string newHost)
        {
            const string pattern = @"(https?://).*?((:\d+)?/.*)?$";
            string replacement = string.Format("$1{0}$2", newHost);
            return Regex.Replace(originalUrl, pattern, replacement, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }

        /// <summary>
        /// Appends a CSS class to a control.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="newClass">The new class.</param>
        public static void AppendCssClass(WebControl control, string newClass)
        {
            if(control == null)
            {
                throw new ArgumentNullException("control");
            }

            if(newClass == null)
            {
                throw new ArgumentNullException("newClass");
            }

            string existingClasses = control.CssClass;
            if(String.IsNullOrEmpty(existingClasses))
            {
                control.CssClass = newClass;
                return;
            }

            string[] classes = existingClasses.Split(' ');
            foreach(string attributeValue in classes)
            {
                if(String.Equals(attributeValue, newClass, StringComparison.Ordinal))
                {
                    //value's already in there.
                    return;
                }
            }
            control.CssClass += " " + newClass;
        }

        /// <summary>
        /// Removes a CSS class to a control.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="classToRemove">The new class.</param>
        public static void RemoveCssClass(WebControl control, string classToRemove)
        {
            if(control == null)
            {
                throw new ArgumentNullException("control");
            }

            if(classToRemove == null)
            {
                throw new ArgumentNullException("classToRemove");
            }

            string existingClasses = control.CssClass;
            if(String.IsNullOrEmpty(existingClasses))
            {
                return; //nothing to remove
            }

            string[] classes = existingClasses.Split(new[] {" ", "\t", "\r\n", "\n", "\r"},
                                                     StringSplitOptions.RemoveEmptyEntries);
            string newClasses = string.Empty;
            foreach(string cssClass in classes)
            {
                if(!String.Equals(cssClass, classToRemove, StringComparison.Ordinal))
                {
                    newClasses += cssClass + " ";
                }
            }

            if(newClasses.EndsWith(" "))
            {
                newClasses = newClasses.Substring(0, newClasses.Length - 1);
            }
            control.CssClass = newClasses;
        }

        /// <summary>
        /// Appends the attribute value to the control appropriately.
        /// </summary>
        /// <param name="control"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void AppendAttributeValue(WebControl control, string name, string value)
        {
            string existingValue = control.Attributes[name];
            if(String.IsNullOrEmpty(existingValue))
            {
                control.Attributes[name] = value;
                return;
            }
            string[] attributeValues = control.Attributes[name].Split(' ');
            foreach(string attributeValue in attributeValues)
            {
                if(String.Equals(attributeValue, value, StringComparison.Ordinal))
                {
                    //value's already in there.
                    return;
                }
            }
            control.Attributes[name] += " " + value;
        }

        /// <summary>
        /// Returns a string with all HTML tags and comments removed.
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string RemoveHtml(string html)
        {
            //Yeah, this is ugly, but it's perf optimized! ;)
            if(html == null)
            {
                return string.Empty;
            }

            var strippedHtml = new char[html.Length];
            bool inHtmlTag = false;
            int cleanCount = 0;

            for(int i = 0; i < html.Length; i++)
            {
                char current = html[i];

                if(!inHtmlTag)
                {
                    if(current == '<')
                    {
                        if(NextCharBeginsHtmlTag(html, i))
                        {
                            inHtmlTag = true;
                            continue;
                        }
                    }
                }
                else
                {
                    //in html tag
                    if(current == '>')
                    {
                        inHtmlTag = false;
                    }
                    if(current == '<')
                    {
                        if(!NextCharBeginsHtmlTag(html, i))
                        {
                            strippedHtml[cleanCount++] = current;
                            inHtmlTag = false;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    if(current == '=')
                    {
                        //Potentially in attribute value...
                        i++;
                        bool foundAttrStart = false;
                        char attrStartDelimiter = char.MinValue;

                        // We'll just "eat" the attribute here:
                        while(i < html.Length)
                        {
                            char currentAttrChar = html[i];

                            //Find start delimiter...
                            if(!foundAttrStart)
                            {
                                if(IsAttributeValueStartCharacter(currentAttrChar))
                                {
                                    attrStartDelimiter = currentAttrChar;
                                    foundAttrStart = true;
                                    i++;
                                    continue;
                                }
                            }
                            else
                            {
                                //Find end delimiter...
                                if(IsAttributeValueEndCharacter(currentAttrChar, attrStartDelimiter))
                                {
                                    //Special case. The '>' ended the attr value and the tag
                                    //in the case of unquoted attr value
                                    if(currentAttrChar == '>')
                                    {
                                        inHtmlTag = false;
                                    }
                                    break;
                                }
                            }
                            i++;
                        }
                    }
                    continue;
                }

                strippedHtml[cleanCount++] = current;
            }

            return new String(strippedHtml, 0, cleanCount);
        }

        private static bool NextCharBeginsHtmlTag(string html, int i)
        {
            return i + 1 < html.Length && IsHtmlTagBeginCharacter(html[i + 1]);
        }

        private static bool IsAttributeValueStartCharacter(char c)
        {
            return !Char.IsWhiteSpace(c) || c == '\'' || c == '"';
        }

        private static bool IsAttributeValueEndCharacter(char c, char attributeStartChar)
        {
            if(attributeStartChar == '\'' || attributeStartChar == '"')
            {
                return c == attributeStartChar;
            }

            return Char.IsWhiteSpace(c) || c == '>';
        }

        private static bool IsHtmlTagBeginCharacter(char c)
        {
            return c == '!' || c == '/' || IsEnglishLetter(c);
        }

        private static bool IsEnglishLetter(char nextChar)
        {
            return ('a' <= nextChar && nextChar <= 'z') || ('A' <= nextChar && nextChar <= 'Z');
        }


        /// <summary>
        /// Tests the specified string looking for illegal characters 
        /// or html tags.
        /// </summary>
        /// <param name="s">S.</param>
        /// <returns></returns>
        public static bool HasIllegalContent(string s)
        {
            if(String.IsNullOrEmpty(s))
            {
                return false;
            }
            if(s.IndexOf("<script", StringComparison.InvariantCultureIgnoreCase) > -1
               || s.IndexOf("&#60script", StringComparison.InvariantCultureIgnoreCase) > -1
               || s.IndexOf("&60script", StringComparison.InvariantCultureIgnoreCase) > -1
               || s.IndexOf("%60script", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Wraps an anchor tag around all urls. Makes sure not to wrap already 
        /// wrapped urls.
        /// </summary>
        /// <param name="html">Html containing urls to convert.</param>
        /// <returns></returns>
        public static string ConvertUrlsToHyperLinks(string html)
        {
            if(html == null)
            {
                throw new ArgumentNullException("html");
            }

            if(html.Length == 0)
            {
                return string.Empty;
            }

            var xhtmlConverter = new XhtmlConverter(text =>
                {
                    const string pattern = @"((https?|ftp)://|www\.)[\w]+(.[\w]+)([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])";
                    MatchCollection matches =
                        Regex.Matches(text, pattern,
                                      RegexOptions.
                                          IgnoreCase |
                                      RegexOptions.Compiled);
                    foreach(Match m in matches)
                    {
                        string httpPortion = string.Empty;
                        if(!m.Value.Contains("://"))
                        {
                            httpPortion = "http://";
                        }

                        text =
                            text.Replace(m.Value,
                                         string.Format(CultureInfo.InvariantCulture,
                                                       "<a rel=\"nofollow external\" href=\"{0}{1}\" title=\"{1}\">{2}</a>",
                                                       httpPortion, m.Value, ShortenUrl(m.Value, 50))
                                );
                    }
                    return text;
                });

            return xhtmlConverter.Transform(html);
        }

        private static IEnumerable<Func<string, string>> GetShorteners(int max)
        {
            yield return url => url; // identity
            yield return url => url.RightAfter("://");
            yield return ReplacePathSegmentsWithElipses;
            yield return url => url.LeftBefore("#", StringComparison.Ordinal);
            yield return url => url.LeftBefore("?", StringComparison.Ordinal);
            yield return ChopLastSegment;
            yield return url => url.Chomp("/", StringComparison.Ordinal);
            yield return url =>
                {
                    if(url.Length > 8)
                    {
                        url = url.Substring(0, max - 3) + "...";
                    }
                    else
                    {
                        url = url.Substring(0, max);
                    }
                    return url;
                };
        }

        private static string ReplacePathSegmentsWithElipses(string urlAfterProtocol)
        {
            // examples:
            //  example.com/foo/bar/baz/ => example.com/.../baz/
            //  example.com/foo/bar/baz => example.com/.../baz
            //  example.com/foo/bar#baz => example.com/.../bar#baz
            //  example.com/foo/bar/baz/?beels => example.com/.../baz/?beels
            //  example.com/foobeels.txt => example.com/foobeels.txt

            int lastIndex = urlAfterProtocol.IndexOf('?');
            if(lastIndex < 0)
            {
                lastIndex = urlAfterProtocol.IndexOf('#');
            }
            if(lastIndex < 0)
            {
                lastIndex = urlAfterProtocol.Length;
            }

            // First slash after domain name
            int firstSlashIndex = urlAfterProtocol.IndexOf('/');
            if(firstSlashIndex < 0)
            {
                return urlAfterProtocol;
            }

            // ignore query and fragment
            string urlWithoutTrailingStuff = urlAfterProtocol.Substring(0, lastIndex);
            urlWithoutTrailingStuff = urlWithoutTrailingStuff.Chomp("/", StringComparison.Ordinal);

            int lastSlashIndex = urlWithoutTrailingStuff.LastIndexOf('/');
            if(lastSlashIndex < 0)
            {
                return urlAfterProtocol;
            }

            if(lastSlashIndex < firstSlashIndex + 5)
            {
                return urlAfterProtocol;
            }
            return urlAfterProtocol.Substring(0, firstSlashIndex) + "/..." + urlAfterProtocol.Substring(lastSlashIndex);
        }

        private static string ChopLastSegment(string urlWithoutProtocolNorQuery)
        {
            urlWithoutProtocolNorQuery = urlWithoutProtocolNorQuery.Chomp("/", StringComparison.OrdinalIgnoreCase);
            int lastIndex = urlWithoutProtocolNorQuery.LastIndexOf('/');
            if(lastIndex > -1)
            {
                urlWithoutProtocolNorQuery = urlWithoutProtocolNorQuery.Substring(0, lastIndex);
            }
            return urlWithoutProtocolNorQuery;
        }


        /// <summary>
        /// Shortens a url for display.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="max">Maximum size for the url. Anything longer gets shortened.</param>
        /// <returns></returns>
        public static string ShortenUrl(this string url, int max)
        {
            if(url == null)
            {
                throw new ArgumentNullException("url");
            }

            if(max < 5)
            {
                throw new ArgumentOutOfRangeException("max", max, Resources.ArgumentException_TooShortUrl);
            }

            foreach(var shortener in GetShorteners(max))
            {
                url = shortener(url);
                if(url.Length <= max)
                {
                    return url;
                }
            }

            return url;
        }

        /// <summary>
        /// The only HTML we will allow is hyperlinks. 
        /// We will however, check for line breaks and replace 
        /// them with <br />
        /// </summary>
        /// <returns></returns>
        public static string SafeFormat(string stringToTransform, HttpServerUtilityBase server)
        {
            if(stringToTransform == null)
            {
                throw new ArgumentNullException("stringToTransform");
            }

            stringToTransform = HttpUtility.HtmlEncode(stringToTransform);
            return stringToTransform.Replace(Environment.NewLine, "<br />");
        }

        /// <summary>
        /// Checks the text and prepends "http://" if it doesn't have it already.
        /// </summary>
        /// <param name="text">Text.</param>
        /// <returns></returns>
        public static Uri EnsureUrl(this string text)
        {
            if(text == null)
            {
                return null;
            }

            text = text.Trim();

            if(String.IsNullOrEmpty(text))
            {
                return null;
            }

            if(!text.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase))
            {
                text = string.Format("http://{0}", text);
            }

            return new Uri(text);
        }

        /// <summary>
        /// Filters text to only allow defined HTML.
        /// </summary>
        /// <param name="text">Text.</param>
        /// <returns></returns>
        public static string ConvertToAllowedHtml(string text)
        {
            if(text == null)
            {
                throw new ArgumentNullException("text");
            }

            NameValueCollection allowedHtmlTags = Config.Settings.AllowedHtmlTags;
            return ConvertToAllowedHtml(allowedHtmlTags, text);
        }

        /// <summary>
        /// Filters text to only allow defined HTML.
        /// </summary>
        /// <param name="allowedHtmlTags">The allowed html tags.</param>
        /// <param name="text">Text.</param>
        /// <returns></returns>
        public static string ConvertToAllowedHtml(NameValueCollection allowedHtmlTags, string text)
        {
            if(allowedHtmlTags == null || allowedHtmlTags.Count == 0)
            {
                //This indicates that the AllowableCommentHtml configuration is either missing or
                //has no values, therefore just strip the text as normal.
                return HtmlSafe(text);
            }
            var regex = new HtmlTagRegex();
            MatchCollection matches = regex.Matches(text);

            if(matches.Count == 0)
            {
                return HtmlSafe(text);
            }

            var sb = new StringBuilder();

            int currentIndex = 0;
            foreach(Match match in matches)
            {
                //Append text before the match.
                if(currentIndex < match.Index)
                {
                    sb.Append(HtmlSafe(text.Substring(currentIndex, match.Index - currentIndex)));
                }

                string tagName = match.Groups["tagname"].Value.ToLower(CultureInfo.InvariantCulture);

                //check each match against the list of allowable tags.
                if(allowedHtmlTags.Get(tagName) == null)
                {
                    sb.Append(HtmlSafe(match.Value));
                }
                else
                {
                    bool isEndTag = match.Groups["endTag"].Value.Length > 0;
                    if(isEndTag)
                    {
                        sb.AppendFormat("</{0}>", tagName);
                    }
                    else
                    {
                        sb.Append("<" + tagName);
                        sb.Append(FilterAttributes(tagName, match, allowedHtmlTags) + ">");
                    }
                }
                currentIndex = match.Index + match.Length;
            }
            //add the remaining text.
            if(currentIndex < text.Length)
            {
                sb.Append(HtmlSafe(text.Substring(currentIndex)));
            }

            var converter = new XhtmlConverter();
            return converter.Transform(sb.ToString());
        }

        private static string HtmlSafe(string text)
        {
            //replace &, <, >, and line breaks with <br />
            text = text.Replace("&", "&amp;");
            text = text.Replace("<", "&lt;");
            text = text.Replace(">", "&gt;");
            text = text.Replace("\r", string.Empty);
            text = text.Replace("\n", "<br />");
            return text;
        }

        /// <summary>
        /// Removes any non-permitted attributes for the given tagName. The permitted attributes 
        /// are determined by the given allowedHtml collection.
        /// </summary>
        /// <param name="tagName"></param>
        /// <param name="match"></param>
        /// <param name="allowedHtml"></param>
        /// <returns></returns>
        /// <remarks>This will be a high volume method, so make it as efficient as possible</remarks>
        private static string FilterAttributes(string tagName, Match match, NameValueCollection allowedHtml)
        {
            string allowedAttributesText = allowedHtml[tagName];

            if(!string.IsNullOrEmpty(allowedAttributesText))
            {
                var attributesStringBuilder = new StringBuilder();

                //look to see which tag's attributes we are matching
                char[] splitter = {','};

                NameValueCollection attributes = GetAttributeNameValues(match);

                string[] allowedAttrs = allowedHtml[tagName].ToLower(CultureInfo.InvariantCulture).Split(splitter);

                // go thru each matched attribute, and determine if it's allowed
                foreach(string attributeName in attributes.Keys)
                {
                    foreach(string allowedAttr in allowedAttrs)
                    {
                        if(String.Equals(allowedAttr.Trim(), attributeName, StringComparison.InvariantCultureIgnoreCase))
                        {
                            // found an allowed attribute, so get the attribute value
                            string attrValue = attributes[attributeName];

                            // and now add the full attribute (key=value) to be returned
                            attributesStringBuilder.Append(" " + attributeName.ToLower(CultureInfo.InvariantCulture) + "=\"" + attrValue +
                                          "\"");
                        }
                    }
                }
                return attributesStringBuilder.ToString();
            }
            return string.Empty;
        }

        public static IEnumerable<string> GetAttributeValues(this string html, string tagName, string attributeName)
        {
            var reader = new SgmlReader
            {
                DocType = "html",
                WhitespaceHandling = WhitespaceHandling.All,
                InputStream = new StringReader(string.Format("<html>{0}</html>", html))
            };

            while(reader.Read() && !reader.EOF)
            {
                if(reader.NodeType == XmlNodeType.Element && reader.LocalName == tagName)
                {
                    yield return reader.GetAttribute(attributeName);
                }
            }
        }

        private static NameValueCollection GetAttributeNameValues(Match match)
        {
            CaptureCollection nameCaptures = match.Groups["attName"].Captures;
            CaptureCollection valueCaptures = match.Groups["attVal"].Captures;

            var attributes = new NameValueCollection();

            if(nameCaptures.Count == valueCaptures.Count)
            {
                for(int i = 0; i < nameCaptures.Count; i++)
                {
                    attributes.Add(nameCaptures[i].Value, valueCaptures[i].Value);
                }
                return attributes;
            }

            if(valueCaptures.Count == 0)
            {
                return attributes;
            }

            //this is a much more complicated issue.  
            //One of the attribute names doesn't have a value.
            //so we need to match them up somehow.
            int valueIndex = 0;
            for(int i = 0; i < nameCaptures.Count; i++)
            {
                Capture currentNameCapture = nameCaptures[i];
                string name = currentNameCapture.Value;

                if(valueIndex == valueCaptures.Count)
                {
                    //No more values to worry about.
                    continue;
                }

                Capture currentValueCapture = valueCaptures[valueIndex];

                //Peek ahead.
                if(i < nameCaptures.Count - 1)
                {
                    Capture peekAhead = nameCaptures[i + 1];
                    if(peekAhead.Index > currentValueCapture.Index &&
                       currentValueCapture.Index > currentNameCapture.Index)
                    {
                        attributes.Add(name, currentValueCapture.Value);
                        //We are on the right value.
                        valueIndex++;
                        continue;
                    }
                }
                else
                {
                    //we're on the last item.
                    if(currentValueCapture.Index > currentNameCapture.Index)
                    {
                        attributes.Add(name, currentValueCapture.Value);
                        //We are on the right value.
                        valueIndex++;
                        continue;
                    }
                }
            }
            return attributes;
        }


        /// <summary>
        /// Returns a string collection of URLs within the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static ICollection<string> GetLinks(string text)
        {
            var links = new List<string>();
            const string sPattern = @"(?:[hH][rR][eE][fF]\s*=)" +
                                    @"(?:[\s""']*)(?!#|[Mm]ailto|[lL]ocation.|[jJ]avascript|.*css|.*this\.)" +
                                    @"(.*?)(?:[\s>""'])";

            var r = new Regex(sPattern, RegexOptions.IgnoreCase);
            for(Match m = r.Match(text); m.Success; m = m.NextMatch())
            {
                if(m.Groups.ToString().Length > 0)
                {
                    string link = m.Groups[1].ToString();
                    if(!links.Contains(link))
                    {
                        links.Add(link);
                    }
                }
            }
            return links;
        }

        /// <summary>
        /// Parses the URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        public static Uri ParseUri(this string uri)
        {
            if(uri == null)
            {
                return null;
            }

            try
            {
                return new Uri(uri);
            }
            catch(UriFormatException)
            {
                return null;
            }
        }

        /// <summary>
        /// Parses some html and returns a string collection of the tag names contained 
        /// within the HTML.
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static IList<string> ParseTags(this string html)
        {
            var tags = new List<string>();
            if(String.IsNullOrEmpty(html))
            {
                return tags;
            }

            var loweredTags = new List<string>();

            foreach(Match m in AnchorRegex.Matches(html))
            {
                string anchorHtml = m.Value;
                if(!RelRegex.IsMatch(anchorHtml))
                {
                    continue;
                }

                Match urlMatch = HrefRegex.Match(anchorHtml);
                if(urlMatch.Success)
                {
                    string urlStr = urlMatch.Groups["url"].Value;
                    if(urlStr.EndsWith("/default.aspx", StringComparison.InvariantCultureIgnoreCase))
                    {
                        urlStr = urlStr.Substring(0, urlStr.Length - 13);
                    }
                    Uri url;
                    if(Uri.TryCreate(urlStr, UriKind.RelativeOrAbsolute, out url))
                    {
                        string[] seg = url.Segments;
                        string tag = HttpUtility.UrlDecode(seg[seg.Length - 1].Replace("/", ""));

                        //Keep a list of lowered tags so we can prevent duplicates without modifying capitalization
                        string loweredTag = tag.ToLower(CultureInfo.InvariantCulture);
                        if(!loweredTags.Contains(loweredTag))
                        {
                            loweredTags.Add(loweredTag);
                            tags.Add(tag);
                        }
                    }
                }
            }
            return tags;
        }
    }
}