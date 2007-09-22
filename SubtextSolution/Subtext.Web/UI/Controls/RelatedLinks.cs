using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Data;
using Subtext.Framework.Providers;

namespace Subtext.Web.UI.Controls
{
    /// <summary>
	/// Summary description for RelatedLinks.
	/// </summary>
	public class RelatedLinks : BaseControl
	{
		protected Repeater urlRelatedLinks;

		private void Page_Load(object sender, EventArgs e)
		{
			Entry entry = Cacher.GetEntryFromRequest(CacheDuration.Short);

			IList<RelatedLink> links = ObjectProvider.Instance().GetRelatedLinks(entry.Id);

			urlRelatedLinks.DataSource = links;
			urlRelatedLinks.DataBind();

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
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.Load += new EventHandler(this.Page_Load);
		}

		#endregion

	}

}
