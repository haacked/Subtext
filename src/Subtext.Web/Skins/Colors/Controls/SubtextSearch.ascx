<%@ Control Language="C#" EnableTheming="false"  AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.SubtextSearch" %>
<div id="search">
	<label for="txtSearch">search for term</label> <asp:TextBox id="txtSearch" runat="server" CssClass="searchterm" /> <asp:Button id="btnSearch" runat="server" CssClass="searchButton" Text="GO" CausesValidation="False" OnClick="btnSearch_Click"/>
</div>