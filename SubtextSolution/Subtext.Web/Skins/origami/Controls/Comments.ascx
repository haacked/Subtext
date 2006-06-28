<%@ Control Language="c#" AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.Comments" %>
<div id="comments" class="section">
	<h2 class="section-title">Your Comments.</h2>
	<p><asp:Literal ID = "NoCommentMessage" Runat ="server" /></p>
	<asp:Repeater ID="CommentList" runat="server" OnItemCreated="CommentsCreated" OnItemCommand="RemoveComment_ItemCommand">
	    <HeaderTemplate><ol class="comment-list" id="commentListing"></HeaderTemplate>
	    <ItemTemplate>
	        <li class="comment normal-comment">
	            <div class="comment-body">
		            <cite>
			            <strong>
				            <asp:Literal ID="Title" Runat="server" />
			            </strong>
		            </cite>
 		            <p><asp:Literal ID="PostText" Runat="server" /></p>
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
 		            <p><asp:Literal ID="PostText" Runat="server" /></p>
	            </div>
	            <div class="comment-date">Left by <asp:HyperLink Target="_blank" Runat="server" ID="NameLink" /> at <asp:Literal id = "PostDate" Runat = "server" /></div>
	            <asp:LinkButton Runat="server" cssclass="editlink" ID="EditLink" CausesValidation="False" />
            </li>
	    </AlternatingItemTemplate>
	    <FooterTemplate></ol></FooterTemplate>
	</asp:Repeater>    
</div>