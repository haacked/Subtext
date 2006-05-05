<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<%@ Page language="c#" Codebehind="BlogNotConfiguredError.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.BlogNotConfiguredError" %>
<MP:MasterPage id="MPContainer" TemplateFile="~/Install/PageTemplate.ascx" runat="server">
	<MP:ContentRegion id="MPTitle" runat="server">Subtext - Your Blog Has Not Been Configured Yet</MP:ContentRegion>
	<MP:ContentRegion id="MPSubTitle" runat="server">But I Can Help You</MP:ContentRegion>
	<asp:Literal id="ltlMessage" Runat="server"></asp:Literal>
</MP:MasterPage>
