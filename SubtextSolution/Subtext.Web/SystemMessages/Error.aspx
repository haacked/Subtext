<%@ Page language="c#" EnableViewState="False" Codebehind="Error.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.Pages.Error" %>
<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<MP:MasterPage id="MPContainer" TemplateFile="~/SystemMessages/PageTemplate.ascx" runat="server">
	<MP:ContentRegion id="MPTitleBar" runat="server">Application Error!</MP:ContentRegion>
	<MP:ContentRegion id="MPTitle" runat="server">Application Error!</MP:ContentRegion>
	<MP:ContentRegion id="MPSubTitle" runat="server">Details.</MP:ContentRegion>
		<p>
			<asp:Label id="ErrorMessageLabel" runat="server" />
		</p>
		<p style="MARGIN-TOP: 24px">
			<asp:HyperLink id="HomeLink" runat="server">Return to site</asp:HyperLink>
		</p>
</MP:MasterPage>
