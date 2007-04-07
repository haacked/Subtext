<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.MyLinks" %>
<%@ Register TagPrefix="st" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>

<ul id="menu" class="horizontal">
	<li><st:NavigationLink Runat="server"  NavigateUrl="~/Default.aspx" Text="Home" ID="HomeLink" ActiveCssClass="active" /></li>
	<li><st:NavigationLink Runat="server" NavigateUrl="~/Archives.aspx" Text="Archives" ID="Archives" title="Archives" ActiveCssClass="active" /></li>
	<li><st:NavigationLink Runat="server" NavigateUrl="~/Contact.aspx" Text="Contact" ID="ContactLink" ActiveCssClass="active" /></li>
	<li><st:NavigationLink Runat="server" Text="Syndication" ID="Syndication" /></li>
	<li><st:NavigationLink Runat="server" Text="Admin" ID="Admin" /></li>
</ul>