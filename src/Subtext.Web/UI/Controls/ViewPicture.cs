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
using System.Drawing;
using System.Web.UI.WebControls;
using Subtext.Framework.Util;
using Image = Subtext.Framework.Components.Image;

namespace Subtext.Web.UI.Controls
{
    /// <summary>
    ///		Summary description for GalleryThumbNailViewer.
    /// </summary>
    public partial class ViewPicture : BaseControl
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (Context != null)
            {
                int? imageId = SubtextContext.RequestContext.GetIdFromRequest();
                Image image = Repository.GetImage(imageId.Value, true /* activeOnly */);
                image.Blog = Blog;

                Title.Text = image.Title;
                GalleryImage.ImageUrl = Url.GalleryImageUrl(image, image.ResizedFile);
                GalleryImage.Height = image.Height;
                GalleryImage.Width = image.Width;
                GalleryImage.AlternateText = image.Title;
                GalleryImage.BorderColor = Color.Black;
                GalleryImage.BorderWidth = 2;

                OriginalImage.NavigateUrl = Url.GalleryImageUrl(image, image.OriginalFile);
                ReturnUrl.NavigateUrl = Url.GalleryUrl(image.CategoryID);
            }
        }

        protected void ImageCreated(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var image = (Image)e.Item.DataItem;
                if (image != null)
                {
                    var thumbNailImage = (HyperLink)e.Item.FindControl("ThumbNailImage");
                    if (thumbNailImage != null)
                    {
                        thumbNailImage.ImageUrl = Url.GalleryImageUrl(image, image.ThumbNailFile);
                        thumbNailImage.NavigateUrl = Url.GalleryImagePageUrl(image);
                        thumbNailImage.ToolTip = image.Title;
                    }
                }
            }
        }
    }
}