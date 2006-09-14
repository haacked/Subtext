<%@ Control Language="c#" AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.PostComment" %>
<%@ Register TagPrefix="sub" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<div id="comments">
	<div id="commentform">
		<h3>Enter Your Comment Here</h3>
		<p><b><asp:Label id="Message" runat="server" ForeColor="Red" /></b></p>
		<p>I hate spam as much as you. I assure you that your e-mail address will not be distributed to anyone
		or used to mass mail about the site updates. It's only purpose is to fetch your avatar from www.gravatar.com</p>
		<p align="center"><b>INAPPROPRIATE COMMENTS WILL BE DELETED!!!</b></p>
		<div class="label">
			<label for="PostComment.ascx_tbTitle">Title:</label>
		</div>
		<div class="input">
			<asp:TextBox id="tbTitle" runat="server" CssClass="fixed"></asp:TextBox><br/><asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ErrorMessage="Please enter a title" ControlToValidate="tbTitle" Display="Dynamic" />
		</div>
		<br />

		<div class="label">
			<label for="PostComment.ascx_tbName">Your Name:</label>
		</div>
		<div class="input">
			<asp:TextBox id="tbName" runat="server" CssClass="fixed"></asp:TextBox><br/><asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" ErrorMessage="Please enter your name" ControlToValidate="tbName" Display="Dynamic" />
		</div>
		<br />

		<div class="label">
			<label for="PostComment.ascx_tbEmail">Your E-mail address:</label>&nbsp;(will not be displayed)
			<a class="helpLink" onclick="showHelpTip(event, 'Get yourself an AVATAR at <a href=\'http://www.gravatar.com/\'>Gravatar</a> so that I can display your avatar next to your comment.'); 
			return false;" href="?">
			<img border="0" src="~/Admin/Resources/Scripts/Images/ms_information_small.gif" id="helpImg" runat="server"/>
			</a>

		</div>
		<div class="input">
			<asp:TextBox id="tbEmail" runat="server" CssClass="fixed"></asp:TextBox><br/><asp:RequiredFieldValidator id="RequiredFieldValidator3" runat="server" ErrorMessage="Please enter your email" ControlToValidate="tbEmail" Display="Dynamic" />
		</div>
		<br />

		<div class="label">
			<label for="PostComment.ascx_tbUrl">Your blog address:</label>&nbsp;(will be a link on your name)
		</div>
		<div class="input">
			<asp:TextBox id="tbUrl" runat="server" CssClass="fixed"></asp:TextBox>
		</div>
		<br />

		<div class="label">
			<label for="PostComment.ascx_tbComment">Your Say:</label>
		</div>

		<div class="input">
			<table>
			<tr>
				<td width="80%">
					<asp:TextBox id="tbComment" runat="server" Rows="10" Columns="50" width="80%" Height="75%"
					TextMode="MultiLine" class="livepreview"></asp:TextBox>
				</td>
				<td>
					<b>Allowed HTML tags</b>
					<br />
			        &lt;a href=, title= &gt;<br />
			        &lt;b&gt;<br />
			        &lt;strong&gt;<br />
			        &lt;blockquote cite&gt;<br />
			        &lt;i&gt;<br />
			        &lt;em&gt;<br />
			        &lt;u&gt;<br />
			        &lt;strike&gt;<br />
			        &lt;super&gt;<br />
			        &lt;sub&gt;<br />
					&lt;code&gt;
					<br/>
				</td>
			</tr>
		</table>
			<asp:RequiredFieldValidator id="Requiredfieldvalidator4" runat="server" ErrorMessage="Please enter a comment" ControlToValidate="tbComment"></asp:RequiredFieldValidator>
		</div>
		<br/>
		<!--
		<p>
		<asp:PlaceHolder id="coCommentPlaceholder" runat="server">CoComment</asp:PlaceHolder>
		<p/>
		-->
		


		<div class="input">
			<asp:CheckBox id="chkRemember" runat="server" Text="Remember Me?" visible="false"></asp:CheckBox>
		</div>

		<div class="input">
			<sub:CompliantButton id="btnCompliantSubmit" CssClass="button" runat="server" Text="Send Your Thoughts"></sub:CompliantButton>
			<input type="reset" name="reset" value="Erase What You've Entered" class="button" />
		</div>

		<tr class="live">
			<td>
				<div class="livePrev">
					<div class="livePrevTitle">Live Comment Preview:</div>
					<div id="commentPreview" class="commentText livepreview">&nbsp;</div>
				</div>
			</td>
			<td></td>
		</tr>

	</div>
</div>