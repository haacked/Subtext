<%@ Control Language="c#" AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.PostComment" %>
<%@ Register TagPrefix="sub" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<div id="CommentForm">
<fieldset>
	<legend>Post a comment</legend>
	<p><label for="PostComment.ascx_tbTitle" AccessKey="T"><u>T</u>itle:</label> <asp:TextBox id="tbTitle" runat="server" Size="40" TabIndex="1" CssClass="textinput"></asp:TextBox><br/><asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ErrorMessage="Please enter a title" ControlToValidate="tbTitle" Display="Dynamic" /></p>
	<p><label for="PostComment.ascx_tbName" AccessKey="N"><u>N</u>ame:</label> <asp:TextBox id="tbName" runat="server" Size="40" TabIndex="2" CssClass="textinput"></asp:TextBox><br/><asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" ErrorMessage="Please enter your name" ControlToValidate="tbName" Display="Dynamic" /></p>
	<p><label for="PostComment.ascx_tbUrl" AccessKey="W"><u>W</u>ebsite:</label> <asp:TextBox id="tbUrl" runat="server" Size="40" TabIndex="3" CssClass="textinput"></asp:TextBox></p>
	<p>
		<label for="PostComment.ascx_tbComment" AccessKey="C"><u>C</u>omment:</label> 
		<asp:TextBox id="tbComment" runat="server" Rows="10" Columns="50"
				TabIndex="4"
				CssClass="textarea"
				TextMode="MultiLine"></asp:TextBox></asp:Label><br/><asp:RequiredFieldValidator id="Requiredfieldvalidator4" runat="server" ErrorMessage="Please enter a comment"
				ControlToValidate="tbComment"></asp:RequiredFieldValidator>
	</p>
	<asp:CheckBox id="chkRemember" runat="server" Text="Remember Me?" Visible="False" TabIndex="5"></asp:CheckBox>
	<p><sub:CompliantButton id="Compliantbutton1" CssClass="buttonSubmit" runat="server" Text="Post" TabIndex="6"></sub:CompliantButton>
	   <asp:Label id="Message" runat="server" ForeColor="Red" /></p>
	<div id="stylesheetTest"></div>
</fieldset>
</div>
