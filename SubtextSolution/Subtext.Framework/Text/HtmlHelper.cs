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
	/// Static class used for parseing, formatting, and validating HTML.
	/// </summary>
	public static class HtmlHelper
	{
		public static void AppendCssClass(WebControl control, string newClass)
		{
			string existingClasses = control.CssClass;
			if (String.IsNullOrEmpty(existingClasses))
			{
				control.CssClass = newClass;
				return;
			}
			else
			{
				string[] classes = control.CssClass.Split(' ');
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
		}
		
		/// <summary>
		/// Appends the attribute value to the control appropriately.
		/// </summary>
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
			else
			{
				string[] attributeValues = control.Attributes[name].Split(' ');
				foreach (string attributeValue in attributeValues)
				{
					if(String.Equals(attributeValue, value, StringComparison.Ordinal))
					{
						//value's already in there.
						return;
					}
				}
				control.Attributes[name] += " " + value;
			}
		}
		
		/// <summary>
		/// Strips HTML tags from the specified text.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <returns></returns>
		public static string RemoveHtml(string text)
		{
			if (!String.IsNullOrEmpty(text))
			{
			    HtmlTagRegex regex = new HtmlTagRegex();
                return regex.Replace(text, string.Empty);
			}
            return text;
		}

		/// <summary>
		/// Converts the entry body into XHTML compliant text. 
		/// Returns false if it encounters a problem in doing so.
		/// </summary>
		/// <param name="entry">Entry.</param>
		/// <returns></returns>
		public static bool ConvertHtmlToXHtml(Entry entry)
		{
			try
			{
				SgmlReader reader = new SgmlReader();
				reader.SetBaseUri(Config.CurrentBlog.RootUrl);
				reader.DocType = "html";
				reader.WhitespaceHandling = WhitespaceHandling.All;
				reader.InputStream = new StringReader("<html>" + entry.Body + "</html>");
				reader.CaseFolding = CaseFolding.ToLower;
				StringWriter writer = new StringWriter();
				XmlTextWriter xmlWriter = null;
				try
				{
					xmlWriter = new XmlTextWriter(writer);
				
					while (reader.Read()) 
					{
						xmlWriter.WriteNode(reader, true);
					}
				}
				finally
				{
					if(xmlWriter != null)
					{
						xmlWriter.Close(); 
					}
				}

				string xml = writer.ToString();
				entry.Body = xml.Substring("<html>".Length, xml.Length - "<html></html>".Length);
				entry.IsXHMTL = true;
				return true;
			}
			catch(XmlException)
			{
				entry.IsXHMTL = false;
				throw;
			}
		}

		/// <summary>
		/// Tests the specified string looking for illegal characters 
		/// or html tags.
		/// </summary>
		/// <param name="s">S.</param>
		/// <returns></returns>
		public static bool HasIllegalContent(string s)
		{
			if (s == null || s.Trim().Length == 0)
			{
				return false;
			}
			if (s.IndexOf("<script")> - 1 
				|| s.IndexOf("&#60script")> - 1 
				|| s.IndexOf("&60script")> - 1 
				|| s.IndexOf("%60script")> - 1)
			{
				throw new IllegalPostCharactersException("Illegal Characters Found");
			}
			return false;
		}

		/// <summary>
		/// Wraps an anchor tag around urls.
		/// </summary>
		/// <param name="text">Text.</param>
		/// <returns></returns>
		public static string EnableUrls(string text)
		{
			string pattern = @"(http|ftp|https):\/\/[\w]+(.[\w]+)([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])";
			MatchCollection matches = Regex.Matches(text, pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
			foreach (Match m in matches) 
			{
				text = text.Replace(m.ToString(), "<a rel=\"nofollow\" target=\"_new\" href=\"" + m.ToString() + "\">" + m.ToString() + "</a>");
			}
			return text;			
		}

		/// <summary>
		/// The only HTML we will allow is hyperlinks. 
		/// We will however, check for line breaks and replace them with <br />
		/// </summary>
		/// <param name="stringToTransform"></param>
		/// <returns></returns>
		public static string SafeFormatWithUrl(string stringToTransform)
		{
			return EnableUrls(SafeFormat(stringToTransform));
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
			string s = Regex.Replace(text, "/localhost/S*Admin/","", RegexOptions.IgnoreCase);
			return Regex.Replace(s,"<A href=\"/","<A href=\"" + "http://" + host + "/",RegexOptions.IgnoreCase);			
		}

		/// <summary>
		/// Checks the text and prepends "http://" if it doesn't have it already.
		/// </summary>
		/// <param name="text">Text.</param>
		/// <returns></returns>
		public static string CheckForUrl(string text)
		{
			if (text == null 
				|| text.Trim().Length == 0 
				|| text.Trim().ToLower(CultureInfo.InvariantCulture).StartsWith("http://"))
			{
				return text;
			}
			return "http://" + text;
		}

		/// <summary>
		/// Filters text to only allow defined HTML.
		/// </summary>
		/// <param name="text">Text.</param>
		/// <returns></returns>
		public static string ConvertToAllowedHtml(string text)
		{
			if(text == null)
				throw new ArgumentNullException("text", "Cannot convert null to allowed html.");

			NameValueCollection allowedHtmlTags = Config.Settings.AllowedHtmlTags;
			
#if DEBUG
			//Assert that the NameValueCollection is case insensitive!
			if(allowedHtmlTags != null && allowedHtmlTags.Get("strong") != null && allowedHtmlTags.Get("STRONG") == null)
			{
				throw new InvalidOperationException("Darn it, it's case sensitive!" + allowedHtmlTags.Get("STRONG"));
			}
#endif		
			return ConvertToAllowedHtml(allowedHtmlTags, text);
		}

		/// <summary>
		/// Filters text to only allow defined HTML.
		/// </summary>
		/// <param name="text">Text.</param>
		/// <param name="allowedHtmlTags">The allowed html tags.</param>
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

				if(matches.Count == 0)
				{
					return HtmlSafe(text);
				}
			
				StringBuilder sb = new StringBuilder();

				int currentIndex = 0;
				foreach (Match match in matches)
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
				if(currentIndex < text.Length)
				{
					sb.Append(HtmlSafe(text.Substring(currentIndex)));
				}

				return sb.ToString();
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
				char[] splitter  = {','};
			
				NameValueCollection attributes = GetAttributeNameValues(match);

				string[] allowedAttrs = allowedHtml[tagName].ToLower(CultureInfo.InvariantCulture).Split(splitter);

				// go thru each matched attribute, and determine if it's allowed
				foreach (string attributeName in attributes.Keys)
				{
					foreach (string allowedAttr in allowedAttrs)
					{
						if(StringHelper.AreEqualIgnoringCase(allowedAttr.Trim(), attributeName))
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
				Capture currentValueCapture;
				string name = currentNameCapture.Value;
				
				if(valueIndex == valueCaptures.Count)
				{
					//No more values to worry about.
					continue;
				}
				
				currentValueCapture = valueCaptures[valueIndex];

				//Peek ahead.
				if(i < nameCaptures.Count - 1)
				{
					Capture peekAhead = nameCaptures[i+1];
					if(peekAhead.Index > currentValueCapture.Index && currentValueCapture.Index > currentNameCapture.Index)
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
		public static StringCollection GetLinks(string text)
		{			
			StringCollection links = new StringCollection();
			string sPattern = @"(?:[hH][rR][eE][fF]\s*=)" +
				@"(?:[\s""']*)(?!#|[Mm]ailto|[lL]ocation.|[jJ]avascript|.*css|.*this\.)" +
				@"(.*?)(?:[\s>""'])";

			Regex r = new Regex(sPattern,RegexOptions.IgnoreCase);
			Match m;
			string link;
			for (m = r.Match(text); m.Success; m = m.NextMatch()) 
			{
				if(m.Groups.ToString().Length > 0 )
				{
					link = 	m.Groups[1].ToString();	
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
		public static Uri ParseUri(string uri)
		{
			if(uri == null)
				return null;

			try
			{
				return new Uri(uri);
			}
			catch(UriFormatException)
			{
				return null;
			}
		}
	}
}
