<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.Header" %>
<div id="header">
	<div id="title">
		<h1><asp:HyperLink id="HeaderTitle" CssClass="headermaintitle" runat="server" /></h1>
		<h2><asp:Literal id="HeaderSubTitle" runat="server" /></h2>
	</div>
	<div id="search">
		<label for="txtSearch">search for term</label> <input type="text" class="searchterm" /> <input type="submit" class="searchButton" value="GO" />
	</div>
</div>