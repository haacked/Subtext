<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.MyLinks" %>
<div id="links">
    <div>
        <h5>About</h5>
        <ul>
            <li><st:NavigationLink ActiveCssClass="active" AccessKey="9" Runat="server" NavigateUrl="~/Contact.aspx" Text="Contact" ID="ContactLink" CssClass="rounded"/></li>
            <li><asp:HyperLink Runat="server" Text="Admin" ID="Admin" CssClass="rounded"/></li>
        </ul>
    </div>
</div>
