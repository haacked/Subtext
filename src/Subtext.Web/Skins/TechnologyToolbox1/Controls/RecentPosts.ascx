<%@ Control Language="C#" EnableTheming="false" AutoEventWireup="true" Inherits="Subtext.Web.UI.Controls.RecentPosts" %>
<!-- Begin: RecentPosts.ascx -->
<%-- For simplicity, this control uses the same settings as the RecentComments control in the comments section of the options admin UI --%>
<div class="posts-recent">
    <h2>Recent Posts</h2>
    <asp:Repeater ID="postList" runat="server" OnItemCreated="PostCreated">
        <HeaderTemplate>
            <ul>
        </HeaderTemplate>
        <ItemTemplate>
                <li><asp:HyperLink runat="server" ID="Link" /></li>
        </ItemTemplate>
        <FooterTemplate>
            <asp:Literal runat="server" Text="<li>There are no recent posts.</li>"
                Visible='<%# postList.Items.Count == 0 %>' />
            </ul>
        </FooterTemplate>
    </asp:Repeater>
</div>
<!-- End: RecentPosts.ascx -->
