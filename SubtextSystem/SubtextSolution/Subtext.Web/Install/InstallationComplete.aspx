<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<%@ Page language="c#" Codebehind="InstallationComplete.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.Install.InstallationComplete" %>
<MP:MasterPage id="MPContainer" TemplateFile="~/Install/PageTemplate.ascx" runat="server">
	<MP:ContentRegion id="MPTitle" runat="server">Subtext Installation: Installation Complete</MP:ContentRegion>
	<MP:ContentRegion id="MPSubTitle" runat="server">Installation Is Complete</MP:ContentRegion>
	<p>Congratulations. This Subtext Installation is complete.</p>
	<p>
		Click <A id="lnkHostAdmin" href="~/HostAdmin/" runat="server">here</A> to visit 
		the Host Admin tool.
	</p>
</MP:MasterPage>
