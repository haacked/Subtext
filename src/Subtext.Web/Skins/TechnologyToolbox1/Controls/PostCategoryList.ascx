<%@ Control Language="C#" EnableTheming="false" Inherits="Subtext.Web.UI.Controls.PostCategoryList" %>
<!-- Begin: PostCategoryList.ascx -->
<div class="post-categories">
    Categories:
    <asp:Repeater ID="CatList" runat="server" OnItemCreated="CategoryCreated">
        <HeaderTemplate>
            <ul>
        </HeaderTemplate>
        <ItemTemplate>
            <li><asp:HyperLink runat="server" ID="Link"
                rel="tag" title="View all posts in this category" /></li>
        </ItemTemplate>
        <FooterTemplate>
                <asp:Literal runat="server"
                    Text="<li>No categories assigned to this post.</li>"
                    Visible='<%# CatList.Items.Count == 0 %>' />
            </ul>
        </FooterTemplate>
    </asp:Repeater>
</div>
<!-- End: PostCategoryList.ascx -->
