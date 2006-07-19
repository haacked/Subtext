<%@ Control Language="c#" AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.PostComment" %>
<%@ Register TagPrefix="sub" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
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
		<asp:TextBox id="tbEmail" runat="server" size="40" AccessKey="E"></asp:TextBox><br/>
	</div>

	<div class="label"><label for="PostComment_ascx_tbUrl"><span class="accessKey">W</span>ebsite:</label></div>
	<div class="input">
		<asp:TextBox id="tbUrl" runat="server" size="40" AccessKey="W"></asp:TextBox>
	</div>

	<div class="label"><label for="PostComment_ascx_tbComment"><span class="accessKey">C</span>omment:</label></div>
	<div class="input">
		<asp:TextBox id="tbComment" runat="server" Rows="10" Columns="40" AccessKey="C"
			TextMode="MultiLine" Width="400px" class="livepreview"></asp:TextBox><br/>
			<asp:RequiredFieldValidator id="Requiredfieldvalidator4" runat="server" ErrorMessage="Please enter a comment"
			ControlToValidate="tbComment"></asp:RequiredFieldValidator></div>

	<div class="input">
		<sub:CompliantButton id="btnCompliantSubmit" runat="server" Text="Leave Your Mark" /> 
		<asp:Label id="Message" runat="server" ForeColor="Red" />
	</div>
</div>