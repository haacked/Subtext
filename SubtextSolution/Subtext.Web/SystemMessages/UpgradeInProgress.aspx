<%@ Page Language="C#" EnableTheming="false"  Title="Subtext - Upgrade In Progress" MasterPageFile="~/SystemMessages/SystemMessageTemplate.Master" Codebehind="UpgradeInProgress.aspx.cs" AutoEventWireup="True" Inherits="Subtext.Web.UpgradeInProgress" %>
<asp:Content id="titleBar" ContentPlaceHolderID="MPTitle" runat="server">This Blog is being upgraded.</asp:Content>
<asp:Content id="subtitle" ContentPlaceHolderID="MPSubTitle" runat="server">Please be patient.</asp:Content>
<asp:Content id="mainContent" ContentPlaceHolderID="Content" runat="server">
	<asp:PlaceHolder id="plcUpgradeInProgressMessage" runat="server" Visible="false">
		<p>
			<strong>This blog is in the middle of an upgrade. Please be patient and it will 
			be back shortly.</strong>
		</p>
		<p>
			If you are the host administrator of this installation, and the upgrade 
			does not complete within five minutes, check the contents of the <code>subtext_Log</code> 
			table for error messages regarding the upgrade.
		</p>
	</asp:PlaceHolder>
	
	<asp:PlaceHolder id="plcNothingToSeeHere" runat="server" Visible="true">
		<p>
			Hey there, I think you made it to this page by mistake.  
			This <a id="lnkBlog" runat="server">blog seems</a> to be leading an active and healthy life.
		</p>
	</asp:PlaceHolder>
</asp:Content>