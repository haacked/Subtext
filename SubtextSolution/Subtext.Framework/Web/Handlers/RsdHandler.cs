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
using System.Globalization;
using System.Text;
using System.Web;
using System.Xml;
using Subtext.Extensibility.Web;
using Subtext.Framework.Configuration;
using Subtext.Framework.Properties;

namespace Subtext.Framework.Web.Handlers
{
	/// <summary>
	/// HttpHandler for rendering the Really Simple Syndication (RSD) Format.
	/// </summary>
	/// <remarks>
	/// The specs for RSD can be found here. http://cyber.law.harvard.edu/blogs/gems/tech/rsd.html
	/// </remarks>
	public class RsdHandler : BaseHttpHandler
	{
		/// <summary>
		/// Handles the request.  This is where you put your
		/// business logic.
		/// </summary>
		/// <remarks>
		/// <p>This method should result in a call to one 
		/// (or more) of the following methods:</p>
		/// <p><code>context.Response.BinaryWrite();</code></p>
		/// <p><code>context.Response.Write();</code></p>
		/// <p><code>context.Response.WriteFile();</code></p>
		/// <p>
		/// <code>
		/// someStream.Save(context.Response.OutputStream);
		/// </code>
		/// </p>
		/// <p>etc...</p>
		/// <p>
		/// If you want a download box to show up with a 
		/// pre-populated filename, add this call here 
		/// (supplying a real filename).
		/// </p>
		/// <p>
		/// </p>
		/// <code>Response.AddHeader("Content-Disposition"
		/// , "attachment; filename=\"" + Filename + "\"");</code>
		/// </p>
		/// </remarks>
		/// <param name="context">Context.</param>
		public override void HandleRequest(HttpContext context)
		{
            if (context == null)
            {
                throw new ArgumentNullException("context", Resources.ArgumentNull_Generic);
            }

			if(Config.CurrentBlog == null)
				return;

			context.Response.Charset = "utf-8";
			
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.Indent = true;
			settings.IndentChars = "  ";
			settings.Encoding = Encoding.UTF8;
			XmlWriter writer = XmlWriter.Create(context.Response.OutputStream, settings);
			WriteRsd(writer, Config.CurrentBlog);
		}
		
		/// <summary>
		/// Writes the RSD for the specified blog into the XmlWriter.
		/// </summary>
		/// <param name="writer"></param>
		/// <param name="blog"></param>
		public static void WriteRsd(XmlWriter writer, BlogInfo blog)
		{
            if (writer == null)
            {
                throw new ArgumentNullException("writer", Resources.ArgumentNull_Generic);
            }

            if (blog == null)
            {
                throw new ArgumentNullException("blog", Resources.ArgumentNull_Generic);
            }

            writer.WriteStartDocument();
			writer.WriteStartElement("rsd", "http://archipelago.phrasewise.com/rsd");
			writer.WriteAttributeString("version", "1.0");
			writer.WriteStartElement("service");
			writer.WriteElementString("engineName", "Subtext");
			writer.WriteElementString("engineLink", "http://subtextproject.com/");
			writer.WriteElementString("homePageLink", blog.HomeFullyQualifiedUrl.ToString());
			
			writer.WriteStartElement("apis");
			
			//When we have more than one API, we'll list them here.
			writer.WriteStartElement("api");
			writer.WriteAttributeString("name", "MetaWeblog");
			writer.WriteAttributeString("preferred", "true");
			writer.WriteAttributeString("apiLink", blog.RootUrl + "services/metablogapi.aspx");
			writer.WriteAttributeString("blogID", Config.CurrentBlog.Id.ToString(CultureInfo.InvariantCulture));
			writer.WriteEndElement(); // </api>
			
			writer.WriteEndElement(); // </apis>

			writer.WriteEndElement(); // </service>
			writer.WriteEndElement(); // </rsd>
			writer.WriteEndDocument();
			writer.Flush();
		}

		/// <summary>
		/// Validates the parameters.  Inheriting classes must
		/// implement this and return true if the parameters are
		/// valid, otherwise false.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <returns><c>true</c> if the parameters are valid,
		/// otherwise <c>false</c></returns>
		public override bool ValidateParameters(HttpContext context)
		{
			return true;
		}

		/// <summary>
		/// Gets a value indicating whether this handler
		/// requires users to be authenticated.
		/// </summary>
		/// <value>
		///    <c>true</c> if authentication is required
		///    otherwise, <c>false</c>.
		/// </value>
		public override bool RequiresAuthentication
		{
			get { return false; }
		}

		/// <summary>
		/// Gets the content MIME type.
		/// </summary>
		/// <value></value>
		public override string ContentMimeType
		{
			get
			{
				return "text/xml";
			}
		}
	}
}
