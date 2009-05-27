<%@ Control Language="C#" Inherits="Subtext.Web.UI.Controls.PopularPosts" %>
<st:RepeaterWithEmptyDataTemplate id="postList" Runat="server" OnItemCreated="PostCreated">
    <HeaderTemplate>
            <div class="filter">
                <% if (FilterType == DateFilter.None) { %>All Time<% } else {%><a href="?popular-posts=None">All Time</a><% }%> | 
                <% if (FilterType == DateFilter.LastMonth) { %>Last Month<% } else {%><a href="?popular-posts=LastMonth">Last Month</a><% }%> | 
                <% if (FilterType == DateFilter.LastWeek) { %>Last Week<% } else {%><a href="?popular-posts=LastWeek">Last Week</a><% }%>
            </div>
		    <table>
		        <tr>
		            <th>Title</th>
		            <th>Comments</th>
		            <th>Web</th>
		            <th>Agg</th>
		        </tr>
    </HeaderTemplate>
    <ItemTemplate>
		        <tr>
		            <td><a href="<%# Url.EntryUrl(Entry) %>" title="Recent post"><%# Entry.Title %></a></td>
		            <td><%# Entry.FeedBackCount %></td>
		            <td><%# Entry.WebCount %></td>
		            <td><%# Entry.AggCount %></td>
		        </tr>
    </ItemTemplate>
    <FooterTemplate>
		    </table>
    </FooterTemplate>
    <EmptyDataTemplate>
        <p>No recent posts. Go create one now!</p>
    </EmptyDataTemplate>
</st:RepeaterWithEmptyDataTemplate>
