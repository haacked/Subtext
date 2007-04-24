<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.MyLinks" %>
<div class="links">
<div>
<h5>About</h5>
<ul>
<li><asp:HyperLink AccessKey="9" Runat="server" NavigateUrl="~/Contact.aspx" Text="Contact" ID="ContactLink" CssClass="rounded"/></li>
<li><asp:HyperLink Runat="server" Text="Admin" ID="Admin" CssClass="rounded"/></li>
</ul>
</div>
</div>

<!-- Not Visible -->
<asp:HyperLink Runat="server" NavigateUrl="~/Default.aspx" Text="Home" ID="HomeLink" Visible="False" />
<asp:HyperLink Runat="server" NavigateUrl="~/Archives.aspx" Text="Archives" ID="Archives" Visible="False"></asp:HyperLink>
<asp:HyperLink Runat="server" ID="XMLLink" Visible="False"></asp:HyperLink>
<asp:HyperLink Runat="server" Text="Syndication" ID="Syndication" Visible="False" />
