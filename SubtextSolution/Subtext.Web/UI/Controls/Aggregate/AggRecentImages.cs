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
using System.Web;
using System.Web.UI.WebControls;
using Subtext.Framework;
using Subtext.Framework.Providers;

namespace Subtext.Web.UI.Controls
{
    public partial class AggRecentImages : AggregateUserControl
    {
        protected Repeater recentImagesRepeater;
        
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

            var images = Repository.GetImages(HostInfo.Instance.AggregateBlog.Host, groupId, Count);
            recentImagesRepeater.DataSource = images;
            recentImagesRepeater.DataBind();
        }

        protected Subtext.Framework.Components.Image GetImage(object dataItem)
        {
            return dataItem as Subtext.Framework.Components.Image;
        }

        protected string ImageUrl(object image)
        {
            return Url.GalleryImageUrl(GetImage(image));
        }

        protected string GalleryImageUrl(object image)
        {
            return Url.GalleryImagePageUrl(GetImage(image));
        }

        protected string GalleryUrl(object image)
        {
            return Url.GalleryUrl(GetImage(image));
        }
    }
}
