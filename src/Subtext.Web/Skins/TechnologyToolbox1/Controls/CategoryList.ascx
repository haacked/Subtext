<%@ Control Language="C#" EnableTheming="false" Inherits="Subtext.Web.UI.Controls.CategoryList" %>
<!-- Begin: CategoryList.ascx -->
<div class="posts-categories">
    <h2>
        Categories</h2>
    <asp:Repeater ID="CatList" runat="server" OnItemCreated="CategoryCreated">
        <ItemTemplate>
            <%-- 
                HACK: Subtext.Web.UI.Controls.SingleColumn provides multiple
                "categories" of links during data binding -- e.g. "Archive"
                (posts by month) and "Post Categories" -- but we only want to
                show Post Categories. Therefore, wrap each category in a panel
                and only show the panel containing the Post Categories.
            --%>
            <asp:Panel runat="server"
                Visible='<%# ((string)Eval("Title")) == "Post Categories" %>'>
                <asp:Repeater ID="LinkList" runat="server" OnItemCreated="LinkCreated">
                    <HeaderTemplate>
                        <ul>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <li><asp:HyperLink runat="server" ID="Link" /></li>
                    </ItemTemplate>
                    <FooterTemplate>
                        </ul>
                    </FooterTemplate>
                </asp:Repeater>
            </asp:Panel>
        </ItemTemplate>
    </asp:Repeater>
</div>
<!-- End: CategoryList.ascx -->
