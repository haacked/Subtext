<%@ Control Language="c#" AutoEventWireup="false" Codebehind="ConnectionStringBuilder.ascx.cs" Inherits="Subtext.Web.Install.ConnectionStringBuilder" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>

<script>

function toggleAuth(value) {

alert(value);

}

</script>

<table>
<tr><td>Server Name</td><td><asp:dropdownlist id="machineName" runat="server"></asp:dropdownlist><br><asp:textbox id="otherMachineName" runat="server" textmode="SingleLine"></asp:textbox></td></tr>
<tr><td>Authentication</td>
<td><asp:radiobuttonlist id="authMode" runat="server" autopostback="True">
<asp:ListItem Value="sql">SQL</asp:ListItem>
<asp:ListItem Value="win">Trusted</asp:ListItem>
</asp:radiobuttonlist></td></tr>
  <tr>
    <td>Username</td>
    <td><asp:textbox id="username" runat="server" textmode="SingleLine"></asp:textbox></td></tr>
  <tr>
    <td>Password</td>
    <td><asp:textbox id="password" runat="server" textmode="SingleLine"></asp:textbox></td></tr>
<tr><td>Database</td><td><asp:dropdownlist id="databaseName" runat="server"></asp:dropdownlist><asp:LinkButton id="refreshDatabase" runat="server">Refresh Database</asp:LinkButton></td></tr>
<tr><td colspan="2"><asp:button id="testConnection" runat="server" text="Test Connection"></asp:button><br><asp:label id="connResult" runat="server"></asp:label></td></tr>
</table>
