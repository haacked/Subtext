using System;
using System.IO;
using System.Text;
using System.Web;
using System.Xml;
using Subtext.Framework.Configuration;
using Subtext.Framework.Import;

namespace Subtext.Web.Admin.Handlers
{
	/// <summary>
	/// Summary description for BlogMLExport.
	/// </summary>
	public class BlogMLExport : IHttpHandler 
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="BlogMLExport"/> class.
		/// </summary>
		public BlogMLExport()
		{
		}

		/// <summary>
		/// Implements the <see cref="T:System.Web.IHttpHandler"/> interface 
		/// and generates a BlogML export of the current blog's contents.
		/// </summary>
		/// <param name="context">An <see cref="T:System.Web.HttpContext"/> object that provides references to the intrinsic server objects (for example, <see langword="Request"/>, <see langword="Response"/>, <see langword="Session"/>, and <see langword="Server"/>)<see langword=""/> used to service HTTP requests.</param>
		public void ProcessRequest(HttpContext context)
		{
			context.Response.AddHeader("content-disposition", "attachment; filename=BlogMLExport.xml");
			
			string embedValue = context.Request.QueryString["embed"];
			bool embedAttachments = false;
			try
			{
				embedAttachments = embedValue != null && embedValue.Length > 0 && bool.Parse(embedValue);
			}
			catch(System.FormatException)
			{
				//Ignore.
			}
			context.Response.Clear();
			WriteBlogML(context.Response.OutputStream, embedAttachments);
			context.Response.End();
		}

		private void WriteBlogML(Stream outStream, bool embedAttachments)
		{
			string connStr = Config.Settings.ConnectionString;
			SubtextBlogMLWriter blogWriter = new SubtextBlogMLWriter(connStr, Config.CurrentBlog.BlogID, false);
			blogWriter.EmbedAttachments = embedAttachments;
			
			using(StreamWriter strWriter = new StreamWriter(outStream, Encoding.UTF8))
			{
				XmlTextWriter xWriter = null;
				try
				{
					xWriter = new XmlTextWriter(strWriter);
					xWriter.Formatting = Formatting.Indented;
					blogWriter.Write(xWriter);
				}
				finally
				{
					if(xWriter != null)
					{
						xWriter.Close();
					}
				}
			}
		}

		/// <summary>
		/// Gets a value indicating whether another request can use
		/// the <see cref="T:System.Web.IHttpHandler"/>
		/// instance.
		/// </summary>
		/// <value></value>
		public bool IsReusable
		{
			get { return true; }
		}
	}
}
