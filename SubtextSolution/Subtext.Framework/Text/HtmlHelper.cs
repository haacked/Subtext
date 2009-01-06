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
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Velocit.RegularExpressions;

namespace Subtext.Framework.Text
{
    /// <summary>
    /// Static class used for parsing, formatting, and validating HTML.
    /// </summary>
    public static class HtmlHelper
    {
        /// <summary>
        /// Replaces the host in the given url with the new host.
        /// </summary>
        /// <param name="originalUrl"></param>
        /// <param name="newHost"></param>
        /// <returns></returns>
        public static string ReplaceHost(string originalUrl, string newHost)
        {
            return Regex.Replace(originalUrl, @"(https?://).*?((:\d+)?/.*)?$", "$1" + newHost + "$2", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }

        /// <summary>
        /// Appends a CSS class to a control.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="newClass">The new class.</param>
        public static void AppendCssClass(WebControl control, string newClass)
        {
            if (control == null)
                throw new ArgumentNullException("control", "Cannot add a css class to a null control");

            if (newClass == null)
                throw new ArgumentNullException("newClass", "Cannot add a null css class to a control");

            string existingClasses = control.CssClass;
            if (String.IsNullOrEmpty(existingClasses))
            {
                control.CssClass = newClass;
                return;
            }

            string[] classes = existingClasses.Split(' ');
            foreach (string attributeValue in classes)
            {
                if (String.Equals(attributeValue, newClass, StringComparison.Ordinal))
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
            if (control == null)
                throw new ArgumentNullException("control", "Cannot remove a css class from a null control");

            if (classToRemove == null)
                throw new ArgumentNullException("classToRemove", "Cannot remove a null css class from a control");

            string existingClasses = control.CssClass;
            if (String.IsNullOrEmpty(existingClasses))
                return; //nothing to remove

            string[] classes = existingClasses.Split(new string[] { " ", "\t", "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            string newClasses = string.Empty;
            foreach (string cssClass in classes)
            {
                if (!String.Equals(cssClass, classToRemove, StringComparison.Ordinal))
                {
                    newClasses += cssClass + " ";
                }
            }

            if (newClasses.EndsWith(" "))
                newClasses = newClasses.Substring(0, newClasses.Length - 1);
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
            if (String.IsNullOrEmpty(existingValue))
            {
                control.Attributes[name] = value;
                return;
            }
            else
            {
                string[] attributeValues = control.Attributes[name].Split(' ');
                foreach (string attributeValue in attributeValues)
                {
                    if (String.Equals(attributeValue, value, StringComparison.Ordinal))
                    {
                        //value's already in there.
                        return;
                    }
                }
                control.Attributes[name] += " " + value;
            }
        }

        /// <summary>
        /// Returns a string with all HTML tags and comments removed.
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string RemoveHtml(string html)
        {
            //Yeah, this is ugly, but it's perf optimized! ;)
            if (html == null) {
                return string.Empty;
            }

            char[] strippedHtml = new char[html.Length];
            bool inHtmlTag = false;
            bool inHtmlAttribute = false;
            int cleanCount = 0;

            for (int i = 0; i < html.Length; i++)
            {
                char current = html[i];

                if (!inHtmlTag)
                {
                    if (current == '<')
                    {
                        if (NextCharBeginsHtmlTag(html, i))
                        {
                            inHtmlTag = true;
                            continue;
                        }
                    }
                }
                else
                { //in html tag
                    if (!inHtmlAttribute)
                    {
                        if (current == '>')
                        {
                            inHtmlTag = false;
                        }
                        if (current == '<')
                        {
                            if (!NextCharBeginsHtmlTag(html, i))
                            {
                                strippedHtml[cleanCount++] = current;
                                inHtmlTag = false;
                            }
                            else {
                                continue;
                            }
                        }
                        if (current == '=')
                        {
                            //Potentially in attribute value...
                            i++;
                            bool foundAttrStart = false;
                            char attrStartDelimiter = char.MinValue;
                            char currentAttrChar;

                            // We'll just "eat" the attribute here:
                            while (i < html.Length)
                            {
                                currentAttrChar = html[i];

                                //Find start delimiter...
                                if (!foundAttrStart)
                                {
                                    if (IsAttributeValueStartCharacter(currentAttrChar))
                                    {
                                        attrStartDelimiter = currentAttrChar;
                                        foundAttrStart = true;
                                        i++;
                                        continue;
                                    }
                                }
                                else
                                { //Find end delimiter...
                                    if (IsAttributeValueEndCharacter(currentAttrChar, attrStartDelimiter))
                                    {
                                        inHtmlAttribute = false;
                                        //Special case. The '>' ended the attr value and the tag
                                        //in the case of unquoted attr value
                                        if (currentAttrChar == '>')
                                        {
                                            inHtmlTag = false;
                                        }
                                        break;
                                    }
                                }
                                i++;
                            }
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
            if (attributeStartChar == '\'' || attributeStartChar == '"')
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
        /// Converts the entry body into XHTML compliant text. 
        /// Returns false if it encounters a problem in doing so.
        /// </summary>
        /// <param name="entry">Entry.</param>
        /// <returns></returns>
        public static bool ConvertHtmlToXHtml(Entry entry)
        {
            SgmlReader reader = new SgmlReader();
            reader.SetBaseUri(Config.CurrentBlog.RootUrl.ToString());
			entry.Body = ConvertHtmlToXHtml(reader, entry.Body, null);
            return true;
        }

		/// <summary>
		/// Converts the specified html into XHTML compliant text.
		/// </summary>
		/// <param name="html">html to convert.</param>
		/// <param name="converter">The converter.</param>
		/// <returns></returns>
        public static string ConvertHtmlToXHtml(string html, Converter<string, string> converter)
        {
            SgmlReader reader = new SgmlReader();
            return ConvertHtmlToXHtml(reader, html, converter);
        }

		/// <summary>
		/// Converts the specified html into XHTML compliant text.
		/// </summary>
		/// <param name="reader">sgml reader.</param>
		/// <param name="html">html to convert.</param>
		/// <param name="converter">The converter.</param>
		/// <returns></returns>
		/// ///
        private static string ConvertHtmlToXHtml(SgmlReader reader, string html, Converter<string, string> converter)
        {
            reader.DocType = "html";
            reader.WhitespaceHandling = WhitespaceHandling.All;
            // Hack to fix SF bug #1678030
            html = RemoveNewLineBeforeCDATA(html);
            reader.InputStream = new StringReader("<html>" + html + "</html>");
            reader.CaseFolding = CaseFolding.ToLower;
            StringWriter writer = new StringWriter();
            XmlWriter xmlWriter = null;
            try
            {
                xmlWriter = new XmlTextWriter(writer);

            	bool insideAnchor = false;
            	bool skipRead = false;
            	while ((skipRead || reader.Read()) && !reader.EOF)
                {
                	skipRead = false;
					switch(reader.NodeType)
					{
						case XmlNodeType.Element:
							//Special case for anchor tags for the time being. 
							//We need some way to communicate which elements the current node is nested within 
							if (reader.IsEmptyElement)
							{
								xmlWriter.WriteStartElement(reader.LocalName);
								xmlWriter.WriteAttributes(reader, true);
								if (reader.LocalName == "a" || reader.LocalName == "script" || reader.LocalName == "iframe" || reader.LocalName == "object")
									xmlWriter.WriteFullEndElement();
								else
									xmlWriter.WriteEndElement();
							}
							else
							{
								if (reader.LocalName == "a")
									insideAnchor = true;
								xmlWriter.WriteStartElement(reader.LocalName);
								xmlWriter.WriteAttributes(reader, true);
							}
							break;
						
						case XmlNodeType.Text:
							string text = reader.Value;

							if (converter != null && !insideAnchor)
								xmlWriter.WriteRaw(converter(HttpUtility.HtmlEncode(text)));
							else
								xmlWriter.WriteString(text);
							break;

						case XmlNodeType.EndElement:
							if (reader.LocalName == "a")
								insideAnchor = false;

                            if (reader.LocalName == "a" || reader.LocalName == "script" || reader.LocalName == "iframe" || reader.LocalName == "object")
								xmlWriter.WriteFullEndElement();
							else
								xmlWriter.WriteEndElement();
							break;

						default:
							xmlWriter.WriteNode(reader, true);
							skipRead = true;
							break;
					}
                }
            }
            finally
            {
                if (xmlWriter != null)
                {
                    xmlWriter.Close();
                }
            }

            string xml = writer.ToString();
            return xml.Substring("<html>".Length, xml.Length - "<html></html>".Length);
        }


        // Ugly hack to remove any new line that sits between a tag end
        // and the beginning of a CDATA section.
        // This to make sure the Xhtml is well formatted before processing it
        private static string RemoveNewLineBeforeCDATA(string text)
        {
            if (!String.IsNullOrEmpty(text))
            {
                string regex = @">(\r\n)+<!\[CDATA\[";
                Regex newLineStripper = new Regex(regex);

                return newLineStripper.Replace(text, "><![CDATA[");
            }
            return text;
        }

        /// <summary>
        /// Tests the specified string looking for illegal characters 
        /// or html tags.
        /// </summary>
        /// <param name="s">S.</param>
        /// <returns></returns>
        public static bool HasIllegalContent(string s)
        {
            if (String.IsNullOrEmpty(s))
            {
                return false;
            }
            if (s.IndexOf("<script", StringComparison.InvariantCultureIgnoreCase) > -1
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
			if (html == null)
				throw new ArgumentNullException("html");

			if (html.Length == 0)
				return string.Empty;

        	return ConvertHtmlToXHtml(html, delegate(string text)
    		{
    			string pattern =
    				@"((https?|ftp)://|www\.)[\w]+(.[\w]+)([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])";
    			MatchCollection matches =
    				Regex.Matches(text, pattern,
    							  RegexOptions.
    			              		IgnoreCase |
    							  RegexOptions.Compiled);
    			foreach (Match m in matches)
    			{
    				string httpPortion = string.Empty;
					if (!m.Value.Contains("://"))
					{
						httpPortion = "http://";
					}

    				text =
    					text.Replace(m.Value,
							string.Format("<a rel=\"nofollow external\" href=\"{0}{1}\" title=\"{1}\">{2}</a>", httpPortion, m.Value, ShortenUrl(m.Value, 50))
						);
    			}
    			return text;
    		});
        }

		/// <summary>
		/// Shortens a url for display.
		/// </summary>
		/// <param name="url">The URL.</param>
		/// <param name="max">Maximum size for the url. Anything longer gets shortened.</param>
		/// <returns></returns>
		public static string ShortenUrl(string url, int max)
		{
			if (url == null)
				throw new ArgumentNullException("url");

			if (max < 5)
				throw new ArgumentException("We will not shorten a URL to less than 5 characters. Come on now!", "max");

			if (url.Length <= max)
				return url;

			// Remove the protocal
			url = StringHelper.RightAfter(url, "://");

			if (url.Length <= max)
				return url;

			// Remove the folder structure, except for the last folder.
			int firstIndex = url.IndexOf("/") + 1;
			int startIndexForLastSlash = url.Length - 1;
			if (url.EndsWith("/"))
				startIndexForLastSlash--;

			int lastIndex = url.LastIndexOf("/", startIndexForLastSlash);
			
			if (firstIndex < lastIndex)
				url = StringHelper.LeftBefore(url, "/") + "/.../" + StringHelper.RightAfterLast(url, "/", startIndexForLastSlash, StringComparison.Ordinal);

			if (url.Length <= max)
				return url;

			// Remove URL parameters
			url = StringHelper.LeftBefore(url, "?");

			if (url.Length <= max)
				return url;

			// Remove URL fragment
			url = StringHelper.LeftBefore(url, "#");

			if (url.Length <= max)
				return url;

			// Shorten page
			firstIndex = url.LastIndexOf("/") + 1;
			lastIndex = url.LastIndexOf(".");
			if (lastIndex - firstIndex > 10)
			{
				string page = url.Substring(firstIndex, lastIndex - firstIndex);
				int length = url.Length - max + 3;
				url = url.Replace(page, "..." + page.Substring(length));
			}

			if (url.Length <= max)
				return url;

			//Trim of trailing slash if any.
			if (url.Length > max && url.EndsWith("/"))
				url = url.Substring(0, url.Length - 1);

			if (url.Length <= max)
				return url;

			if (url.Length > max)
				url = url.Substring(0, max - 3) + "...";

			return url;
		}

    	/// <summary>
        /// The only HTML we will allow is hyperlinks. 
        /// We will however, check for line breaks and replace 
        /// them with <br />
        /// </summary>
        /// <param name="stringToTransform"></param>
        /// <returns></returns>
        public static string SafeFormat(string stringToTransform)
        {
            if (stringToTransform == null)
                throw new ArgumentNullException("stringToTransform", "Cannot transform a null string.");

            stringToTransform = HttpContext.Current.Server.HtmlEncode(stringToTransform);
            string brTag = "<br />";
            return stringToTransform.Replace(Environment.NewLine, brTag);
        }

        /// <summary>
        /// Strips the RTB, whatever that is.
        /// </summary>
        /// <param name="text">Text.</param>
        /// <param name="host">Host.</param>
        /// <returns></returns>
        public static string StripRTB(string text, string host)
        {
            string s = Regex.Replace(text, "/localhost/S*Admin/", "", RegexOptions.IgnoreCase);
            return Regex.Replace(s, "<a href=\"/", "<a href=\"" + "http://" + host + "/", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Checks the text and prepends "http://" if it doesn't have it already.
        /// </summary>
        /// <param name="text">Text.</param>
        /// <returns></returns>
        public static Uri CheckForUrl(string text)
        {
            if (text == null)
                return null;

            text = text.Trim();

            if (String.IsNullOrEmpty(text))
                return null;

            if (!text.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase))
            {
                text = "http://" + text;
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
            if (text == null)
                throw new ArgumentNullException("text", "Cannot convert null to allowed html.");

            NameValueCollection allowedHtmlTags = Config.Settings.AllowedHtmlTags;

#if DEBUG
            //Assert that the NameValueCollection is case insensitive!
            if (allowedHtmlTags != null && allowedHtmlTags.Get("strong") != null && allowedHtmlTags.Get("STRONG") == null)
            {
                throw new InvalidOperationException("Darn it, it's case sensitive!" + allowedHtmlTags.Get("STRONG"));
            }
#endif
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
            if (allowedHtmlTags == null || allowedHtmlTags.Count == 0)
            {
                //This indicates that the AllowableCommentHtml configuration is either missing or
                //has no values, therefore just strip the text as normal.
                return HtmlSafe(text);
            }
            else
            {
                HtmlTagRegex regex = new HtmlTagRegex();
                MatchCollection matches = regex.Matches(text);

                if (matches.Count == 0)
                {
                    return HtmlSafe(text);
                }

                StringBuilder sb = new StringBuilder();

                int currentIndex = 0;
                foreach (Match match in matches)
                {
                    //Append text before the match.
                    if (currentIndex < match.Index)
                    {
                        sb.Append(HtmlSafe(text.Substring(currentIndex, match.Index - currentIndex)));
                    }

                    string tagName = match.Groups["tagname"].Value.ToLower(CultureInfo.InvariantCulture);

                    //check each match against the list of allowable tags.
                    if (allowedHtmlTags.Get(tagName) == null)
                    {
                        sb.Append(HtmlSafe(match.Value));
                    }
                    else
                    {
                        bool isEndTag = match.Groups["endTag"].Value.Length > 0;
                        if (isEndTag)
                        {
                            sb.Append("</" + tagName + ">");
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
                if (currentIndex < text.Length)
                {
                    sb.Append(HtmlSafe(text.Substring(currentIndex)));
                }

				return ConvertHtmlToXHtml(sb.ToString(), null);
            }
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

            if (allowedAttributesText != null && allowedAttributesText.Length > 0)
            {
                StringBuilder attrSB = new StringBuilder();

                //look to see which tag's attributes we are matching
                char[] splitter = { ',' };

                NameValueCollection attributes = GetAttributeNameValues(match);

                string[] allowedAttrs = allowedHtml[tagName].ToLower(CultureInfo.InvariantCulture).Split(splitter);

                // go thru each matched attribute, and determine if it's allowed
                foreach (string attributeName in attributes.Keys)
                {
                    foreach (string allowedAttr in allowedAttrs)
                    {
                        if (String.Equals(allowedAttr.Trim(), attributeName, StringComparison.InvariantCultureIgnoreCase))
                        {
                            // found an allowed attribute, so get the attribute value
                            string attrValue = attributes[attributeName];

                            // and now add the full attribute (key=value) to be returned
                            attrSB.Append(" " + attributeName.ToLower(CultureInfo.InvariantCulture) + "=\"" + attrValue + "\"");
                        }
                    }
                }
                return attrSB.ToString();
            }
            return string.Empty;
        }

        private static NameValueCollection GetAttributeNameValues(Match match)
        {
            CaptureCollection nameCaptures = match.Groups["attName"].Captures;
            CaptureCollection valueCaptures = match.Groups["attVal"].Captures;

            NameValueCollection attributes = new NameValueCollection();

            if (nameCaptures.Count == valueCaptures.Count)
            {
                for (int i = 0; i < nameCaptures.Count; i++)
                {
                    attributes.Add(nameCaptures[i].Value, valueCaptures[i].Value);
                }
                return attributes;
            }

            if (valueCaptures.Count == 0)
            {
                return attributes;
            }

            //this is a much more complicated issue.  
            //One of the attribute names doesn't have a value.
            //so we need to match them up somehow.
            int valueIndex = 0;
            for (int i = 0; i < nameCaptures.Count; i++)
            {
                Capture currentNameCapture = nameCaptures[i];
                Capture currentValueCapture;
                string name = currentNameCapture.Value;

                if (valueIndex == valueCaptures.Count)
                {
                    //No more values to worry about.
                    continue;
                }

                currentValueCapture = valueCaptures[valueIndex];

                //Peek ahead.
                if (i < nameCaptures.Count - 1)
                {
                    Capture peekAhead = nameCaptures[i + 1];
                    if (peekAhead.Index > currentValueCapture.Index && currentValueCapture.Index > currentNameCapture.Index)
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
                    if (currentValueCapture.Index > currentNameCapture.Index)
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
            string sPattern = @"(?:[hH][rR][eE][fF]\s*=)" +
                @"(?:[\s""']*)(?!#|[Mm]ailto|[lL]ocation.|[jJ]avascript|.*css|.*this\.)" +
                @"(.*?)(?:[\s>""'])";

            Regex r = new Regex(sPattern, RegexOptions.IgnoreCase);
            Match m;
            for (m = r.Match(text); m.Success; m = m.NextMatch())
            {
                if (m.Groups.ToString().Length > 0)
                {
                    string link = m.Groups[1].ToString();
                    if (!links.Contains(link))
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
        public static Uri ParseUri(string uri)
        {
            if (uri == null)
                return null;

            try
            {
                return new Uri(uri);
            }
            catch (UriFormatException)
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
        public static List<string> ParseTags(string html)
        {
            Regex relRegex = new Regex(@"\s+rel\s*=\s*(""[^""]*?\btag\b.*?""|'[^']*?\btag\b.*?')", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Regex hrefRegex = new Regex(@"\s+href\s*=\s*(""(?<url>[^""]*?)""|'(?<url>[^']*?)')", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Regex anchorRegex = new Regex(@"<a(\s+\w+\s*=\s*(?:""[^""]*?""|'[^']*?')(?!\w))+\s*>.*?</a>", RegexOptions.IgnoreCase | RegexOptions.Singleline);

            List<string> tags = new List<string>();
            List<string> loweredTags = new List<string>();

            foreach (Match m in anchorRegex.Matches(html))
            {
                string anchorHtml = m.Value;
                if (!relRegex.IsMatch(anchorHtml))
                    continue;

                Match urlMatch = hrefRegex.Match(anchorHtml);
                if (urlMatch.Success)
                {
                    string urlStr = urlMatch.Groups["url"].Value;
                    if (urlStr.EndsWith("/default.aspx", StringComparison.InvariantCultureIgnoreCase))
                        urlStr = urlStr.Substring(0, urlStr.Length - 13);
                    Uri url;
                    if (Uri.TryCreate(urlStr, UriKind.RelativeOrAbsolute, out url))
                    {
                        string[] seg = url.Segments;
                        string tag = HttpUtility.UrlDecode(seg[seg.Length - 1].Replace("/", ""));

                        //Keep a list of lowered tags so we can prevent duplicates without modifying capitalization
                        string loweredTag = tag.ToLower();
                        if (!loweredTags.Contains(loweredTag))
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
