<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.MyLinks" %>
<h3>My Links</h3>
<ul>
	<li>
		<asp:HyperLink Runat="server" NavigateUrl="~/Default.aspx" Text="Home" ID="HomeLink" />
	<li>
		<asp:HyperLink AccessKey="9" Runat="server" NavigateUrl="~/Contact.aspx" Text="Contact" ID="ContactLink" />
	<li>
		<asp:HyperLink ImageUrl="~/images/xml.gif" Runat="server" ID="XMLLink">RSS 2.0 Feed</asp:HyperLink><asp:HyperLink Runat="server" Text="Syndication" ID="Syndication" Visible="False" />
	<li>
		<asp:HyperLink Runat="server" Text="Admin" ID="Admin" /></li>
</ul>
