<%@ Page Language="C#" EnableTheming="false"  Title="Subtext Installation: Welcome" MasterPageFile="~/Install/InstallTemplate.Master" Codebehind="Default.aspx.cs" AutoEventWireup="True" Inherits="Subtext.Web.Install.Default" %>

<asp:Content id="subTitleContent" ContentPlaceHolderID="MPSubTitle" runat="server">Welcome</asp:Content>
<asp:Content ID="mainContent" ContentPlaceHolderID="Content" runat="server">
	<p>Welcome to the Subtext Installation Wizard.</p>
	<p>Here are the following steps you will take...</p>
	<ol>
		<li><strong>Confirm Installation Information</strong></li>
		<li>Install the database</li>
		<li>Configure the Host Admin</li>
		<li>Create or Import a Blog</li>
	</ol>
	<div>
		<p>
		Make sure that the database user specified by the connection string in 
		web.config has <strong>db owner rights to the database</strong>.  After the installation 
		process, you can remove the db owner rights.
		</p>
		<p>
		According to the connection string, Subtext will be installed on the 
		<strong><asp:Literal id="litDatabaseName" Runat="server"></asp:Literal></strong> database. 
		If this is correct, click &#8220;On to Step 2&#8221; to continue.
		</p>
		<p>
		If it is not correct, update the connection string within web.config and 
		refresh this page when you are done.
		</p>
	</div>
	<p><asp:Button id="btnNext" runat="server" text="On to Step 2"></asp:Button></p>
	<p class="footnote">* Please note that the current version of Subtext only supports 
		Microsoft SQL Server 2000 and above. Future versions of Subtext may add support 
		for other databases and file system storage.
	</p>
</asp:Content>