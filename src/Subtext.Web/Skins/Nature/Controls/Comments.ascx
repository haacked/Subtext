<%@ Control Language="C#" EnableTheming="false"  AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.Comments" %>
<a name="feedback" title="feedback anchor"></a>
<div id="comments">
<h2>Feedback</h2>
	<asp:Literal ID = "NoCommentMessage" Runat ="server" />
	<asp:Repeater id="CommentList" runat="server" OnItemCreated="CommentsCreated">
		<ItemTemplate>
			<a name="<%# Comment.Id %>"></a>
			<div class="target comment<%# AuthorCssClass %>">
				<h3>
				    <asp:HyperLink Runat="server" ID="EditCommentImgLink" /><asp:Literal Runat="server" ID="Title" /> 
				</h3>
				<div class="commentInfo">Left by <asp:HyperLink Target="_blank" Runat="server" ID="NameLink" /> at <asp:Literal id="PostDate" Runat="server" /></div>
				<div class="commentText">
					<asp:Image runat="server" id="GravatarImg" visible="False" CssClass="avatar" AlternateText="Gravatar" />
					<asp:Literal id="PostText" Runat="server" />
				</div>
			    <span class="adminLink">
			        <% if(Request.IsAuthenticated && User.IsAdministrator()) {%>
	                    <strong class="undoable"><a href="#<%#Comment.Id %>" class="Deleted">Remove Comment</a></strong>
	                    | <strong class="undoable"><a href="#<%#Comment.Id %>" class="FlaggedAsSpam">Flag as Spam</a></strong>
	                <% } %>
			    </span>
			</div>
		</ItemTemplate>

	</asp:Repeater>
</div>
