<%@ Control Language="c#" AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.Comments" %>
<a name="feedback" title="feedback anchor"></a>
<div id="comments">
	<h2>Feedback</h2>
	<asp:Literal ID="NoCommentMessage" Runat="server" />
	<asp:Repeater id="CommentList" runat="server" OnItemCreated="CommentsCreated" OnItemCommand="RemoveComment_ItemCommand">
		<ItemTemplate>
			<div class="blogpost">
				<h2><span class="title"><asp:Literal Runat = "server" ID = "Title" /></span>
					<span class="adminLink"><asp:LinkButton Runat="server" ID="EditLink" CausesValidation="False" title="edit comment" /></span>
				</h2>
				<asp:Literal id = "PostText" Runat = "server" />
				<div class="postfoot">
					<asp:Literal id = "PostDate" Runat = "server" /> | <asp:HyperLink Target="_blank" Runat="server" ID="NameLink" title="Date posted" />
				</div>
			</div>
		</ItemTemplate>
	</asp:Repeater>
</div>