<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.MyLinks" %>
<span id="myLinks">
<asp:HyperLink Runat="server" NavigateUrl="~/Default.aspx" Text="Home" ID="HomeLink" Title="Home" /> | 
<asp:HyperLink Runat="server" NavigateUrl="~/Archives.aspx" Text="Archives" ID="Archives" Title="Archives"></asp:HyperLink> |
<asp:HyperLink AccessKey="9" Runat="server" NavigateUrl="~/Contact.aspx" Text="Contact" ID="ContactLink" Title="Contact" /> | 
<span id="loginMenuLink"><asp:HyperLink Runat="server" Text="Admin" ID="Admin" Title="Admin" /> | </span>
<a href="/Docs/PhillipHaack_Resume.html" title="Resume">Resume</a>

<!-- Not Visible -->
<asp:HyperLink Runat="server" NavigateUrl="~/Rss.aspx" ID="XMLLink" Visible="False" Title="RSS Feed"></asp:HyperLink>
<asp:HyperLink Runat="server" NavigateUrl="~/Rss.aspx" Text="Syndication" ID="Syndication" Title="Syndication" Visible="False" />
</span>