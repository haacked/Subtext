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
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
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
	public sealed class HtmlHelper
	{
		private HtmlHelper()
		{
		}

		/// <summary>
		/// Strips HTML tags from the specified text.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <returns></returns>
		public static string RemoveHtml(string text)
		{
			HtmlTagRegex regex = new HtmlTagRegex();
			return regex.Replace(text, string.Empty);
		}

		/// <summary>
		/// Converts the entry body into XHTML compliant text. 
		/// Returns false if it encounters a problem in doing so.
		/// </summary>
		/// <param name="entry">Entry.</param>
		/// <returns></returns>
		public static bool ConvertHtmlToXHtml(ref Entry entry)
		{
			try
			{
				SgmlReader reader = new SgmlReader();
				reader.SetBaseUri(Config.CurrentBlog.RootUrl);
				reader.DocType = "HTML";
				reader.InputStream = new StringReader("<bloghelper>" + entry.Body + "</bloghelper>");
				StringWriter writer = new StringWriter();
				XmlTextWriter xmlWriter = new XmlTextWriter(writer);
				while (reader.Read()) 
				{
					xmlWriter.WriteNode(reader, true);
				}
				xmlWriter.Close(); 

				string xml = writer.ToString();
				entry.Body = xml.Substring(12, xml.Length - 25);
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
			if (!Config.Settings.UseXHTML)
			{
				brTag = "<br />";
			}

			return stringToTransform.Replace("\n", brTag);
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
			NameValueCollection AllowedHtml = null;
			AllowedHtml = ((NameValueCollection)(ConfigurationSettings.GetConfig("AllowableCommentHtml")));

			if (AllowedHtml == null || AllowedHtml.Count == 0)
			{
				//This indicates that the AllowableCommentHtml configuration is either missing or
                //has no values, therefore just strip the text as normal.
				return HtmlSafe(text);
			}
			else
			{
				//this regex matches any tag. Tags with < or > inside of quotes will cause this
				//to fail. Nothing that should be left in comments should have this anyway.
				Regex RegX = new Regex("(<.+?>)", RegexOptions.Singleline | RegexOptions.IgnoreCase);
				string[] splits;
				//now we split at each match, then examine each resulting string to determine
				//if allowed HTML code is matched
				splits = RegX.Split(text);
			
				//build stupidly complex regex
				System.Text.StringBuilder sb = new System.Text.StringBuilder();
				sb.Append("<\\s*?\\/??\\s*((?:)");
				for (int i = 0; i <= AllowedHtml.Count - 1; i++)
				{
					sb.Append(AllowedHtml.GetKey(i));
					if (i < AllowedHtml.Count - 1)
					{
						sb.Append("|");
					}
				}
				sb.Append(")"); //\s*")
				string pattern = sb.ToString();
			
				sb = new System.Text.StringBuilder();
			
				foreach (string s in splits)
				{
					//check each match against the list of allowable tags.
					if (Regex.IsMatch(s, pattern, RegexOptions.Singleline | RegexOptions.IgnoreCase))
					{
						//this is a tag that we allow
						//check if it is the opening tag or close
						if (Regex.IsMatch(s, "<\\s*?/", RegexOptions.Singleline | RegexOptions.IgnoreCase))
						{
							//this is the closing tag
							//determine the tag type and return only the correctly formated close tag
							sb.Append("</" + Regex.Match(s, "(\\w+)").Value + ">");
						}
						else
						{
							//this is the opening tag
							//create the opening portion
							sb.Append("<" + Regex.Match(s, "(\\w+)").Value);
							//now determine which attributes (if any) to add
							sb.Append(FilterAttributes(Regex.Match(s, "(\\w+)").Value, Regex.Matches(s, "(\\w+(\\s*=\\s*)((?:)\".*?\"|[^\"]\\S+))", RegexOptions.Singleline), ref AllowedHtml) + ">");
						}
						//sb.Append("Match found at " & s & vbCrLf)
					}
					else
					{
						sb.Append(HtmlSafe(s));
					}
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
			text = text.Replace("\r\n", "<br />");
			text = text.Replace("\n", "<br />");
			return text;
		}

		/// <summary>
		/// Removes any non-permitted attributes for the given tagName. The permitted attributes 
		/// are determined by the given allowedHtml collection.
		/// </summary>
		/// <param name="tagName"></param>
		/// <param name="attrMatches"></param>
		/// <param name="allowedHtml"></param>
		/// <returns></returns>
		/// <remarks>This will be a high volume method, so make it as efficient as possible</remarks>
		private static string FilterAttributes(string tagName, MatchCollection attrMatches, ref NameValueCollection allowedHtml)
		{
			string allowedAttrsStr = allowedHtml[tagName];
			if (allowedAttrsStr != null && allowedAttrsStr.Length>0)
			{
				System.Text.StringBuilder attrSB = new System.Text.StringBuilder();
				string attrKey,attrValue;
				//look to see which tag's attributes we are matching
				tagName = tagName.ToLower();
				char[] splitter  = {','};
				char[] eqSplitter = {'='};
			
				string[] allowedAttrs = allowedHtml[tagName].Split(splitter);
				// go thru each matched attribute, and determine if it's allowed
				foreach (Match attrMatch in attrMatches)
				{
					// get the actual markup attribute (the key) from attrMatch which is a key=value pair.
					attrKey = Regex.Match(attrMatch.Value, "\\w+").Value.ToLower().Trim();

					foreach (string allowedAttr in allowedAttrs)
					{
						if (allowedAttr.Equals(attrKey))
						{
							// found an allowed attribute, so get the attribute value
							attrValue = attrMatch.Value.Split(eqSplitter)[1];

							// and now add the full attribute (key=value) to be returned
							attrSB.Append(" " + attrKey + "=\"" + attrValue.Replace("\"", "") + "\"");
						}
					}
				}
				return attrSB.ToString();
			}
			return string.Empty;
		}
	}
}
