<%@ Control Language="C#" EnableTheming="false"  AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.Contact" %>
<div class="post">
	<h2>Contact</h2>
		
	<div class="info">
		<asp:label id="lblMessage" runat="server" />
	</div>

	<asp:ValidationSummary ID="vldSummary" runat="server" CssClass="error" />
			
	<p>
		Please use the form below if you have any comments, questions, or suggestions. (required fields in bold)
	</p>
	<dl class="form">
		<dt>
			<label for="Contact_ascx_tbName" accesskey="N" class="required"><span class="accessKey">N</span>ame</label> <asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" ErrorMessage="Please enter your name"
					ControlToValidate="tbName" Display="Static">*</asp:RequiredFieldValidator>
		</dt>
		<dd>
			<asp:textbox id="tbName" cssclass="Text" runat="server" />
		</dd>
		<dt>
			<label for="Contact_ascx_tbEmail" accesskey="E" class="required"><span class="accessKey">E</span>mail</label> 
			<asp:RequiredFieldValidator id="RequiredFieldValidator4" runat="server" ErrorMessage="Please enter an email address."
					ControlToValidate="tbEmail" Display="Static">*</asp:RequiredFieldValidator>
			<asp:RegularExpressionValidator id="vldEmailRegex" runat="server" ErrorMessage="The email address you've entered does not appear to be valid." ControlToValidate="tbEmail" Display="Dynamic" ValidationExpression="^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$">*</asp:RegularExpressionValidator>
		</dt>
		<dd>
			<asp:textbox id="tbEmail" cssclass="Text" runat="server" />
		</dd>
		<dt>
			<label for="Contact_ascx_tbSubject" accesskey="S" class="required"><span class="accessKey">S</span>ubject</label> <asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ErrorMessage="Please enter a subject"
					ControlToValidate="tbName" Display="Static">*</asp:RequiredFieldValidator>
		</dt>
		<dd>
			<asp:textbox id="tbSubject" cssclass="Text" runat="server" />
		</dd>
		<dt>
			<label for="Contact_ascx_tbMessage" accesskey="M" class="required"><span class="accessKey">M</span>essage</label> <asp:RequiredFieldValidator id="RequiredFieldValidator3" runat="server" ErrorMessage="Please enter a message"
					ControlToValidate="tbName" Display="Static">*</asp:RequiredFieldValidator>
		</dt>
		<dd>
			<asp:textbox id="tbMessage" runat="server" textmode="MultiLine" />
		</dd>
	</dl>
	<div class="action">
		<asp:button id="btnSend" runat="server" text="Send" />
	</div>
</div>