using System;
using System.Drawing;
using System.Web.UI.WebControls;
using Subtext.Framework;
using Subtext.Framework.Configuration;
using Subtext.Framework.Format;

#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// .Text WebLog
// 
// .Text is an open source weblog system started by Scott Watermasysk. 
// Blog: http://ScottWater.com/blog 
// RSS: http://scottwater.com/blog/rss.aspx
// Email: Dottext@ScottWater.com
//
// For updated news and information please visit http://scottwater.com/dottext and subscribe to 
// the Rss feed @ http://scottwater.com/dottext/rss.aspx
//
// On its release (on or about August 1, 2003) this application is licensed under the BSD. However, I reserve the 
// right to change or modify this at any time. The most recent and up to date license can always be fount at:
// http://ScottWater.com/License.txt
// 
// Please direct all code related questions to:
// GotDotNet Workspace: http://www.gotdotnet.com/Community/Workspaces/workspace.aspx?id=e99fccb3-1a8c-42b5-90ee-348f6b77c407
// Yahoo Group http://groups.yahoo.com/group/DotText/
// 
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

namespace Subtext.Web.UI.Controls
{
	using System;

	/// <summary>
	///		Summary description for GalleryThumbNailViewer.
	/// </summary>
	public class ViewPicture : Subtext.Web.UI.Controls.BaseControl
	{
		protected System.Web.UI.WebControls.Literal Title;
		protected System.Web.UI.WebControls.Image GalleryImage;
		protected System.Web.UI.WebControls.HyperLink ReturnUrl;
		protected System.Web.UI.WebControls.HyperLink OriginalImage;

		private string _baseImagePath = null;

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);

			if(Context != null)
			{
				int imageID = UrlFormats.GetPostIDFromUrl(Request.Path);
				Subtext.Framework.Components.Image _image = Images.GetSingleImage(imageID,true);
				_baseImagePath = Images.HttpGalleryFilePath(Context,_image.CategoryID);

				Title.Text = _image.Title;
				GalleryImage.ImageUrl = _baseImagePath +  _image.ResizedFile;
				GalleryImage.Height = _image.Height;
				GalleryImage.Width = _image.Width;
				GalleryImage.AlternateText = _image.Title;
				GalleryImage.BorderColor = Color.Black;
				GalleryImage.BorderWidth = 2;

				OriginalImage.NavigateUrl = _baseImagePath + _image.OriginalFile;

				ReturnUrl.NavigateUrl = Config.CurrentBlog.UrlFormats.GalleryUrl(null,_image.CategoryID);
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
						
						ThumbNailImage.ImageUrl = _baseImagePath + "t_" + _image.File;
						ThumbNailImage.NavigateUrl = Subtext.Framework.Configuration.Config.CurrentBlog.UrlFormats.ImageUrl(null,_image.ImageID);
						ThumbNailImage.ToolTip = _image.Title;

					}
				}
			}
		}
	}
}

