<%@ Control Language="C#" EnableTheming="false"  AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.Comments" %>
<a name="feedback" title="feedback anchor"></a>
<div id="comments">
<h3>Feedback</h3>
	<asp:Literal ID="NoCommentMessage" Runat="server" />
	<asp:Repeater id="CommentList" runat="server" OnItemCreated="CommentsCreated" OnItemCommand="RemoveComment_ItemCommand">
		<ItemTemplate>
		    <asp:Image runat="server" id="GravatarImg" visible="False" CssClass="avatar" AlternateText="Gravatar" />
			<div class="post">
				<h2>
					<asp:Literal Runat = "server" ID = "Title" />
					<asp:LinkButton Runat="server" ID="EditLink" CausesValidation="False" />
				</h2>
				<asp:Literal id = "PostText" Runat = "server" />
				<div class="postfoot">
					<asp:Literal id = "PostDate" Runat = "server" /> | <asp:HyperLink Target="_blank" Runat="server" ID="NameLink" />
				</div>
			</div>
		</ItemTemplate>

	</asp:Repeater>
</div>