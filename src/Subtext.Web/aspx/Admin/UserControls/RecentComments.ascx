<%@ Control Language="C#" Inherits="Subtext.Web.UI.Controls.RecentComments" %>

<st:RepeaterWithEmptyDataTemplate ID="feedList" runat="server" OnItemCreated="EntryCreated" OnItemDataBound="OnItemBound">
    <HeaderTemplate>
        <ul class="comments">
    </HeaderTemplate>
    <ItemTemplate>
        <li class="target recent-comment<%# AlternatingCssClass %>">
            <img src="<%# H(Gravatar.GenerateUrl(Comment.Email)) %>" alt="commenter gravatar icon" class="gravatar" />
            <div class="info">
                <span class="meta">
                    From <span class="author"><asp:Literal runat="server" Text="<%# H(Comment.Author) %>" /></span>
                    <asp:Literal runat="server" Text="<%# H(Comment.Title) %>" />
                </span> 
                <div class="comment">
                    <asp:Literal runat="server" Text="<%# SafeCommentBody %>" />
                </div>
                <div class="actions">
                    <ul class="inline">
                        <%--<li><a href="#">Unnapprove</a></li>--%>
                        <li><a href="<%# H(EditUrl(Comment)) %>" class="edit" title="Edit Comment">Edit</a></li>
                        <li><a href="<%# Url.FeedbackUrl(Comment) %>" class="reply" title="Reply to this comment">Reply</a></li>
                        <li class="undoable"><a href="#<%# Comment.Id %>" class="FlaggedAsSpam">Spam</a></li>
                        <li class="undoable"><a href="#<%# Comment.Id %>" class="Deleted" title="Move to trash">Delete</a></li>
                    </ul>
                </div>
            </div>
        </li>
    </ItemTemplate>
    <FooterTemplate>
        </ul>
        <div class="footer"><a href="<%= AdminUrl.FeedbackList() %>" title="View All Comments">View All</a></div>
    </FooterTemplate>
    <EmptyDataTemplate>
        <p>No comments. Write something worth commenting about. :)</p>
    </EmptyDataTemplate>
</st:RepeaterWithEmptyDataTemplate>

