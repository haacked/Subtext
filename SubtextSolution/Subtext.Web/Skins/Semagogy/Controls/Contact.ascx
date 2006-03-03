<%@ Control Language="c#" AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.Contact" %>
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
				</dt>
				<dd>
					<asp:textbox id="tbName" cssclass="Text" runat="server" />
				</dd>
				<dt>
					Email <em class="Required">*</em>
				</dt>
				<dd>
					<asp:textbox id="tbEmail" cssclass="Text" runat="server" />
				</dd>
				<dt>
					Subject
				</dt>
				<dd>
					<asp:textbox id="tbSubject" cssclass="Text" runat="server" /></td>
				</dd>
				<dt>
					Message <em class="Required">*</em>
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
