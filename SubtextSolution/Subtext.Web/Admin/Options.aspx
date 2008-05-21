<%@ Page Language="C#" EnableTheming="false"  Title="Subtext Admin - Blog Options" MasterPageFile="~/Admin/WebUI/AdminPageTemplate.Master" Codebehind="Options.aspx.cs" AutoEventWireup="True" Inherits="Subtext.Web.Admin.Pages.AdminOptionsPage" %>


<asp:Content ID="actions" ContentPlaceHolderID="actionsHeading" runat="server">
    <h2>Actions</h2>
</asp:Content>

<asp:Content ID="categoryListTitle" ContentPlaceHolderID="categoryListHeading" runat="server" />

<asp:Content ID="categoriesLinkListing" ContentPlaceHolderID="categoryListLinks" runat="server" />

<asp:Content ID="entryEditor" ContentPlaceHolderID="pageContent" runat="server">
    <st:AdvancedPanel id="Results" runat="server" Collapsible="False" HeaderText="Options" HeaderCssClass="CollapsibleHeader"
	    DisplayHeader="true" BodyCssClass="Edit">
	    <br />
	    <p>
		    <a href="Configure.aspx">Configure</a>: Manage your blog.
	    </p>
	    <p>
		    <a href="Customize.aspx">Customize</a>: Customize your blog.
	    </p>
	    <p>
		    <a href="Preferences.aspx">Preferences</a>: Set common preferences.
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
		    <a href="Security.aspx">Security</a>: Update your password or change your security options.
	    </p>		
	    <br class="clear" />
    </st:AdvancedPanel>
</asp:Content>