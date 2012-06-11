<%@ Page Language="C#" Title="Subtext - Upgrade Is Not Complete" CodeBehind="DeprecatedPhysicalPaths.aspx.cs" Inherits="Subtext.Web.pages.SystemMessages.DeprecatedPhysicalPaths" MasterPageFile="~/aspx/SystemMessages/SystemMessageTemplate.Master"  %>
<asp:Content id="titleBar" ContentPlaceHolderID="MPTitle" runat="server">Some Old Files Need To Be Deleted</asp:Content>
<asp:Content id="subtitle" ContentPlaceHolderID="MPSubTitle" runat="server">But I Can Help You</asp:Content>
<asp:Content id="mainContent" ContentPlaceHolderID="Content" runat="server">
	<% if (DeprecatedPaths == null || DeprecatedPaths.Count == 0) { %>
	    <p>
	        Unless you&#8217;re just testing the app, you shouldn&#8217;t see this 
	        page because there is no exception being reported and this page is 
	        for reporting on a specific exception.
	    </p>
	<% } else { %>
	<p>
        While upgrading your Subtext installation, there were some old files left 
        on disk	from a previous upgrade that Subtext could not clean up automatically.
    </p>
    <p>
        The latest version of Subtext moved these files/folders into the "aspx" directory.
    </p>
    <h2>Please delete the following files or folders</h2>
        <table>
            <tr>
                <th>Virtual Path</th>
                <% if (IsAdminOrHostAdmin) { %>
                <th>Physical Path</th>
                <% } %>
            </tr>
        <% foreach (var path in DeprecatedPaths) {%>
            <tr>
                <td><%= path %></td>
                <% if(IsAdminOrHostAdmin) { %>
                <td><%= Server.MapPath(path) %></td>
                <% } %>
        <% } %>
        </table>
        <p>
            Login to see physical paths.
        </p>
    <% } %>
</asp:Content>