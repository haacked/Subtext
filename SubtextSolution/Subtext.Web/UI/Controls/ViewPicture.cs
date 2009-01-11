using System;
using System.Drawing;
using System.Web.UI.WebControls;
using Subtext.Framework;
using Subtext.Framework.Configuration;
using Subtext.Framework.Format;

#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at SourceForge at http://sourceforge.net/projects/subtext
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

namespace Subtext.Web.UI.Controls
{
	using System;

	/// <summary>
	///		Summary description for GalleryThumbNailViewer.
	/// </summary>
	public class ViewPicture : BaseControl
	{
		protected System.Web.UI.WebControls.Literal Title;
		protected System.Web.UI.WebControls.Image GalleryImage;
		protected System.Web.UI.WebControls.HyperLink ReturnUrl;
		protected System.Web.UI.WebControls.HyperLink OriginalImage;

		private string _baseImagePath;

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);

			if(Context != null)
			{
				int imageID = UrlFormats.GetPostIDFromUrl(Request.Path);
				Subtext.Framework.Components.Image _image = Images.GetSingleImage(imageID,true);
				_baseImagePath = Images.GalleryVirtualUrl(_image.CategoryID);

				Title.Text = _image.Title;
				GalleryImage.ImageUrl = _baseImagePath +  _image.ResizedFile;
				GalleryImage.Height = _image.Height;
				GalleryImage.Width = _image.Width;
				GalleryImage.AlternateText = _image.Title;
				GalleryImage.BorderColor = Color.Black;
				GalleryImage.BorderWidth = 2;

				OriginalImage.NavigateUrl = _baseImagePath + _image.OriginalFile;

				ReturnUrl.NavigateUrl = Url.GalleryUrl(_image.CategoryID);
			}
		}

		protected void ImageCreated(object sender,  DataListItemEventArgs e)
		{
			if(e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				Subtext.Framework.Components.Image _image = (Subtext.Framework.Components.Image)e.Item.DataItem;
				if(_image != null)
				{
					HyperLink ThumbNailImage = (HyperLink)e.Item.FindControl("ThumbNailImage");
					if(ThumbNailImage != null)
					{
						
						ThumbNailImage.ImageUrl = _baseImagePath + "t_" + _image.FileName;
						ThumbNailImage.NavigateUrl = Url.ImageUrl(_image);
						ThumbNailImage.ToolTip = _image.Title;

					}
				}
			}
		}
	}
}

