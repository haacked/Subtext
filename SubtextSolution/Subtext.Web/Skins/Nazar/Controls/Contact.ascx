<%@ Control Language="c#" AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.Contact" %>

<div id="contact">
	<P align="center">Please use the form below if you have any comments, questions, or suggestions.
		Your e-mail will be answered as soon as possible. Please don't use HTML.</P>
	<table cellSpacing="1" cellPadding="1" border="0">
		<tr>
			<td>
				Name:<br />
				<asp:TextBox id="tbName" Size = "50" runat="server" class="contact"></asp:TextBox>
			</td>
		</tr>
		<tr>
			<td>
				Email:<br />
				<asp:TextBox id="tbEmail" runat="server" Size = "50" class="contact"></asp:TextBox>
				<asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" ErrorMessage="Please enter your email address" ControlToValidate="tbEmail" Display="Dynamic">*</asp:RequiredFieldValidator>
				<asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server" ErrorMessage="Invalid email address format" ControlToValidate="tbEmail" Display="Dynamic" ValidationExpression="^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$">*</asp:RegularExpressionValidator>
			</td>
		</tr>
		<tr>
			<td>
				Subject:<br />
				<asp:TextBox id="tbSubject" runat="server" Size = "50" class="contact"></asp:TextBox>
			</td>
		</tr>
		<tr>
			<td>
				Message:<br />
				<asp:TextBox id="tbMessage" runat="server" Rows = "10" Columns = "40"  TextMode="MultiLine" Height="131px" class="contact"></asp:TextBox>
				<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ErrorMessage="Please tell me something" ControlToValidate="tbMessage" Display="Dynamic">*</asp:RequiredFieldValidator></td>
		</tr>
		<tr>
			<td>
				<asp:Button id="btnSend" runat="server" Text="Send" CssClass="button"></asp:Button></td>
			<td>
				<asp:Label id="lblMessage" runat="server" ForeColor="Red"></asp:Label>
				<asp:ValidationSummary id="ValidationSummary1" runat="server" HeaderText="There is an error:"></asp:ValidationSummary></td>
		</tr>
	</table>
</div>