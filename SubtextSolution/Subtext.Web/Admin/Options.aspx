<%@ Page language="c#" Title="Subtext Admin - Blog Options" MasterPageFile="~/Admin/WebUI/AdminPageTemplate.Master" Codebehind="Options.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.Admin.Pages.AdminOptionsPage" %>
<%@ Register TagPrefix="st" Namespace="Subtext.Web.Admin.WebUI" Assembly="Subtext.Web" %>
<asp:Content ID="actions" ContentPlaceHolderID="actionsHeading" runat="server">
</asp:Content>

<asp:Content ID="categoryListTitle" ContentPlaceHolderID="categoryListHeading" runat="server">
</asp:Content>

<asp:Content ID="categoriesLinkListing" ContentPlaceHolderID="categoryListLinks" runat="server">
</asp:Content>

<asp:Content ID="entryEditor" ContentPlaceHolderID="pageContent" runat="server">
    <st:Page id="PageContainer" TabSectionID="Options" runat="server">
	    <st:AdvancedPanel id="Results" runat="server" Collapsible="False" HeaderText="Options" HeaderCssClass="CollapsibleHeader"
		    DisplayHeader="true" BodyCssClass="Edit">
		    <br />
		    <p>
			    <a href="Configure.aspx">Configure</a>: Manage your blog.
		    </p>
		    <p>
			    <a href="Syndication.aspx">Syndication</a>: Manage your RSS (or ATOM) Feed.
		    </p>
		    <p>
			    <a href="Comments.aspx">Comments</a>: Manage comment and trackback settings.
		    </p>
		    <p>
			    <a href="EditKeyWords.aspx">Key Words</a>: Auto transform specific words/patterns to links.
		    </p>
		    <p>
			    <a href="Password.aspx">Password</a>: Update your password.
		    </p>		
		    <p>
			    <a href="Preferences.aspx">Preferences</a>: Set common preferences.
		    </p>
		    <br class="clear" />
	    </st:AdvancedPanel>
    </st:Page>
</asp:Content>