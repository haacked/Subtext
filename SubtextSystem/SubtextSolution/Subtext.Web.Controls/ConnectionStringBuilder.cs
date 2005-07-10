using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Subtext.Web.Controls
{
	/// <summary>
	/// Control used to specify a connection string.
	/// </summary>
	[ValidationProperty("ConnectionString")]
	public class ConnectionStringBuilder : System.Web.UI.WebControls.WebControl, INamingContainer
	{
		const string CONNECTION_STRING_FIELD = "txtConnectionStringBuilderConnectionString";
		const string DESCRIPTION_FIELD = "ltlConnectionStringBuilderDescriptionText";
		const string TITLE_FIELD = "ltlConnectionStringBuilderTitleText";

		/// <summary>
		/// Called during the page init event.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPreRender(EventArgs e)
		{
			this.TitleLiteralControl.Text = this.Title;
			this.DescriptionLiteralControl.Text = this.Description;
			base.OnPreRender(e);
		}

		/// <summary>
		/// Creates the child controls for this control.
		/// </summary>
		protected override void CreateChildControls()
		{
			HtmlTable table = new HtmlTable();
			table.CellPadding = 3;
			table.CellSpacing = 0;

			// Create header row
			HtmlTableRow headerRow = new HtmlTableRow();
			headerRow.BgColor = "#EEEEEE";
			HtmlTableCell fieldCell = new HtmlTableCell("TH");
			fieldCell.ColSpan = 2;
			HtmlGenericControl strongControl = new HtmlGenericControl("strong");
			LiteralControl titleControl = new LiteralControl("Connection String");
			titleControl.ID = TITLE_FIELD;

			strongControl.Controls.Add(titleControl);
			fieldCell.Controls.Add(strongControl);
			headerRow.Cells.Add(fieldCell);
			table.Rows.Add(headerRow);

			// Create Text Input Row
			HtmlTableRow row = new HtmlTableRow();
			row.VAlign = "top";
			HtmlTableCell questionCell = new HtmlTableCell();
			TextBox textbox = new TextBox();
			textbox.ID = CONNECTION_STRING_FIELD;
			questionCell.Controls.Add(textbox);
			row.Cells.Add(questionCell);

			HtmlTableCell descriptionCell = new HtmlTableCell();
			LiteralControl descriptionText = new LiteralControl(Description);
			descriptionText.ID = DESCRIPTION_FIELD;
			descriptionCell.Controls.Add(descriptionText);
			row.Cells.Add(descriptionCell);
			
			table.Rows.Add(row);
			this.Controls.Add(table);
			base.CreateChildControls();
		}

		/// <summary>
		/// Gets or sets the connection string.
		/// </summary>
		/// <value></value>
		[Browsable(true)]
		[DefaultValue("")]
		[Category("Behavior")]
		[Description("The Connection String.")]
		public string ConnectionString
		{
			get
			{
				return ConnectionStringTextBox.Text;
			}
			set
			{
				ConnectionStringTextBox.Text = value;
			}
		}

		/// <summary>
		/// Gets or sets the title.
		/// </summary>
		/// <value></value>
		[Browsable(true)]
		[DefaultValue("Connection String")]
		[Category("Display")]
		[Description("Title of this control.")]
		public string Title
		{
			get
			{
				if(ViewState["Title"] != null)
					return ViewState["Title"] as string;
				return string.Empty;
			}
			set
			{
				ViewState["Title"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the description.
		/// </summary>
		/// <value></value>
		[Browsable(true)]
		[DefaultValue("")]
		[Category("Display")]
		[Description("Description of what the connection string is used for.")]
		public string Description
		{
			get
			{
				if(ViewState["Description"] != null)
					return ViewState["Description"] as string;
				return string.Empty;
			}
			set
			{
				ViewState["Description"] = value;
			}
		}

		TextBox ConnectionStringTextBox
		{
			get
			{
				EnsureChildControls();
				return FindControl(CONNECTION_STRING_FIELD) as TextBox;
			}
		}

		LiteralControl DescriptionLiteralControl
		{
			get
			{
				EnsureChildControls();
				return FindControl(DESCRIPTION_FIELD) as LiteralControl;
			}
		}

		LiteralControl TitleLiteralControl
		{
			get
			{
				EnsureChildControls();
				return FindControl(TITLE_FIELD) as LiteralControl;
			}
		}
	}
}
