<%@ Page Language="C#" EnableTheming="false"  Title="Subtext - Your Blog Has Not Been Configured Yet" MasterPageFile="~/aspx/SystemMessages/SystemMessageTemplate.Master" Codebehind="BlogNotConfiguredError.aspx.cs" AutoEventWireup="True" Inherits="Subtext.Web.BlogNotConfiguredError" EnableViewState="false" %>

<asp:Content ContentPlaceHolderID="MPTitle" runat="server">
    Blog Not Found
</asp:Content>

<asp:Content ContentPlaceHolderID="MPSubTitle" runat="server">
    We tried, but we could not find the blog you&#8217;re looking for.
</asp:Content>


<asp:Content ID="mainContent" ContentPlaceHolderID="Content" runat="server">
    <asp:Literal id="ltlMessage" Runat="server"></asp:Literal>
</asp:Content>