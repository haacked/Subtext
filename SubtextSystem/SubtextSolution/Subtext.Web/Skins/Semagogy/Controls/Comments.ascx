<%@ Control Language="c#" AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.Comments" %>
	<a name="feedback"></a>
	<dl class="Comments">
		<dt>
			Comments
		</dt>
		<dd>
		<asp:literal id="NoCommentMessage" runat="server" />
		<asp:repeater id="CommentList" runat="server" onitemcreated="CommentsCreated" onitemcommand="RemoveComment_ItemCommand">
			<itemtemplate>
				<dl class="Comment">
					<dt>
						<asp:literal runat = "server" id = "Title" />
					</dt>
					<dd class="Name">
						<asp:hyperlink runat="server" id="NameLink" />
					</dd>
					<dd class="Date">
						<asp:literal id = "PostDate" runat = "server" />
					</dd>
					<dd class="Admin">
						<asp:linkbutton runat="server" id="EditLink" causesvalidation="False" />
					</dd>
					<dd class="Content">
						<asp:literal id = "PostText" runat = "server" />
					</dd>
				</dl>
			</itemtemplate>
		</asp:repeater>
		</dd>
	</dl>
