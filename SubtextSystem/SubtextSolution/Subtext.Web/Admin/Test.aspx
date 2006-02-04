<%@ Page language="c#" Codebehind="Test.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.Admin.Test" %>
<%@ Register TagPrefix="ANW" Namespace="Subtext.Web.Admin.WebUI" Assembly="Subtext.Web" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
  <HEAD>
    <title>Test</title>
    <meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" Content="C#">
    <meta name=vs_defaultClientScript content="JavaScript">
    <meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5">
  </HEAD>
  <body >
	
    <form id="Form1" method="post" runat="server">

		<ANW:AdvancedPanel id="Results" runat="server" Collapsible="false" LinkText="[toggle]" HeaderText="Links" HeaderCssClass="CollapsibleHeader" DisplayHeader="True" LinkBeforeHeader="True" LinkImage="~/admin/resources/toggle_gray_up.gif" LinkImageCollapsed="~/admin/resources/toggle_gray_down.gif" LinkStyle="Image">
			<div style="margin: 10px;">
				<label>Test</label><br />
				<asp:TextBox id="txbTest1" runat="server"></asp:TextBox><br />
				<asp:TextBox id="txbTest2" runat="server"></asp:TextBox><br />
				
				<asp:RequiredFieldValidator id=RequiredFieldValidator1 runat="server" ForeColor="#990066" ErrorMessage="Field 1 is required!" Font-Bold="true" ControlToValidate=txbTest1></asp:RequiredFieldValidator><br />
				<asp:RequiredFieldValidator id="Requiredfieldvalidator2" runat="server" ForeColor="#990066" ErrorMessage="Field 2 is required!" Font-Bold="true" ControlToValidate=txbTest2></asp:RequiredFieldValidator><br />
				<asp:CompareValidator id=CompareValidator1 runat="server" runat="server" ForeColor="#990066" ErrorMessage="Fields don't match, sucker!" Font-Bold="true"  ControlToCompare="txbTest2" ControlToValidate="txbTest1"></asp:CompareValidator>
				<br /><br />
				<asp:Button id="btnSubmit" runat="server" Text="Go" width="70"></asp:Button>
			<div>			
		</ANW:AdvancedPanel>	
		
		<p>
			<asp:Label id="lblOutput" runat="server"></asp:Label>
		</p>
     </form>
	
  </body>
</HTML>
