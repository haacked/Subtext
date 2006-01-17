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
		const string ConnectionStringControlId = "txtConnectionStringBuilderConnectionString";
		const string DescriptionControlId = "ltlConnectionStringBuilderDescriptionText";
		const string TitleControlId = "ltlConnectionStringBuilderTitleText";
		const string CheckboxControlId = "chkUseConnectionStringInWebConfig";

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
			titleControl.ID = TitleControlId;

			strongControl.Controls.Add(titleControl);
			fieldCell.Controls.Add(strongControl);
			headerRow.Cells.Add(fieldCell);
			table.Rows.Add(headerRow);

			// Create Text Input Row
			HtmlTableRow row = new HtmlTableRow();
			row.VAlign = "top";
			HtmlTableCell questionCell = new HtmlTableCell();
			TextBox textbox = new TextBox();
			textbox.ID = ConnectionStringControlId;
			questionCell.Controls.Add(textbox);
			row.Cells.Add(questionCell);

			//Checkbox to use connection string in web.config
			if(AllowWebConfigOverride)
			{
				CheckBox checkbox = new CheckBox();
				checkbox.ID = CheckboxControlId;
				checkbox.Text = "Use Connection String In Web.config";
				//This next line is a hack since we don't yet know the client id yet.
				checkbox.Attributes["onclick"] = "if(this.checked) {" + this.ID + "_" + textbox.ClientID + ".disabled = true;} else {" + this.ID + "_" + textbox.ClientID + ".disabled = false;} ;";
				questionCell.Controls.Add(new LiteralControl("<br />"));
				questionCell.Controls.Add(checkbox);
			}

			HtmlTableCell descriptionCell = new HtmlTableCell();
			LiteralControl descriptionText = new LiteralControl(Description);
			descriptionText.ID = DescriptionControlId;
			descriptionCell.Controls.Add(descriptionText);
			row.Cells.Add(descriptionCell);
			
			table.Rows.Add(row);
			this.Controls.Add(table);
			base.CreateChildControls();
		}

		/// <summary>
		/// Gets a value indicating whether [allow web config override].
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [allow web config override]; otherwise, <c>false</c>.
		/// </value>
		[Browsable(true)]
		[DefaultValue("false")]
		[Category("Behavior")]
		[Description("Whether or not to display checkbox...")]
		public bool AllowWebConfigOverride
		{
			get
			{
				if(ViewState["AllowWebConfigOverride"] != null)
					return (bool)ViewState["AllowWebConfigOverride"];
				return false;
			}
			set
			{
				ViewState["AllowWebConfigOverride"] = value;
			}
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
				if(UseWebConfigCheckBox != null && UseWebConfigCheckBox.Checked)
					return System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"];

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
				return FindControl(ConnectionStringControlId) as TextBox;
			}
		}

		CheckBox UseWebConfigCheckBox
		{
			get
			{
				EnsureChildControls();
				return FindControl(CheckboxControlId) as CheckBox;
			}

		}

		LiteralControl DescriptionLiteralControl
		{
			get
			{
				EnsureChildControls();
				return FindControl(DescriptionControlId) as LiteralControl;
			}
		}

		LiteralControl TitleLiteralControl
		{
			get
			{
				EnsureChildControls();
				return FindControl(TitleControlId) as LiteralControl;
			}
		}
	}
}
