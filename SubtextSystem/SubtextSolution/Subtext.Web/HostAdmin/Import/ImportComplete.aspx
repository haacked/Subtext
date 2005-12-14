<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<%@ Page language="c#" Codebehind="ImportComplete.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.HostAdmin.ImportComplete" %>
<MP:MasterPage id="MPContainer" TemplateFile="~/HostAdmin/PageTemplate.ascx" runat="server">
	<MP:ContentRegion id="MPTitle" runat="server">Subtext - Import Complete!</MP:ContentRegion>
	<MP:ContentRegion id="MPSectionTitle" runat="server">Import Complete!</MP:ContentRegion>
	<P>Good News! The import is complete.</P>
	<P>Click <A id="lnkHostAdmin" href="~/HostAdmin/" runat="server">here</A> to visit 
		the Host Admin tool. You might need to reset the password for the blog (or 
		blogs) since the password format may not match Subtext’s format.
	</P>
</MP:MasterPage>
