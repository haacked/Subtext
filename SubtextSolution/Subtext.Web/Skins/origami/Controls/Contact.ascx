<%@ Control Language="c#" AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.Contact" %>
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
		<label for="Contact_ascx_tbEmail" accesskey="E"><span class="accessKey">E</span>mail</label>
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
	<asp:button id="btnSend" cssclass="button" runat="server" text="Send" />
</div>


