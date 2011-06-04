<%@ Control Language="C#" EnableTheming="false"  AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.Comments" %>

<a name="feedback" title="feedback anchor"></a>
<div id="comments">
	<h3>Comments on this entry:</h3>
	<p><asp:Literal ID = "NoCommentMessage" Runat ="server" /></p>
	<asp:Repeater id="CommentList" runat="server" OnItemCreated="CommentsCreated">
		<ItemTemplate>
			<a name="<%# Comment.Id %>"></a>
			<div class="target comment<%# AuthorCssClass %>">
				<div class="commentHeader">
				    <asp:Image runat="server" id="GravatarImg" visible="False" CssClass="avatar" AlternateText="Gravatar" />
					<strong><asp:Literal Runat="server" ID="Title" /></strong>
					<div class="info">by <asp:HyperLink Target="_blank" Runat="server" ID="NameLink" /> at <asp:Literal id="PostDate" Runat="server" /></div>
					<hr style="clear:left; visibility:hidden;" />
				</div>
				<div class="post">
					<asp:Literal id="PostText" Runat="server" />
				</div>
				<asp:HyperLink Runat="server" ID="EditCommentTextLink" /> 
				<% if(Request.IsAuthenticated && User.IsAdministrator()) {%>
				    | <strong class="undoable"><a href="#<%#Comment.Id %>" class="Deleted">Remove Comment</a></strong>
				    | <strong class="undoable"><a href="#<%#Comment.Id %>" class="FlaggedAsSpam">Flag as Spam</a></strong>
				<% } %>
			</div>
		</ItemTemplate>
	</asp:Repeater>
</div>


