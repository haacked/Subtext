<%@ Register TagPrefix="ANW" Namespace="Subtext.Web.Admin.WebUI" Assembly="Subtext.Web" %>
<%@ Page language="c#" Codebehind="Confirm.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.Admin.Pages.Confirm" %>
<ANW:Page id="PageContainer" TabSectionID="Posts" runat="server">
	<ANW:MessagePanel id="Messages" runat="server" MessageCssClass="MessagePanel" MessageIconUrl="~/admin/resources/ico_info.gif" ErrorCssClass="ErrorPanel" ErrorIconUrl="~/admin/resources/ico_critical.gif"/>
	<ANW:AdvancedPanel id="Header" runat="server" DisplayHeader="true" CssClass="Dialog" HeaderCssClass="DialogTitle" BodyCssClass="DialogBody" HeaderText="Confirm Action" LinkText="[toggle list]"> 
		<ASP:Label id="lblOutput" runat="server" />
		<div style="margin-top: 12px;">
			<ASP:Button id="lkbContinue" runat="server" text="Continue" visible="false" CssClass="buttonSubmit" />
			<ASP:Button id="lkbYes" runat="server" Text="Yes" CssClass="buttonSubmit" />
			<ASP:Button id="lkbNo" runat="server" Text="No" CssClass="buttonSubmit" />
			<BR>&nbsp;
		</div>
	</ANW:AdvancedPanel>
	
</ANW:Page>
