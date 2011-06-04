<%@ Control Language="C#" EnableTheming="false"  AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.Comments" %>

<a name="feedback" title="feedback anchor"></a>
<div id="comments">
	<h3>Comments on this entry:</h3>
	<p><asp:Literal ID = "NoCommentMessage" Runat ="server" /></p>
	<asp:Repeater id="CommentList" runat="server" OnItemCreated="CommentsCreated">
		<ItemTemplate>
			<a name="<%# Comment.Id %>"></a>
			<div class="target comment<%# AuthorCssClass %>">
				<h4>
					<asp:HyperLink Runat="server" ID="EditCommentImgLink" /><asp:Literal Runat = "server" ID = "Title" />
				</h4>
				<div class="info">Left by <asp:HyperLink Target="_blank" Runat="server" ID="NameLink" /> at <asp:Literal id = "PostDate" Runat = "server" /></div>
				<div class="gravatar"><asp:Image runat="server" id="GravatarImg" visible="False" width="50px" AlternateText="Gravatar" /></div>
				<div class="post">
					<div>
						<asp:Literal id="PostText" Runat="server" />
					</div>
					<span class="adminLink">
                        <% if(Request.IsAuthenticated && User.IsAdministrator()) {%>
                            <strong class="undoable"><a href="#<%#Comment.Id %>" class="Deleted">Remove Comment</a></strong>
                            | <strong class="undoable"><a href="#<%#Comment.Id %>" class="FlaggedAsSpam">Flag as Spam</a></strong>
                        <% } %>
                    </span>

				</div>
			</div>
		</ItemTemplate>
	</asp:Repeater>
</div>


