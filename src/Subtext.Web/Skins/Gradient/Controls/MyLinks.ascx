<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.MyLinks" %>
<ul>
    <li><st:NavigationLink ActiveCssClass="active" Runat="server" NavigateUrl="~/Default.aspx" Text="Home" ID="HomeLink" title="Home" /></li>
    <li><st:NavigationLink ActiveCssClass="active" Runat="server" NavigateUrl="~/Archives.aspx" Text="Archives" ID="Archives" title="Archives" /></li>
    <li><st:NavigationLink ActiveCssClass="active" AccessKey="9" Runat="server" NavigateUrl="~/Contact.aspx" Text="Contact" ID="ContactLink" title="Contact" /></li>
    <li><asp:HyperLink Runat="server" Text="Admin" ID="Admin" title="Admin" /></li>
</ul>
