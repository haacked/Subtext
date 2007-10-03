<%@ Control Language="C#" EnableTheming="false"  AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.Comments" %>
	<a name="feedback" title="feedback anchor"></a>
	<dl class="Comments">
		<dt>
			Comments
		</dt>
		<dd>
		<asp:literal id="NoCommentMessage" runat="server" />
		<asp:repeater id="CommentList" runat="server" onitemcreated="CommentsCreated" onitemcommand="RemoveComment_ItemCommand">
			<itemtemplate>
			<asp:Image runat="server" id="GravatarImg" visible="False" CssClass="avatar" AlternateText="Gravatar" />
				<dl class="Comment<%# AuthorCssClass %>">
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
						<asp:HyperLink Runat="server" ID="EditCommentTextLink" /> &nbsp;&nbsp; <asp:linkbutton runat="server" id="EditLink" causesvalidation="False" />
					</dd>
					<dd class="Content">
						<asp:literal id = "PostText" runat = "server" />
					</dd>
				</dl>
			</itemtemplate>
		</asp:repeater>
		</dd>
	</dl>
