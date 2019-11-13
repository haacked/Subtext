<%@ Control Language="C#" EnableTheming="false"  AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.Comments" %>
<div class="post">
<a name="feedback"></a>
<div class="moreinfo">
	<div class="moreinfotitle">
		Comments
	</div>
	<asp:Literal ID = "NoCommentMessage" Runat ="server" />
	<asp:Repeater id="CommentList" runat="server" OnItemCreated="CommentsCreated">
		<HeaderTemplate>
			<div class="comments">
		</HeaderTemplate>
		<ItemTemplate>
		    <div class="target">
		        <asp:Image runat="server" id="GravatarImg" visible="False" CssClass="avatar" AlternateText="Gravatar" />
			    <a name="<%# Comment.Id %>"></a>
			    <div class="comment<%# AuthorCssClass %>">
				    <div class="comment_title">
					    <asp:HyperLink Runat="server" ID="EditCommentImgLink" /><asp:Literal Runat="server" ID="Title" />
				    </div>
				    <div class="comment_author"><asp:HyperLink Target="_blank" Runat="server" ID="NameLink" /></div>
				    <div class="comment_content"><asp:Literal id="PostText" Runat="server" /></div>
				    <div class="itemdesc">Posted @ <asp:Literal id="PostDate" Runat="server" /> 
				        <% if(Request.IsAuthenticated && User.IsAdministrator()) {%>
				            | <strong class="undoable"><a href="#<%#Comment.Id %>" class="Deleted">Remove Comment</a></strong>
				            | <strong class="undoable"><a href="#<%#Comment.Id %>" class="FlaggedAsSpam">Flag as Spam</a></strong>
				        <% } %>
				    </div>
    				
			    </div>
			</div>
		</ItemTemplate>
		<FooterTemplate>
			</div>
		</FooterTemplate>
	</asp:Repeater>
</div>
</div>
<div class="seperator">&nbsp;</div>
