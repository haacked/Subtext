<%@ Control Language="c#" AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.PostComment" %>
<div id="commentform">
<h3>Post Comment</h3>
	<table cellspacing="1" cellpadding="1" border="0">
		<tr>
			<td>Title</td>
			<td>
				<asp:TextBox id="tbTitle" runat="server" size="40" width="300px" CssClass="Textbox"></asp:TextBox>
			</td>
			<td>
				<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ErrorMessage="Please enter a title"
					ControlToValidate="tbTitle"></asp:RequiredFieldValidator>
			</td>
		</tr>
		<tr>
			<td>Name</td>
			<td>
				<asp:TextBox id="tbName" runat="server" size="40" width="300px" CssClass="Textbox"></asp:TextBox>
			</td>
			<td>
				<asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" ErrorMessage="Please enter your name"
					ControlToValidate="tbName"></asp:RequiredFieldValidator>
			</td>
		</tr>
		<tr>
			<td>Url</td>
			<td>
				<asp:TextBox id="tbUrl" runat="server" size="40" width="300px" CssClass="Textbox"></asp:TextBox>
			</td>
			<td></td>
		</tr>
		<tr>
			<td colspan="3">Comment&nbsp;
				<asp:RequiredFieldValidator id="RequiredFieldValidator3" runat="server" ErrorMessage="Please enter a comment"
					ControlToValidate="tbComment"></asp:RequiredFieldValidator><br />
				<asp:TextBox id="tbComment" runat="server" Rows="10" Columns="40" width="100%" Height="193px"
					TextMode="MultiLine"></asp:TextBox>
			</td>
		</tr>
		<tr>
			<td colspan="3">
				<asp:CheckBox id="chkRemember" runat="server" Text="Remember Me?" Checked="True"></asp:CheckBox>
			</td>
		</tr>
		<tr>
			<td>
				<asp:Button id="btnSubmit" CssClass="Button" runat="server" Text="Submit"></asp:Button>
			</td>
			<td colspan="2">
				<asp:Label id="Message" runat="server" ForeColor="Red"></asp:Label>
			</td>
		</tr>
	</table>
</div>
