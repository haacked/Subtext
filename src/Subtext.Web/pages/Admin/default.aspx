<%@ Page Language="C#" EnableTheming="false" CodeBehind="default.aspx.cs" AutoEventWireup="true" Inherits="Subtext.Web.Admin.Pages.HomePageDefault" %>

<asp:Content ContentPlaceHolderID="head" runat="server">
    <link id="Link1" type="text/css" rel="stylesheet" href="<%= VirtualPathUtility.ToAbsolute("~/pages/Admin/Resources/dashboard.css") %>" />
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
<div id="content">
    <h2>Subtext Dashboard</h2>
    
    <div class="column">
        <div class="container">
            <div class="section" id="summary">
                <h3>Summary</h3>
                <div class="body">
                    <div class="table">
                        <table>
                            <tr class="highlight">
                                <td class="stat"><%# Statistics.ActivePostCount %></td>
                                <td>Posts</td>
                                <td class="stat"><%# Statistics.FeedbackCount %></td>
                                <td title="Does not include pingbacks/trackbacks">Comments</td>
                            </tr>
                            <tr>
                                <td class="stat"><%# Statistics.ActiveArticleCount %></td>
                                <td>Articles</td>
                                <td class="stat"><%# Statistics.ApprovedFeedbackCount %></td>
                                <td>Approved</td>
                            </tr>
                            <tr class="highlight">
                                <td class="stat"><%# CategoryCount %></td>
                                <td>Categories</td>
                                <td class="stat"><%# Statistics.AwaitingModerationFeedbackCount %></td>
                                <td>Pending</td>
                            </tr>
                            <tr>
                                <td class="stat"><%# Blog.PingTrackCount %></td>
                                <td>Pingbacks/Trackbacks</td>
                                <td class="stat"><%# Statistics.FlaggedAsSpamFeedbackCount %></td>
                                <td title="Flagged as spam, but not confirmed">Spam</td>
                            </tr>
                        </table>
                    </div>
                    Current Skin: <%# Blog.Skin.SkinKey %> <a href="<%# Url.AdminUrl("Skins.aspx") %>" title="Change skin">Change Skin</a>
                </div>
            </div>

            <div class="section" id="frequency">
                <h3>Activity</h3>
                <div class="body">
                    <div class="table">
                        <table>
                            <tr class="highlight">
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
                </div>
            </div>


            <div class="section" id="recent-comments">
                <h3>Recent Comments</h3>
                
                <div class="body">
                    <st:RecentComments id="recentComments" runat="server" />
                </div>
            </div>

            
        </div>
    </div>
    <div class="column">
        <div class="container">
            <div class="section" id="recent-posts">
                <h3>Recent Posts</h3>
                <div class="body">
                    <div class="table">
                        <st:RecentPosts id="recentPosts" runat="server" />
                    </div>
                </div>
            </div>
            
            <div class="section" id="Div2">
                <h3>Top Posts</h3> 
                
                <div class="body">
                    <div class="table">
                        <st:PopularPosts id="popularPosts" runat="server" />
                    </div>
                </div>
            </div>
        </div>
    </div>    
</div>
</asp:Content>
