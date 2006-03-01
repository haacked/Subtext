<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<%@ Page language="c#" Codebehind="ImportComplete.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.HostAdmin.ImportComplete" %>
<MP:MasterPage id="MPContainer" TemplateFile="~/HostAdmin/PageTemplate.ascx" runat="server">
	<MP:ContentRegion id="MPTitle" runat="server">Subtext - Import Complete!</MP:ContentRegion>
	<MP:ContentRegion id="MPSectionTitle" runat="server">Import Complete!</MP:ContentRegion>
	<h2>Good News! The import is complete.</h2>
	<p>
		<strong><a id="lnkHostAdmin" href="~/HostAdmin/" runat="server">Click here</a></strong> to visit 
		the Host Admin tool. You might need to reset the password for the blog (or 
		blogs) since the password format may not match Subtext’s format.
	</p>
	<h2>Configuration</h2>
	<p>
		Also, you may need to change the value of the &#8220;Subfolder&#8221; 
		property of your blog.  Once you are in the HostAdmin, click on the 
		edit link next to your blog and take a look at the Url Preview to make 
		sure that is the proper URL you wish for accessing your blog.
	</p>
	<p>
		For more information, see the <a href="http://subtextproject.com/Docs/Configuration/" title="Configuration Guide" rel="external">configuration guide</a> 
		in the <a href="http://subtextproject.com/" title="Subtext Project Website" rel="external">Subtext Project site</a>.
	</p>
</MP:MasterPage>
