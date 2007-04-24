<%@ Page Language="C#" EnableTheming="false"  Title="Subtext Admin - Preferences" MasterPageFile="~/Admin/WebUI/AdminPageTemplate.Master" Codebehind="Preferences.aspx.cs" AutoEventWireup="True" Inherits="Subtext.Web.Admin.Pages.EditPreferences" %>

<asp:Content ID="actions" ContentPlaceHolderID="actionsHeading" runat="server">
</asp:Content>

<asp:Content ID="categoryListTitle" ContentPlaceHolderID="categoryListHeading" runat="server">
</asp:Content>

<asp:Content ID="categoriesLinkListing" ContentPlaceHolderID="categoryListLinks" runat="server">
</asp:Content>

<asp:Content ID="preferencesContent" ContentPlaceHolderID="pageContent" runat="server">
	<fieldset>
		<legend>Preferences</legend>
		
		<label for="Edit_ddlPublished">Always create new items as Published</label>&nbsp;
		<asp:DropDownList id="ddlPublished" runat="server" AutoPostBack="false">
			<asp:ListItem Value="true">Yes</asp:ListItem>
			<asp:ListItem Value="false">No</asp:ListItem>
		</asp:DropDownList>
	
		<label for="Edit_ddlExpandAdvanced">Always expand advanced options</label>&nbsp;
		<asp:DropDownList id="ddlExpandAdvanced" runat="server" AutoPostBack="false">
			<asp:ListItem Value="true">Yes</asp:ListItem>
			<asp:ListItem Value="false">No</asp:ListItem>
		</asp:DropDownList>
	
		<div>
			<asp:CheckBox id="chkAutoGenerate" runat="server" CssClass="checkbox" Text="Auto-Generate Friendly URL" />
			<st:HelpToolTip id="Helptooltip4" runat="server" HelpText="If checked, blog posts and articles will have friendly Urls auto-generated based on the title. For example, the title 'My Blog Post' will become 'MyBlogPost.aspx'.">
				<img id="Img2" src="~/Admin/Resources/Scripts/Images/ms_information_small.gif" runat="Server" />
			</st:HelpToolTip> 
		</div>

		<div class="buttons">
			<ASP:Button id="lkbUpdate" runat="server" Text="Save" CssClass="button" onclick="lkbUpdate_Click" />
		</div>
	</fieldset>
	
</asp:Content>