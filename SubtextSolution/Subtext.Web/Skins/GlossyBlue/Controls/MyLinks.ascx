<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.MyLinks" %>
<%@ Register TagPrefix="SP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<div id="menu">
    <ul>
	    <li><asp:HyperLink Runat="server" NavigateUrl="~/Default.aspx" Text="Home" ID="HomeLink" /></li>
	    <li><asp:HyperLink AccessKey="9" Runat="server" NavigateUrl="~/Contact.aspx" Text="Contact" ID="ContactLink" /></li>
	    <li><asp:HyperLink Runat="server" Text="Admin" ID="Admin" /></li>
    </ul>
</div>
<!-- Not Visible --><asp:HyperLink Runat="server" ID="XMLLink" Visible="False"></asp:HyperLink><asp:HyperLink Runat="server" Text="Syndication" ID="Syndication" Visible="False" />