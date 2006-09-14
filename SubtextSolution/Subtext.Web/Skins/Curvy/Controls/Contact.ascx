<%@ Control Language="c#" AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.Contact" %>
<div id="contactForm">
	<div class="title">Contact</div>			

	
	<div class="info"><asp:label id="lblMessage" runat="server" /></div>
		<div class="Form">
	<p align="center">
		Please use the form below if you have any comments, questions, or suggestions.
	</p>
	<p align="center">
		<em class="Required">*</em> denotes required field.
	</p>
			<dl class="Inputs">
				<dt>
					Name <em class="Required">*</em>
				</dt>
				<dd>
					<asp:textbox id="tbName" cssclass="Text" runat="server" />
				</dd>
				<br />
				<dt>
					Email <em class="Required">*</em>
				</dt>
				<dd>
					<asp:textbox id="tbEmail" cssclass="Text" runat="server" />
				</dd>
				<br />
				<dt>
					Subject
				</dt>
				<dd>
					<asp:textbox id="tbSubject" cssclass="Text" runat="server" /></td>
				</dd>
				<br />
				<dt>
					Message <em class="Required">*</em>
				</dt>
				<dd>
					<asp:textbox id="tbMessage" runat="server" textmode="MultiLine" cssclass="Text" Rows="10" Columns="50"/>
				</dd>
				<br />
			</dl>
			<div class="Action">
				<asp:button id="btnSend" cssclass="button" runat="server" text="Send" width="100"/>
			</div>
		</div>

</div>