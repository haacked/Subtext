<%@ Control Language="C#" AutoEventWireup="true" Inherits="Subtext.Web.UI.Controls.AggBloggers" %>
<%@ Import Namespace="Subtext.Framework.Providers" %>
<%
    var blogs = ObjectProvider.Instance().GetBlogsByGroup(Blog.AggregateBlog.Host, BlogGroup);
    System.Collections.Generic.ICollection<BlogGroup> groups;
    if (ShowGroups)
    {
        groups = ObjectProvider.Instance().GroupBlogs(blogs);
    }
    else {
        groups = new System.Collections.Generic.List<BlogGroup>();
        groups.Add(new BlogGroup {Blogs = blogs});
    }
%>
<div id="aggbloggers">
<h2>Bloggers (posts, last update)</h2>
<% foreach (var group in groups) { 
     if(ShowGroups) {%>
  <ul>
    <li>
      <h3><%= group.Title %></h3>
  <% } %>
      <ul>
      <% foreach (var blog in group.Blogs) { %>
        <li>
          <div>
            <a href="<%= GetFullUrl(blog.Host, blog.Subfolder) %>" title="<%= blog.Author %>"><%= blog.Author%></a>
          </div>
          <div>
            <%= blog.PostCount%>, <%= blog.LastUpdated.ToString("MM/dd/yyyy h:mm tt", System.Globalization.CultureInfo.CurrentCulture)%>
          </div>
        </li>
        <% } %>
        </ul>
    <% if(ShowGroups) { %>
      </li>
  </ul>
  <% } %>
<% } %>
</div>
