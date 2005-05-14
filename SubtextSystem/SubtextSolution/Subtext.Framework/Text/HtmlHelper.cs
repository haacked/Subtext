using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using Sgml;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;

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
				reader.SetBaseUri(Config.CurrentBlog.FullyQualifiedUrl);
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
			catch(Exception)
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
			if(s == null || s.Trim().Length == 0)
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
		/// We will however, check for line breaks and replace them with <br>
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
		/// them with <br> or <br />
		/// </summary>
		/// <param name="stringToTransform"></param>
		/// <returns></returns>
		public static string SafeFormat(string stringToTransform) 
		{
			stringToTransform = HttpContext.Current.Server.HtmlEncode(stringToTransform);
			string brTag = "<br />";
			if(!Config.Settings.UseXHTML)
			{
				brTag = "<br>";
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
			if(text == null 
				|| text.Trim().Length == 0 
				|| text.Trim().ToLower(CultureInfo.InvariantCulture).StartsWith("http://"))
			{
				return text;
			}
			return "http://" + text;
		}
	}
}
