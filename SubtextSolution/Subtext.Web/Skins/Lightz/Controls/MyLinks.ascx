<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.MyLinks" %>
<h1>My Links</h1>
<ul class="list">
	<li class="listitem"><asp:HyperLink Runat="server" CssClass="listitem" NavigateUrl="~/Default.aspx" Text="Home" ID="HomeLink" /></li>
	<li class="listitem"><asp:HyperLink Runat="server" CssClass="listitem" Text="Archives" ID="Archives" /></li>
	<li class="listitem"><asp:HyperLink Runat="server" CssClass="listitem" Text="Contact" ID="ContactLink" /></li>
	<li class="listitem"><asp:HyperLink Runat="server" CssClass="listitem" ImageUrl="~/Skins/Lightz/images/xml.gif" Text="Syndication" ID="Syndication" />
	<li class="listitem"><asp:HyperLink Runat="server" CssClass="listitem" Text="Admin" ID="Admin" /></li>
</ul>