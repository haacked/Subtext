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

using System;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using Subtext.Framework.Properties;

namespace Subtext.Framework.ModelBinders
{
    public class XmlModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            HttpRequestBase request = controllerContext.HttpContext.Request;
            if(request.ContentType != "text/xml")
            {
                throw new InvalidOperationException(Resources.InvalidOperation_ContentTypeMustBeXml);
            }

            var doc = new XmlDocument();
            doc.Load(controllerContext.HttpContext.Request.InputStream);
            return doc;
        }
    }
}