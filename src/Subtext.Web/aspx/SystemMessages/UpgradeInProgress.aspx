<%@ Page Language="C#" EnableTheming="false"  Title="Subtext - Upgrade In Progress" MasterPageFile="~/aspx/SystemMessages/SystemMessageTemplate.Master" Codebehind="UpgradeInProgress.aspx.cs" AutoEventWireup="True" Inherits="Subtext.Web.UpgradeInProgress" %>
<asp:Content id="titleBar" ContentPlaceHolderID="MPTitle" runat="server">This Blog is being upgraded.</asp:Content>
<asp:Content id="subtitle" ContentPlaceHolderID="MPSubTitle" runat="server">Please be patient.</asp:Content>
<asp:Content id="mainContent" ContentPlaceHolderID="Content" runat="server">
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
</asp:Content>