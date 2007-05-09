using System;

namespace Subtext.Web.HostAdmin.UserControls
{
	public partial class CreateUserControl : System.Web.UI.UserControl
	{
		protected void Page_Load(object sender, EventArgs e)
		{
		}

		protected void OnCreatedUser(object sender, EventArgs e)
		{
			OnSaveComplete();
		}

		protected void OnCancelClick(object sender, EventArgs e)
		{
			OnCancel();
		}

		public event EventHandler SaveComplete;

		protected void OnSaveComplete()
		{
			EventHandler saveComplete = SaveComplete;
			if (saveComplete != null)
				saveComplete(this, EventArgs.Empty);
		}

		public event EventHandler Cancelled;

		protected void OnCancel()
		{
			EventHandler cancel = Cancelled;
			if (cancel != null)
				cancel(this, EventArgs.Empty);
		}

		public string CreatedUserName
		{
			get { return (string)ViewState["CreatedUserName"] ?? string.Empty; }
			set { ViewState["CreatedUserName"] = value; }
		}
	}
}