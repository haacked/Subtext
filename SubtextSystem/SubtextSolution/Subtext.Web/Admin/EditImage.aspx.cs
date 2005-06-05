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

using System;
using System.Globalization;
using System.Web.UI;
using Subtext.Framework;
using Subtext.Framework.Components;

namespace Subtext.Web.Admin.Pages
{
	public class EditImage : AdminPage
	{
		protected const string VSKEY_IMAGEID = "ImageID";
		protected int _imageID;
		protected Subtext.Framework.Components.Image _image;
		protected string _galleryTitle;

		protected Subtext.Web.Admin.WebUI.Page PageContainer;
		protected Subtext.Web.Admin.WebUI.AdvancedPanel ImageDetails;
		protected Subtext.Web.Admin.WebUI.MessagePanel Messages;
		protected System.Web.UI.WebControls.TextBox txbTitle;
		protected System.Web.UI.WebControls.DropDownList ddlGalleries;
		protected System.Web.UI.WebControls.CheckBox ckbPublished;
		protected System.Web.UI.WebControls.LinkButton lbkAddImage;
		protected Subtext.Web.Admin.WebUI.AdvancedPanel Advanced;
		protected System.Web.UI.HtmlControls.HtmlInputFile ImageFile;
		protected System.Web.UI.WebControls.LinkButton lkbUpdateImage;
		protected System.Web.UI.WebControls.HyperLink lnkThumbnail;

		#region Accessors
		private int ImageID
		{
			get { return (int)ViewState[VSKEY_IMAGEID]; }
			set { ViewState[VSKEY_IMAGEID] = value; }
		}

		public Subtext.Framework.Components.Image Image
		{
			get 
			{
				if (null != _image)
					return _image;
				else
					throw new Exception("Image not defined.");
			}
		}

		#endregion

		private void Page_Load(object sender, System.EventArgs e)
		{
			if (!IsPostBack)
			{
				if (null != Request.QueryString[Keys.QRYSTR_IMAGEID])
					ImageID = Convert.ToInt32(Request.QueryString[Keys.QRYSTR_IMAGEID]);

				BindImage();
			}
		}

		private void BindImage()
		{
			BindImage(this.ImageID);
		}

		private void BindImage(int imageID)
		{
			if (Constants.NULL_IMAGEID != imageID)
			{
				LinkCategoryCollection selectionList = Links.GetCategories(CategoryType.ImageCollection, false);
				if (selectionList.Count > 0)
				{
					ddlGalleries.DataSource = selectionList;
					ddlGalleries.DataValueField = "CategoryID";
					ddlGalleries.DataTextField = "Title";
					
					_image = Images.GetSingleImage(imageID, false);
					lnkThumbnail.ImageUrl = EvalImageUrl(_image);
					lnkThumbnail.NavigateUrl = EvalImageNavigateUrl(_image);
					lnkThumbnail.Visible = true;

					ckbPublished.Checked = _image.IsActive;

					SetGalleryInfo(_image);

					Page.DataBind();

					ddlGalleries.Items.FindByValue(_image.CategoryID.ToString(CultureInfo.InvariantCulture)).Selected = true;
					// HACK: we're disabling this until we do something with/around the provider
					// that will let us actually move the files too.
					ddlGalleries.Enabled = false;

					Advanced.Collapsed = Preferences.AlwaysExpandAdvanced;

					Control container = Page.FindControl("PageContainer");
					if (null != container && container is Subtext.Web.Admin.WebUI.Page)
					{	
						Subtext.Web.Admin.WebUI.Page page = (Subtext.Web.Admin.WebUI.Page)container;
						string title = string.Format(System.Globalization.CultureInfo.InvariantCulture, "Editing Image \"{0}\"", _image.Title);

						page.BreadCrumbs.AddLastItem(title);
						page.Title = title;
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

		protected string EvalImageNavigateUrl(object imageObject)
		{
			if (imageObject is Subtext.Framework.Components.Image)
			{
				Subtext.Framework.Components.Image image = (Subtext.Framework.Components.Image)imageObject;
				return Subtext.Framework.Configuration.Config.CurrentBlog.UrlFormats.ImageUrl(null,image.ImageID);
			}
			else
				return String.Empty;
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
				_image = Images.GetSingleImage(ImageID, false);
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
				_image = Images.GetSingleImage(ImageID, false);
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

		private void ConfirmDeleteImage(int imageID)
		{
			(Page as AdminPage).Command = new DeleteImageCommand(imageID);
			(Page as AdminPage).Command.RedirectUrl = Request.Url.ToString();
			Server.Transfer(Constants.URL_CONFIRM);
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);

			ViewState[VSKEY_IMAGEID] = Constants.NULL_IMAGEID;
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.lkbUpdateImage.Click += new System.EventHandler(this.lkbUpdateImage_Click);
			this.lbkAddImage.Click += new System.EventHandler(this.lbkReplaceImage_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void lbkReplaceImage_Click(object sender, System.EventArgs e)
		{
			ReplaceImage();
		}

		private void lkbUpdateImage_Click(object sender, System.EventArgs e)
		{
			UpdateImage();		
		}
	}
}

