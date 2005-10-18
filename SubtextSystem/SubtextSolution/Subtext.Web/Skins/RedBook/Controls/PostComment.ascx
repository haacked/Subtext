<%@ Control Language="c#" AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.PostComment" %>
<div id="CommentForm">
<h2>Post a comment</h2>
<p><label for="PostComment.ascx_tbTitle">Title:</label> <asp:TextBox id="tbTitle" runat="server" Size="40" Width="300px"></asp:TextBox><br/><asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ErrorMessage="Please enter a title" ControlToValidate="tbTitle" Display="Dynamic" /></p>
	<p><label for="PostComment.ascx_tbName">Name:</label> <asp:TextBox id="tbName" runat="server" Size="40" Width="300px"></asp:TextBox><br/><asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" ErrorMessage="Please enter your name" ControlToValidate="tbName" Display="Dynamic" /></p>
	<p><label for="PostComment.ascx_tbUrl">Website:</label> <asp:TextBox id="tbUrl" runat="server" Size="40" Width="300px"></asp:TextBox></p>
<p>
	<label for="PostComment.ascx_tbComment">Comment:</label> <asp:TextBox id="tbComment" runat="server" Rows="10" Columns="50" Width="400px" Height="193px"
					TextMode="MultiLine"></asp:TextBox></asp:Label><br/><asp:RequiredFieldValidator id="Requiredfieldvalidator4" runat="server" ErrorMessage="Please enter a comment"
					ControlToValidate="tbComment"></asp:RequiredFieldValidator>
</p>
<asp:CheckBox id="chkRemember" runat="server" Text="Remember Me?" Visible="False"></asp:CheckBox>
<p><asp:Button id="btnSubmit" runat="server" Text="Post" CssClass="postbutton" /><asp:Label id="Message" runat="server" ForeColor="Red" /></p>
</div>