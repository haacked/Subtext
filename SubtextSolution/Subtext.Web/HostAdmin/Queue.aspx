<%@ Page language="c#" Codebehind="Queue.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.HostAdmin.Queue" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
	<head>
		<title>Queue</title>
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<p>Active Threads:
				<asp:Literal id="Literal1" runat="server"></asp:Literal>
			</p>
			<p>Waiting Callbacks:
				<asp:Literal id="Literal2" runat="server"></asp:Literal>
			</p>
		</form>
	</body>
</html>
