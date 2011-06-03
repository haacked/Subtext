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
using System.Web.UI;
using System.Web.UI.WebControls;
using Ionic.Zip;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Web;
using Subtext.Web.Admin.Commands;
using Subtext.Web.Properties;
using Image = Subtext.Framework.Components.Image;

namespace Subtext.Web.Admin.Pages
{
    public partial class EditGalleries : AdminPage
    {
        protected bool IsListHidden;
        // jsbright added to support prompting for new file name

        protected EditGalleries()
        {
            TabSectionId = "Galleries";
        }

        private int CategoryId
        {
            get
            {
                if (null != ViewState["CategoryId"])
                {
                    return (int)ViewState["CategoryId"];
                }
                return NullValue.NullInt32;
            }
            set { ViewState["CategoryId"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Config.Settings.AllowImages)
            {
                Response.Redirect(AdminUrl.Home());
            }

            if (!IsPostBack)
            {
                HideImages();
                ShowResults();
                BindList();
                ckbIsActiveImage.Checked = Preferences.AlwaysCreateIsActive;
                ckbNewIsActive.Checked = Preferences.AlwaysCreateIsActive;

                if (null != Request.QueryString[Keys.QRYSTR_CATEGORYID])
                {
                    CategoryId = Convert.ToInt32(Request.QueryString[Keys.QRYSTR_CATEGORYID]);
                    BindGallery(CategoryId);
                }
            }
        }

        private void BindList()
        {
            // TODO: possibly, later on, add paging support a la other cat editors
            ICollection<LinkCategory> selectionList = Repository.GetCategories(CategoryType.ImageCollection,
                                                                          ActiveFilter.None);
            dgrSelectionList.DataSource = selectionList;
            dgrSelectionList.DataKeyField = "Id";
            dgrSelectionList.DataBind();

            dgrSelectionList.Visible = selectionList.Count > 0; //need not be shown when there are no galleries...
        }

        private void BindGallery()
        {
            // HACK: reverse the call order with the overloaded version
            BindGallery(CategoryId);
        }

        private void BindGallery(int galleryId)
        {
            CategoryId = galleryId;
            LinkCategory selectedGallery = SubtextContext.Repository.GetLinkCategory(galleryId, false);
            ICollection<Image> imageList = Repository.GetImagesByCategory(galleryId, false);

            plhImageHeader.Controls.Clear();
            if (selectedGallery != null)
            {
                string galleryTitle = string.Format(CultureInfo.InvariantCulture, "{0} - {1} " + Resources.Label_Images,
                                                    selectedGallery.Title, imageList.Count);
                plhImageHeader.Controls.Add(new LiteralControl(galleryTitle));
            }
            else //invalid gallery
            {
                Messages.ShowError("The gallery does not exist anymore. Please update your bookmarks.");
                return;
            }

            rprImages.DataSource = imageList;
            rprImages.DataBind();

            ShowImages();

            if (AdminMasterPage != null)
            {
                string title = string.Format(CultureInfo.InvariantCulture, Resources.EditGalleries_ViewingGallery,
                                             selectedGallery.Title);
                AdminMasterPage.Title = title;
            }

            AddImages.Collapsed = !Preferences.AlwaysExpandAdvanced;
        }

        private void ShowResults()
        {
            Results.Visible = true;
        }

        private void HideResults()
        {
            Results.Visible = false;
        }

        private void ShowImages()
        {
            HideResults();
            ImagesDiv.Visible = true;
        }

        private void HideImages()
        {
            ShowResults();
            ImagesDiv.Visible = false;
        }

        protected string EvalImageUrl(object potentialImage)
        {
            var image = potentialImage as Image;
            if (image != null)
            {
                image.Blog = Blog;
                return Url.GalleryImageUrl(image, image.ThumbNailFile);
            }
            return String.Empty;
        }

        protected string EvalImageNavigateUrl(object potentialImage)
        {
            var image = potentialImage as Image;
            if (image != null)
            {
                return Url.GalleryImagePageUrl(image);
            }
            return String.Empty;
        }

        protected string EvalImageTitle(object potentialImage)
        {
            const int targetHeight = 138;
            const int maxImageHeight = 120;
            const int charPerLine = 19;
            const int lineHeightPixels = 16;

            var image = potentialImage as Image;
            if (image != null)
            {
                // do a rough calculation of how many chars we can shoehorn into the title space
                // we have to back into an estimated thumbnail height right now with aspect * max
                double aspectRatio = (double)image.Height / image.Width;
                if (aspectRatio > 1 || aspectRatio <= 0)
                {
                    aspectRatio = 1;
                }
                var allowedChars = (int)((targetHeight - maxImageHeight * aspectRatio)
                                         / lineHeightPixels * charPerLine);

                return Utilities.Truncate(image.Title, allowedChars);
            }
            return String.Empty;
        }

        // REFACTOR: duplicate from category editor; generalize a la EntryEditor
        private void PersistCategory(LinkCategory category)
        {
            try
            {
                if (category.Id > 0)
                {
                    Repository.UpdateLinkCategory(category);
                    Messages.ShowMessage(string.Format(CultureInfo.InvariantCulture, Resources.Message_CategoryUpdated,
                                                       category.Title));
                }
                else
                {
                    category.Id = Repository.CreateLinkCategory(category);
                    Messages.ShowMessage(string.Format(CultureInfo.InvariantCulture, Resources.Message_CategoryAdded,
                                                       category.Title));
                }
            }
            catch (Exception ex)
            {
                Messages.ShowError(String.Format(Constants.RES_EXCEPTION, "TODO...", ex.Message));
            }
        }

        /// <summary>
        /// We're being asked to upload and store an image on the server (re-sizing and
        /// all of that). Ideally this will work. It may not. We may have to ask
        /// the user for an alternative file name. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnAddImage(object sender, EventArgs e)
        {
            string fileName = ImageFile.PostedFile.FileName;

            string extension = Path.GetExtension(fileName);
            if (extension.Equals(".zip", StringComparison.OrdinalIgnoreCase))
            {
                // Handle as an archive
                PersistImageArchive();
                return;
            }

            // If there was no dot, or extension wasn't ZIP, then treat as a single image
            PersistImage(fileName);
        }


        private void PersistImageArchive()
        {
            List<string> goodFiles = new List<string>(),
                         badFiles = new List<string>(),
                         updatedFiles = new List<string>();

            byte[] archiveData = ImageFile.PostedFile.GetFileStream();

            using (var zipArchive = ZipFile.Read(new MemoryStream(archiveData)))
            {
                foreach (var entry in zipArchive.Entries)
                {
                    var image = new Image
                    {
                        Blog = Blog,
                        CategoryID = CategoryId,
                        Title = entry.FileName,
                        IsActive = ckbIsActiveImage.Checked,
                        FileName = Path.GetFileName(entry.FileName),
                        Url = Url.ImageGalleryDirectoryUrl(Blog, CategoryId),
                        LocalDirectoryPath = Url.GalleryDirectoryPath(Blog, CategoryId)
                    };

                    var memoryStream = new MemoryStream();

                    entry.Extract(memoryStream);
                    var fileData = memoryStream.ToArray();
                    try
                    {
                        // If it exists, update it
                        if (File.Exists(image.OriginalFilePath))
                        {
                            Repository.Update(image, fileData);
                            updatedFiles.Add(entry.FileName);
                        }
                        else
                        {
                            // Attempt insertion as a new image
                            int imageId = Repository.Insert(image, fileData);
                            if (imageId > 0)
                            {
                                goodFiles.Add(entry.FileName);
                            }
                            else
                            {
                                // Wrong format, perhaps?
                                badFiles.Add(entry.FileName);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        badFiles.Add(entry.FileName + " (" + ex.Message + ")");
                    }
                }
            }

            // Construct and display the status message of added/updated/deleted images
            string status = string.Format(CultureInfo.InvariantCulture,
                                          Resources.EditGalleries_ArchiveProcessed +
                                          @"<br />
                <b><a onclick=""javascript:ToggleVisibility(document.getElementById('ImportAddDetails'))"">" +
                                          Resources.Label_Adds +
                                          @" ({0})</a></b><span id=""ImportAddDetails"" style=""display:none""> : <br />&nbsp;&nbsp;{1}</span><br />
                <b><a onclick=""javascript:ToggleVisibility(document.getElementById('ImportUpdateDetails'))"">" +
                                          Resources.Label_Updates +
                                          @"  ({2})</a></b><span id=""ImportUpdateDetails"" style=""display:none""> : <br />&nbsp;&nbsp;{3}</span><br />
                <b><a onclick=""javascript:ToggleVisibility(document.getElementById('ImportErrorDetails'))"">" +
                                          Resources.Label_Errors +
                                          @" ({4})</a></b><span id=""ImportErrorDetails"" style=""display:none""> : <br />&nbsp;&nbsp;{5}</span>",
                                          goodFiles.Count,
                                          (goodFiles.Count > 0
                                               ? string.Join("<br />&nbsp;&nbsp;", goodFiles.ToArray())
                                               : "none"),
                                          updatedFiles.Count,
                                          (updatedFiles.Count > 0
                                               ? string.Join("<br />&nbsp;&nbsp;", updatedFiles.ToArray())
                                               : "none"),
                                          badFiles.Count,
                                          (badFiles.Count > 0
                                               ? string.Join("<br />&nbsp;&nbsp;", badFiles.ToArray())
                                               : "none"));

            Messages.ShowMessage(status);
            txbImageTitle.Text = String.Empty;

            // if we're successful we need to revert back to our standard view
            PanelSuggestNewName.Visible = false;
            PanelDefaultName.Visible = true;

            // re-bind the gallery; note we'll skip this step if a correctable error occurs.
            BindGallery();
        }

        /// <summary>
        /// The user is providing the file name here. 
        /// </summary>
        protected void OnAddImageUserProvidedName(object sender, EventArgs e)
        {
            if (TextBoxImageFileName.Text.Length == 0)
            {
                Messages.ShowError(Resources.EditGalleries_ValidFilenameRequired);
                return;
            }

            PersistImage(TextBoxImageFileName.Text);
        }

        /// <summary>
        /// A fancy term for saving the image to disk :-). We'll take the image and try to save
        /// it. This currently puts all images in the same directory which can cause a conflict
        /// if the file already exists. So we'll add in a way to take a new file name. 
        /// </summary>
        private void PersistImage(string fileName)
        {
            if (Page.IsValid)
            {
                var image = new Image
                {
                    Blog = Blog,
                    CategoryID = CategoryId,
                    Title = txbImageTitle.Text,
                    IsActive = ckbIsActiveImage.Checked,
                    FileName = Path.GetFileName(fileName),
                    Url = Url.ImageGalleryDirectoryUrl(Blog, CategoryId),
                    LocalDirectoryPath = Url.GalleryDirectoryPath(Blog, CategoryId)
                };

                try
                {
                    if (File.Exists(image.OriginalFilePath))
                    {
                        // tell the user we can't accept this file.
                        Messages.ShowError(Resources.EditGalleries_FileAlreadyExists);

                        // switch around our GUI.
                        PanelSuggestNewName.Visible = true;
                        PanelDefaultName.Visible = false;

                        AddImages.Collapsed = false;
                        // Unfortunately you can't set ImageFile.PostedFile.FileName. At least suggest
                        // a name for the new file.
                        TextBoxImageFileName.Text = image.FileName;
                        return;
                    }

                    int imageId = Repository.Insert(image, ImageFile.PostedFile.GetFileStream());
                    if (imageId > 0)
                    {
                        Messages.ShowMessage(Resources.EditGalleries_ImageAdded);
                        txbImageTitle.Text = String.Empty;
                    }
                    else
                    {
                        Messages.ShowError(Constants.RES_FAILUREEDIT + " " + Resources.EditGalleries_ProblemPosting);
                    }
                }
                catch (Exception ex)
                {
                    Messages.ShowError(String.Format(Constants.RES_EXCEPTION, "TODO...", ex.Message));
                }
            }

            // if we're successful we need to revert back to our standard view
            PanelSuggestNewName.Visible = false;
            PanelDefaultName.Visible = true;

            // re-bind the gallery; note we'll skip this step if a correctable error occurs.
            BindGallery();
        }

        private void DeleteGallery(int categoryId, string categoryTitle)
        {
            var command = new DeleteGalleryCommand(Repository, SubtextContext.HttpContext.Server, Url.ImageGalleryDirectoryUrl(Blog, categoryId), categoryId,
                                                   categoryTitle)
            {
                ExecuteSuccessMessage = String.Format(CultureInfo.CurrentCulture, "Gallery '{0}' deleted",
                                                      categoryTitle)
            };
            Messages.ShowMessage(command.Execute());
            BindList();
        }

        private void DeleteImage(int imageId)
        {
            Image image = Repository.GetImage(imageId, false /* activeOnly */);

            string galleryDirectoryPath = SubtextContext.HttpContext.Server.MapPath(Url.ImageGalleryDirectoryUrl(Blog, image.CategoryID));

            var command = new DeleteImageCommand(Repository, image, galleryDirectoryPath)
            {
                ExecuteSuccessMessage = string.Format(CultureInfo.CurrentCulture, "Image '{0}' deleted",
                                                      image.OriginalFile)
            };
            Messages.ShowMessage(command.Execute());
            BindGallery();
        }

        override protected void OnInit(EventArgs e)
        {
            dgrSelectionList.ItemCommand += dgrSelectionList_ItemCommand;
            dgrSelectionList.CancelCommand += dgrSelectionList_CancelCommand;
            dgrSelectionList.EditCommand += dgrSelectionList_EditCommand;
            dgrSelectionList.UpdateCommand += dgrSelectionList_UpdateCommand;
            dgrSelectionList.DeleteCommand += dgrSelectionList_DeleteCommand;
            rprImages.ItemCommand += rprImages_ItemCommand;
            base.OnInit(e);
        }

        private void dgrSelectionList_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            switch (e.CommandName.ToLower(CultureInfo.InvariantCulture))
            {
                case "view":
                    int galleryId = Convert.ToInt32(e.CommandArgument);
                    BindGallery(galleryId);
                    break;
                default:
                    break;
            }
        }

        private void dgrSelectionList_EditCommand(object source, DataGridCommandEventArgs e)
        {
            HideImages();
            dgrSelectionList.EditItemIndex = e.Item.ItemIndex;
            BindList();
            Messages.Clear();
        }

        private void dgrSelectionList_UpdateCommand(object source, DataGridCommandEventArgs e)
        {
            var title = e.Item.FindControl("txbTitle") as TextBox;
            var desc = e.Item.FindControl("txbDescription") as TextBox;

            var isActive = e.Item.FindControl("ckbIsActive") as CheckBox;

            if (Page.IsValid && null != title && null != isActive)
            {
                int id = Convert.ToInt32(dgrSelectionList.DataKeys[e.Item.ItemIndex]);

                LinkCategory existingCategory = SubtextContext.Repository.GetLinkCategory(id, false);
                existingCategory.Title = title.Text;
                existingCategory.IsActive = isActive.Checked;
                if (desc != null)
                {
                    existingCategory.Description = desc.Text;
                }

                if (id != 0)
                {
                    PersistCategory(existingCategory);
                }

                dgrSelectionList.EditItemIndex = -1;
                BindList();
            }
        }

        private void dgrSelectionList_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            int id = Convert.ToInt32(dgrSelectionList.DataKeys[e.Item.ItemIndex]);
            LinkCategory lc = SubtextContext.Repository.GetLinkCategory(id, false);
            if (lc != null)
            {
                DeleteGallery(id, lc.Title);
            }
            else
            {
                Messages.ShowError("The gallery does not exist. Possibly you refreshed the page after deleting the gallery?");
            }
            BindList();
        }

        private void dgrSelectionList_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            dgrSelectionList.EditItemIndex = -1;
            BindList();
            Messages.Clear();
        }

        protected void lkbPost_Click(object sender, EventArgs e)
        {
            var newCategory = new LinkCategory
            {
                CategoryType = CategoryType.ImageCollection,
                Title = txbNewTitle.Text,
                IsActive = ckbNewIsActive.Checked,
                Description = txbNewDescription.Text
            };
            PersistCategory(newCategory);

            BindList();
            txbNewTitle.Text = String.Empty;
            ckbNewIsActive.Checked = Preferences.AlwaysCreateIsActive;
        }

        private void rprImages_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName.ToLower(CultureInfo.InvariantCulture))
            {
                case "deleteimage":
                    DeleteImage(Convert.ToInt32(e.CommandArgument));
                    break;
                default:
                    break;
            }
        }
    }
}