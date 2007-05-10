<%@ Page Language="C#" MasterPageFile="~/SystemMessages/SystemMessageTemplate.Master" AutoEventWireup="true" CodeBehind="AccessDenied.aspx.cs" Inherits="Subtext.Web.SystemMessages.AccessDenied" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MPTitle" runat="server">Access Denied</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MPSubTitle" runat="server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Content" runat="server">
	Sorry, but you do not have the proper permissions 
	to access that page.
</asp:Content>
