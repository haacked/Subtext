<%@ Control Language="C#" EnableTheming="false" AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.RecentComments" %>
<!-- Begin: RecentComments.ascx -->
<div class="comments-recent">
    <h2>
        Recent Comments</h2>
    <asp:Repeater ID="feedList" runat="server" OnItemCreated="EntryCreated">
        <HeaderTemplate>
            <ul>
        </HeaderTemplate>
        <ItemTemplate>
            <li>
                <asp:Literal runat="server" ID="Author" />
                said
                <br />
                <asp:HyperLink runat="server" ID="Link" /></li>
        </ItemTemplate>
        <FooterTemplate>
            <asp:Literal runat="server" Text="<li>There are no recent comments.</li>"
                Visible='<%# feedList.Items.Count == 0 %>' />
            </ul>
        </FooterTemplate>
    </asp:Repeater>
</div>
<!-- End: RecentComments.ascx -->
