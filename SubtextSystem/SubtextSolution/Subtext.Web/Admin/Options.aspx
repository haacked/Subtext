<%@ Page language="c#" Codebehind="Options.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.Admin.Pages.AdminOptionsPage" %>
<%@ Register TagPrefix="ANW" Namespace="Subtext.Web.Admin.WebUI" Assembly="Subtext.Web" %>
<ANW:Page id="PageContainer" TabSectionID="Options" runat="server">
	<ANW:AdvancedPanel id="Results" runat="server" Collapsible="False" HeaderText="Options" HeaderCssClass="CollapsibleHeader"
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
		<p>
			<a href="Import/">Import Wizard</a>: Import data from another blog.
		</p>
		<br class="Clear">
	</ANW:AdvancedPanel>
</ANW:Page>
