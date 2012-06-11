<%@ Control Language="C#" AutoEventWireup="true" Inherits="Subtext.Web.UI.Controls.AggBlogStats" %>
<div id="aggblogstats">
<h2>Blog Stats</h2>
<ul>
    <li>Blogs -	<asp:Literal ID="BlogCount" Runat="server" /></li>
    <li>Posts - <asp:Literal ID="PostCount" Runat="server" /></li>
    <li>Articles - <asp:Literal ID="StoryCount" Runat="server" /></li>
    <li>Comments - <asp:Literal ID="CommentCount" Runat="server" /></li>
    <li>Trackbacks - <asp:Literal ID="PingtrackCount" Runat="server" /></li>
</ul>
</div>
