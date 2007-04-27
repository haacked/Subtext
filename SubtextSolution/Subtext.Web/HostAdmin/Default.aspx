<%@ Page Title="Subtext - Host Admin - Installed Blogs" MasterPageFile="~/HostAdmin/HostAdminTemplate.Master" EnableViewState="true" %>
<%@ Register TagPrefix="st" TagName="BlogsEditor" Src="~/HostAdmin/UserControls/BlogsEditor.ascx" %>
<asp:Content id="sectionTitle" ContentPlaceHolderID="MPSectionTitle" runat="server">Subtext - Host Admin - Installed Blogs</asp:Content>
<asp:Content id="blogList" ContentPlaceHolderID="MPContent" runat="server">
	<div id="blogs-editor">
		<st:BlogsEditor id="blogsEditor" runat="server" />
	</div>
</asp:Content>