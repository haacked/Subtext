<%@ Page language="c#" Codebehind="StringConnectionControlTestPage.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.Install.StringConnectionControlTestPage" %>
<%@ Register TagPrefix="UC" TagName="ConnString" Src="ConnectionStringBuilder.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" > 

<html>
  <head>
    <title>StringConnectionControlTestPage</title>
    <meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" Content="C#">
    <meta name=vs_defaultClientScript content="JavaScript">
    <meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5">
  </head>
  <body MS_POSITIONING="GridLayout">
	
    <form id="Form1" method="post" runat="server">
    <table>
		<tr><th>Connection String</th></tr>
		<tr><td><UC:ConnString id="connStr" runat="server" ConnStr="Server=localhost;Database=SubtextData;User ID=sa;Password=password"></uc:connstring></td></tr>
    </table>
     </form>
	
  </body>
</html>
