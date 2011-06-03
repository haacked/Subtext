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
using System.Web.UI.WebControls;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Web.Admin.Commands;
using Subtext.Web.Properties;
using CategoryTypeEnum = Subtext.Framework.Components.CategoryType;

namespace Subtext.Web.Admin.Pages
{
    public partial class EditCategories : AdminPage
    {
        protected CategoryTypeEnum CategoryType
        {
            get
            {
                if (ViewState["CategoryType"] != null)
                {
                    return (CategoryType)ViewState["CategoryType"];
                }
                return CategoryType.None;
            }
            set { ViewState["CategoryType"] = value; }
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!IsPostBack)
            {
                if (null != Request.QueryString[Keys.QRYSTR_CATEGORYTYPE])
                {
                    CategoryType =
                        (CategoryType)Enum.Parse(typeof(CategoryType), Request.QueryString[Keys.QRYSTR_CATEGORYTYPE]);
                }
                else
                {
                    CategoryType = CategoryType.LinkCollection;
                }

                if (CategoryType == CategoryType.LinkCollection)
                {
                    TabSectionId = "Links";
                }

                ckbNewIsActive.Checked = Preferences.AlwaysCreateIsActive;

                BindLocalUI();
                BindList();
            }
            else
            {
                if (CategoryType == CategoryType.LinkCollection)
                {
                    TabSectionId = "Links";
                }
            }
            base.OnLoad(e);
        }

        // REFACTOR: Maybe. Some sections can be inferred from the catType, but not the not cat pages.
        private void BindLocalUI()
        {
            switch (CategoryType)
            {
                case CategoryType.PostCollection:
                    TabSectionId = "Posts";
                    break;

                case CategoryType.StoryCollection:
                    TabSectionId = "Articles";
                    break;

                case CategoryType.LinkCollection:
                    TabSectionId = "Links";
                    break;

                case CategoryType.ImageCollection:
                    TabSectionId = "Galleries";
                    // TODO: redirect to galleries? or just have original link stay there?
                    break;

                default:
                    TabSectionId = "Posts";
                    break;
            }
        }

        private void BindList()
        {
            ICollection<LinkCategory> cats = Repository.GetCategories(CategoryType, false /*activeOnly*/);
            dgrItems.DataSource = cats;
            dgrItems.DataKeyField = "Id";
            dgrItems.DataBind();
        }

        private void ToggleAddNew(bool showAddNew)
        {
            Add.Visible = showAddNew;
            valtxbNewTitleRequired.Enabled = showAddNew;
        }

        private void PersistCategory(LinkCategory category)
        {
            try
            {
                if (category.Id > 0)
                {
                    Repository.UpdateLinkCategory(category);
                    Messages.ShowMessage(string.Format(CultureInfo.InvariantCulture, "Category \"{0}\" was updated.",
                                                       category.Title));
                }
                else
                {
                    category.Id = Repository.CreateLinkCategory(category);
                    Messages.ShowMessage(string.Format(CultureInfo.InvariantCulture, "Category \"{0}\" was added.",
                                                       category.Title));
                }
            }
            catch (Exception ex)
            {
                Messages.ShowError(String.Format(Constants.RES_EXCEPTION, "TODO...", ex.Message));
            }
        }

        private void ConfirmDelete(int categoryID, string categoryTitle)
        {
            var command = new DeleteCategoryCommand(Repository, categoryID, categoryTitle)
            {
                ExecuteSuccessMessage = string.Format(CultureInfo.InvariantCulture, "Category \"{0}\" was deleted.",
                                                      categoryTitle)
            };
            Messages.ShowMessage(command.Execute());
            BindList();
        }

        override protected void OnInit(EventArgs e)
        {
            dgrItems.CancelCommand += dgrCategories_CancelCommand;
            dgrItems.EditCommand += dgrCategories_EditCommand;
            dgrItems.UpdateCommand += dgrCategories_UpdateCommand;
            dgrItems.DeleteCommand += dgrCategories_DeleteCommand;
            base.OnInit(e);
        }

        private void dgrCategories_EditCommand(object source, DataGridCommandEventArgs e)
        {
            dgrItems.EditItemIndex = e.Item.ItemIndex;
            BindList();
            Messages.Clear();
            ToggleAddNew(false);
        }

        private void dgrCategories_UpdateCommand(object source, DataGridCommandEventArgs e)
        {
            var title = e.Item.FindControl("txbTitle") as TextBox;
            var isActive = e.Item.FindControl("ckbIsActive") as CheckBox;
            var txbDescription = e.Item.FindControl("txbDescription") as TextBox;

            if (null == title || null == isActive)
            {
                throw new InvalidOperationException(Resources.InvalidOperation_EditCategoriesControlsNotFound);
            }

            if (Page.IsValid)
            {
                if (Utilities.IsNullorEmpty(title.Text))
                {
                    Messages.ShowError(Resources.Message_CategoryMustHaveTitle);
                    return;
                }

                int id = Convert.ToInt32(dgrItems.DataKeys[e.Item.ItemIndex]);

                LinkCategory existingCategory = SubtextContext.Repository.GetLinkCategory(id, false);
                existingCategory.Description = txbDescription.Text;
                existingCategory.Title = title.Text;
                existingCategory.IsActive = isActive.Checked;

                if (id != 0)
                {
                    PersistCategory(existingCategory);
                }

                dgrItems.EditItemIndex = -1;
                BindList();
                ToggleAddNew(true);
            }
        }

        private void dgrCategories_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            int id = Convert.ToInt32(dgrItems.DataKeys[e.Item.ItemIndex]);
            LinkCategory lc = SubtextContext.Repository.GetLinkCategory(id, false);
            ConfirmDelete(id, lc.Title);
        }

        private void dgrCategories_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            dgrItems.EditItemIndex = -1;
            BindList();
            Messages.Clear();
            ToggleAddNew(true);
        }

        protected void lkbPost_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                var newCategory = new LinkCategory
                {
                    CategoryType = CategoryType,
                    Title = txbNewTitle.Text,
                    IsActive = ckbNewIsActive.Checked,
                    Description = txbNewDescription.Text
                };
                PersistCategory(newCategory);

                Response.Redirect(Request.RawUrl);
            }
        }
    }
}