<%@ Control Language="C#" EnableTheming="false"  AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.Comments" %>
<a name="feedback" title="feedback anchor"></a>
<div id="comments">
<h3>Feedback</h3>
	<asp:Literal ID="NoCommentMessage" Runat="server" />
	<asp:Repeater id="CommentList" runat="server" OnItemCreated="CommentsCreated">
		<ItemTemplate>
		    <asp:Image runat="server" id="GravatarImg" visible="False" CssClass="avatar" AlternateText="Gravatar" />
			<div class="target post">
				<h2>
					<asp:Literal Runat="server" ID="Title" />
				</h2>
				<asp:Literal id="PostText" Runat="server" />
				<div class="postfoot">
					<asp:Literal id="PostDate" Runat = "server" /> | <asp:HyperLink Target="_blank" Runat="server" ID="NameLink" /> 
					<% if(Request.IsAuthenticated && User.IsAdministrator()) {%>
					    | <strong class="undoable"><a href="#<%#Comment.Id %>" class="Deleted">Remove Comment</a></strong>
					    | <strong class="undoable"><a href="#<%#Comment.Id %>" class="FlaggedAsSpam">Flag as Spam</a></strong>
					<% } %>
				</div>
			</div>
		</ItemTemplate>

	</asp:Repeater>
</div>