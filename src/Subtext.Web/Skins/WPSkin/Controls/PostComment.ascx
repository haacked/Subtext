<%@ Control Language="c#" AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.PostComment" %>

<div id="commentform">
	<h2>Your comment:</h2>
	<div class="label">
		<label for="PostComment_ascx_tbTitle">Title: <asp:RequiredFieldValidator id="vldTitleRequired" runat="server" ErrorMessage="*So what is this about?" ControlToValidate="tbTitle" Display="Dynamic" /></label>
	</div>
	<div class="input">
		<asp:TextBox id="tbTitle" runat="server" size="40"></asp:TextBox>		
	</div>

	<div class="label">
		<label for="PostComment_ascx_tbName">Name: <asp:RequiredFieldValidator id="vldNameRequired" runat="server" ErrorMessage="*And who are you?" ControlToValidate="tbName" Display="Dynamic" /></label>
	</div>
	<div class="input">
		<asp:TextBox id="tbName" runat="server" size="40"></asp:TextBox>
	</div>

	<div class="label">
		<label for="PostComment_ascx_tbEmail">Email: (never displayed)<asp:RegularExpressionValidator id="vldEmailRegex" runat="server" ErrorMessage="*Email is optional, but if you enter one at least make sure it is valid." ControlToValidate="tbEmail" Display="Dynamic" ValidationExpression="^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$" /></label>
	</div>
	<div class="input">
		<asp:TextBox id="tbEmail" runat="server" CssClass="fixed" size="35"></asp:TextBox>&nbsp;(will show your <a href="http://gravatar.com/" title="gravatar">gravatar</a>)
	</div>

	<div class="label"><label for="PostComment_ascx_tbUrl">Website:</label></div>
	<div class="input">
		<asp:TextBox id="tbUrl" runat="server" size="40"></asp:TextBox>
	</div>

	<div class="label"><label for="PostComment_ascx_tbComment">Comment: <asp:RequiredFieldValidator id="vldCommentRequired" runat="server" Display="Dynamic" ErrorMessage="*I do want to hear your thoughts. Please enter a comment." ControlToValidate="tbComment" /><em class="smallnote">Allowed tags: blockquote, a, strong, em, p, u, strike, super, sub, code</em></label></div>
	<div class="input">
		<asp:TextBox id="tbComment" runat="server" Rows="10" Columns="40" TextMode="MultiLine" Width="400px" class="livepreview"></asp:TextBox>
	</div>
	<div class="label">&nbsp;</div>

	<div class="input">
		<st:CompliantButton id="btnCompliantSubmit" runat="server" Text="Leave Your Mark" /> 
		<a name="message"></a>
		<asp:Label id="Message" runat="server" ForeColor="Red" />
	</div>
</div>