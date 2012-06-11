<%@ Control Language="C#" EnableTheming="false"  AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.Comments" %>
<a name="feedback" title="feedback anchor"></a>
<div class="comments">
	<h2>Comments</h2>
	<asp:literal id="NoCommentMessage" runat="server" />
	<asp:repeater id="CommentList" runat="server" onitemcreated="CommentsCreated">
		<itemtemplate>
		<div class="comment">
		    <asp:Image runat="server" id="GravatarImg" visible="False" CssClass="avatar" AlternateText="Gravatar" />
			<div class="title">
				<asp:HyperLink Runat="server" ID="EditCommentImgLink" /><asp:literal runat="server" id="Title" />
			</div>
			<div class="author">Posted by
				<asp:hyperlink runat="server" id="NameLink" /></div>
			<div class="postedDate">on
				<asp:literal id="PostDate" runat="server" /></div>
			<asp:linkbutton runat="server" id="EditLink" causesvalidation="False" />
			<div class="content">
				<asp:literal id="PostText" runat="server" />
			</div>
		</div>
		</itemtemplate>
	</asp:repeater>
</div>
