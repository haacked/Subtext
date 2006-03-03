<%@ Control Language="c#" AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.Comments" %>
<a name = "feedback" />
<div id="comments">
<h3>Feedback</h3>
	<asp:Literal ID = "NoCommentMessage" Runat ="server" />
	<asp:Repeater id="CommentList" runat="server" OnItemCreated="CommentsCreated" OnItemCommand="RemoveComment_ItemCommand">
		<ItemTemplate>
			<h4>
				<asp:Literal Runat = "server" ID = "Title" />
					<span>
						<asp:Literal id = "PostDate" Runat = "server" />
					</span>
				<asp:HyperLink Target="_blank" Runat="server" ID="NameLink" />
			</h4>
			<p>
				<asp:Literal id = "PostText" Runat = "server" />
				<asp:LinkButton Runat="server" ID="EditLink" CausesValidation="False" />
			</p>
		</ItemTemplate>

	</asp:Repeater>
</div>