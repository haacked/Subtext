<%@ Control Language="C#" EnableTheming="false" Inherits="Subtext.Web.UI.Controls.BlogStats" %>
<!-- Begin: BlogStats.ascx -->
<div class="blog-stats">
    <h2>
        Blog Stats</h2>
    <ul>
        <li>Posts :
            <asp:Literal runat="server" ID="PostCount" /></li>
        <li>Comments :
            <asp:Literal runat="server" ID="CommentCount" /></li>
        <li>Trackbacks :
            <asp:Literal runat="server" ID="PingTrackCount" /></li>
    </ul>
    <asp:Literal runat="server" ID="StoryCount" Visible="false" />
</div>
<!-- End: BlogStats.ascx -->
