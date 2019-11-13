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
using System.Security;
using Jayrock.JsonRpc;
using Jayrock.JsonRpc.Web;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Providers;

namespace Subtext.Web.Admin.Services.Ajax
{
    //NOTE: This uses Jayrock for Ajax services. Please see http://jayrock.berlios.de/ for more info   
    public class AjaxServices : JsonRpcHandler
    {
        public AjaxServices(ObjectRepository repository)
        {
            Repository = repository;
        }

        public ObjectRepository Repository
        {
            get;
            private set;
        }

        public override void ProcessRequest()
        {
            if (!User.IsInRole("Admins"))
            {
                throw new SecurityException();
            }
            base.ProcessRequest();
        }

        [JsonRpcMethod("addMetaTagForBlog")]
        public MetaTag AddMetaTagForBlog(string content, string name, string httpEquiv)
        {
            var newTag = new MetaTag(content) { Name = name, HttpEquiv = httpEquiv, BlogId = Config.CurrentBlog.Id, DateCreatedUtc = DateTime.UtcNow };

            Repository.Create(newTag);

            return newTag;
        }

        [JsonRpcMethod("updateMetaTag")]
        public MetaTag UpdateMetaTag(MetaTag updatedTag)
        {
            updatedTag.BlogId = Config.CurrentBlog.Id;
            Repository.Update(updatedTag);

            return updatedTag;
        }

        [JsonRpcMethod("deleteMetaTag")]
        public bool DeleteMetaTag(int id)
        {
            return Repository.DeleteMetaTag(id);
        }

        [JsonRpcMethod("detectMimeType")]
        public string DetectMimeType(string url)
        {
            return MimeTypesMapper.Mappings.ParseUrl(url);
        }

        [JsonRpcMethod("getTimeZoneInfo")]
        public ServerTimeZoneInfo GetTimeZoneInfo(string timeZoneText)
        {
            return new ServerTimeZoneInfo(timeZoneText);
        }
    }
}