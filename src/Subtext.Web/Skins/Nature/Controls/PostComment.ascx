<%@ Control Language="C#" EnableTheming="false"  AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.PostComment" %>

<div id="commentform">
	<h3>Your comment:</h3>
	<div class="label">
		<label for="PostComment_ascx_tbTitle"><span class="accessKey">T</span>itle:</label>
	</div>
	<div class="input">
		<asp:TextBox id="tbTitle" runat="server" size="40" AccessKey="T"></asp:TextBox><br/>
		<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ErrorMessage="Please enter a title" ControlToValidate="tbTitle" Display="Dynamic" />
	</div>

	<div class="label">
		<label for="PostComment_ascx_tbName"><span class="accessKey">N</span>ame:</label>
	</div>
	<div class="input">
		<asp:TextBox id="tbName" runat="server" size="40" AccessKey="N"></asp:TextBox><br/>
			<asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" ErrorMessage="Please enter your name" ControlToValidate="tbName" Display="Dynamic" />
	</div>
	
	<div class="label">
		<label for="PostComment_ascx_tbEmail"><span class="accessKey">E</span>mail: <em>Not Displayed</em></label>
	</div>
	<div class="input">
		<asp:TextBox id="tbEmail" runat="server" size="40" AccessKey="E" /><br/>
		<asp:RegularExpressionValidator ID="vldEmail" runat="server" ControlToValidate="tbEmail" ValidationExpression="^.*?@.+\..+$" Display="dynamic" ErrorMessage="Email is not required, but it must be valid if specified." EnableClientScript="true" />
	</div>

	<div class="label"><label for="PostComment_ascx_tbUrl"><span class="accessKey">W</span>ebsite:</label></div>
	<div class="input">
		<asp:TextBox id="tbUrl" runat="server" size="40" AccessKey="W" />
		<asp:RegularExpressionValidator ID="vldUrl" runat="server" ControlToValidate="tbUrl" ValidationExpression="^(https?://)?([\w-]+\.)+[\w-]+([\w-./?%&=:]*)?$" Display="dynamic" ErrorMessage="Url is not required, but it must be valid if specified." EnableClientScript="true" />
	</div>

	<div class="label"><label for="PostComment_ascx_tbComment"><span class="accessKey">C</span>omment:</label></div>
	<div class="input">
		<asp:TextBox id="tbComment" runat="server" Rows="10" Columns="40" AccessKey="C"
			TextMode="MultiLine" Width="400px" class="livepreview" />
			<br/>
			<asp:RequiredFieldValidator id="vldCommentBodyRequired" runat="server" ErrorMessage="Please enter a comment"
			ControlToValidate="tbComment" />
	</div>

	<div class="input">
		<st:CompliantButton id="btnCompliantSubmit" runat="server" Text="Leave Your Mark" /> 
		<asp:Label id="Message" runat="server" ForeColor="Red" />
	</div>
</div>
