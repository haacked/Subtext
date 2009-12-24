<%@ Page Language="C#" EnableTheming="false"  Title="Subtext Admin - Blog Options" Codebehind="Options.aspx.cs" AutoEventWireup="True" Inherits="Subtext.Web.Admin.Pages.AdminOptionsPage" %>

<asp:Content ContentPlaceHolderID="actionsHeading" runat="server">
    <h2>Options</h2>
</asp:Content>

<asp:Content ContentPlaceHolderID="categoryListHeading" runat="server" />

<asp:Content ContentPlaceHolderID="categoryListLinks" runat="server" />

<asp:Content ContentPlaceHolderID="pageContent" runat="server">
    <h2>Options</h2>
    <dl id="admin-options">
	    <dt><a href="Configure.aspx" title="Configure your blog">Configure</a></dt>
	    <dd>Manage your blog</dd>
        
        <dt><a href="Skins.aspx" title="Choose a Skin">Skins</a></dt>
	    <dd>Choose a Skin</dd>

	    <dt><a href="Comments.aspx" title="Comment Settings">Comments</a></dt>
	    <dd>Manage comment and trackback settings</dd>
	    
	    <dt><a href="Syndication.aspx" title="Edit syndication settings">Syndication</a></dt>
	    <dd>Manage your RSS (or ATOM) Feed</dd>
	   
	   	<dt><a href="Security.aspx" title="Control security">Security</a></dt>
	    <dd>Update your password or change your security options</dd>
	   
	    <dt><a href="EditKeyWords.aspx" title="Manage keywords">Key Words</a></dt>
	    <dd>Auto transform specific words/patterns to links</dd>
	   	
        <dt><a href="Customize.aspx" title="Customize your blog's Meta Tags">Meta Tags</a></dt>
	    <dd>Customize the meta tags for your blog</dd>
	   
	    <dt><a href="Preferences.aspx" title="Manage Preferences">Preferences</a></dt>
	    <dd>Set common preferences</dd>
	        
	    <dt><a href="ImportExport.aspx" title="Import/Export">Import/Export</a></dt>
	    <dd>It&#8217;s your data. Import from BlogML or Export to BlogML</dd>
	    
	    <dt><a href="FullTextSearch.aspx" title="FullTextSearch">FullText Search</a></dt>
	    <dd>Manage the FullText search of your blog</dd>
    </dl>
</asp:Content>