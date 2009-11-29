<%@ Control Language="C#" EnableTheming="false"  AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.Contact" %>
<P>Use this form to contact me.
	<BR>
	All correspondence is considered public unless otherwise noted.</P>
<table cellSpacing="1" cellPadding="1" border="0" class="contactForm">
	<tr>
		<td><strong>Name</strong></td>
		<td>
			<asp:TextBox id="tbName" Size="50" runat="server" Width="300px" />
			<asp:RequiredFieldValidator id="RequiredFieldValidator3" runat="server" ErrorMessage="Please enter your name"
				ControlToValidate="tbName" Display="Dynamic">*</asp:RequiredFieldValidator>
		</td>
	</tr>
	<tr>
		<td><strong>Email:</strong></td>
		<td>
			<asp:TextBox id="tbEmail" runat="server" Size="50" Width="300px"></asp:TextBox>
			<asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" ErrorMessage="Please enter your email address"
				ControlToValidate="tbEmail" Display="Dynamic">*</asp:RequiredFieldValidator>
			<asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server" ErrorMessage="Invalid email address format"
				ControlToValidate="tbEmail" Display="Dynamic" ValidationExpression="^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$">*</asp:RegularExpressionValidator></td>
	</tr>
	<tr>
		<td><strong>Subject:</strong></td>
		<td>
			<asp:TextBox id="tbSubject" runat="server" Size="50" Width="300px"></asp:TextBox></td>
	</tr>
	<tr>
		<td><strong>Message</strong></td>
		<td>
			<asp:TextBox id="tbMessage" runat="server" Rows="10" Columns="40" Width="300px" TextMode="MultiLine"
				Height="131px"></asp:TextBox>
			<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ErrorMessage="Please tell me something"
				ControlToValidate="tbMessage" Display="Dynamic">*</asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td></td>
		<td align="center">
			<asp:Button id="btnSend" runat="server" Text="Send"></asp:Button><BR>
			<asp:Label id="lblMessage" runat="server" ForeColor="Red"></asp:Label>
			<asp:ValidationSummary id="ValidationSummary1" runat="server" HeaderText="There is an error:"></asp:ValidationSummary></td>
	</tr>
</table>
