<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.MyLinks" %>

<ul id="menu" class="horizontal">
	<li><st:NavigationLink Runat="server" Text="Home" ID="HomeLink" ActiveCssClass="active" /></li>
	<li><st:NavigationLink Runat="server" Text="Archives" ID="Archives" title="Archives" ActiveCssClass="active" /></li>
	<li><st:NavigationLink Runat="server" Text="Contact" ID="ContactLink" ActiveCssClass="active" /></li>
	<li><st:NavigationLink Runat="server" Text="Syndication" ID="Syndication" /></li>
	<li><st:NavigationLink Runat="server" Text="Admin" ID="Admin" /></li>
</ul>