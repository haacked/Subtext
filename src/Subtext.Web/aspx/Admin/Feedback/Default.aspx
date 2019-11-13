<%@ Page Language="C#" MasterPageFile="~/aspx/Admin/Feedback/Feedback.Master" Title="Subtext Admin - Feedback" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Subtext.Web.Admin.Feedback.Default" %>

<asp:Content ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(function() {
            var currentStatus = '<%= Master.FeedbackStatus %>';
            $("td.undoable a[class!='destroy']").undoable({
                url: '<%= Url.CommentUpdateStatus() %>',
                showingStatus: function(undoable) {
                    var bodyRow = undoable.next('tr');
                    bodyRow.hide().addClass('undoable').fadeIn('slow').show();
                },
                hidingStatus: function(undoable, target) {
                    undoable.next('tr').removeClass('undoable');
                },
                getUndoPostData: function(clickSource, target) {
                    var data = this.getPostData(clickSource, target);
                    data.status = 'Approved';
                    return data;
                },
                getPostData: function(clickSource, target) {
                    var data = this.getPostData(clickSource, target);
                    data.status = clickSource.attr('class');
                    return data;
                }
            });

            $('td.undoable a.destroy').undoable({
                url: '<%= Url.CommentDestroy() %>',
                showingStatus: function(undoable) {
                    var bodyRow = undoable.next('tr');
                    bodyRow.hide().addClass('undoable').fadeIn('slow').show();
                },
                showingStatus: function(undoable) {
                    undoable.find('td.undo a').remove();
                    var bodyRow = undoable.next('tr');
                    bodyRow.hide().addClass('undoable').fadeIn('slow').show();
                }
            });
        });
    </script>

    <script type="text/javascript">
        function changeFilter(index) {
            if (index == 0) {
                self.location.href = '<%= Master.ListUrl(FeedbackType.None) %>';
            }
            else if (index == 1) {
                self.location.href = '<%= Master.ListUrl(FeedbackType.Comment) %>';
            }
            else if (index == 2) {
                self.location.href = '<%= Master.ListUrl(FeedbackType.PingTrack) %>';
            }
            else if (index == 3) {
                self.location.href = '<%= Master.ListUrl(FeedbackType.ContactPage) %>';
            }
        }
    </script>
</asp:Content>

<asp:Content ID="feedbackListContent" ContentPlaceHolderID="feedbackContent" runat="server">
    <st:MessagePanel id="Messages" runat="server" />
        <div id="feedback-filter">
            <asp:DropDownList runat="server" ID="filterTypeDropDown" onchange="changeFilter(this.selectedIndex);">
                <asp:ListItem Selected="True" Value="None">All</asp:ListItem>
                <asp:ListItem Value="Comment">Comments Only</asp:ListItem>
                <asp:ListItem Value="PingTrack">PingTracks Only</asp:ListItem>
                <asp:ListItem Value="ContactPage">Contact Only</asp:ListItem>
            </asp:DropDownList>
        </div>
        
        <h2 ID="headerLiteral" runat="server">Comments</h2>
        <asp:Literal ID="noCommentsMessage" runat="server" />
        <asp:Repeater id="feedbackRepeater" runat="server">
            <HeaderTemplate>
                <table id="feedback" class="listing">
                    <tr>
                        <th width="16"></th>
                        <th>Title</th>						
                        <th>Author</th>
                        <th width="100">Date</th>
                        <th width="50"></th>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr class="heading">
                    <td>
                        <a href="Edit.aspx?FeedBackID=<%# Eval("Id") %>&<%= Master.CurrentQuery %>" title="Edit this item"><asp:Image runat="server" ImageUrl="~/Images/icons/edit.gif" /></a>
                    </td>
                    <td>
                        <%# GetTitle(Container.DataItem) %>
                    </td>
                    <td class="author">
                        <span title="<%# Author.IpAddress %>"><%# Author.Name %></span> <%# Author.MailTo %> <%# Author.UrlLink %>
                    </td>
                    <td>
                        <%# Eval("DateCreated", "{0:yyyy/M/d h:mm tt}")%>
                    </td>
                    <td class="undoable">
                        <ul class="horizontal">
                            <% if (FeedbackState.Spammable) { %>
                            <li><a href="#<%# Eval("Id") %>" class="FlaggedAsSpam">spam</a></li>
                            <% } %>
                            <% if (FeedbackState.Deletable) { %>
                                <li><a href="#<%# Eval("Id") %>" class="Deleted">delete</a></li>
                            <% } %>
                            <% if (FeedbackState.Approvable) { %>
                             <li><a href="#<%# Eval("Id") %>" class="Approved">approve</a></li>
                            <% } %>
                            <% if (FeedbackState.Destroyable) { %>
                             <li><a href="#<%# Eval("Id") %>" class="destroy">destroy</a></li>
                            <% } %>
                        </ul>
                    </td>
                </tr>
                <tr class="body">
                    <td>
                    </td>
                    <td colspan="3">
                        <div>
                            <%# GetBody(Container.DataItem) %>
                        </div>
                    </td>
                    <td>
                        <table class="author-details">
                            <tr>
                                <td colspan="2">
                                    <%# Author.Name %> <%# Author.FormattedEmail ?? @"<span class=""none"">(no email)</span>"%>
                                </td>
                            </tr>
                            <tr>
                                <td>URL:</td><td><%# Author.Url ?? @"<span class=""none"">(no website)</span>" %></td>
                            </tr>
                            <tr>
                                <td>IP:</td><td><%# Author.IpAddress %></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
        <div class="Pager">
            <st:PagingControl id="resultsPager" runat="server" 
                PrefixText="<div>Goto page</div>" 
                LinkFormatActive='<a href="{0}" class="Current">{1}</a>' 
                UrlFormat="Default.aspx?pg={0}" 
                CssClass="Pager" />
            </div>
        <div class="clear">
            <asp:Button id="btnEmpty" runat="server" CssClass="buttonSubmit" style="float:right" Text="Empty" OnClick="OnEmptyClick" OnClientClick="return confirm('This will permanently delete every comment of this type. Continue?');" ToolTip="Empty" Visible="false" />
        </div>
</asp:Content>