<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.Header" %>
<%@ Import namespace="Subtext.Framework.Configuration"%>
<div id="header">
    <div id="headerimg">
        <h1><asp:HyperLink id="HeaderTitle" runat="server" /></h1>
        <div class="description"><asp:Literal id="HeaderSubTitle" runat="server" /></div>
    </div>
    <ul id="nav">
        <li class="page_item"><a id="A5" href="<%= Config.CurrentBlog.RootUrl %>" title="Home Page">Home</a></li>
        <li class="page_item"><a id="A6" href="<%= Config.CurrentBlog.RootUrl %>archives.aspx" title="Archives">Archives</a></li>
        <li class="page_item"><a id="A7" href="<%= Config.CurrentBlog.RootUrl %>contact.aspx" title="Contact Us">Contact</a></li>
    </ul>
</div>