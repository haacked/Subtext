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
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Subtext.Framework;
using Image = Subtext.Framework.Components.Image;

namespace Subtext.Web.UI.Controls
{
    public class AggRecentImages : AggregateUserControl
    {
        protected Repeater recentImagesRepeater;

        /// <summary>
        /// Prroperty to limit the number of images displayed. Default is 35.
        /// </summary>
        public int Count { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            int? groupId = GetGroupIdFromQueryString();

            ICollection<Image> images = Repository.GetImages(HostInfo.AggregateBlog.Host, groupId, Count);
            recentImagesRepeater.DataSource = images;
            recentImagesRepeater.DataBind();
        }

        protected Image GetImage(object dataItem)
        {
            return dataItem as Image;
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