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

using System.Web;
using System.Web.Mvc;
using System.Xml;
using Subtext.ImportExport;

namespace Subtext.Infrastructure.ActionResults
{
    public class ExportActionResult : FileResult
    {
        public ExportActionResult(IBlogMLWriter blogMLWriter, string fileName)
            : base("text/xml")
        {
            BlogMLWriter = blogMLWriter;
            FileDownloadName = fileName;
        }

        public IBlogMLWriter BlogMLWriter
        {
            get;
            private set;
        }

        protected override void WriteFile(HttpResponseBase response)
        {
            var writer = new XmlTextWriter(response.Output);
            BlogMLWriter.Write(writer);
        }
    }
}
