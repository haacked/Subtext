<%@ Control Language="c#" AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.Comments" %>

<a name="feedback"></a>
<div id="comments">
	<h3>Comments on this entry:</h3>
	<p><asp:Literal ID = "NoCommentMessage" Runat ="server" /></p>
	<asp:Repeater id="CommentList" runat="server" OnItemCreated="CommentsCreated" OnItemCommand="RemoveComment_ItemCommand">
		<ItemTemplate>
			<div class="comment">
					<h4>
						<asp:Literal Runat = "server" ID = "Title" />
						<asp:LinkButton Runat="server" ID="EditLink" CausesValidation="False" visible="false"/>
					</h4>
					<p class="body">
						<asp:Literal id = "PostText" Runat = "server" />
					</p>
						<div class="info">
							Left by <asp:HyperLink Target="_blank" Runat="server" ID="NameLink" /> 
							at <asp:Literal id = "PostDate" Runat = "server" />
						</div>
			</div>
		</ItemTemplate>
	</asp:Repeater>
</div>

