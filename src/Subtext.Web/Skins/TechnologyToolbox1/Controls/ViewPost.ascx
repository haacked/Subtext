<%@ Control Language="C#" EnableTheming="false" Inherits="Subtext.Web.UI.Controls.ViewPost" %>
<%@ Register TagPrefix="uc1" TagName="previousNext" Src="PreviousNext.ascx" %>
<%@ Register TagPrefix="uc1" TagName="PostCategoryList" Src="PostCategoryList.ascx" %>
<!-- Begin: ViewPost.ascx -->
<div id="blogPost">
    <div class="hentry">
	    <h2 class="entry-title"><%= Entry.Title %><asp:HyperLink runat="server" ID="TitleUrl" Visible="false" />
            <asp:HyperLink runat="server" ID="editLink" /></h2>
	    <ul class="post-info">
		    <li class="published"><span class="label">Published </span><span class="value"><%=
                Entry.DatePublishedUtc.ToString("MMMM d, yyyy") %></span><span class="label"> at </span>
                <span class="value"><%= Entry.DatePublishedUtc.ToString("t") %></span></li>
		    <li class="vcard author">by <span class="fn"><%= Entry.Author %></span></li>
		    <li class="comments<%= Entry.FeedBackCount == 0 ? " none" : string.Empty %>"><a href="#postComments">
                <span class="label">Comments: </span><span class="value count"><%= Entry.FeedBackCount %></span></a></li>
		    <li class="categories">
			    <uc1:PostCategoryList runat="server" ID="Categories" /></li>
	    </ul>
        <div class="entry-content">
            <asp:Literal ID="Body" runat="server" />
        </div>
        <div class="attachment" <%= (Entry.Enclosure == null
                || Entry.Enclosure.ShowWithPost == false) ? "style='display: none'" : "" %>>
            <h3>Attachment</h3>
            <p><asp:Label runat="server" ID="Enclosure" DisplaySize="true" /></p>
        </div>
        <%-- <asp:Literal ID="PingBack" runat="server" /> --%>
        <%-- <asp:Literal ID="TrackBack" runat="server" /> --%>
    </div>
</div>
<!-- End: ViewPost.ascx -->
