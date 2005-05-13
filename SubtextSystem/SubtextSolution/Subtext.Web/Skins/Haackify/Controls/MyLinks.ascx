<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.MyLinks" %>
<span id="myLinks">
<asp:HyperLink Runat="server" NavigateUrl="~/Default.aspx" Text="Home" ID="HomeLink" /> | 
<asp:HyperLink Runat="server" NavigateUrl="~/Archives.aspx" Text="Archives" ID="Archives"></asp:HyperLink> |
<asp:HyperLink AccessKey="9" Runat="server" NavigateUrl="~/Contact.aspx" Text="Contact" ID="ContactLink" /> | 
<asp:HyperLink Runat="server" Text="Admin" ID="Admin" />

<!-- Not Visible -->
<asp:HyperLink Runat="server" NavigateUrl="~/Rss.aspx" ID="XMLLink" Visible="False"></asp:HyperLink>
<asp:HyperLink Runat="server" NavigateUrl="~/Rss.aspx" Text="Syndication" ID="Syndication" Visible="False" />
</span>