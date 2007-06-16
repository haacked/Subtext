<%@ Control Language="C#" EnableTheming="false"  AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.PostComment" %>
	
<h3>Leave A Comment</h3>
<div class="form">
	<p>
	<asp:Label ID="titleLabel" runat="server" AssociatedControlID="tbTitle" Text="Title *" /> <asp:RequiredFieldValidator id="vldTitleRequired" runat="server" ErrorMessage="Please enter a title" ControlToValidate="tbTitle" Display="Dynamic" EnableClientScript="true" />
	<asp:textbox id="tbTitle" runat="server" cssclass="Text" />
	<asp:Label ID="nameLabel" runat="server" AssociatedControlID="tbName" Text="Name *" /> <asp:RequiredFieldValidator id="vldNameRequired" runat="server" ErrorMessage="Please enter your name" ControlToValidate="tbName" Display="Dynamic" EnableClientScript="true" />
	<asp:textbox id="tbName" runat="server" cssclass="Text" />
	<asp:Label ID="emailLabel" runat="server" AssociatedControlID="tbEmail" Text="Email" /> <asp:RegularExpressionValidator ID="vldEmail" runat="server" ControlToValidate="tbEmail" ValidationExpression="^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$" Display="dynamic" ErrorMessage="Email is not required, but it must be valid if specified." EnableClientScript="true" />
	<asp:textbox id="tbEmail" runat="server" cssclass="Text" />
	<asp:Label ID="urlLabel" runat="server" AssociatedControlID="tbUrl" Text="Url" /> <asp:RegularExpressionValidator ID="vldUrl" runat="server" ControlToValidate="tbUrl" ValidationExpression="^(https?://)?([\w-]+\.)+[\w-]+([\w-./?%&=:]*)?$" Display="dynamic" ErrorMessage="Url is not required, but it must be valid if specified." EnableClientScript="true" />
	<asp:textbox id="tbUrl" runat="server" cssclass="Text" />
	<asp:Label ID="commentLabel" runat="server" AssociatedControlID="tbComment" Text="Comment *" /><asp:RequiredFieldValidator id="vldCommentBody" runat="server" ErrorMessage="Please enter a comment" ControlToValidate="tbComment" EnableClientScript="true" />
	<asp:textbox id="tbComment" runat="server" textmode="MultiLine" />
	<asp:RequiredFieldValidator id="vldCommentBodyRequired" runat="server" ErrorMessage="Please enter your comment" ControlToValidate="tbComment" Display="Dynamic" EnableClientScript="true" />
	<asp:checkbox id="chkRemember" runat="server" text="Remember Me?" checked="True" />
	<br />
	<asp:button id="btnSubmit" cssclass="Submit" runat="server" text="Submit" />
	</p>
</div>