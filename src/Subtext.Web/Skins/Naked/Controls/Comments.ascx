<%@ Control Language="C#" EnableTheming="false"  AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.Comments" %>
<a name="feedback" title="feedback anchor"></a>
<div class="comments">
	<h2>Comments</h2>
	<asp:literal id="NoCommentMessage" runat="server" />
	<asp:repeater id="CommentList" runat="server" onitemcreated="CommentsCreated">
		<ItemTemplate>
		    <div class="target comment">
		        <asp:Image runat="server" id="GravatarImg" visible="False" CssClass="avatar" AlternateText="Gravatar" />
			    <div class="title">
				    <asp:HyperLink Runat="server" ID="EditCommentImgLink" /><asp:literal runat="server" id="Title" />
			    </div>
			    <div class="author">Posted by
				    <asp:hyperlink runat="server" id="NameLink" /></div>
			    <div class="postedDate">on
				    <asp:literal id="PostDate" runat="server" /></div>
		            <% if(Request.IsAuthenticated && User.IsAdministrator()) {%>
		                <strong class="undoable"><a href="#<%#Comment.Id %>" class="Deleted">Remove Comment</a></strong>
		                | <strong class="undoable"><a href="#<%#Comment.Id %>" class="FlaggedAsSpam">Flag as Spam</a></strong>
		            <% } %>

			    <div class="content">
				    <asp:literal id="PostText" runat="server" />
			    </div>
		    </div>
		</ItemTemplate>
	</asp:repeater>
</div>
