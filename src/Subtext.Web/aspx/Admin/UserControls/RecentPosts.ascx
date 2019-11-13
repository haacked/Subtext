<%@ Control Language="C#" Inherits="Subtext.Web.UI.Controls.RecentPosts" %>
<st:RepeaterWithEmptyDataTemplate id="postList" Runat="server" OnItemCreated="PostCreated">
    <HeaderTemplate>
		    <table>
		        <tr>
		            <th>Title</th>
		            <th>Comments</th>
		        </tr>
    </HeaderTemplate>
    <ItemTemplate>
		        <tr>
		            <td><a href="<%# Url.EntryUrl(Entry) %>" title="Recent post"><%# Entry.Title %></a></td>
		            <td><%# Entry.FeedBackCount %></td>
		        </tr>
    </ItemTemplate>
    <FooterTemplate>
		    </table>
    </FooterTemplate>
    <EmptyDataTemplate>
        <p>No recent posts. Go create one now!</p>
    </EmptyDataTemplate>
</st:RepeaterWithEmptyDataTemplate>
