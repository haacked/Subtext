<%@ Page Title="Subtext - Host Admin - Installed Blogs" MasterPageFile="~/HostAdmin/HostAdminTemplate.Master" EnableViewState="true" CodeBehind="Default.aspx.cs" Inherits="Subtext.Web.HostAdmin.Default"  %>
<%@ Register TagPrefix="st" TagName="BlogsList" Src="~/HostAdmin/UserControls/BlogsList.ascx" %>
<asp:Content id="sectionTitle" ContentPlaceHolderID="MPSectionTitle" runat="server">Subtext - Host Admin - Installed Blogs</asp:Content>
<asp:Content id="blogList" ContentPlaceHolderID="MPContent" runat="server">
	<st:BlogsList id="blogsList" runat="server" />
</asp:Content>