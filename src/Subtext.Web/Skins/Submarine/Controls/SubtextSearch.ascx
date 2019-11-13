<%@ Control Language="C#" EnableTheming="false"  AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.SubtextSearch" %>
<div id="searchBox">
<asp:TextBox id="txtSearch" runat="server" class="searchInput"></asp:TextBox>
<asp:Button id="btnSearch" runat="server"  class="searchButton" CausesValidation="False"  OnClick="btnSearch_Click"></asp:Button>
</div>