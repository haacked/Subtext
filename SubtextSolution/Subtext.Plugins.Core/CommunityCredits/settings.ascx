<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="settings.ascx.cs" Inherits="Subtext.Plugins.Core.CommunityCredits.settings" %>
<asp:PlaceHolder ID="overridableConfiguration" runat="server" Visible="false">
<p>
	Leave that textbox empty to use the general site configuration(Code = "<asp:Literal ID="globalAffiliateCode" runat="server" />", Key = "<asp:Literal ID="globalAffiliateKey" runat="server" />"):
</p>
<p>
	<label class="Block">Personal AffiliateCode</label>
	<asp:TextBox id="affiliateCode" runat="server" CssClass="textinput"></asp:TextBox>
</p>
<p>
	<label class="Block">Personal AffiliateKey</label>
	<asp:TextBox id="affiliateKey" runat="server" CssClass="textinput"></asp:TextBox>
</p>
<p>
	Currently using the following data:<br />
	Code = "<asp:Literal ID="effectiveAffiliateCode" runat="server" />", Key = "<asp:Literal ID="effectiveAffiliateKey" runat="server" />"
</p>
</asp:PlaceHolder>
<asp:PlaceHolder ID="notOverridableConfiguration" runat="server" Visible="false">
<p>
	You cannot change the configuration for this plugin.<br />
	If you think you should please contact the blog administrator
</p>
</asp:PlaceHolder>