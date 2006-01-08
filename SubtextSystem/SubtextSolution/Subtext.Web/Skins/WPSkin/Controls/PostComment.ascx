<%@ Control Language="c#" AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.PostComment" %>
<div id="commentform">
<table cellSpacing="1" cellPadding="1"  border="0" >
	<tr>
		<td width="75">Title</td>
		<td>
			<asp:TextBox id="tbTitle" runat="server" Size = "40" Width="300px"></asp:TextBox></td>
		<td>
			<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ErrorMessage="Please enter a title" ControlToValidate="tbTitle"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td width="75">Name</td>
		<td>
			<asp:TextBox id="tbName" runat="server" Size = "40" Width="300px"></asp:TextBox></td>
		<td>
			<asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" ErrorMessage="Please enter your name" ControlToValidate="tbName"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td>Url</td>
		<td>
			<asp:TextBox id="tbUrl" runat="server" Size = "40" Width="300px"></asp:TextBox></td>
		<td></td>
	</tr>
	<tr>
		<td colSpan="3">Comments&nbsp;
			<asp:RequiredFieldValidator id="RequiredFieldValidator3" runat="server" ErrorMessage="Please enter a comment" ControlToValidate="tbComment"></asp:RequiredFieldValidator><BR>
			<asp:TextBox id="tbComment" runat="server" Rows = "10" Columns = "50" Width="400px" Height="193px" TextMode="MultiLine"></asp:TextBox></td>
	</tr>
	<tr>
		<td colSpan="3">
			<asp:CheckBox id="chkRemember" runat="server" Text="Remember Me?"></asp:CheckBox></td>
	</tr>
	<tr>
		<td>
			<asp:Button id="btnSubmit" runat="server" Text="Submit"></asp:Button></td>
		<td colspan="2">
			<asp:Label id="Message" runat="server" ForeColor="Red"></asp:Label></td>
	</tr>
</table>
</div>