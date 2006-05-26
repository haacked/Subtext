<%@ Page language="c#" Codebehind="Preferences.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.Admin.Pages.EditPreferences" %>
<%@ Register TagPrefix="ANW" Namespace="Subtext.Web.Admin.WebUI" Assembly="Subtext.Web" %>
<%@ Register TagPrefix="SP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<ANW:Page runat="server" id="PageContainer" TabSectionID="Options" CategoriesLabel="Other Items">
	<ANW:AdvancedPanel id="Edit" runat="server" BodyCssClass="Edit" HeaderCssClass="CollapsibleHeader"
		HeaderText="Preferences" DisplayHeader="True">
		<p>
			<label for="Edit_ddlPublished">Always create new items as Published</label> &nbsp;
			<asp:DropDownList id="ddlPublished" runat="server" AutoPostBack="false">
				<asp:ListItem Value="true">Yes</asp:ListItem>
				<asp:ListItem Value="false">No</asp:ListItem>
			</asp:DropDownList></p>
		<p>
			<label for="Edit_ddlExpandAdvanced">Always expand advanced options</label> &nbsp;
			<asp:DropDownList id="ddlExpandAdvanced" runat="server" AutoPostBack="false">
				<asp:ListItem Value="true">Yes</asp:ListItem>
				<asp:ListItem Value="false">No</asp:ListItem>
			</asp:DropDownList></p>
		<p style="MARGIN-TOP: 8px">
			<label class="Block" for="Edit_chkAutoGenerate">
				<SP:HelpToolTip id="Helptooltip4" runat="server" HelpText="If checked, blog posts and articles will have friendly Urls auto-generated based on the title. For example, the title \'My Blog Post\' will become \'MyBlogPost.aspx\'.">
				Auto-Generate Friendly Url
				</sp:HelpToolTip> 
				<asp:CheckBox id="chkAutoGenerate" runat="server"></asp:CheckBox>
			</label>
			
		</p>
		<div style="MARGIN-TOP: 12px">
			<ASP:Button id="lkbUpdate" runat="server" Text="Save" CssClass="buttonSubmit" />
		</div>
		
	</ANW:AdvancedPanel>
</ANW:Page>
