using System;
using System.ComponentModel;
using System.Web.UI.WebControls;

namespace Subtext.Web.Controls.DataSources
{
	/// <summary>
	/// Acts as a static data source for Membership Users 
	/// and is compatible with the <see cref="ObjectDataSource">ObjectDataSource</see>.
	/// </summary>
	[DisplayName("MembershipUserDataSource")]
	[Description("Custom ObjectDataSource for MembershipUsers")]
	public class MembershipUserDataSource : SubtextObjectDataSource
	{
		public MembershipUserDataSource()
		{
			//The Data Access class.
			this.DataObjectTypeName = "Subtext.Web.Controls.DataSources.SubtextMembershipUser";
			
			//The "Business" object.
			this.TypeName = "Subtext.Web.Controls.DataSources.MembershipDataLayer";
			
			this.InsertMethod = "Insert";
			this.DeleteMethod = "Delete";
			this.SelectMethod = "Select";
			this.UpdateMethod = "Update";
			this.SelectCountMethod = "SelectCount";

			this.StartRowIndexParameterName = "pageIndex";
			this.MaximumRowsParameterName = "pageSize";

			this.DeleteParameters.Add("UserName", TypeCode.String, "");
			this.EnablePaging = true;
		}
	}
}

