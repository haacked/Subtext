<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.News" %>
<%@ Register TagPrefix="uc1" TagName="SubtextSearch" Src="~/Skins/_System/Controls/SearchInput.ascx" %>

<div class="sidebar">
    <div class="side-bar-top">
    </div>
    <div class="side-bar-middle">
        <uc1:SubtextSearch id="search" runat="server" />
        <div id="news">
            <div class="title">
                News
            </div>
            <div class="body">
                <asp:Literal ID="NewsItem" runat="server" />
            </div>
        </div>
    </div>
    <div class="side-bar-bottom">
    </div>
</div>
