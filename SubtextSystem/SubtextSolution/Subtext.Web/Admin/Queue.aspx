<%@ Page language="c#" Codebehind="Queue.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.Queue" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Queue</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<P>Active Threads:
				<asp:Literal id="Literal1" runat="server"></asp:Literal></P>
			<P>Waiting Callbacks:
				<asp:Literal id="Literal2" runat="server"></asp:Literal></P>
		</form>
	</body>
</HTML>
