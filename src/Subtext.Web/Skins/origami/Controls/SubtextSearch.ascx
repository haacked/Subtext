<%@ Control Language="C#" EnableTheming="false"  AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.SubtextSearch" %>
<div id="searchWrapper">
	<div id="search">
		<asp:TextBox id="txtSearch" runat="server" class="searchterm" /> <asp:Button id="btnSearch" runat="server" class="searchButton" Text="Search" CausesValidation="False"  OnClick="btnSearch_Click" />
	</div>
</div>