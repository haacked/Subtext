<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.ViewPost" %>
<%@ Register TagPrefix="uc1" TagName="PreviousNext" Src="PreviousNext.ascx" %>
<uc1:PreviousNext ID="PreviousNext" runat="server" />
<div class="post">
    <div class="post-date"><span class="post-month"><asp:Label ID="postDate" CssClass="postTitleDate" runat="server" Format="MMM" /></span> <span class="post-day"></span></div>
    <div class="entry"><h2><asp:HyperLink runat="server" ID="editLink" /> <asp:HyperLink Runat="server" ID="editInWlwLink" /> <asp:HyperLink CssClass="singleposttitle" runat="server" ID="TitleUrl" /></h2></div>
    <span class="post-comments"><asp:Literal id="PostDescription"  runat="server" /></span>
    <div class="post-content"><asp:Literal ID="Body" runat="server" /></div>
</div>
<asp:Literal ID="TrackBack" runat="server" />