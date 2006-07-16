<%@ Control Language="c#" AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.Comments" %>
<a name="feedback" title="feedback anchor"></a>
<div id="comments">
	<h2>Feedback</h2>
	<asp:Literal ID="NoCommentMessage" Runat="server" />
	<asp:Repeater id="CommentList" runat="server" OnItemCreated="CommentsCreated" OnItemCommand="RemoveComment_ItemCommand">
		<ItemTemplate>
			<div class="rbroundbox">
				<div class="rbtop">
					<div>
					</div>
				</div>
            
				<div class="rbcontent blogpost comment">
					<h4><span class="title"><asp:Literal Runat = "server" ID = "Title" /></span>
						<span class="adminLink"><asp:LinkButton Runat="server" ID="EditLink" CausesValidation="False" title="edit comment" /></span>
					</h4>
					<asp:Image runat="server" id="GravatarImg" visible="False" CssClass="avatar" />
					<div class="commentText"><asp:Literal id="PostText" Runat="server" /></div>
					<div class="postfoot commentInfo">
						<asp:Literal id="PostDate" Runat="server" /> | <asp:HyperLink Target="_blank" Runat="server" ID="NameLink" title="Date posted" />
					</div>
				</div>
				
				<div class="rbbot">
					<div>
					</div>
				</div>
			</div>
		</ItemTemplate>
	</asp:Repeater>
</div>