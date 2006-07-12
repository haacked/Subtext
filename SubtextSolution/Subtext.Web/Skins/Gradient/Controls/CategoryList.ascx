<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.CategoryList" %>
<%@ Import Namespace="Subtext.Framework" %>
<asp:Repeater ID="CatList" runat="server" OnItemCreated="CategoryCreated">
    <ItemTemplate>
        <div class="rbroundbox">
            <div class="rbtop">
                <div>
                </div>
            </div>
            
            <div class="rbcontent">
                <p>
                    <h3>
                        <asp:Literal runat="server" ID="Title" /></h3>
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
                </p>
            </div>
            
            <!-- /rbcontent -->
            <div class="rbbot">
                <div>
                </div>
            </div>
        </div>
        <!-- /rbroundbox -->
    </ItemTemplate>
</asp:Repeater>
