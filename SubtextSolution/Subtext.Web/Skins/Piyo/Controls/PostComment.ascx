<%@ Control Language="c#" AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.PostComment" %>
<div id="comments">
<div id="commentform">
<h3>Your comment:</h3>
<div class="label"><label for="PostComment_ascx_tbTitle">Title:</label></div>
<div class="input"><asp:TextBox id="tbTitle" runat="server" CssClass="fixed"></asp:TextBox><br/><asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ErrorMessage="Please enter a title" ControlToValidate="tbTitle" Display="Dynamic" /></div>

<div class="label"><label for="PostComment_ascx_tbName">Name:</label></div>
<div class="input"><asp:TextBox id="tbName" runat="server" CssClass="fixed"></asp:TextBox><br/><asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" ErrorMessage="Please enter your name" ControlToValidate="tbName" Display="Dynamic" /></div>

<div class="label"><label for="PostComment_ascx_tbEmail">Email:</label></div>
<div class="input"><asp:TextBox id="tbEmail" runat="server" CssClass="fixed"></asp:TextBox>&nbsp;(will not be displayed)<br/><asp:RequiredFieldValidator id="RequiredFieldValidator3" runat="server" ErrorMessage="Please enter your email" ControlToValidate="tbEmail" Display="Dynamic" /></div>

<div class="label"><label for="PostComment_ascx_tbUrl">Website:</label></div>
<div class="input"><asp:TextBox id="tbUrl" runat="server" CssClass="fixed"></asp:TextBox></div>

<div class="label"><label for="PostComment_ascx_tbComment">Comment:</label></div>
<div class="input"><asp:TextBox id="tbComment" runat="server" rows="7" cols="55" CssClass="fixed"
					TextMode="MultiLine" onkeyup="reloadPreviewDiv();"></asp:TextBox><br/><asp:RequiredFieldValidator id="Requiredfieldvalidator4" runat="server" ErrorMessage="Please enter a comment"
					ControlToValidate="tbComment"></asp:RequiredFieldValidator></div>

<div class="label">&nbsp;</div>
<div class="input"><asp:CheckBox id="chkRemember" runat="server" Text="Remember Me?" Visible="True"></asp:CheckBox></div>

<div class="clear"></div>

<div class="label">&nbsp;</div>
<div class="input"><asp:Button id="btnSubmit" runat="server" Text="Comment" CssClass="button" /><input type="reset" name="reset" value="Reset" class="button" /><asp:Label id="Message" runat="server" ForeColor="Red" /></div>

<div class="clear"></div>

<div class="label">&nbsp;</div>
<div class="input">
	<h4>Live Comment Preview:</h4>
	<div id="commentPreview">&nbsp;</div>
</div>

</div></div>