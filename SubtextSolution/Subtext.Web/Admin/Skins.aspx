<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/WebUI/AdminPageTemplate.Master" AutoEventWireup="true" CodeBehind="Skins.aspx.cs" Inherits="Subtext.Web.Admin.Skins" %>
<asp:Content ID="Content1" ContentPlaceHolderID="actionsHeading" runat="server">
    <h2>Actions</h2>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="categoryListHeading" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="categoryListLinks" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pageContent" runat="server">
    <st:MessagePanel ID="Messages" runat="server" />
    <div id="skin-selector">
        <h2>Select a Skin <asp:Button ID="saveButton" runat="server" OnClick="OnSaveSkinClicked" Text="Save Skin" /></h2>
        <asp:Repeater ID="skinRepeater" runat="server">
            <ItemTemplate>
                <div class="skinOption<%#EvalSelected(Container.DataItem) %>" onclick="$('#<%# GetSkinClientId(Container.DataItem) %>').attr('checked', 'checked');">
                    <img src="<%# GetSkinIconImage(Container.DataItem) %>" />
                    <div class="skin-selection">
                        <input type="radio" 
                            id="<%# GetSkinClientId(Container.DataItem) %>" 
                            name="SkinKey" 
                            <%# EvalChecked(Container.DataItem) %>
                            value="<%# EvalSkin(Container.DataItem).SkinKey %>" /> 
                        <label for="<%# GetSkinClientId(Container.DataItem) %>"><%# EvalSkin(Container.DataItem).Name %></label>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
        <hr />
        <h2>Select a Mobile Skin</h2>
        <asp:Repeater ID="mobileSkinRepeater" runat="server">
            <ItemTemplate>
                <div class="skinOption<%#EvalSelected(Container.DataItem) %>" onclick="$('#<%# GetSkinClientId(Container.DataItem) %>').attr('checked', 'checked');">
                    <img src="<%# GetSkinIconImage(Container.DataItem) %>" />
                    <div class="skin-selection">
                        <input type="radio" 
                            id="<%# GetSkinClientId(Container.DataItem) %>" 
                            name="MobileSkinKey" 
                            <%# EvalChecked(Container.DataItem) %>
                            value="<%# EvalSkin(Container.DataItem).SkinKey %>" /> 
                        <label for="<%# GetSkinClientId(Container.DataItem) %>"><%# EvalSkin(Container.DataItem).Name %></label>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    
</asp:Content>
