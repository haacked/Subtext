<%@ Control Language="c#" AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.Comments" %>
<a name="feedback" title="feedback anchor"></a>
<div class = "moreinfo">
	<div class = "moreinfotitle">
		Feedback
	</div>
	<asp:Literal ID = "NoCommentMessage" Runat ="server" />
	<asp:Repeater id="CommentList" runat="server" OnItemCreated="CommentsCreated" OnItemCommand="RemoveComment_ItemCommand">
		<ItemTemplate>
			<div class="content">
				<div class="moreinfotitle">
					<asp:Literal Runat = "server" ID = "Title" />
				</div>
				<asp:HyperLink Target="_blank" Runat="server" ID="NameLink" />
				<p class="itemdesc">Posted @ <asp:Literal id = "PostDate" Runat = "server" /></p>
				<asp:Literal id="PostText" Runat="server" />
				<asp:LinkButton Runat="server" ID="EditLink" CausesValidation="False" />
			</div>
		</ItemTemplate>
		
	</asp:Repeater>
</div>


