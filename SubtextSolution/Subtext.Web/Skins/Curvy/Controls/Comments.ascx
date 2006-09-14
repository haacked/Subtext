<%@ Control Language="c#" AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.Comments" %>
<a name="feedback" title="feedback anchor"></a>
<div class="comments">
	<h2>Comments</h2>
	<asp:literal id="NoCommentMessage" runat="server" />
	<p></p>
	<asp:repeater id="CommentList" runat="server" onitemcreated="CommentsCreated" onitemcommand="RemoveComment_ItemCommand">
		<itemtemplate>
		<div class="comment">
			<div class="title">
				<asp:literal runat="server" id="Title" />&nbsp;&nbsp;&nbsp;&nbsp;<asp:linkbutton runat="server" id="EditLink" causesvalidation="False" visible="false" CssClass="editComment"/>
			</div>
			
			<div class="content">
				<table border="0" width="99.5%" id="table1">
					<tr>
						<td width="55px"><asp:Image runat="server" id="GravatarImg" visible="False" width="50px" CssClass="avatar"></asp:Image></td>
						<td width="99.5%"><p class="body"><asp:Literal id = "PostText" Runat = "server" /></p></td>
					</tr>
				</table>
			</div>
			<div class="author">
				Posted by&nbsp;&nbsp;
				<asp:hyperlink runat="server" id="NameLink" /> on&nbsp;<asp:literal id="PostDate" runat="server" />
			</div>
		</div>
		</itemtemplate>
	</asp:repeater>
</div>
