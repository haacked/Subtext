<%@ Control Language="C#" EnableTheming="false"  AutoEventWireup="true" CodeBehind="CategoryLinkList.ascx.cs" Inherits="Subtext.Web.Admin.UserControls.CategoryLinkList" %>

<asp:Repeater ID="rptCategories" runat="server">
    <HeaderTemplate>
        <ul id="LinksCategories" class="categoriesList">        
    </HeaderTemplate>
    <ItemTemplate>
        <li><a href="<%# DataBinder.Eval(Container.DataItem, "Url")%>" title="category"><%# Eval("Title")%></a></li>
    </ItemTemplate>
    <FooterTemplate>
        </ul>        
    </FooterTemplate>
</asp:Repeater>
