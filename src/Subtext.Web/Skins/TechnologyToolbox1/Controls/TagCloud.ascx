<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.TagCloud" %>
<!-- Begin: TagCloud.ascx -->
<div class="posts-tags">
    <h2>Tags</h2>
    <asp:Repeater runat="server" ID="Tags" OnItemDataBound="Tags_ItemDataBound">
        <HeaderTemplate>
            <ul>
        </HeaderTemplate>
        <ItemTemplate>
            <li class='<%# Eval("Weight", "tag-weight-{0}") %>'>
                <asp:HyperLink runat="server" ID="TagUrl"
                    Text='<%# UrlDecode(Eval("TagName")) %>'
                    ToolTip='<%# UrlDecode(Eval("TagName")) + " (" + Eval("Count") + ")" %>' /></li>
        </ItemTemplate>
        <FooterTemplate>
                <asp:Literal runat="server"
                    Text="<li>No tags found on any blog posts.</li>"
                    Visible='<%# Tags.Items.Count == 0 %>' />
                <li><asp:HyperLink runat="server" ID="DefaultTagLink">More...</asp:HyperLink></li>
            </ul>
        </FooterTemplate>
    </asp:Repeater>
</div>
<!-- End: TagCloud.ascx -->
