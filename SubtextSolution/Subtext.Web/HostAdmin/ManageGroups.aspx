<%@ Page Title="Subtext - Host Admin - Manage Groups" MasterPageFile="~/HostAdmin/HostAdminTemplate.Master" EnableViewState="true" %>
<%@ Register TagPrefix="st" TagName="GroupsEditor" Src="~/HostAdmin/UserControls/GroupsEditor.ascx" %>
<asp:Content id="sectionTitle" ContentPlaceHolderID="MPSectionTitle" runat="server">Subtext - Host Admin - Blog Groups</asp:Content>
<asp:Content id="sidebar" ContentPlaceHolderID="MPSideBar" runat="server"></asp:Content>
<asp:Content id="blogList" ContentPlaceHolderID="MPContent" runat="server">
	<st:GroupsEditor id="blogsEditor" runat="server" />
</asp:Content>
