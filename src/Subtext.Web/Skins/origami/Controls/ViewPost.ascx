<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.ViewPost" %>
<%@ Register TagPrefix="uc1" TagName="PreviousNext" Src="PreviousNext.ascx" %>

<div class="post normal-post"> <asp:HyperLink ID="editLink" runat="server" Visible="false" /> <asp:HyperLink Runat="server" ID="editInWlwLink" /> 
    <uc1:PreviousNext id="PreviousNext" runat="server" />
    <h1 class="post-title"><asp:HyperLink  Runat="server" ID="TitleUrl" /></h1>
    <div class="post-content"><br/><asp:Literal  runat="server" ID="Body" /></div>
    <p class="meta"><asp:Literal runat="server" ID="PostDescription" /></p>    
</div>
<asp:Literal ID="PingBack" Runat="server" />
<asp:Literal ID="TrackBack" Runat="server" />