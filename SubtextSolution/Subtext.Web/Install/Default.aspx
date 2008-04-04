<%@ Page Language="C#" EnableTheming="false"  Title="Subtext Installation: Welcome" MasterPageFile="~/Install/InstallTemplate.Master" Codebehind="Default.aspx.cs" AutoEventWireup="True" Inherits="Subtext.Web.Install.Default" %>
<%@ Import namespace="Subtext.Framework.Configuration"%>

<asp:Content ID="mainContent" ContentPlaceHolderID="Content" runat="server">
	<fieldset>
	    <legend>Install Database</legend>
	    <ol>
		    <li><strong>Step 1: Install Database</strong></li>
		    <li>Step 2: Configure the Admin</li>
		    <li>Step 3: Create or Import a Blog</li>
	    </ol>
		<p>
		Subtext will be installed to the <strong><asp:Literal id="litDatabaseName" Runat="server" /></strong> database. 
		If this is not correct, please update the connection string in web.config and 
		refresh this page when you are done.
		</p>

		<p>
		Make sure that the database user specified in the connection string temporarily  
		has <strong>db owner rights to the database</strong>.  After the installation 
		process, you should remove the db owner rights.
		</p>
	    <p><asp:Button id="btnInstallClick" runat="server" text="Install Now!" OnClick="OnInstallClick" /></p>
	    <p class="footnote">* Please note that the current version of Subtext only supports 
		    Microsoft SQL Server 2000 and above and SQL Server Express 2005. Future versions 
		    of Subtext may add support for other databases and file system storage.
	    </p>
	</fieldset>
</asp:Content>