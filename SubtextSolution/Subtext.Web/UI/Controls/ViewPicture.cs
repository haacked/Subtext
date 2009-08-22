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
using System.Drawing;
using System.Web.UI.WebControls;
using Subtext.Framework;
using Subtext.Framework.Routing;

namespace Subtext.Web.UI.Controls
{
	/// <summary>
	///		Summary description for GalleryThumbNailViewer.
	/// </summary>
	public partial class ViewPicture : BaseControl
	{
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);

			if(Context != null)
			{
                int imageId = RouteValues.GetId();
				var image = Repository.GetImage(imageId, true /* activeOnly */);
                image.Blog = Blog;

				Title.Text = image.Title;
				GalleryImage.ImageUrl = Url.ImageUrl(image, image.ResizedFile);
				GalleryImage.Height = image.Height;
				GalleryImage.Width = image.Width;
				GalleryImage.AlternateText = image.Title;
				GalleryImage.BorderColor = Color.Black;
				GalleryImage.BorderWidth = 2;

				OriginalImage.NavigateUrl = Url.ImageUrl(image, image.OriginalFile);
				ReturnUrl.NavigateUrl = Url.GalleryUrl(image.CategoryID);
			}
		}

		protected void ImageCreated(object sender,  DataListItemEventArgs e)
		{
			if(e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				Subtext.Framework.Components.Image image = (Subtext.Framework.Components.Image)e.Item.DataItem;
				if(image != null)
				{
					HyperLink ThumbNailImage = (HyperLink)e.Item.FindControl("ThumbNailImage");
					if(ThumbNailImage != null)
					{
						
						ThumbNailImage.ImageUrl = Url.ImageUrl(image, image.ThumbNailFile);
						ThumbNailImage.NavigateUrl = Url.GalleryImageUrl(image);
						ThumbNailImage.ToolTip = image.Title;

					}
				}
			}
		}
	}
}

