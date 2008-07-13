<%@ Page Language="C#" MasterPageFile="~/Admin/Feedback/Feedback.Master" Title="Subtext Admin - Feedback" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Subtext.Web.Admin.Feedback.Default" %>

<asp:Content ID="feedbackListContent" ContentPlaceHolderID="feedbackContent" runat="server">
    
    <script type="text/javascript">
        function changeFilter(index)
        {
            if(index == 0) {
              self.location.href = '<%= Master.ListUrl(FeedbackType.None) %>';
            }
            else if(index == 1) {
                self.location.href = '<%= Master.ListUrl(FeedbackType.Comment) %>';
            }
            else if(index == 2) {
                self.location.href = '<%= Master.ListUrl(FeedbackType.PingTrack) %>';
            }  
        }
    </script>
    <st:MessagePanel id="Messages" runat="server" />
        <div id="feedback-filter">
            <asp:DropDownList runat="server" ID="filterTypeDropDown" onchange="changeFilter(this.selectedIndex);">
                <asp:ListItem Selected="True" Value="None">All</asp:ListItem>
                <asp:ListItem Value="Comment">Comments Only</asp:ListItem>
                <asp:ListItem Value="PingTrack">PingTracks Only</asp:ListItem>
            </asp:DropDownList>
        </div>
        
        <h2 ID="headerLiteral" runat="server">Comments</h2>
        <asp:Literal ID="noCommentsMessage" runat="server" />
		<asp:Repeater id="feedbackRepeater" runat="server">
			<HeaderTemplate>
				<table id="feedback" class="listing highlightTable">
					<tr>
						<th width="16"></th>
						<th>Title</th>						
						<th>Posted By</th>
						<th width="100">Date</th>
						<th width="50"><input id="cbCheckAll" class="inline" type="checkbox" onclick="ToggleCheckAll(this);" title="Check/Uncheck All" /><label for="cbCheckAll" title="Check/Uncheck All">All</label></th>
					</tr>
			</HeaderTemplate>
			<ItemTemplate>
				<tr>
					<td>
					    <a href="Edit.aspx?FeedBackID=<%# Eval("Id") %>&<%= Master.CurrentQuery %>" title="Edit this item"><asp:Image runat="server" ImageUrl="~/Images/edit.gif" /></a>
					</td>
					<td>
						<strong><%# GetTitle(Container.DataItem) %></strong>
					</td>
					<td>
						<strong><%# GetAuthor(Container.DataItem) %></strong> <%# GetAuthorInfo(Container.DataItem) %>
					</td>
					<td nowrap="nowrap">
						<%# DataBinder.Eval(Container.DataItem, "DateCreated", "{0:M/d/yy h:mmt}") %>
					</td>
					<td>
						<asp:CheckBox id="chkDelete" Runat="Server" />
						<input type="hidden" id="FeedbackId" name="FeedbackId" value='<%# DataBinder.Eval(Container.DataItem, "Id") %>' runat="server" />
					</td>
				</tr>
				<tr class="body">
					<td>
					</td>
					<td colspan="5">
						<%# GetBody(Container.DataItem) %>
					</td>
				</tr>
			</ItemTemplate>
			<AlternatingItemTemplate>
				<tr class="alt">
					<td>	
						<a href="Edit.aspx?FeedBackID=<%# Eval("Id") %>&<%= Master.CurrentQuery %>" title="Edit this item"><asp:Image runat="server" ImageUrl="~/Images/edit.gif" /></a>
					</td>
					<td>
						<strong><%# GetTitle(Container.DataItem) %></strong>
					</td>
					<td>
						<strong><%# GetAuthor(Container.DataItem) %></strong> <%# GetAuthorInfo(Container.DataItem) %>
					</td>
					<td nowrap="nowrap">
						<%# DataBinder.Eval(Container.DataItem, "DateCreated", "{0:M/d/yy h:mmt}") %>
					</td>   
					<td>
						<asp:CheckBox id="chkDeleteAlt" Runat="Server"></asp:CheckBox>
						<input type="hidden" id="FeedbackIdAlt" name="FeedbackIdAlt" value='<%# DataBinder.Eval(Container.DataItem, "Id") %>' runat="server" />
					</td>
				</tr>
				<tr class="body alt">
					<td>
					</td>
					<td colspan="4">
						<%# GetBody(Container.DataItem) %>
					</td>
				</tr>				
			</AlternatingItemTemplate>
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
		    <asp:Button id="btnDelete" runat="server" CssClass="buttonSubmit" style="float:right" Text="Delete" onclick="OnDeleteClick" ToolTip="Move To Trash" />
		    <asp:Button id="btnDestroy" runat="server" CssClass="buttonSubmit" style="float:right" Text="Destroy" onclick="OnDestroyClick" Visible="false" OnClientClick="return confirm('This will delete these comments permanently. Continue?');" ToolTip="Delete Forever" />
		    <asp:Button id="btnConfirmSpam" runat="server" CssClass="buttonSubmit" style="float:right" Text="Spam" onclick="OnConfirmSpam" ToolTip="Confirm Spam Moves Item To Trash" />
		    <asp:Button id="btnApprove" runat="server" CssClass="buttonSubmit" style="float:right" Text="Approve" onclick="OnApproveClick" ToolTip="Approve" Visible="false" />
		    <asp:Button id="btnEmpty" runat="server" CssClass="buttonSubmit" style="float:right" Text="Empty" OnClick="OnEmptyClick" OnClientClick="return confirm('This will permanently delete every comment of this type. Continue?');" ToolTip="Empty" Visible="false" />
		</div>
</asp:Content>
