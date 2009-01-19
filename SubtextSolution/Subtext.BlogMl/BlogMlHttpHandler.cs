using System;
using System.IO;
using System.Text;
using System.Web;
using System.Xml;
using Subtext.BlogML.Interfaces;
using Subtext.Extensibility.Web;

namespace Subtext.BlogML
{
	public abstract class BlogMLHttpHandler : BaseHttpHandler
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
		protected override void HandleRequest(HttpContext context)
		{
			context.Response.AddHeader("content-disposition", "attachment; filename=BlogMLExport.xml");

			context.Response.Clear();
			WriteBlogML(context.Response.OutputStream);
			context.Response.End();
		}

        public abstract IBlogMLProvider GetBlogMlProvider();

		private void WriteBlogML(Stream outStream)
		{
			IBlogMLProvider provider = GetBlogMlProvider();

            if (provider.GetBlogMlContext() == null) {
                throw new InvalidOperationException("The BlogMl provider did not set the context.");
            }

			BlogMLWriter writer = BlogMLWriter.Create(provider);

			using(XmlTextWriter xmlWriter = new XmlTextWriter(outStream, Encoding.UTF8)) {
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
		protected override bool ValidateParameters(HttpContext context)
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
		protected override bool RequiresAuthentication
		{
			get {
				return false;
			}
		}

		/// <summary>
		/// Gets the content MIME type.
		/// </summary>
		/// <value></value>
		protected override string ContentMimeType
		{
			get {
				return "text/xml";
			}
		}
	}
}
