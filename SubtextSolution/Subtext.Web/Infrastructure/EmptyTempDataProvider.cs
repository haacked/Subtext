using System.Collections.Generic;
using System.Web.Mvc;

namespace Subtext.Web.Infrastructure
{
    public class EmptyTempDataProvider : ITempDataProvider
    {
        static readonly Dictionary<string, object> tempData = new Dictionary<string, object>();

        public IDictionary<string, object> LoadTempData(ControllerContext controllerContext)
        {
            
            return tempData;    
        }

        public void SaveTempData(ControllerContext controllerContext, IDictionary<string, object> values)
        {
            
        }
    }
}
