<%@ Control Language="C#" EnableTheming="false"  AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.Contact" %>

<p>
	Please use the form below if you have any comments, questions, or 
	suggestions.
</p>

<div id="commentform">
	<fieldset>
		<legend>Contact Form</legend>
		<p>
			<label for="PostComment_ascx_tbName" AccessKey="N">
				<u>N</u>ame:
				<asp:RequiredFieldValidator id="vldNameRequired" runat="server" ErrorMessage="Please enter your name" ControlToValidate="tbName" Display="Dynamic" EnableClientScript="true" />
			</label> 
			<asp:TextBox id="tbName" runat="server" Size="40" TabIndex="2" CssClass="textbox" />		
		</p>
		<p>
			<label for="PostComment_ascx_tbEmail" AccessKey="E">
				<u>E</u>mail:
				<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ErrorMessage="Please enter your email" ControlToValidate="tbEmail" Display="Dynamic" EnableClientScript="true" />
				<asp:RegularExpressionValidator ID="vldEmail" runat="server" ControlToValidate="tbEmail" ValidationExpression="^.*?@.+\..+$" Display="dynamic" ErrorMessage="The email address you've entered does not appear to be valid." EnableClientScript="true" />
			</label> 
			<asp:TextBox id="tbEmail" runat="server" Size="40" TabIndex="2" CssClass="textbox" />			
		</p>
		<p>
			<label for="PostComment_ascx_tbSubject" AccessKey="S"><u>S</u>ubject:</label> 
			<asp:TextBox id="tbSubject" runat="server" Size = "50" Width="300px" />
		</p>
		<p>
			<label for="PostComment_ascx_tbMessage" AccessKey="M">
				<u>M</u>essage:
				<asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" ErrorMessage="Please tell me something" ControlToValidate="tbMessage" Display="Dynamic" />
			</label> 
			<asp:TextBox id="tbMessage" runat="server" Rows="10" Columns="50"
					TabIndex="4"
					CssClass="textarea"
					TextMode="MultiLine" />
		</p>
		<p>
			<st:SubtextCaptchaControl id="captcha" runat="server" ErrorMessage="Please enter the correct word" />
			<asp:Button id="btnSend" CssClass="buttonSubmit" runat="server" Text="Post" TabIndex="6" CausesValidation="true" />
			<asp:Label id="lblMessage" runat="server" ForeColor="Red" />
		</p>
	</fieldset>
</div>