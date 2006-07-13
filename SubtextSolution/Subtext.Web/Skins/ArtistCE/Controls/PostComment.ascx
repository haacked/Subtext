<%@ Control Language="c#" AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.PostComment" %>
<div id="comments">
	<div id="commentform">
		<h3>Enter your comment here</h3>
		<p align="center">Don't leave your e-mail address. HTML is not allowed. </p>
		<p align="center"><b>OFFENSIVE COMMENTS WILL BE DELETED!!!</b></p>
		<div class="label">
			<label for="PostComment_ascx_tbTitle">Title:</label>
		</div>
		<div class="input">
			<asp:TextBox id="tbTitle" runat="server" CssClass="fixed"></asp:TextBox><br/><asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ErrorMessage="Please enter a title" ControlToValidate="tbTitle" Display="Dynamic" />
		</div>
		<br />
		<div class="label">
			<label for="PostComment_ascx_tbName">Name:</label>
		</div>
		<div class="input">
			<asp:TextBox id="tbName" runat="server" CssClass="fixed"></asp:TextBox><br/><asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" ErrorMessage="Please enter your name" ControlToValidate="tbName" Display="Dynamic" />
		</div>
		<br />
		<div class="label">
			<label for="PostComment_ascx_tbUrl">Website:</label>
		</div>
		<div class="input">
			<asp:TextBox id="tbUrl" runat="server" CssClass="fixed"></asp:TextBox>
		</div>
		<br />
		<div class="label">
			<label for="PostComment_ascx_tbComment">Comment:</label>
		</div>
		<div class="input">
			<asp:TextBox id="tbComment" runat="server" rows="7" cols="55" CssClass="fixed"
				TextMode="MultiLine"></asp:TextBox>
			<br/>
			<asp:RequiredFieldValidator id="Requiredfieldvalidator4" runat="server" ErrorMessage="Please enter a comment" ControlToValidate="tbComment"></asp:RequiredFieldValidator>
		</div>
		<div class="input">
			<asp:CheckBox id="chkRemember" runat="server" Text="Remember Me?" Visible="False"></asp:CheckBox>
		</div>
		<div class="input">
			<asp:Button id="btnSubmit" runat="server" Text="Send Comment" CssClass="button" />
			<input type="reset" name="reset" value="Reset" class="button" />
			<asp:Label id="Message" runat="server" ForeColor="Red" />
		</div>
	</div>
</div>