<%@ Control Language="C#" EnableTheming="false"  AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.SubtextSearch" %>
<div id="search">
    <div id="search-box">
		<asp:TextBox id="txtSearch" runat="server" cssclass="searchterm" /> <asp:Button id="btnSearch" runat="server" cssclass="searchButton" Text="Search" CausesValidation="False"  OnClick="btnSearch_Click" />
	</div>
</div>