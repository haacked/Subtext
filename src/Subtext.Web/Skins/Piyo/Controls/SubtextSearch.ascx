<%@ Control Language="C#" EnableTheming="false"  AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.SubtextSearch" %>
<div id="search">
	<div id="search-box">
	    <asp:TextBox id="txtSearch" runat="server" CssClass="searchterm" /> <asp:Button id="btnSearch" runat="server" class="searchButton" Text="" CausesValidation="False"  OnClick="btnSearch_Click" />
	</div>
</div>