<%@ Control Language="c#" AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.PostComment" %>
	<dl class="Comment">
		<dt>Post Comment</dt>
		<dd class="Form">
			<dl class="Inputs">
				<dt>
					Title <em class="Required">*</em>
				</dt>
				<dd>
					<asp:textbox id="tbTitle" runat="server" cssclass="Text" />
				</dd>
				<dt>
					Name <em class="Required">*</em>
				</dt>
				<dd>
					<asp:textbox id="tbName" runat="server" cssclass="Text" />
				</dd>
				<dt>
					Url
				</dt>
				<dd>
					<asp:textbox id="tbUrl" runat="server" cssclass="Text" />
				</dd>
				<dt>
					Comment <em class="Required">*</em>
				</dt>
				<dd>
					<asp:textbox id="tbComment" runat="server" textmode="MultiLine" />
				</dd>
				<dd>
					<asp:checkbox id="chkRemember" runat="server" text="Remember Me?" checked="True" />
				</dd>
			</dl>
			<div class="Action">
				<asp:button id="btnSubmit" cssclass="Submit" runat="server" text="Submit" />
			</div>
		</dd>
	</dl>