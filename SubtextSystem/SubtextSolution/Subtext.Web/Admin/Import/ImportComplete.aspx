<%@ Page language="c#" Codebehind="ImportComplete.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.Admin.Import.ImportComplete" %>
<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<MP:MasterPage id="MPContainer" TemplateFile="~/Admin/Import/PageTemplate.ascx" runat="server">
	<MP:ContentRegion id="MPTitle" runat="server">Subtext - Import Complete!</MP:ContentRegion>
	<MP:ContentRegion id="MPSubTitle" runat="server">Import Complete!</MP:ContentRegion>
	<p>Good News! The import is complete.</p>
	<p>
		Click <A id="lnkHostAdmin" href="~/HostAdmin/" runat="server">here</A> to visit 
		the Host Admin tool.  You might need to reset the password for the blog (or blogs) since 
		the password format may not match Subtext&#8217;s format.
	</p>
</MP:MasterPage>
