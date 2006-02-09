<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.MyLinks" %>
<div id="links">
	<asp:HyperLink Runat="server" ToolTip="Home" ImageUrl="~/Images/home.gif" ID="HomeLink" NavigateUrl="~/Default.aspx" Visible="True" />
	<asp:HyperLink Runat="server" ToolTip="Contact" ImageUrl="~/Images/contact.gif" ID="ContactLink" NavigateUrl="~/Contact.aspx" AccessKey="9" />
	<asp:HyperLink Runat="server" ToolTip="Login" ID="Admin" ImageUrl="~/Images/admin.gif" />
	<asp:HyperLink Runat="server" ToolTip="Subscribe RSS" ID="Syndication" ImageUrl="~/Images/rss20icon.gif" NavigateUrl="~/Rss.aspx" />
</div>

<!-- Not Visible -->

<asp:HyperLink Runat="server" NavigateUrl="~/Archives.aspx" Text="Archives" ID="Archives" Visible="False"></asp:HyperLink>
<asp:HyperLink Runat="server" NavigateUrl="~/Rss.aspx" ID="XMLLink" Visible="False"></asp:HyperLink>

