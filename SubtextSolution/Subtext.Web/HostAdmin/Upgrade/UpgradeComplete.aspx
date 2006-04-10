<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<%@ Page language="c#" Codebehind="UpgradeComplete.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.HostAdmin.Upgrade.UpgradeComplete" %>
<MP:MasterPage id="MPContainer" runat="server">
	<MP:ContentRegion id="MPTitle" runat="server">Subtext Upgrade: Upgrade Complete</MP:ContentRegion>
	<MP:ContentRegion id="MPSideBar" runat="server"></MP:ContentRegion>
	<p>Congratulations. The Subtext upgrade is complete.</p>
	<p>
		Click <a id="lnkHostAdmin" href="~/HostAdmin/" runat="server">here</a> to visit 
		the Host Admin tool.
	</p>
</MP:MasterPage>
