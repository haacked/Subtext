<%@ Control Language="C#" AutoEventWireup="true" Inherits="Subtext.Web.UI.Controls.AggBloggers" %>

<div id="aggbloggers">
    <h2>
        Bloggers (posts, last update)</h2>
    <% foreach (var group in BlogGroups) {
           if (ShowGroups) {%>
    <ul>
        <li>
            <h3>
                <%= H(group.Title) %></h3>
            <% } %>
            <ul>
                <% foreach (var blog in group.Blogs) { %>
                <li>
                    <div>
                        <a href="<%= BlogUrl(blog) %>" title="<%= H(blog.Author) %>">
                            <%= H(blog.Author) %></a>
                    </div>
                    <div>
                        <%= blog.PostCount%>,
                        <%= blog.DateModifiedUtc.ToLocalTime().ToString("MM/dd/yyyy h:mm tt", System.Globalization.CultureInfo.CurrentCulture)%>
                    </div>
                </li>
                <% } %>
            </ul>
            <% if (ShowGroups) { %>
        </li>
    </ul>
    <% } %>
    <% } %>
</div>
