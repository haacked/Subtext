<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.MyLinks" %>
<span id="myLinks">
<asp:HyperLink Runat="server" NavigateUrl="~/Default.aspx" Text="Home" ID="HomeLink" title="Home" /> | 
<asp:HyperLink Runat="server" NavigateUrl="~/Archives.aspx" Text="Archives" ID="Archives" title="Archives"></asp:HyperLink> |
<asp:HyperLink AccessKey="9" Runat="server" NavigateUrl="~/Contact.aspx" Text="Contact" ID="ContactLink" title="Contact" /> | 
<span id="loginMenuLink"><asp:HyperLink Runat="server" Text="Admin" ID="Admin" title="Admin" /> | </span>
<a href="/Docs/PhillipHaack_Resume.html" title="Resume">Resume</a>

<!-- Not Visible -->
<asp:HyperLink Runat="server" NavigateUrl="~/Rss.aspx" ID="XMLLink" Visible="False" title="RSS Feed"></asp:HyperLink>
<asp:HyperLink Runat="server" NavigateUrl="~/Rss.aspx" Text="Syndication" ID="Syndication" title="Syndication" Visible="False" />
</span>