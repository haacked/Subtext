using System;
using System.Web.UI.WebControls;
using Subtext.Framework;
using Subtext.Framework.Components;
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
	public class GalleryThumbNailViewer : Subtext.Web.UI.Controls.BaseControl
	{
		protected System.Web.UI.WebControls.Literal GalleryTitle;
		protected System.Web.UI.WebControls.DataList ThumbNails;
		protected System.Web.UI.WebControls.Literal Description;

		private string _baseImagePath = null;

		protected string BaseImagePath
		{
			get 
			{
				return _baseImagePath;
			}
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);
			// Put user code to initialize the page here
			if(Context != null)
			{
				int catID = UrlFormats.GetPostIDFromUrl(Request.Path);
				_baseImagePath = Images.HttpGalleryFilePath(Context,catID);

				ImageCollection ic = Images.GetImagesByCategoryID(catID,true);
				if(ic != null)
				{
					GalleryTitle.Text = ic.Category.Title;
					if(ic.Category.HasDescription)
					{
						Description.Text = string.Format(System.Globalization.CultureInfo.InvariantCulture, "<p>{0}</p>",ic.Category.Description);
					}
					ThumbNails.DataSource = ic;
					ThumbNails.DataBind();
				}
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
						
						ThumbNailImage.ImageUrl = _baseImagePath + _image.ThumbNailFile;
						ThumbNailImage.NavigateUrl = Subtext.Framework.Configuration.Config.CurrentBlog.UrlFormats.ImageUrl(null,_image.ImageID);
						ThumbNailImage.ToolTip = _image.Title;

					}
				}
			}
		}
	}
}

