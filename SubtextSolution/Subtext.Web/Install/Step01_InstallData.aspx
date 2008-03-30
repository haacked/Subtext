<%@ Page Language="C#" EnableTheming="false"  Title="Subtext Installation: Step 2 - Data Installation" MasterPageFile="~/Install/InstallTemplate.Master" Codebehind="Step01_InstallData.aspx.cs" AutoEventWireup="True" Inherits="Subtext.Web.Install.Step01_InstallData" %>

<asp:Content id="subTitleContent" ContentPlaceHolderID="MPSubTitle" runat="server">Step 2 - Data Installation</asp:Content>
<asp:Content ID="mainContent" ContentPlaceHolderID="Content" runat="server">
	<ol>
		<li>Gather Installation Information</li>
		<li><strong>Install the database</strong></li>
		<li>Configure the Host Admin</li>
		<li>Create a Blog</li>
	</ol>
	<div class="bordered">
		<p>
			You are now ready to install the Subtext database.
		</p>
		<p>
			If you haven&#8217;t already, <strong>don&#8217;t forget to give the database 
			user owner rights to the database</strong>.  You can remove those rights after 
			this step.
		</p>
		<p><asp:Literal id="installationStateMessage" Runat="server"></asp:Literal></p>
		<p>
			<asp:CheckBox id="chkFullInstallation" runat="Server" Text="Full Install" />
		</p>
		<p>
			Click &#8220;Install Now!&#8221; to continue.
		</p>
		<p>
			<asp:Button id="btnInstall" runat="server" Text="Install Now!"></asp:Button>
		</p>
	</div>
</asp:Content>
