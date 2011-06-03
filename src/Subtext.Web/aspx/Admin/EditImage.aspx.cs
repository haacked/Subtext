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
using System.Globalization;
using System.IO;
using System.Web.UI.WebControls;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Web;
using Subtext.Web.Properties;
using Image = Subtext.Framework.Components.Image;

namespace Subtext.Web.Admin.Pages
{
    public partial class EditImage : AdminPage
    {
        protected const string VskeyImageid = "ImageId";
        protected string GalleryTitle;
        protected Image _image;
        protected int _imageID;

        public EditImage()
        {
            TabSectionId = "Galleries";
        }

        private int ImageId
        {
            get
            {
                if (ViewState[VskeyImageid] == null || NullValue.NullInt32 == (int)ViewState[VskeyImageid])
                {
                    if (null != Request.QueryString[Keys.QRYSTR_IMAGEID])
                    {
                        ViewState[VskeyImageid] = Convert.ToInt32(Request.QueryString[Keys.QRYSTR_IMAGEID]);
                    }
                }
                return (int)ViewState[VskeyImageid];
            }
        }

        public Image Image
        {
            get
            {
                if (_image == null)
                {
                    _image = Repository.GetImage(ImageId, false /* activeOnly */);
                }

                if (_image == null)
                {
                    throw new InvalidOperationException(Resources.InvalidOperation_ImageUndefined);
                }

                return _image;
            }
        }

        public override void DataBind()
        {
            BindImage();
            base.DataBind();
        }

        private void BindImage()
        {
            ICollection<LinkCategory> selectionList = Repository.GetCategories(CategoryType.ImageCollection,
                                                                          ActiveFilter.None);
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

                ListItem listItem =
                    ddlGalleries.Items.FindByValue(_image.CategoryID.ToString(CultureInfo.InvariantCulture));
                if (listItem != null)
                {
                    ddlGalleries.SelectedIndex = ddlGalleries.Items.IndexOf(listItem);
                }
                // HACK: we're disabling this until we do something with/around the provider
                // that will let us actually move the files too.
                ddlGalleries.Enabled = false;

                if (AdminMasterPage != null)
                {
                    string title = string.Format(CultureInfo.InvariantCulture, Resources.EditGalleries_EditImage,
                                                 Image.Title);
                    AdminMasterPage.Title = title;
                }
            }
        }

        protected void SetGalleryInfo(Image image)
        {
            GalleryTitle = SubtextContext.Repository.GetLinkCategory(image.CategoryID, false).Title;
        }

        protected string EvalImageUrl(object imageObject)
        {
            if (imageObject is Image)
            {
                var image = (Image)imageObject;
                image.Blog = Blog;
                return Url.GalleryImageUrl(image);
            }
            return String.Empty;
        }

        protected string EvalImageNavigateUrl(object imageObject)
        {
            if (imageObject is Image)
            {
                var image = (Image)imageObject;
                return Url.GalleryImagePageUrl(image);
            }
            return String.Empty;
        }

        protected string GetImageGalleryUrl()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}?{1}={2}", Constants.URL_EDITGALLERIES,
                                 Keys.QRYSTR_CATEGORYID, Image.CategoryID);
        }

        private void UpdateImage()
        {
            if (Page.IsValid)
            {
                _image = Repository.GetImage(ImageId, false /* activeOnly */);
                _image.CategoryID = Convert.ToInt32(ddlGalleries.SelectedItem.Value);
                _image.Title = txbTitle.Text;
                _image.IsActive = ckbPublished.Checked;

                try
                {
                    Repository.Update(_image);

                    // would need to also move files for this to work here. should happen
                    // in the provider though.

                    Messages.ShowMessage(Resources.EditGalleries_ImageUpdated);
                    BindImage();
                }
                catch (Exception ex)
                {
                    Messages.ShowError(String.Format(Constants.RES_EXCEPTION, "TODO...", ex.Message));
                }
            }
        }

        private void ReplaceImage()
        {
            if (Page.IsValid)
            {
                _image = Repository.GetImage(ImageId, false /* activeOnly */);
                _image.CategoryID = Convert.ToInt32(ddlGalleries.SelectedItem.Value);
                _image.Title = txbTitle.Text;
                _image.IsActive = ckbPublished.Checked;

                try
                {
                    _image.FileName = Path.GetFileName(ImageFile.PostedFile.FileName);
                    _image.Url = Url.ImageGalleryDirectoryUrl(Blog, _image.CategoryID);
                    _image.LocalDirectoryPath = Url.GalleryDirectoryPath(Blog, _image.CategoryID);
                    Repository.Update(_image, ImageFile.PostedFile.GetFileStream());

                    Messages.ShowMessage(Resources.EditGalleries_ImageUpdated);
                    BindImage();
                }
                catch (Exception ex)
                {
                    Messages.ShowError(String.Format(Constants.RES_EXCEPTION, "TODO...", ex.Message));
                }
            }
        }

        override protected void OnInit(EventArgs e)
        {
            ViewState[VskeyImageid] = NullValue.NullInt32;
        }

        protected void lbkReplaceImage_Click(object sender, EventArgs e)
        {
            ReplaceImage();
        }

        protected void lkbUpdateImage_Click(object sender, EventArgs e)
        {
            UpdateImage();
        }
    }
}