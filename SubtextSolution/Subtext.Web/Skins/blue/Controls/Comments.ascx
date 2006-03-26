<%@ Control Language="c#" AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.Comments" %>
<a name="feedback" title="feedback anchor"></a>
<div class = "moreinfo">
	<div class = "moreinfotitle">
		Comments
	</div>
		<asp:Literal ID = "NoCommentMessage" Runat ="server" />
	<asp:Repeater id="CommentList" runat="server" OnItemCreated="CommentsCreated" OnItemCommand="RemoveComment_ItemCommand">
		<HeaderTemplate>
			<ul class = "morelist">
		</HeaderTemplate>
		<ItemTemplate>
			<li class = "morelistitem">
				<div class = "moreinfotitle">
					<asp:Literal Runat = "server" ID = "Title" />
				</div>
				<asp:HyperLink Target="_blank" Runat="server" ID="NameLink" /><br />
				Posted @ <asp:Literal id = "PostDate" Runat = "server" /><br />
				<asp:Literal id = "PostText" Runat = "server" />
				<asp:LinkButton Runat="server" ID="EditLink" CausesValidation="False" />
			</li>
		</ItemTemplate>
		<FooterTemplate>
			</ul>
		</FooterTemplate>
	</asp:Repeater>
</div>


