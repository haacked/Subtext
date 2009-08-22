using System;
using System.Web;
using System.Web.UI.WebControls;
using Subtext.Framework;
using Subtext.Framework.Providers;

namespace Subtext.Web.UI.Controls
{
    public partial class AggRecentImages : BaseControl
    {
        protected Repeater recentImagesRepeater;

        private string _fullUrl = HttpContext.Current.Request.Url.Scheme + "://{0}{1}{2}/";

        /// <summary>
        /// Prroperty to limit the number of images displayed. Default is 35.
        /// </summary>
        public int Count
        {
            get;
            set;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            int? groupId = GetGroupIdFromQueryString();

            var images = ObjectProvider.Instance().GetImages(HostInfo.Instance.AggregateBlog.Host, groupId, Count);
            recentImagesRepeater.DataSource = images;
            recentImagesRepeater.DataBind();
        }

        protected Subtext.Framework.Components.Image GetImage(object dataItem)
        {
            return dataItem as Subtext.Framework.Components.Image;
        }

        private int? GetGroupIdFromQueryString()
        {
            int? groupId = null;
            string groupIdText = Request.QueryString["GroupID"];
            if (groupIdText != null)
            {
                int parsedGroupId;
                if (int.TryParse(groupIdText, out parsedGroupId))
                {
                    groupId = parsedGroupId;
                }
            }
            return groupId;
        }

        protected string ImageUrl(object image)
        {
            return Url.ImageUrl(GetImage(image));
        }

        protected string GalleryImageUrl(object image)
        {
            return Url.GalleryImageUrl(GetImage(image));
        }

        protected string GalleryUrl(object image)
        {
            return Url.GalleryUrl(GetImage(image).CategoryID);
        }
    }
}
