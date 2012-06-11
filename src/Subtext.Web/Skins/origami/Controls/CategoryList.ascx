<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.CategoryList" %>

<asp:Repeater ID="CatList" runat="server" OnItemCreated="CategoryCreated">
    <ItemTemplate>
        <div class="sidebar-node"><h3 class="archives-heading"><asp:Literal ID="Title" runat="server" /></h3>
        <asp:Repeater ID="LinkList" runat="server" OnItemCreated="LinkCreated">
            <HeaderTemplate><ul class="archives-list"></HeaderTemplate>
            <ItemTemplate><li><asp:HyperLink runat="server" ID="Link" /></li></ItemTemplate>
            <FooterTemplate></ul></div></FooterTemplate>
        </asp:Repeater>
    </ItemTemplate>
</asp:Repeater>