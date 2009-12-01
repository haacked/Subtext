<%@ Control Language="C#" EnableTheming="false" Inherits="Subtext.Web.UI.Controls.CategoryList" %>

<asp:Repeater ID="CatList" runat="server" OnItemCreated="CategoryCreated">
    <ItemTemplate>
		<div>
        <h3><asp:Literal runat="server" ID="Title" /></h3>
        <asp:Repeater ID="LinkList" runat="server" OnItemCreated="LinkCreated">
            <HeaderTemplate>
                <ul class="navlist">
            </HeaderTemplate>
            <ItemTemplate>
                <li>
                    <asp:HyperLink runat="server" ID="Link" /></li>
            </ItemTemplate>
            <FooterTemplate>
                </ul>
            </FooterTemplate>
        </asp:Repeater>
        </div>
    </ItemTemplate>
</asp:Repeater>
