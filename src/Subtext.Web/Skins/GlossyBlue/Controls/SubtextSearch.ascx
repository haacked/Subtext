<%@ Control Language="c#" AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.SubtextSearch" %>
<div id="searchBox">
<asp:TextBox id="txtSearch" runat="server" class="sText"></asp:TextBox>
<asp:Button id="btnSearch" runat="server"  class="sButton" CausesValidation="False"  OnClick="btnSearch_Click"></asp:Button>
</div>