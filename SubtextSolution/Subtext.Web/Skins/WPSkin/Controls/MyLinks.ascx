<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.MyLinks" %>
<h3>My Links</h3>
<ul>
	<li><asp:HyperLink Runat="server"  NavigateUrl="~/Default.aspx" Text="Home" ID="HomeLink" /></li>
	<li><asp:HyperLink AccessKey = "9" Runat="server" Text="Contact" ID="ContactLink" /></li>
	<li><asp:HyperLink Runat="server" Text="Subscribe" ID="Syndication" /></li>
	<li><asp:HyperLink Runat="server" Text="Admin" ID="Admin" /></li>
</ul>