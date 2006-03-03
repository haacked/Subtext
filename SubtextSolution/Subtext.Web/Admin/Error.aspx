<%@ Page language="c#" Codebehind="Error.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.Admin.Pages.Error" %>
<%@ Register TagPrefix="ANW" Namespace="Subtext.Web.Admin.WebUI" Assembly="Subtext.Web" %>
<ANW:Page id="PageContainer" runat="server">
	<asp:Panel id="ErrorPanel" runat="server">
		<p style="font-weight: bold; font-size: 150%;">Problem!</p>
		<p>There was an error processing your request.</p>
		<p>Please click&nbsp;<asp:HyperLink id="HomeLink" runat="server">Here</asp:HyperLink>&nbsp;to return.</p>
		<div style="padding: 4px; border: 1px solid #CCC; background: #EEE; color: inherit; margin-top: 24px; margin-bottom: 48px;">
			<p style="font-weight: bold; margin: 0;">The specific error message encountered was as follows:</p>
			<p ><asp:Label id="ErrorMessageLabel" runat="server" /></p>
		</div>
	</asp:Panel>	
</ANW:Page>
