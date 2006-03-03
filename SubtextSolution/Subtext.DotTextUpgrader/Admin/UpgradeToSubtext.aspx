<%@ Page language="c#" Codebehind="UpgradeToSubtext.aspx.cs" AutoEventWireup="false" Inherits="Subtext.DotTextUpgrader.Admin.UpgradeToSubtext" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>.TEXT to Subtext Upgrade Wizard</title>
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			
		<asp:Panel id="pnlStep1" Runat="server" Visible="True">
			<asp:Panel id="pnlFoundConnectionString" Visible="False" runat="server">
				<p>
					<label>Found Connection String:</label>
					<asp:Label ID="lblConnectionString" Runat="server" />
				</p>
				<p>
					<label>Database Name:</label>
					<asp:Label ID="lblDatabaseName" Runat="server" />
				</p>
				<p>
				The .TEXT to Subtext upgrade is ready to begin.  Click &#8220;Upgrade!&#8221; 
				to start the upgrade process.  The upgrader will take the following steps.
				</p>
				<ul>
					<li>Backup existing skins and content files.</li>
					<li>Create new Subtext tables and stored procedures (This will NOT affect existing .TEXT tables and stored procs).</li>
					<li>Unzip and deploy content files.</li>
					<li>Overwrite the web.config</li>
				</ul>
			</asp:Panel>
			
			<asp:Panel ID="pnlConnectionStringNotFound" Visible="False" Runat="server">
				Could not find the connection string. Please enter it here: 
				<asp:TextBox ID="txtConnectionString" Runat="server" />
				<asp:Button ID="btnConfirmString" Text="Next" Runat="server" />
			</asp:Panel>
			
			<p>
				<asp:Button ID="btnNext" Text="Next" Runat="server" />
			</p>
		</asp:Panel>
		
		<asp:Panel ID="pnlStep2" Visible="False" Runat="server">
			Upgrading...
		</asp:Panel>
		</form>
	</body>
</html>
