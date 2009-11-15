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

using System.Drawing;
using System.Drawing.Imaging;
using System.Web;
using System.Web.Mvc;
using Docuverse.Identicon;

namespace Subtext.Framework.Services.Identicon
{
    public class IdenticonResult : FileResult
    {
        public IdenticonResult(int code, int size, string etag) : base("image/png")
        {
            Code = code;
            Size = size;
            Etag = etag;
        }

        public string Etag
        {
            get; 
            private set;
        }
        
        public int Code
        {
            get; 
            private set;
        }
        public int Size
        {
            get; 
            private set;
        }

        private static readonly IdenticonRenderer Renderer = new IdenticonRenderer();

        protected override void WriteFile(HttpResponseBase response)
        {
            response.AppendHeader("ETag", Etag);
            using(Bitmap b = Renderer.Render(Code, Size))
            {
                b.Save(response.OutputStream, ImageFormat.Png);
            }
        }
    }
}
