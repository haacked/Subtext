<%@ Control Language="C#" EnableTheming="false" Inherits="Subtext.Web.UI.Controls.MyLinks" %>
<h3>My Links</h3>
<ul>
	<li>
		<asp:HyperLink Runat="server" Text="Home" ID="HomeLink" />
    </li>
	<li>
		<asp:HyperLink AccessKey="9" Runat="server" Text="Contact" ID="ContactLink" />
	</li>
	<li>
		<asp:HyperLink Runat="server" ID="Syndication">RSS Feed</asp:HyperLink>
	</li>
	<li>
		<asp:HyperLink Runat="server" Text="Admin" ID="Admin" />
	</li>
</ul>
