<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.ViewPost" %>
<div class="post normal-post"> <asp:HyperLink ID="editLink" runat="server" Visible="false" />
    <h1 class="post-title"><asp:HyperLink  Runat="server" ID="TitleUrl" /></h1>
    <div class="post-content"><p><asp:Literal  runat="server" ID="Body" /></p></div>
    <p class="meta"><asp:Literal runat="server" ID="PostDescription" /></p>
    <!--<p class="meta">Tags: <a href="#" rel="tag">Tag1</a></p>-->
</div>