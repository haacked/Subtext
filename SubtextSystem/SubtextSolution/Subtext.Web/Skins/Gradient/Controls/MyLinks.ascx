<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.MyLinks" %>
<div id="navbar">
<ul id="nav">
	<li>
		<span><asp:HyperLink Runat="server" NavigateUrl="~/Default.aspx" Text="Home" ID="HomeLink" title="Home" /></span>
	</li>
	<li>
		<span><asp:HyperLink Runat="server" NavigateUrl="~/Archives.aspx" Text="Archives" ID="Archives" title="Archives"></asp:HyperLink></span>
	</li>
	<li>
		<span><asp:HyperLink AccessKey="9" Runat="server" NavigateUrl="~/Contact.aspx" Text="Contact" ID="ContactLink" title="Contact" /></span>
	</li>
	<li>
		<span id="loginMenuLink"><asp:HyperLink Runat="server" Text="Admin" ID="Admin" title="Admin" /></span>
	</li>
</ul>
</div>
<!-- Not Visible -->
<asp:HyperLink Runat="server" NavigateUrl="~/Rss.aspx" ID="XMLLink" Visible="False" title="RSS Feed"></asp:HyperLink>
<asp:HyperLink Runat="server" NavigateUrl="~/Rss.aspx" Text="Syndication" ID="Syndication" title="Syndication" Visible="False" />