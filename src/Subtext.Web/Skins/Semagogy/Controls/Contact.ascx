<%@ Control Language="C#" EnableTheming="false"  AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.Contact" %>
	<dl class="Contact">
		<dt>
			Contact
		</dt>
		<dd class="Info">
			<asp:label id="lblMessage" runat="server" />
		</dd>
		<dd class="Form">
			<p>
				Please use the form below if you have any comments, questions, or suggestions.
			</p>
			<dl class="Inputs">
				<dt>
					Name <em class="Required">*</em>
					<asp:RequiredFieldValidator id="RequiredFieldValidator3" runat="server" ErrorMessage="Please enter your name" ControlToValidate="tbName" Display="Dynamic" />
				</dt>
				<dd>
					<asp:textbox id="tbName" cssclass="Text" runat="server" />
				</dd>
				<dt>
					Email <em class="Required">*</em>
					<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ErrorMessage="Please enter your email" ControlToValidate="tbEmail" Display="Dynamic" />
					<asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server" ErrorMessage="Invalid email address format" ControlToValidate="tbEmail" Display="Dynamic" ValidationExpression="^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$" />
				</dt>
				<dd>
					<asp:textbox id="tbEmail" cssclass="Text" runat="server" />
				</dd>
				<dt>
					Subject
				</dt>
				<dd>
					<asp:textbox id="tbSubject" cssclass="Text" runat="server" />
				</dd>
				<dt>
					Message <em class="Required">*</em>
					<asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" ErrorMessage="Please enter a message" ControlToValidate="tbMessage" Display="Dynamic" />
				</dt>
				<dd>
					<asp:textbox id="tbMessage" runat="server" textmode="MultiLine" />
				</dd>
			</dl>
			<div class="Action">
				<asp:button id="btnSend" cssclass="Submit" runat="server" text="Send" />
			</div>
		</dd>
	</dl>
