<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="Skins.aspx.cs" Inherits="Subtext.Web.Admin.Skins" %>
<asp:Content ID="Content1" ContentPlaceHolderID="actionsHeading" runat="server">
    <h2>Actions</h2>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="categoryListHeading" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="categoryListLinks" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pageContent" runat="server">
    <script type="text/javascript">
        $(function() {
            $('form').ajaxForm(function() {
            $('.success').css('color', '#eecc00').fadeIn()
                .animate({ opacity: 1.0, color: '#006600' }, 400);
            });

            $('.skinOption').click(function() {
                $('.success').removeClass('success').hide();
                $('.selected').removeClass('selected');
                $(this).addClass('selected').find('input[type=radio]').attr('checked', 'checked');
                $('.current-skin').text($(this).find('label').text());
                $('form').submit();
                $(this).find('div.notice').addClass('success');
            });
        });
    </script>
    
    <div id="skin-selector">
        <h2>
            Current Skin: <span class="current-skin"><%= Blog.Skin.SkinKey %></span>
        </h2>
        <div>
            Choose a skin by clicking on it.
        </div>
        <asp:Repeater ID="skinRepeater" runat="server">
            <ItemTemplate>
                <div class="skinOption<%# EvalSelected(Container.DataItem) %>" style="position: relative;">
                    <div class="notice">
                        Skin Saved
                    </div>
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
