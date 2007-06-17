<%@ Control Language="C#" EnableTheming="false"  AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.Comments" %>
<a name="feedback" title="feedback anchor"></a>
<h1>What others have said</h1>
<asp:Literal ID="NoCommentMessage" Runat="server" />
<asp:Repeater id="CommentList" runat="server" OnItemCreated="CommentsCreated" OnItemCommand="RemoveComment_ItemCommand">
	<ItemTemplate>
		<a name="<%# Comment.Id %>"></a>
		<div class="comment<%# AuthorCssClass %>">
			<h2><a href="<%# Comment.DisplayUrl %>" title="permalink"><asp:Literal Runat="server" ID="title" Text="<%# Comment.Title %>" /></a></h2>
			<div class="commentInfo">
				Left by <asp:HyperLink Target="_blank" Runat="server" ID="NameLink" /> 
				on 
				<asp:Literal id="commentDate" Runat="server" Text='<%# Comment.DateCreated.ToString("MMM dd, yyyy h:mm tt") %>' />
			</div>
			<div class="commentText">
				<div class="gravatar">
					<asp:Image runat="server" id="GravatarImg" visible="False" CssClass="gravatar" Width="80" Height="80" AlternateText="Requesting Gravatar..." />
				</div>
				<div class="commentBody">
					<asp:Literal id="PostText" Runat="server" />
				</div>
			</div>
			<span class="admin-only"><asp:LinkButton Runat="server" ID="EditLink" CausesValidation="False" ToolTip="Remove comment" /></span>
		</div>
	</ItemTemplate>
</asp:Repeater>
