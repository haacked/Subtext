<%@ Control Language="c#" AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.Comments" %>

<a name="feedback" title="feedback anchor"></a>
<div id="comments">
	<h3>Comments on this entry:</h3>
	<p><asp:Literal ID = "NoCommentMessage" Runat ="server" /></p>
	<asp:Repeater id="CommentList" runat="server" OnItemCreated="CommentsCreated" OnItemCommand="RemoveComment_ItemCommand">
		<ItemTemplate>
			<div class="comment">
					<h4>
						<asp:Literal Runat = "server" ID = "Title" />
					</h4>
					<div class="info">Left by <asp:HyperLink Target="_blank" Runat="server" ID="NameLink" /> at <asp:Literal id = "PostDate" Runat = "server" /></div>
					<p class="post">
						<asp:Literal id = "PostText" Runat = "server" />
					</p>
					<asp:LinkButton Runat="server" ID="EditLink" CausesValidation="False" />
			</div>
		</ItemTemplate>
	</asp:Repeater>
</div>

