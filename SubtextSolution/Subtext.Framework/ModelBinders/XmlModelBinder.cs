using System;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using Subtext.Framework.Properties;

namespace Subtext.Framework.ModelBinders
{
    public class XmlModelBinder : IModelBinder
    {
        #region IModelBinder Members

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

        #endregion
    }
}