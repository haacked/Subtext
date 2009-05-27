<%@ Control Language="C#" EnableTheming="false"  AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.PostComment" %>
<div class="moreinfo">
<table id="commentform" class="comments" cellSpacing="1" cellPadding="1"  border="0" >
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
			<asp:RequiredFieldValidator id="vldNameRequired" runat="server" ErrorMessage="Please enter your name" ControlToValidate="tbName" Display="Dynamic" EnableClientScript="true" />
	</tr>
	<tr>
		<td>Email (never displayed)</td>
		<td>
			<asp:TextBox id="tbEmail" runat="server" Size="40" Width="300px" />
		</td>
		<td>
			<asp:RegularExpressionValidator ID="vldEmail" runat="server" ControlToValidate="tbEmail" ValidationExpression="^.*?@.+\..+$" Display="dynamic" ErrorMessage="Email is not required, but it must be valid if specified." EnableClientScript="true" />
		</td>
	</tr>
	<tr>
		<td>Url</td>
		<td>
			<asp:TextBox id="tbUrl" runat="server" Size = "40" Width="300px"></asp:TextBox>
		</td>
		<td>
			<asp:RegularExpressionValidator ID="vldUrl" runat="server" ControlToValidate="tbUrl" ValidationExpression="^(https?://)?([\w-]+\.)+[\w-]+([\w-./?%&=:]*)?$" Display="dynamic" ErrorMessage="Url is not required, but it must be valid if specified." EnableClientScript="true" />
		</td>
	</tr>
	<tr>
		<td colSpan="3">Comments&nbsp;
			<asp:RequiredFieldValidator id="RequiredFieldValidator3" runat="server" ErrorMessage="Please enter a comment" ControlToValidate="tbComment"></asp:RequiredFieldValidator><br />
			<asp:TextBox id="tbComment" runat="server" Rows = "10" Columns = "50" Width="400px" Height="193px" TextMode="MultiLine"></asp:TextBox></td>
	</tr>
	<tr>
		<td colSpan="3">
			<asp:CheckBox id="chkRemember" runat="server" Text="Remember Me?" />
		</td>
	</tr>
	<tr>
		<td>
			<asp:Button id="btnSubmit" runat="server" Text="Submit" /></td>
		<td colspan="2">
			<asp:Label id="Message" runat="server" ForeColor="Red" /></td>
	</tr>
</table>
</div>