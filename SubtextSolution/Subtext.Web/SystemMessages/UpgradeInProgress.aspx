<%@ Page language="c#" Codebehind="UpgradeInProgress.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.UpgradeInProgress" %>
<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<MP:MasterPage id="MPContainer" TemplateFile="~/SystemMessages/PageTemplate.ascx" runat="server">
	<MP:ContentRegion id="MPTitleBar" runat="server">Upgrade in Progress</MP:ContentRegion>
	<MP:ContentRegion id="MPTitle" runat="server">This Blog is being upgraded.</MP:ContentRegion>
	<MP:ContentRegion id="MPSubTitle" runat="server">Please be patient.</MP:ContentRegion>
	<asp:PlaceHolder id="plcUpgradeInProgressMessage" runat="server" Visible="false">
		<p>
			This blog is in the middle of an upgrade. Please be patient and it will 
			be back shortly. 
		</p>
		<p>
			If you are the Host Admin, please 
			<a href="~/HostAdmin/Upgrade/" id="hostAdminLink" runat="server">click here</a> 
			to complete the upgrade.
		</p>
	</asp:PlaceHolder>
	
	<asp:PlaceHolder id="plcNothingToSeeHere" runat="server" Visible="true">
		<p>
			Hey there, I think you made it to this page by mistake.  
			This <a id="lnkBlog" runat="server">blog seems</a> to be leading an active and healthy life.
		</p>
	</asp:PlaceHolder>
</MP:MasterPage>