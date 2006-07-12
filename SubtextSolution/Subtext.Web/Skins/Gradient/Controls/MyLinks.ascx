<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.MyLinks" %>
<ul class="navlist">
    <li><asp:HyperLink Runat="server" NavigateUrl="~/Default.aspx" Text="Home" ID="HyperLink1" title="Home" /></li>
    <li><asp:HyperLink Runat="server" NavigateUrl="~/Archives.aspx" Text="Archives" ID="HyperLink2" title="Archives"></asp:HyperLink></li>
    <li><asp:HyperLink AccessKey="9" Runat="server" NavigateUrl="~/Contact.aspx" Text="Contact" ID="HyperLink3" title="Contact" /></li>
    <li><asp:HyperLink Runat="server" Text="Admin" ID="Admin" title="Admin" /></li>
</ul>

<!-- Not Visible -->
<asp:HyperLink Runat="server" NavigateUrl="~/Rss.aspx" ID="XMLLink" Visible="False" title="RSS Feed"></asp:HyperLink>
<asp:HyperLink Runat="server" NavigateUrl="~/Rss.aspx" Text="Syndication" ID="Syndication" title="Syndication" Visible="False" />