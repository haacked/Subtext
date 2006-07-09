<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.BlogStats" %>
<h3>Blog Stats</h3>
<ul>
    <li>Posts - <asp:Literal ID="PostCount" runat="server" /></li>
    <li>Stories - <asp:Literal ID="StoryCount" runat="server" /></li>
    <li>Comments - <asp:Literal ID="CommentCount" runat="server" /></li>
    <li>Trackbacks - <asp:Literal ID="PingTrackCount" runat="server" /></li>
</ul>
