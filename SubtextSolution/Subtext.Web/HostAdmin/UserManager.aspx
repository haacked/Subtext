<%@ Page Language="C#" MasterPageFile="~/HostAdmin/HostAdminTemplate.Master" AutoEventWireup="true" CodeBehind="UserManager.aspx.cs" Inherits="Subtext.Web.HostAdmin.UserManager" Title="Untitled Page" %>
<%@ Register TagPrefix="st" TagName="UserManager" Src="~/Admin/UserControls/UserManager.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MPSectionTitle" runat="server">
User Manager
</asp:Content>

<asp:Content ID="mainContent" ContentPlaceHolderID="MPContent" runat="server">	
	<st:UserManager id="userManager" runat="server" />
</asp:Content>