<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.TagCloud" %>
<%@ Import Namespace = "Subtext.Framework" %>
<div class="sidebar">
    <div class="side-bar-top">
    </div>
    <div class="side-bar-middle">
        <h2>
            Tag Cloud</h2>
        <asp:Repeater runat="server" ID="Tags" OnItemDataBound="Tags_ItemDataBound">
            <HeaderTemplate>
                <ul id="tag-cloud">
            </HeaderTemplate>
            <ItemTemplate>
                <li>
                    <asp:HyperLink runat="server" ID="TagUrl" CssClass='<%# Eval("Weight", "tag-style-{0} tag-item") %>'
                        Text='<%# UrlDecode(Eval("TagName")) %>' ToolTip='<%# UrlDecode(Eval("TagName")) + " (" + Eval("Count") + ")" %>' />
                </li>
            </ItemTemplate>
            <FooterTemplate>
                </ul>
            </FooterTemplate>
        </asp:Repeater>
        <asp:HyperLink runat="server" ID="DefaultTagLink">more tags...</asp:HyperLink>
    </div>
    <div class="side-bar-bottom">
    </div>
</div>
