using System;
using System.Globalization;
using System.Web.UI.WebControls;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Format;
using Image=Subtext.Framework.Components.Image;

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
    /// <summary>
	///		Summary description for GalleryThumbNailViewer.
	/// </summary>
	public class GalleryThumbNailViewer : BaseControl
	{
		protected Literal GalleryTitle;
		protected DataList ThumbNails;
		protected Literal Description;

		private string baseImagePath;

		protected string BaseImagePath
		{
			get 
			{
				return baseImagePath;
			}
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);

			if(Context != null)
			{
				int catID = UrlFormats.GetPostIDFromUrl(Request.Path);
				baseImagePath = Images.GalleryVirtualUrl(catID);

				ImageCollection ic = Images.GetImagesByCategoryID(catID, true);
				if(ic != null)
				{
					GalleryTitle.Text = ic.Category.Title;
					if(ic.Category.HasDescription)
					{
						Description.Text = string.Format(CultureInfo.InvariantCulture, "{0}",ic.Category.Description);
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
				Image image = (Image)e.Item.DataItem;
				if(image != null)
				{
					HyperLink ThumbNailImage = (HyperLink)e.Item.FindControl("ThumbNailImage");
					if(ThumbNailImage != null)
					{
						ThumbNailImage.ImageUrl = BaseImagePath + image.ThumbNailFile;
						ThumbNailImage.NavigateUrl = Config.CurrentBlog.UrlFormats.ImageUrl(null,image.ImageID);
						ThumbNailImage.ToolTip = image.Title;
					}
				}
			}
		}

		protected static Image EvalImage(object dataItem)
		{
			return (Image) dataItem;
		}
	}
}

