<%@ Page Language="C#" EnableTheming="false"  Title="Subtext Installation: Installation Complete" MasterPageFile="~/aspx/Install/InstallTemplate.Master" Codebehind="InstallationComplete.aspx.cs" AutoEventWireup="True" Inherits="Subtext.Web.Install.InstallationComplete" %>

<asp:Content ID="mainContent" ContentPlaceHolderID="Content" runat="server">
	<p>Congratulations. This Subtext Installation is complete.</p>
	<p id="paraBlogLink" runat="server">
		<a id="lnkBlog" href="" runat="server" title="Blog">Visit</a> your blog.
	</p>
	<p id="paraBlogAdminLink" runat="server">
		<a id="lnkBlogAdmin" href="" runat="server" title="Blog Admin">Visit</a> your blog&#8217;s admin.
	</p>
	<p>
		<a id="lnkHostAdmin" href="~/HostAdmin/Default.aspx" runat="server" title="Host Admin Tool">Visit</a> the Host Admin tool.
	</p>
	<p id="paraBlogmlImport" runat="server">
		If you need to import data from another blogging engine (using <a id="lnkBlogMl" href="" title="BlogML Import Tool" runat="server">BlogML</a>), try 
		Import Wizard in your admin section.
	</p>
</asp:Content>