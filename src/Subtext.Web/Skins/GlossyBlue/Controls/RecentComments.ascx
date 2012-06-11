<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.RecentComments" %>

<div class="footer-recent-comments">
    <h4>Recent Comments</h4>
    <asp:Repeater ID="feedList" runat="server" OnItemCreated="EntryCreated">
        <HeaderTemplate>
            <ul>
        </HeaderTemplate>
        <ItemTemplate>
            <li><strong><asp:HyperLink runat="server" ID="Link" /></strong>: <asp:Literal runat="server" ID="Author" /></li>
        </ItemTemplate>
        <FooterTemplate>
            </ul>
        </FooterTemplate>
    </asp:Repeater>
</div>
