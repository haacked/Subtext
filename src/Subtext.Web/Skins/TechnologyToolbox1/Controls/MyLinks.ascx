<%@ Control Language="C#" EnableTheming="false" Inherits="Subtext.Web.UI.Controls.MyLinks" %>
<!-- Begin: MyLinks.ascx -->
<ul class="menu" id="pagebar">
    <li class="page_item current_page_item">
        <asp:HyperLink CssClass="Home" runat="server" NavigateUrl="~/Default.aspx" Text="Home"
            ID="HomeLink" /></li>
    <li class="page_item">
        <asp:HyperLink CssClass="archives" runat="server" NavigateUrl="~/Archives.aspx" Text="Archives"
            ID="Archives" Visible="False" /></li>
    <li class="page_item">
        <asp:HyperLink CssClass="Contact" runat="server" NavigateUrl="~/Contact.aspx" Text="Contact"
            ID="ContactLink" /></li>
    <li class="page_item">
        <asp:HyperLink CssClass="Syndication" runat="server" Text="Subscribe" ID="Syndication" /></li>
</ul>
<div id="grad">
</div>
<!-- End: MyLinks.ascx -->