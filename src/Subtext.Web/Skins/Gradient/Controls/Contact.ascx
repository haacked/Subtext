<%@ Control Language="c#" AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.Contact" %>
	<dl class="Contact form">
		<dt>
			<h2>Contact</h2>
		</dt>
		<dd class="Info">
			<asp:label id="Label1" runat="server" />
		</dd>
		<dd class="Form">
			<p>
				Please use the form below if you have any comments, questions, or suggestions. 
			</p>
			<dl class="Inputs">
				<dt>
					Name<asp:RequiredFieldValidator id="vldNameRequired" runat="server" ErrorMessage="Please enter your name" ControlToValidate="tbName" Display="Dynamic"> *Please enter your name or nickname.</asp:RequiredFieldValidator>
				</dt>
				<dd>
					<asp:textbox id="tbName" runat="server" />
				</dd>
				<dt>
					Email<asp:RequiredFieldValidator id="emailRequiredValidator" runat="server" ErrorMessage="Please enter your email address" ControlToValidate="tbEmail" Display="Dynamic"> *Please enter your email.</asp:RequiredFieldValidator>
					<asp:RegularExpressionValidator id="vldEmailRegex" runat="server" ErrorMessage="Email is optional, but if you enter one at least make sure it is valid." ControlToValidate="tbEmail" Display="Dynamic" ValidationExpression="^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"> *Email is optional, but if you enter one at least make sure it is valid.</asp:RegularExpressionValidator>
				</dt>
				<dd>
					<asp:textbox id="tbEmail" runat="server" />
				</dd>
				<dt>
					Subject
				</dt>
				<dd>
					<asp:textbox id="tbSubject" runat="server" />
				</dd>
				<dt>
					Message<asp:RequiredFieldValidator id="vldMessageRequired" runat="server" ErrorMessage="Please tell me something" ControlToValidate="tbMessage" Display="Dynamic"> *Please tell me something</asp:RequiredFieldValidator>
				</dt>
				<dd>
					<asp:textbox id="tbMessage" runat="server" textmode="MultiLine" />
				</dd>
			</dl>
			<div class="Action">
				<asp:button id="btnSend" cssclass="submit" runat="server" text="Inform Me" />
			</div>
		</dd>
	</dl>