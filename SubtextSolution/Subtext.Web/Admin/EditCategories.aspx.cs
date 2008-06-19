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
using System.Web.UI.WebControls;
using Subtext.Framework;
using Subtext.Framework.Components;
using CategoryTypeEnum = Subtext.Framework.Components.CategoryType;

namespace Subtext.Web.Admin.Pages
{
	public partial class EditCategories : AdminPage
	{
        public EditCategories() : base()
	    {
	    }
	    
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
                if (null != Request.QueryString[Keys.QRYSTR_CATEGORYTYPE])
                {
                    this.CategoryType = (CategoryType)Enum.Parse(typeof(CategoryType), Request.QueryString[Keys.QRYSTR_CATEGORYTYPE]);
                }
			    else
                {
                    this.CategoryType = CategoryTypeEnum.LinkCollection;
                }

				if(this.CategoryType == CategoryTypeEnum.LinkCollection)
				{
					this.TabSectionId = "Links";
				}

				ckbNewIsActive.Checked = Preferences.AlwaysCreateIsActive;

				BindLocalUI();				
				BindList();
			}
			else
			{
				if (this.CategoryType == CategoryTypeEnum.LinkCollection)
				{
					this.TabSectionId = "Links";
				}
			}
		}

        protected CategoryTypeEnum CategoryType
	    {
	        get
	        {
                if (ViewState["CategoryType"] != null)
                {
                    return (CategoryType)ViewState["CategoryType"];    
                }
                else
                {
                    return CategoryType.None;
                }
	        }
	        set
	        {
                ViewState["CategoryType"] = value;
	        }
	    }

		// REFACTOR: Maybe. Some sections can be inferred from the catType, but not the not cat pages.
		private void BindLocalUI()
		{
            switch (CategoryType)
			{
				case CategoryType.PostCollection : 
					TabSectionId = "Posts";
					break;
				
			    case CategoryType.StoryCollection :
                    TabSectionId = "Articles";
					break;
				
			    case CategoryType.LinkCollection :
                    TabSectionId = "Links";
					break;
				
			    case CategoryType.ImageCollection : 
					TabSectionId = "Galleries";
					// TODO: redirect to galleries? or just have original link stay there?
					break;
				
			    default :
					TabSectionId = "Posts";
					break;
			}
		}

		private void BindList()
		{
            IList<LinkCategory> cats = Links.GetCategories(CategoryType, ActiveFilter.None);
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
					Links.UpdateLinkCategory(category);
					this.Messages.ShowMessage(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Category \"{0}\" was updated.", category.Title));
				}
				else
				{
					category.Id = Links.CreateLinkCategory(category);
					this.Messages.ShowMessage(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Category \"{0}\" was added.", category.Title));
				}					
			}
			catch(Exception ex)
			{
				this.Messages.ShowError(String.Format(Constants.RES_EXCEPTION, "TODO...", ex.Message));
			}
		}

		private void ConfirmDelete(int categoryID, string categoryTitle)
		{
			this.Command = new DeleteCategoryCommand(categoryID, categoryTitle);
			this.Command.RedirectUrl = Request.Url.ToString();
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
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.dgrItems.CancelCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgrCategories_CancelCommand);
			this.dgrItems.EditCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgrCategories_EditCommand);
			this.dgrItems.UpdateCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgrCategories_UpdateCommand);
			this.dgrItems.DeleteCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgrCategories_DeleteCommand);

		}
		#endregion

		private void dgrCategories_EditCommand(object source, DataGridCommandEventArgs e)
		{
			dgrItems.EditItemIndex = e.Item.ItemIndex;
			BindList();
			this.Messages.Clear();
			ToggleAddNew(false);
		}

		private void dgrCategories_UpdateCommand(object source, DataGridCommandEventArgs e)
		{	
			TextBox title = e.Item.FindControl("txbTitle") as TextBox;
			CheckBox isActive = e.Item.FindControl("ckbIsActive") as CheckBox;
			TextBox txbDescription = e.Item.FindControl("txbDescription") as TextBox;

			if (null == title || null == isActive)
				throw new ApplicationException("Update failed, could not located either the item Title or IsActive");			

			if (Page.IsValid)
			{
				if (Utilities.IsNullorEmpty(title.Text))
				{
					Messages.ShowError("You cannot have a category with a blank Title");
					return;
				}

				int id = Convert.ToInt32(dgrItems.DataKeys[e.Item.ItemIndex]);
				
				LinkCategory existingCategory = Links.GetLinkCategory(id,false);
				existingCategory.Description = txbDescription.Text;
				existingCategory.Title = title.Text;
				existingCategory.IsActive = isActive.Checked;
		
				if (id != 0) 
					PersistCategory(existingCategory);

				dgrItems.EditItemIndex = -1;
				BindList();
				ToggleAddNew(true);
			}
		}

		private void dgrCategories_DeleteCommand(object source, DataGridCommandEventArgs e)
		{
			int id = Convert.ToInt32(dgrItems.DataKeys[e.Item.ItemIndex]);
			LinkCategory lc = Links.GetLinkCategory(id,false);
			ConfirmDelete(id, lc.Title);
		}

		private void dgrCategories_CancelCommand(object source, DataGridCommandEventArgs e)
		{
			dgrItems.EditItemIndex = -1;			
			BindList();
			this.Messages.Clear();
			ToggleAddNew(true);
		}

		protected void lkbPost_Click(object sender, EventArgs e)
		{
			if (Page.IsValid)
			{
				LinkCategory newCategory = new LinkCategory();
                newCategory.CategoryType = CategoryType;
				newCategory.Title = txbNewTitle.Text;
				newCategory.IsActive = ckbNewIsActive.Checked;
				newCategory.Description = txbNewDescription.Text;
				PersistCategory(newCategory);

				BindList();	
				txbNewTitle.Text = string.Empty;
				ckbNewIsActive.Checked = Preferences.AlwaysCreateIsActive;
				txbNewDescription.Text = string.Empty;
			}
		}
	}
}

