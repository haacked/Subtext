<%@ Control Language="c#" AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.PostComment" %>
<div class="post">
<div class="moreinfo">
	<div class="moreinfotitle">Post a comment</div>
	<div id="postcomment">
		<div>
			Title<br />
			<asp:TextBox id="tbTitle" runat="server" Size = "40" Width="400px" CssClass="text"></asp:TextBox>
			<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ErrorMessage="Please enter a title" ControlToValidate="tbTitle"></asp:RequiredFieldValidator>
		</div>
		<div>
			Name<br />
			<asp:TextBox id="tbName" runat="server" Size = "40" Width="400px" CssClass="text"></asp:TextBox>
			<asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" ErrorMessage="Please enter your name" ControlToValidate="tbName"></asp:RequiredFieldValidator>
		</div>
		<div>
			Url<br />
			<asp:TextBox id="tbUrl" runat="server" Size = "40" Width="400px" CssClass="text"></asp:TextBox><br /><br />
		</div>
		<div>
			Comments<br />
			<asp:TextBox id="tbComment" runat="server" Rows = "10" Columns = "50" Width="400px" Height="193px" TextMode="MultiLine"></asp:TextBox>
			<asp:RequiredFieldValidator id="RequiredFieldValidator3" runat="server" ErrorMessage="Please enter a comment" ControlToValidate="tbComment"></asp:RequiredFieldValidator>
		</div>
		<div>
			<asp:CheckBox id="chkRemember" runat="server" Text="Remember Me"></asp:CheckBox>
		</div>
		<div>
			<asp:Button id="btnSubmit" runat="server" Text="Submit"></asp:Button><asp:Label id="Message" runat="server" ForeColor="Red"></asp:Label>
		</div>
	</div>
</div>
</div>
<div class="block_footer">&nbsp;</div>
</div>