using System;
using System.Security.Permissions;
using Jayrock.JsonRpc;
using Jayrock.JsonRpc.Web;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;

namespace Subtext.Web.Admin.Services.Ajax
{
    //NOTE: we are now using Jayrock for Ajax services. Please see http://jayrock.berlios.de/ for more info
    [PrincipalPermission(SecurityAction.Demand, Role = "Admins")]
    public class AjaxServices : JsonRpcHandler
    {
        [JsonRpcMethod("addMetaTagForBlog")]
        public MetaTag AddMetaTagForBlog(string content, string name, string httpEquiv)
        {
            MetaTag newTag = new MetaTag(content);
            newTag.Name = name;
            newTag.HttpEquiv = httpEquiv;
            newTag.BlogId = Config.CurrentBlog.Id;
            newTag.DateCreated = DateTime.Now;

            MetaTags.Create(newTag);

            return newTag;
        }

        [JsonRpcMethod("updateMetaTag")]
        public MetaTag UpdateMetaTag(MetaTag updatedTag)
        {
            updatedTag.BlogId = Config.CurrentBlog.Id;
            MetaTags.Update(updatedTag);

            return updatedTag;
        }

        [JsonRpcMethod("deleteMetaTag")]
        public bool DeleteMetaTag(int id)
        {
            return MetaTags.Delete(id);
        }

        [JsonRpcMethod("detectMimeType")]
        public string DetectMimeType(string url)
        {
            return MimeTypesMapper.Mappings.ParseUrl(url);
        }
    }
}