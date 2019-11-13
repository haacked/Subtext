<%@ Control Language="C#" AutoEventWireup="true" Inherits="Subtext.Web.UI.Controls.AggRecentPosts" %>
<div id="aggrecentposts">
    <h2>Latest Posts</h2>
    <asp:repeater id="RecentPosts" runat="server">
        <ItemTemplate>
            <div class="post">
                <h3 class="title">
                    <asp:HyperLink Runat="server" NavigateUrl='<%# EntryUrl(Container.DataItem) %>' Text='<%# H(Get<Entry>(Container.DataItem).Title) %>' ID="Hyperlink2"/>
                </h3>
                <div class="body">
                    <asp:Literal runat="server" Text='<%# Get<Entry>(Container.DataItem).Description %>' ID="Label4"/>
                </div>
                <p class="postfoot">
                    posted @
                    <asp:Literal runat="server" Text='<%# Get<Entry>(Container.DataItem).DateSyndicated.ToShortDateString() + " " + Get<Entry>(Container.DataItem).DateSyndicated.ToLocalTime().ToShortTimeString() %>' ID="Label5" />
                    by
                    <asp:HyperLink Runat="server" CssClass="clsSubtext" NavigateUrl='<%# BlogUrl(Get<Entry>(Container.DataItem).Blog) %>' Text='<%# Get<Entry>(Container.DataItem).Author %>' ID="Hyperlink3"/>
                </p>
            </div>
        </ItemTemplate>
    </asp:repeater>
</div>
