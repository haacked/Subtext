<%@ Control Language="C#" EnableTheming="false"  AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.PostComment" %>
	<div id="postComment">
		<div class="title">Post Comment</div>
		
			<dl class="Inputs">
				<dt>
					Title <em class="Required">*</em> <asp:RequiredFieldValidator id="vldTitleRequired" runat="server" ErrorMessage="Please enter a title" ControlToValidate="tbTitle" Display="Dynamic" EnableClientScript="true" />
				</dt>
				<dd>
					<asp:textbox id="tbTitle" runat="server" cssclass="Text" />
				</dd>
				<dt>
					Name <em class="Required">*</em> <asp:RequiredFieldValidator id="vldNameRequired" runat="server" ErrorMessage="Please enter your name" ControlToValidate="tbName" Display="Dynamic" EnableClientScript="true" />
				</dt>
				<dd>
					<asp:textbox id="tbName" runat="server" cssclass="Text" />
				</dd>
				<dt>
					Email <asp:RegularExpressionValidator ID="vldEmail" runat="server" ControlToValidate="tbEmail" ValidationExpression="^.*?@.+\..+$" Display="dynamic" ErrorMessage="Email is not required, but it must be valid if specified." EnableClientScript="true" />
				</dt>
				<dd>
					<asp:textbox id="tbEmail" runat="server" cssclass="Text" />
				</dd>
				<dt>
					Url <asp:RegularExpressionValidator ID="vldUrl" runat="server" ControlToValidate="tbUrl" ValidationExpression="^(https?://)?([\w-]+\.)+[\w-]+([\w-./?%&=:]*)?$" Display="dynamic" ErrorMessage="Url is not required, but it must be valid if specified." EnableClientScript="true" />
				</dt>
				<dd>
					<asp:textbox id="tbUrl" runat="server" cssclass="Text" />
				</dd>
				<dt>
					Comment <em class="Required">*</em> <asp:RequiredFieldValidator id="vldCommentBody" runat="server" ErrorMessage="Please enter a comment" ControlToValidate="tbComment" EnableClientScript="true" />
				</dt>
				<dd>
					<asp:textbox id="tbComment" runat="server" textmode="MultiLine" />
					<asp:RequiredFieldValidator id="vldCommentBodyRequired" runat="server" ErrorMessage="Please enter your comment" ControlToValidate="tbComment" Display="Dynamic" EnableClientScript="true" />
				</dd>
				<dd>
					<asp:checkbox id="chkRemember" runat="server" text="Remember Me?" checked="True" />
				</dd>
			</dl>
			
				<asp:button id="btnSubmit" cssclass="Submit" runat="server" text="Submit" />
			

	</div>