<%@ Page Language="C#" EnableTheming="false"  Title="Subtext - Your Blog Has Not Been Configured Yet" MasterPageFile="~/aspx/Install/InstallTemplate.Master" Codebehind="BlogNotConfiguredError.aspx.cs" AutoEventWireup="True" Inherits="Subtext.Web.BlogNotConfiguredError" EnableViewState="false" %>


<asp:Content ID="mainContent" ContentPlaceHolderID="Content" runat="server">
	<asp:Literal id="ltlMessage" Runat="server"></asp:Literal>
</asp:Content>