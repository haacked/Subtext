<%@ Page language="c#" Title="Subtext Admin - Preferences" MasterPageFile="~/Admin/WebUI/AdminPageTemplate.Master" Codebehind="Preferences.aspx.cs" AutoEventWireup="True" Inherits="Subtext.Web.Admin.Pages.EditPreferences" %>
<%@ Register TagPrefix="st" Namespace="Subtext.Web.Admin.WebUI" Assembly="Subtext.Web" %>
<%@ Register TagPrefix="st" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>

<asp:Content ID="actions" ContentPlaceHolderID="actionsHeading" runat="server">
</asp:Content>

<asp:Content ID="categoryListTitle" ContentPlaceHolderID="categoryListHeading" runat="server">
</asp:Content>

<asp:Content ID="categoriesLinkListing" ContentPlaceHolderID="categoryListLinks" runat="server">
</asp:Content>

<asp:Content ID="preferencesContent" ContentPlaceHolderID="pageContent" runat="server">
	<st:AdvancedPanel id="Edit" runat="server" BodyCssClass="Edit" HeaderCssClass="CollapsibleHeader"
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
				<st:HelpToolTip id="Helptooltip4" runat="server" HelpText="If checked, blog posts and articles will have friendly Urls auto-generated based on the title. For example, the title \'My Blog Post\' will become \'MyBlogPost.aspx\'.">
				Auto-Generate Friendly Url
				</st:HelpToolTip> 
				<asp:CheckBox id="chkAutoGenerate" runat="server"></asp:CheckBox>
			</label>
			
		</p>
		<div style="MARGIN-TOP: 12px">
			<ASP:Button id="lkbUpdate" runat="server" Text="Save" CssClass="buttonSubmit" onclick="lkbUpdate_Click" />
		</div>
		
	</st:AdvancedPanel>
</asp:Content>