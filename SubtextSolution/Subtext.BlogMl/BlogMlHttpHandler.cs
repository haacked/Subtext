using System;
using System.IO;
using System.Text;
using System.Web;
using System.Xml;
using Subtext.BlogML.Interfaces;
using Subtext.Extensibility.Web;
using Subtext.BlogML.Properties;

namespace Subtext.BlogML
{
	public class BlogMLHttpHandler : BaseHttpHandler
	{
		/// <summary>
		/// Http handler used to export BlogML.
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
				throw new ArgumentNullException("context", Resources.ArgumentNull_Generic);
			
			context.Response.AddHeader("content-disposition", "attachment; filename=BlogMLExport.xml");

			context.Response.Clear();
			WriteBlogML(context.Response.OutputStream);
			context.Response.End();
		}

		private static void WriteBlogML(Stream outStream)
		{
			if (outStream == null)
				throw new ArgumentNullException("outStream", Resources.ArgumentNull_Stream);
			
			IBlogMLProvider provider = BlogMLProvider.Instance();
			
			if(provider.GetBlogMLContext() == null)
				throw new InvalidOperationException(Resources.InvalidOperation_BlogMLNullContext);

			BlogMLWriter writer = BlogMLWriter.Create(provider);

			using(XmlTextWriter xmlWriter = new XmlTextWriter(outStream, Encoding.UTF8))
			{
                xmlWriter.Formatting = Formatting.Indented;
				writer.Write(xmlWriter);
				xmlWriter.Flush();
			}
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
			get
			{
				return false;
			}
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
