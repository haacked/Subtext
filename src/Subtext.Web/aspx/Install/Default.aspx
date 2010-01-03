<%@ Page Language="C#" EnableTheming="false"  Title="Subtext Installation: Welcome" MasterPageFile="~/aspx/Install/InstallTemplate.Master" Codebehind="Default.aspx.cs" AutoEventWireup="True" Inherits="Subtext.Web.Install.Default" %>

<asp:Content ID="mainContent" ContentPlaceHolderID="Content" runat="server">
	<fieldset>
	    <legend>Install Database</legend>
	    <ol>
		    <li><strong>Step 1: Install Database</strong></li>
		    <li>Step 2: Configure the Admin Account</li>
		    <li>Step 3: Create A Blog</li>
	    </ol>
		<p class="emphasis">
		    Database: <strong><asp:Literal id="litDatabaseName" Runat="server" /></strong>
		</p>
		<p> 
		    If this is not correct, please correct the connection string in web.config and then refresh this page.
		</p>

		<p>
		    Make sure that the database user specified in the connection string temporarily  
		    has <strong>db owner rights to the database</strong>.  After the installation 
		    process, you should remove the db owner rights.
		</p>
	    <p><asp:Button id="btnInstallClick" runat="server" text="Install Now!" OnClick="OnInstallClick" CssClass="big-button" /></p>
	    <p class="footnote">
	        * Please note that this version of Subtext only supports Microsoft SQL Server 2005 and above (including Express editions).
	    </p>
	</fieldset>
</asp:Content>