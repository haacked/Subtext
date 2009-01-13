<%@ Control Language="C#" EnableTheming="false" Inherits="Subtext.Web.UI.Controls.MyLinks" %>
<div>
	<h5>Syndication:</h5>
	<p><asp:HyperLink Runat="server" Text="RSS" ID="Syndication" CssClass="rounded" /></p>
</div>

<!-- Not Visible -->
<asp:HyperLink Runat="server" NavigateUrl="~/Default.aspx" Text="Home" ID="HomeLink" Visible="False" />
<asp:HyperLink Runat="server" NavigateUrl="~/Archives.aspx" Text="Archives" ID="Archives" Visible="False" />
<asp:HyperLink Runat="server" Text="Admin" ID="Admin"  Visible="False" />
<asp:HyperLink AccessKey="9" Runat="server" NavigateUrl="~/Contact.aspx" Text="Contact" ID="ContactLink" Visible="False"/>
