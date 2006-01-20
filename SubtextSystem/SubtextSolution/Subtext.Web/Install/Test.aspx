<%@ Page language="c#" Codebehind="Test.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.Install.Test"%>
<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" > 

<html>
  <head>
    <title>Test</title>
    <meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" Content="C#">
    <meta name=vs_defaultClientScript content="JavaScript">
    <meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5">
  </head>
  <body MS_POSITIONING="GridLayout">
	
    <form id="Form1" method="post" runat="server">
<MP:ConnectionStringBuilder id="pippo" runat="server" AllowWebConfigOverride="true" Title="Connection String" Description = "A SQL Connection String with the rights to create SQL Database objects such as Stored Procedures, Table, and Views."></MP:ConnectionStringBuilder><br>
<asp:Button id="invia" text="leggi connection string" runat="server"></asp:Button>
<br> 
<asp:Label id="conn" runat="server"></asp:Label>
     </form>
	
  </body>
</html>
