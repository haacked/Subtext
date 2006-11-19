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

using System;
using System.Collections.Generic;
using System.Globalization;
using Subtext.Framework;
using Subtext.Framework.Components;

namespace Subtext.Web.Admin.Pages
{
	public partial class EditImage : AdminPage
	{
		protected const string VSKEY_IMAGEID = "ImageID";
		protected int _imageID;
		protected Subtext.Framework.Components.Image _image;
		protected string _galleryTitle;

		#region Accessors
		private int ImageId
		{
			get
			{
                if (ViewState[VSKEY_IMAGEID] == null || NullValue.NullInt32 == (int)ViewState[VSKEY_IMAGEID])
                {
                    if (null != Request.QueryString[Keys.QRYSTR_IMAGEID])
                        ViewState[VSKEY_IMAGEID] = Convert.ToInt32(Request.QueryString[Keys.QRYSTR_IMAGEID]);
                }
			    return (int)ViewState[VSKEY_IMAGEID];
			}
		}

		public Subtext.Framework.Components.Image Image
		{
			get 
			{
                if (null == _image)
                {
                    _image = Images.GetSingleImage(this.ImageId, false);
                }

			    if(_image == null)
                    throw new Exception("Image not defined.");
			    
			    return _image;
			}
		}

		#endregion
	    
	    public EditImage() : base()
	    {
            this.TabSectionId = "Galleries";
	    }

		protected void Page_Load(object sender, System.EventArgs e)
		{
		}
	    
	    public override void DataBind()
	    {
            BindImage();
	    }
	    

		private void BindImage()
		{
			if (NullValue.NullInt32 != ImageId)
			{
                ICollection<LinkCategory> selectionList = Links.GetCategories(CategoryType.ImageCollection, ActiveFilter.None);
				if (selectionList.Count > 0)
				{
					ddlGalleries.DataSource = selectionList;
					ddlGalleries.DataValueField = "Id";
					ddlGalleries.DataTextField = "Title";
					
					lnkThumbnail.ImageUrl = EvalImageUrl(Image);
					lnkThumbnail.NavigateUrl = EvalImageNavigateUrl(Image);
					lnkThumbnail.Visible = true;

					ckbPublished.Checked = Image.IsActive;

					SetGalleryInfo(Image);

					ddlGalleries.DataBind();

					ddlGalleries.Items.FindByValue(_image.CategoryID.ToString(CultureInfo.InvariantCulture)).Selected = true;
					// HACK: we're disabling this until we do something with/around the provider
					// that will let us actually move the files too.
					ddlGalleries.Enabled = false;

					Advanced.Collapsed = Preferences.AlwaysExpandAdvanced;

					if(AdminMasterPage != null && AdminMasterPage.BreadCrumb != null)
					{	
						string title = string.Format(System.Globalization.CultureInfo.InvariantCulture, "Editing Image \"{0}\"", Image.Title);

						AdminMasterPage.BreadCrumb.AddLastItem(title);
						AdminMasterPage.Title = title;
					}
				}
				else
				{
					ImageDetails.Visible = false;
					this.Messages.ShowError("You must have at least one valid Gallery before working with individual images.");
				}
			}
			else
			{	
				ImageDetails.Visible = false;
				this.Messages.ShowError("An image identifier was not available, could not load your image.");
			}
		}

		protected void SetGalleryInfo(Subtext.Framework.Components.Image image)
		{
			_galleryTitle = Links.GetLinkCategory(image.CategoryID,false).Title;
		}

		protected string EvalImageUrl(object imageObject)
		{
			if (imageObject is Subtext.Framework.Components.Image)
			{
				Subtext.Framework.Components.Image image = (Subtext.Framework.Components.Image)imageObject;
				return string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}{1}", Images.HttpGalleryFilePath(Context, image.CategoryID), 
					image.ThumbNailFile);
			}
			else
				return String.Empty;
		}

		protected static string EvalImageNavigateUrl(object imageObject)
		{
			if (imageObject is Subtext.Framework.Components.Image)
			{
				Subtext.Framework.Components.Image image = (Subtext.Framework.Components.Image)imageObject;
				return Subtext.Framework.Configuration.Config.CurrentBlog.UrlFormats.ImageUrl(null,image.ImageID);
			}
			else
			{
				return String.Empty;
			}
		}

		protected string GetImageGalleryUrl()
		{
			return string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}?{1}={2}", Constants.URL_EDITGALLERIES, 
				Keys.QRYSTR_CATEGORYID, Image.CategoryID);
		}

		private void UpdateImage()
		{
			if (Page.IsValid)
			{
				_image = Images.GetSingleImage(this.ImageId, false);
				_image.CategoryID = Convert.ToInt32(ddlGalleries.SelectedItem.Value);
				_image.Title = txbTitle.Text;
				_image.IsActive = ckbPublished.Checked;
				
				try
				{
					Images.UpdateImage(_image);

					// would need to also move files for this to work here. should happen
					// in the provider though.

					this.Messages.ShowMessage("The image was successfully updated.");
					BindImage();
				}
				catch(Exception ex)
				{
					this.Messages.ShowError(String.Format(Constants.RES_EXCEPTION, "TODO...", ex.Message));
				}
			}
		}

		private void ReplaceImage()
		{
			if (Page.IsValid)
			{
				_image = Images.GetSingleImage(this.ImageId, false);
				_image.CategoryID = Convert.ToInt32(ddlGalleries.SelectedItem.Value);
				_image.Title = txbTitle.Text;
				_image.IsActive = ckbPublished.Checked;
				
				try
				{
					_image.File = Images.GetFileName(ImageFile.PostedFile.FileName);
					_image.LocalFilePath = Images.LocalGalleryFilePath(Context, _image.CategoryID);
					Images.Update(_image, Images.GetFileStream(ImageFile.PostedFile));				

					this.Messages.ShowMessage("The image was successfully updated.");
					BindImage();
				}
				catch (Exception ex)
				{
					this.Messages.ShowError(String.Format(Constants.RES_EXCEPTION, "TODO...", ex.Message));
				}
			}
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);

			ViewState[VSKEY_IMAGEID] = NullValue.NullInt32;
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion

		protected void lbkReplaceImage_Click(object sender, System.EventArgs e)
		{
			ReplaceImage();
		}

		protected void lkbUpdateImage_Click(object sender, System.EventArgs e)
		{
			UpdateImage();		
		}
	}
}

