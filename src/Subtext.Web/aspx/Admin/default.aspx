<%@ Page Language="C#" EnableTheming="false" CodeBehind="default.aspx.cs" AutoEventWireup="true" Inherits="Subtext.Web.Admin.Pages.HomePageDefault" %>

<asp:Content ContentPlaceHolderID="head" runat="server">
    <link id="Link1" type="text/css" rel="stylesheet" href="<%= VirtualPathUtility.ToAbsolute("~/aspx/Admin/css/dashboard.css") %>" />
    <script type="text/javascript">
        $(function() {
            $(".undoable a").undoable({
                url: '<%= Url.CommentUpdateStatus() %>',
                getPostData: function(clickSource, target) {
                    var data = this.getPostData(clickSource, target);
                    data.status = clickSource.attr('class');
                    return data;
                }
            });
        });
    </script>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
<div id="content">
    <h2>Dashboard</h2>
    
    <div class="column">
        <div class="container">
            <div class="section" id="summary">
                <h3>Summary</h3>
                <div class="body">
                    <div class="sub-header">
                        What&#8217;s happened on your blog
                    </div>

                    <div class="table">
                        <table>
                            <tr>
                                <td class="stat"><a href="<%= AdminUrl.PostsList() %>" title="Posts"><%# Statistics.ActivePostCount %></a></td>
                                <td><a href="<%= AdminUrl.PostsList() %>" title="Posts">Posts</a></td>
                                <td class="stat"><a href="<%= AdminUrl.FeedbackList() %>" title="Feedback"><%# Statistics.FeedbackCount %></a></td>
                                <td title="Does not include pingbacks/trackbacks"><a href="<%= AdminUrl.FeedbackList() %>" title="Feedback">Comments</a></td>
                            </tr>
                            <tr>
                                <td class="stat"><a href="<%= AdminUrl.ArticlesList() %>" title="Articles"><%# Statistics.ActiveArticleCount %></a></td>
                                <td><a href="<%= AdminUrl.ArticlesList() %>" title="Articles">Articles</a></td>
                                <td class="stat"><a href="<%= AdminUrl.FeedbackList() %>" title="Feedback"><%# Statistics.ApprovedFeedbackCount %></a></td>
                                <td class="approved"><a href="<%= AdminUrl.FeedbackList() %>" title="Feedback">Approved</a></td>
                            </tr>
                            <tr>
                                <td class="stat"><a href="<%= AdminUrl.FullTextSearch()%>"><%# IndexedEntryCount%></a></td>
                                <td><a href="<%= AdminUrl.FullTextSearch()%>">Indexed</a></td>
                                <td class="stat">
                                    <% if(Config.CurrentBlog.ModerationEnabled) { %>
                                    <a href="<%= AdminUrl.FeedbackList() %>?status=NeedsModeration" title="Feedback"><%# Statistics.AwaitingModerationFeedbackCount %></a>
                                    <% } else {%>
                                    n/a
                                    <% } %>
                                </td>
                                <td class="pending">
                                    <% if(Config.CurrentBlog.ModerationEnabled) { %>
                                    <a href="<%= AdminUrl.FeedbackList() %>?status=NeedsModeration" title="Feedback">
                                    <% } %>
                                    Pending
                                    <% if(Config.CurrentBlog.ModerationEnabled) { %>
                                    </a>
                                    <% } %>
                                </td>
                            </tr>
                            <tr>
                                <td class="stat"><%# CategoryCount %></td>
                                <td>Categories</td>

                                <td class="stat"><a href="<%= AdminUrl.FeedbackList() %>?status=FlaggedAsSpam" title="Feedback"><%# Statistics.FlaggedAsSpamFeedbackCount %></a></td>
                                <td title="Flagged as spam, but not confirmed" class="spam"><a href="<%= AdminUrl.FeedbackList() %>?status=FlaggedAsSpam" title="Feedback">Spam</a></td>
                            </tr>
                            <tr>
                                <td class="stat"><%# Blog.PingTrackCount %></td>
                                <td>Pingbacks/Trackbacks</td>   
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>
                        </table>
                    </div>
                    <div class="section-footer">
                        <div class="info">
                            Current Skin: <%# Blog.Skin.SkinKey %> 
                        </div>
                        <div class="action">
                            <a href="<%# Url.AdminUrl("Skins.aspx") %>" title="Change skin">Change Skin</a>
                        </div>
                        <div class="clear">
                        </div>
                    </div>
                </div>
            </div>

            <div class="section" id="frequency">
                <h3>Activity</h3>
                <div class="body">
                    <div class="sub-header">
                        How active your blog is
                    </div>
                
                    <div class="table">
                        <table>
                            <tr>
                                <td class="stat"><%# Statistics.AveragePostsPerMonth %></td>
                                <td>Average Posts Per Month</td>
                                <td class="stat"><%# Statistics.AverageCommentsPerMonth %></td>
                                <td>Average Comments Per Month</td>
                            </tr>
                            <tr>
                                <td class="stat"><%# Statistics.AveragePostsPerWeek %></td>
                                <td>Average Posts Per Week</td>
                                <td class="stat"><%# Statistics.AverageCommentsPerPost %></td>
                                <td>Average Comments Per Post</td>
                            </tr>
                        </table>
                    </div>
                    <div class="section-footer"></div>
                </div>
            </div>


            <div class="section" id="recent-comments">
                <h3>Recent Comments</h3>
                
                <div class="body">
                    <div class="sub-header">
                        What readers are saying
                    </div>
                    <st:RecentComments id="recentComments" runat="server" />
                </div>
                <div class="section-footer"></div>
            </div>

            
        </div>
    </div>
    <div class="column">
        <div class="container">
            <div class="section" id="recent-posts">
                <h3>Recent Posts</h3>
                <div class="body">
                    <div class="sub-header">
                        What you&#8217;re writing these days
                    </div>
                    <div class="table">
                        <st:RecentPosts id="recentPosts" runat="server" />
                    </div>
                    <div class="section-footer"></div>
                </div>
            </div>
            
            <div class="section" id="Div2">
                <h3>Top Posts</h3> 
                
                <div class="body">
                    <div class="sub-header">
                        What readers liked
                    </div>
                    <div class="table">
                        <st:PopularPosts id="popularPosts" runat="server" />
                    </div>
                    <div class="section-footer"></div>
                </div>
            </div>
        </div>
    </div>    
</div>
</asp:Content>
