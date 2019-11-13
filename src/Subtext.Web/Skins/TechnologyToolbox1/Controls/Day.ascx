<%@ Control Language="C#" EnableTheming="false" Inherits="Subtext.Web.UI.Controls.Day" %>
<%@ Register TagPrefix="uc1" TagName="PostCategoryList" Src="PostCategoryList.ascx" %>
<!-- Begin: Day.ascx -->
<asp:Repeater runat="server" runat="server" ID="DayList" OnItemCreated="PostCreated">
    <ItemTemplate>
        <div class="hentry">
            <h2 class="entry-title">
                <asp:HyperLink runat="server" ID="TitleUrl" rel="bookmark" />
                <asp:HyperLink runat="server" ID="editLink" /></h2>
            <ul class="post-info">
                <li class="published"><span class="label">Published </span><span class="value"><%# Eval(
                    "DatePublishedUtc",
                    "{0:MMMM d, yyyy}") %></span><span class="label"> at </span>
                    <span class="value"><%# Eval(
                    "DatePublishedUtc",
                    "{0:t}") %></span></li>
                <li class="vcard author">by <span class="fn"><%# Eval("Author") %></span></li>
                <li class="comments<%# (int)Eval("FeedBackCount") == 0 ? " none" : string.Empty %>">
                    <asp:HyperLink runat="server" NavigateUrl='<%# string.Format(
                            "{0}#postComments",
                            Url.EntryUrl((IEntryIdentity)Container.DataItem)) %>'>
                        <span class="label">Comments: </span><span class="value count"><%# Eval(
                            "FeedBackCount") %></span></asp:HyperLink></li>
                <li class="categories">
                    <uc1:PostCategoryList ID="Categories" runat="server" />
                </li>
            </ul>
            <div class="entry-summary">
                <p><asp:Literal runat="server" ID="PostText" /></p>
            </div>
        </div>
    </ItemTemplate>
</asp:Repeater>
<!-- End: Day.ascx -->
