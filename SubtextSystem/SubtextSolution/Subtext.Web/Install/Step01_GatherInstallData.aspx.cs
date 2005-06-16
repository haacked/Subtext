using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Subtext.Framework;

namespace Subtext.Web.Install
{
	/// <summary>
	/// Page used to create an initial configuration for the blog.
	/// </summary>
	/// <remarks>
	/// This page will ONLY be displayed if there are no 
	/// blog configurations within the database.
	/// </remarks>
	public class Step01_GatherInstallData : InstallationBase
	{
		NameValueCollection installationQuestions = InstallationManager.GetInstallationQuestions();
		protected Subtext.Web.Controls.ContentRegion MPTitle;
		protected Subtext.Web.Controls.ContentRegion MPSubTitle;
		protected Subtext.Web.Controls.ContentRegion Content;
		protected Subtext.Web.Controls.MasterPage MPContainer;
		protected System.Web.UI.HtmlControls.HtmlTable installationQuestionTable;
		protected System.Web.UI.WebControls.Button btnSave;
		protected System.Web.UI.WebControls.Literal ltlErrorMessage;
		protected System.Web.UI.WebControls.CheckBox chkFullInstallation;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			EnableViewState = false;
			DataBind();
		}

		/// <summary>
		/// Binds the data to the form controls.
		/// </summary>
		public override void DataBind()
		{
			HtmlTableRow headerRow = new HtmlTableRow();
			headerRow.BgColor = "#EEEEEE";
			HtmlTableCell fieldCell = new HtmlTableCell("TH");
			fieldCell.Controls.Add(new LiteralControl("<strong>Field</strong>"));
			headerRow.Cells.Add(fieldCell);

			HtmlTableCell inputHeaderCell = new HtmlTableCell("TH");
			inputHeaderCell.Controls.Add(new LiteralControl("<strong>Input</strong>"));
			headerRow.Cells.Add(inputHeaderCell);

			HtmlTableCell descriptionHeaderCell = new HtmlTableCell("TH");
			descriptionHeaderCell.Controls.Add(new LiteralControl("<strong>Description</strong>"));
			headerRow.Cells.Add(descriptionHeaderCell);

			installationQuestionTable.Rows.Add(headerRow);

			foreach(string question in installationQuestions.AllKeys)
			{
				HtmlTableRow row = new HtmlTableRow();
				row.VAlign = "top";
				
				HtmlTableCell questionCell = new HtmlTableCell();
				questionCell.Controls.Add(new LiteralControl("<strong>" + question + "</strong>"));
				row.Cells.Add(questionCell);

				HtmlTableCell inputCell = new HtmlTableCell();
				TextBox textBox = new TextBox();
				textBox.ID = question;
				inputCell.Controls.Add(textBox);
				row.Cells.Add(inputCell);

				HtmlTableCell descriptionCell = new HtmlTableCell();
				descriptionCell.Controls.Add(new LiteralControl(installationQuestions[question]));
				row.Cells.Add(descriptionCell);

				installationQuestionTable.Rows.Add(row);
				installationQuestionTable.CellPadding = 3;
				installationQuestionTable.CellSpacing = 0;
			}
			base.DataBind ();
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			NameValueCollection answers = new NameValueCollection();
			foreach(string question in installationQuestions.AllKeys)
			{
				answers.Add(question, Request.Form[question]);
			}

			NameValueCollection errors = InstallationManager.ValidateInstallationAnswers(answers);
			if(errors.Count == 0)
			{			
				InstallationManager.SetInstallationQuestionAnswers(answers);
				Response.Redirect(NextStepUrl);
			}
			else
			{
				string errorMessages = "There were mistakes in the information you provided.<br /><ul>";
				foreach(string name in errors.AllKeys)
				{
					errorMessages += "<li class=\"error\">" + name + " - " + errors[name] + "</li>" + Environment.NewLine;
				}
				ltlErrorMessage.Text = errorMessages + "</ul>";
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
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
