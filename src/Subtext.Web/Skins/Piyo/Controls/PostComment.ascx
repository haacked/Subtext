<%@ Control Language="C#" EnableTheming="false"  AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.PostComment" %>
<div id="comments">
<div id="commentform">
<h3>Your comment:</h3>
<div id="comment-fields">
<div class="label">
	<label for="PostComment_ascx_tbTitle">Title:</label>
</div>

<div class="input">
	<asp:TextBox id="tbTitle" runat="server" CssClass="fixed" />
	<br/>
	<asp:RequiredFieldValidator id="vldTitleRequired" runat="server" ErrorMessage="Please enter a title" ControlToValidate="tbTitle" Display="Dynamic" EnableClientScript="true" />
</div>

<div class="label">
	<label for="PostComment_ascx_tbName">Name:</label>
</div>
<div class="input">
	<asp:TextBox id="tbName" runat="server" CssClass="fixed" />
	<br/>
	<asp:RequiredFieldValidator id="vldNameRequired" runat="server" ErrorMessage="Please enter your name" ControlToValidate="tbName" Display="Dynamic" EnableClientScript="true" />
</div>

<div class="label">
	<label for="PostComment_ascx_tbEmail">Email:</label>
</div>
<div class="input">
	<asp:TextBox id="tbEmail" runat="server" CssClass="fixed" />&nbsp;(will not be displayed)
	<br/>
	<asp:RegularExpressionValidator ID="vldEmail" runat="server" ControlToValidate="tbEmail" ValidationExpression="^.*?@.+\..+$" Display="dynamic" ErrorMessage="Email is not required, but it must be valid if specified." EnableClientScript="true" />
</div>

<div class="label">
	<label for="PostComment_ascx_tbUrl">Website:</label>
</div>
<div class="input">
	<asp:TextBox id="tbUrl" runat="server" CssClass="fixed" /><br />
	<asp:RegularExpressionValidator ID="vldUrl" runat="server" ControlToValidate="tbUrl" ValidationExpression="^(https?://)?([\w-]+\.)+[\w-]+([\w-./?%&=:]*)?$" Display="dynamic" ErrorMessage="Url is not required, but it must be valid if specified." EnableClientScript="true" />
</div>

<div class="label">
	<label for="PostComment_ascx_tbComment">Comment:</label>
</div>
<div class="input comment">
	<asp:TextBox id="tbComment" runat="server" rows="7" cols="55" CssClass="fixed" TextMode="MultiLine" />
	<br/>
	<asp:RequiredFieldValidator id="vldContentBody" runat="server" ErrorMessage="Please enter a comment" ControlToValidate="tbComment" />
</div>

<div class="label">&nbsp;</div>
<div class="input">
	<asp:CheckBox id="chkRemember" runat="server" Text="Remember Me?" Visible="True" />
</div>

<div class="clear"></div>

<div class="label">&nbsp;</div>
<div class="input">
	<asp:Button id="btnSubmit" runat="server" Text="Comment" CssClass="button" /><input type="reset" name="reset" value="Reset" class="button" /><asp:Label id="Message" runat="server" ForeColor="Red" />
</div>

<div class="clear"></div>

<div class="label">&nbsp;</div>
<div class="input">
	<h4>Live Comment Preview:</h4>
	<div id="commentPreview">&nbsp;</div>
</div>
</div>
</div></div>