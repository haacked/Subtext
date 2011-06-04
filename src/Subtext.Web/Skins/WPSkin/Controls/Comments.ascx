<%@ Control Language="c#" AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.Comments" %>
<a name="feedback"></a>
<div id="comments">
	<h2>Comments on this post</h2>
	<asp:Literal ID="NoCommentMessage" Runat="server" />
	<asp:Repeater id="CommentList" runat="server" OnItemCreated="CommentsCreated">
		<ItemTemplate>
			<a name="<%# Comment.Id %>"></a>
			<div class="target comment<%# AuthorCssClass %>">
				<h3>
					<asp:HyperLink Runat="server" ID="EditCommentImgLink" /><a href="<%# Comment.DisplayUrl %>" title="permalink">#</a> <asp:Literal Runat="server" ID="title" Text="<%# Comment.Title %>" />
				</h3>
				<div class="commentText">
					<div class="gravatar">
						<asp:Image runat="server" id="GravatarImg" visible="False" CssClass="gravatar" Width="40" Height="40" AlternateText="Requesting Gravatar..." />
					</div>
					<div class="commentBody">
						<asp:Literal id="PostText" Runat="server" />
					</div>
				</div>
				<div class="commentInfo">
					Left by <asp:HyperLink Target="_blank" Runat="server" ID="NameLink" /> 
					on 
					<asp:Literal id="commentDate" Runat="server" Text='<%# Comment.DateCreated.ToString("MMM dd, yyyy h:mm tt") %>' />
				</div>
				<span class="admin-only">
                    <% if(Request.IsAuthenticated && User.IsAdministrator()) {%>
                        <strong class="undoable"><a href="#<%#Comment.Id %>" class="Deleted">Remove Comment</a></strong>
                        | <strong class="undoable"><a href="#<%#Comment.Id %>" class="FlaggedAsSpam">Flag as Spam</a></strong>
                    <% } %>
			    </span>
			</div>
		</ItemTemplate>
	</asp:Repeater>
</div>
