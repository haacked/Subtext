<%@ Page language="c#" Codebehind="FileNotFound.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.SystemMessages.FileNotFound" %>
<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<MP:MasterPage id="MPContainer" TemplateFile="~/SystemMessages/PageTemplate.ascx" runat="server">
	<MP:ContentRegion id="MPTitleBar" runat="server">File Not Found (404).</MP:ContentRegion>
	<MP:ContentRegion id="MPTitle" runat="server">Missing Page Report</MP:ContentRegion>
	<MP:ContentRegion id="MPSubTitle" runat="server">Details.</MP:ContentRegion>
		<p>
			Sorry, that page you requested cannot be found.  
			If the page does not show up within 24 hours, you 
			can file a missing page report.
		</p>
</MP:MasterPage>