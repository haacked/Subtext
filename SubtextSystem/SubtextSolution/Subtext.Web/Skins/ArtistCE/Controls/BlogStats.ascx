<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.BlogStats" %>
<h5>Blog Stats</h5>
<div id="stats">
posts - <asp:Literal ID = "PostCount" Runat = "server" /> 
<br />comments - <asp:Literal ID = "CommentCount" Runat = "server" /> 
<br />trackbacks - <asp:Literal ID = "PingTrackCount" Runat = "server" />
<br /><asp:Literal ID = "StoryCount" Runat = "server" Visible="False" />
</div>