<%@ Control Language="C#" AutoEventWireup="true" Inherits="Subtext.Web.UI.Controls.AggSyndication" %>
<div id="aggsyndication">
    <h2>Syndication</h2>
    <ul>
        <li><asp:HyperLink ID="OpmlLink" Text="OPML (list of bloggers)" runat="server" NavigateUrl="~/opml.xml.ashx" /></li>
        <li><asp:HyperLink ID="RssLink" Text="RSS (list of recent posts)" runat="server" NavigateUrl="~/MainFeed.aspx" /></li>
        <asp:Repeater ID="blogGroupRepeater" runat="server">
            <ItemTemplate>
                <li><asp:HyperLink ID="groupRssLink" Text='<%# Eval("Title", "RSS ({0})") %>' runat="server" NavigateUrl='<%# Eval("Id", "~/MainFeed.aspx?GroupID={0}") %>' /></li>
            </ItemTemplate>
        </asp:Repeater>
    </ul>
</div>
