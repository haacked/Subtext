<%@ Control Language="c#" AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.Comments" %>
<div id="comments" class="section">
	<h2 class="section-title">Your Comments.</h2>
	<p><asp:Literal ID = "NoCommentMessage" Runat ="server" /></p>
	<asp:Repeater ID="CommentList" runat="server" OnItemCreated="CommentsCreated" OnItemCommand="RemoveComment_ItemCommand">
	    <HeaderTemplate><ul class="comment-list" id="commentList"></HeaderTemplate>
	    <ItemTemplate>
	        <li class="comment normal-comment">
	            <div class="comment-body">
		            <cite>
			            <strong>
				            <asp:Literal ID="Title" Runat="server" />
			            </strong>
		            </cite>
		            <p><asp:Image runat="server" id="GravatarImg" visible="False" CssClass="avatar" PlaceHolderImage="~/images/shadow.gif" AlternateText="Gravatar" /><asp:Literal ID="PostText" Runat="server" /></p>
	            </div>
	            <div class="comment-date">Left by <asp:HyperLink Target="_blank" Runat="server" ID="NameLink" /> at <asp:Literal id = "PostDate" Runat = "server" /></div>
	            <asp:LinkButton Runat="server" cssclass="editlink" ID="EditLink" CausesValidation="False" />
            </li>
	    </ItemTemplate>
	    <AlternatingItemTemplate>
	        <li class="comment alternate-comment">
	            <div class="comment-body">
		            <cite>
			            <strong>
				            <asp:Literal ID="Title" Runat="server" />
			            </strong>
		            </cite>
 		            <p class="commentText"><asp:Image runat="server" id="GravatarImg" visible="False" CssClass="avatar" PlaceHolderImage="~/images/shadow.gif" AlternateText="Gravatar" /><asp:Literal ID="PostText" Runat="server" /></p>
	            </div>
	            <div class="comment-date">Left by <asp:HyperLink Target="_blank" Runat="server" ID="NameLink" /> at <asp:Literal id = "PostDate" Runat = "server" /></div>
	            <asp:LinkButton Runat="server" cssclass="editlink" ID="EditLink" CausesValidation="False" />
            </li>
	    </AlternatingItemTemplate>
	    <FooterTemplate></ul></FooterTemplate>
	</asp:Repeater>    
</div>