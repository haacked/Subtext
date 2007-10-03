<%@ Control Language="C#" EnableTheming="false"  AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.Comments" %>
<a name="feedback" title="feedback anchor"></a>
<div id="comments">
<h2>Feedback</h2>
	<asp:Literal ID = "NoCommentMessage" Runat ="server" />
	<asp:Repeater id="CommentList" runat="server" OnItemCreated="CommentsCreated" OnItemCommand="RemoveComment_ItemCommand">
		<ItemTemplate>
			<a name="<%# Comment.Id %>"></a>
			<div class="comment<%# AuthorCssClass %>">
				<h3><asp:HyperLink Runat="server" ID="EditCommentImgLink" /><asp:Literal Runat="server" ID="Title" /> <span class="adminLink"><asp:LinkButton Runat="server" ID="EditLink" CausesValidation="False" title="edit comment" /></span></h3>
				<div class="commentInfo">Left by <asp:HyperLink Target="_blank" Runat="server" ID="NameLink" /> at <asp:Literal id="PostDate" Runat="server" /></div>
				<div class="commentText">
					<asp:Image runat="server" id="GravatarImg" visible="False" CssClass="avatar" AlternateText="Gravatar" />
					<asp:Literal id="PostText" Runat="server" />
				</div>
				
			</div>
		</ItemTemplate>

	</asp:Repeater>
</div>
