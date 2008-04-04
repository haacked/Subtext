<%@ Control Language="C#" EnableTheming="false"  AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.Contact" %>
<P>Please&nbsp;use the form below if you have any comments, questions, or 
	suggestions.</P>
<table cellSpacing="1" cellPadding="1" border="0">
	<tr>
		<td colspan="2">
			Name<asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" ErrorMessage="Please enter your name"
				ControlToValidate="tbEmail" Display="Dynamic"> *</asp:RequiredFieldValidator><br />
			<asp:TextBox id="tbName" CssClass="textbox" Size="50" runat="server" Width="400px"></asp:TextBox></td>
	</tr>
	<tr>
		<td colspan="2">
			Email<asp:RequiredFieldValidator id="emailRequiredValidator" runat="server" ErrorMessage="Please enter your email address" ControlToValidate="tbEmail" Display="Dynamic"> *</asp:RequiredFieldValidator>
					<asp:RegularExpressionValidator id="vldEmailRegex" runat="server" ErrorMessage="The email address you've entered does not appear to be valid." ControlToValidate="tbEmail" Display="Dynamic" ValidationExpression="^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$">*</asp:RegularExpressionValidator>
		<br />
			<asp:TextBox id="tbEmail" CssClass="textbox" runat="server" Size="50" Width="400px"></asp:TextBox></td>
	</tr>
	<tr>
		<td colspan="2">Subject<br />
			<asp:TextBox id="tbSubject" CssClass="textbox" runat="server" Size="50" Width="400px"></asp:TextBox></td>
	</tr>
	<tr>
		<td colspan="2">Message
			<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ErrorMessage="Please enter a message"
				ControlToValidate="tbMessage" Display="Dynamic">*</asp:RequiredFieldValidator><br />
			<asp:TextBox id="tbMessage" CssClass="textbox" runat="server" Rows="10" Columns="40" Width="400px"
				TextMode="MultiLine" Height="131px"></asp:TextBox></td>
	</tr>
	<tr>
		<td valign="top">
			<asp:Button id="btnSend" CssClass="Button" runat="server" Text="Send"></asp:Button></td>
		<td>
			<asp:Label id="lblMessage" runat="server" ForeColor="Red"></asp:Label>
			<asp:ValidationSummary id="ValidationSummary1" runat="server" HeaderText="There is an error:"></asp:ValidationSummary></td>
	</tr>
</table>