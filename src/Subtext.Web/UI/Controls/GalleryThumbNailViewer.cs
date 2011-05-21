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
using System.Globalization;
using System.Web;
using System.Web.UI.WebControls;
using Subtext.Framework.Components;
using Subtext.Framework.Util;
using Image = Subtext.Framework.Components.Image;

namespace Subtext.Web.UI.Controls
{
    /// <summary>
    ///		Summary description for GalleryThumbNailViewer.
    /// </summary>
    public partial class GalleryThumbNailViewer : BaseControl
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            // Put user code to initialize the page here
            if (Context != null)
            {
                int? categoryId = SubtextContext.RequestContext.GetIdFromRequest();

                ImageCollection ic = Repository.GetImagesByCategoryId(categoryId.Value, true /* activeOnly */);
                if (ic != null)
                {
                    GalleryTitle.Text = ic.Category.Title;
                    if (ic.Category.HasDescription)
                    {
                        Description.Text = string.Format(CultureInfo.InvariantCulture, "{0}",
                                                         HttpUtility.HtmlEncode(ic.Category.Description));
                    }
                    ThumbNails.DataSource = ic;
                    ThumbNails.DataBind();
                }
            }
        }

        protected void ImageCreated(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var image = (Image)e.Item.DataItem;
                if (image != null)
                {
                    image.Blog = Blog;
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

        protected static Image EvalImage(object dataItem)
        {
            return (Image)dataItem;
        }
    }
}