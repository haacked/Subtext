#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.IO;
using System.Web;
using System.Xml;
using Subtext.BlogML.Interfaces;
using Subtext.BlogML.Properties;
using Subtext.Extensibility.Web;

namespace Subtext.BlogML
{
    public abstract class BlogMLHttpHandler : BaseHttpHandler
    {
        public abstract IBlogMLProvider GetBlogMLProvider();

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
            WriteBlogML(context.Response.Output);
            context.Response.End();
        }

        private void WriteBlogML(TextWriter outputWriter)
        {
            IBlogMLProvider provider = GetBlogMLProvider();

            if (provider.GetBlogMLContext() == null)
            {
                throw new InvalidOperationException(Resources.InvalidOperation_BlogMLNullContext);
            }

            BlogMLWriter writer = BlogMLWriter.Create(provider);

            using (XmlTextWriter xmlWriter = new XmlTextWriter(outputWriter))
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
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the content MIME type.
        /// </summary>
        /// <value></value>
        protected override string ContentMimeType
        {
            get
            {
                return "text/xml";
            }
        }
    }
}
