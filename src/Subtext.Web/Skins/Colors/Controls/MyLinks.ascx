<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.MyLinks" %>
<ul id="nav">
	<li><st:NavigationLink ActiveCssClass="active" Runat="server" CssClass="listitem" NavigateUrl="~/Default.aspx" Text="Home" ID="HomeLink" /></li>
	<li><st:NavigationLink ActiveCssClass="active" AccessKey = "9" Runat="server" CssClass="listitem" NavigateUrl="~/Contact.aspx" Text="Contact" ID="ContactLink" /></li>
	<li><asp:HyperLink Runat="server" CssClass="listitem" Text="Syndication" ID="Syndication" /></li>
	<li><asp:HyperLink Runat="server" CssClass="listitem" Text="Admin" ID="Admin" /></li>
</ul>