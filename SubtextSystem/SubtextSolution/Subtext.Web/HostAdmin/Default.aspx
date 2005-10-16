<%@ Page EnableViewState="true" %>
<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<%@ Register TagPrefix="BLE" TagName="BlogsEditor" Src="~/HostAdmin/UserControls/BlogsEditor.ascx" %>
<MP:MasterPage id="MPContainer" runat="server">
	<MP:ContentRegion id="MPTitle" runat="server">Subtext - Host Admin - Installed Blogs</MP:ContentRegion>
	<MP:ContentRegion id="MPSideBar" runat="server"></MP:ContentRegion>
	<BLE:BlogsEditor id="blogsEditor" runat="server" />
</MP:MasterPage>
