using System;
using System.Web.Mvc;
using System.Xml;
using Subtext.Framework.Properties;

namespace Subtext.Framework.ModelBinders
{
    public class XmlModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var request = controllerContext.HttpContext.Request;
            if (request.ContentType != "text/xml") {
                throw new InvalidOperationException(Resources.InvalidOperation_ContentTypeMustBeXml);
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(controllerContext.HttpContext.Request.InputStream);
            return doc;
        }
    }
}
