<%@ Page Language="C#" EnableTheming="false" Title="Subtext Admin - Feedback" MasterPageFile="~/Admin/WebUI/AdminPageTemplate.Master" Codebehind="Feedback.aspx.cs" AutoEventWireup="True" Inherits="Subtext.Web.Admin.Pages.Feedback" %>
<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
<%@ Register TagPrefix="st" Namespace="Subtext.Web.UI.WebControls" Assembly="Subtext.Web" %>
	
<asp:Content ID="actions" ContentPlaceHolderID="actionsHeading" runat="server">
	Folders
</asp:Content>

<asp:Content ID="categoryListTitle" ContentPlaceHolderID="categoryListHeading" runat="server">
</asp:Content>

<asp:Content ID="categoriesLinkListing" ContentPlaceHolderID="categoryListLinks" runat="server">
</asp:Content>

<asp:Content ID="entryEditor" ContentPlaceHolderID="pageContent" runat="server">

	<st:MessagePanel id="Messages" runat="server" />
	
	<span class="FeedbackTypeControl">
	    <asp:CheckBox ID="cbShowOnlyComments" visible="false" runat="server" Text="Show Only Comments" ToolTip="Hide feedback that is not of type 'Comment'" AutoPostBack="true" OnCheckedChanged="rbFeedbackFilter_SelectedIndexChanged"/>
        <asp:RadioButtonList ID="rbFeedbackFilter" runat="server" AutoPostBack="True" RepeatDirection="Horizontal" OnSelectedIndexChanged="rbFeedbackFilter_SelectedIndexChanged">
            <asp:ListItem Selected="True" Value="None">Show All</asp:ListItem>
            <asp:ListItem Value="Comment">Show Only Comments</asp:ListItem>
            <asp:ListItem Value="PingTrack">Show Only PingTrack</asp:ListItem>
        </asp:RadioButtonList></span>
        <st:AdvancedPanel id="Results" runat="server" Collapsible="False" HeaderText="Comments" HeaderCssClass="CollapsibleHeader" DisplayHeader="true">
		<asp:Repeater id="rprSelectionList" runat="server" OnItemCommand="rprSelectionList_ItemCommand">
			<HeaderTemplate>
				<table id="feedback" class="Listing highlightTable">
					<tr>
						<th>Title</th>						
						<th>Posted By</th>
						<th width="100">Date</th>
						<th width="100"></th>
						<th width="50"><input id="cbCheckAll" class="inline" type="checkbox" onclick="ToggleCheckAll(this);" title="Check/Uncheck All" /><label for="cbCheckAll" title="Check/Uncheck All">All</label></th>
					</tr>
			</HeaderTemplate>
			<ItemTemplate>
				<tr>
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
						<asp:LinkButton id="lnkEdit" CausesValidation="False" CommandName="Edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' Text="Edit" runat="server" />
					
					</td>
					<td>
						<asp:CheckBox id="chkDelete" Runat="Server"></asp:CheckBox>
						<input type="hidden" id="FeedbackId" name="FeedbackId" value='<%# DataBinder.Eval(Container.DataItem, "Id") %>' runat="server" />
					</td>
				</tr>
				<tr class="body">
					<td colspan="5">
						<%# GetBody(Container.DataItem) %>
					</td>
				</tr>
			</ItemTemplate>
			<AlternatingItemTemplate>
				<tr class="Alt">
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
						<asp:LinkButton id="lnkEdit" CausesValidation="False" CommandName="Edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' Text="Edit" runat="server" />					
					</td>
					<td>
						<asp:CheckBox id="chkDeleteAlt" Runat="Server"></asp:CheckBox>
						<input type="hidden" id="FeedbackIdAlt" name="FeedbackIdAlt" value='<%# DataBinder.Eval(Container.DataItem, "Id") %>' runat="server" />
					</td>
				</tr>
				<tr class="body Alt">
					<td colspan="4">
						<%# GetBody(Container.DataItem) %>
					</td>
				</tr>				
			</AlternatingItemTemplate>
			<FooterTemplate>
			</table>
		</FooterTemplate>
		</asp:Repeater>
		<st:PagingControl id="resultsPager" runat="server" 
			PrefixText="<div>Goto page</div>" 
			LinkFormatActive='<a href="{0}" class="Current">{1}</a>' 
			UrlFormat="Feedback.aspx?pg={0}" 
			CssClass="Pager" />
		<asp:Button id="btnDelete" runat="server" CssClass="buttonSubmit" style="float:right" Text="Delete" onclick="OnDeleteClick" ToolTip="Move To Trash" />
		<asp:Button id="btnDestroy" runat="server" CssClass="buttonSubmit" style="float:right" Text="Destroy" onclick="OnDestroyClick" Visible="false" OnClientClick="return confirm('This will delete these comments permanently. Continue?');" ToolTip="Delete Forever" />
		<asp:Button id="btnConfirmSpam" runat="server" CssClass="buttonSubmit" style="float:right" Text="Spam" onclick="OnConfirmSpam" ToolTip="Confirm Spam Moves Item To Trash" />
		<asp:Button id="btnApprove" runat="server" CssClass="buttonSubmit" style="float:right" Text="Approve" onclick="OnApproveClick" ToolTip="Approve" Visible="false" />
		<asp:Button id="btnEmpty" runat="server" CssClass="buttonSubmit" style="float:right" Text="Empty" OnClick="OnEmptyClick" OnClientClick="return confirm('This will permanently delete every comment of this type. Continue?');" ToolTip="Empty" Visible="false" />
		<br class="clear" />
	</st:AdvancedPanel>
	<asp:PlaceHolder ID="Edit" runat="server" Visible="false">
		<fieldset id="editPost">
			<legend>Edit</legend>
			<p class="Label"><asp:HyperLink ID="hlAuthorEmail" runat="server"><asp:Label runat="server" ID="lblName"><asp:Label runat="server" ID="lblEmail"></asp:Label></asp:Label></asp:HyperLink></p>
			<p class="Label"><asp:HyperLink id="hlEntryLink" Runat="server" /></p>
			
			<p class="Label">
				<label for="Editor_Edit_txbTitle" accesskey="t">Comment <u>T</u>itle
			&nbsp;<asp:RequiredFieldValidator id="valTitleRequired" runat="server" ControlToValidate="txbTitle" CssClass="error" ErrorMessage="* Your post must have a title" />
			</label>
			<asp:TextBox id="txbTitle" runat="server" CssClass="textbox" />
 			</p>
			<p class="Label">
				<label for="Editor_Edit_txbWebsite" accesskey="w">Comment <u>W</u>ebsite
            <asp:CustomValidator ID="valtxbWebsite" runat="server" Text="Invalid website format. Must be a URI" ControlToValidate="txbWebsite"></asp:CustomValidator>
			</label>
			<asp:TextBox id="txbWebsite" runat="server" CssClass="textbox" />
			</p>
				
			<label for="Editor_Edit_richTextEditor" accesskey="b">Comment <u>B</u>ody
			&nbsp;<asp:RequiredFieldValidator id="valtbBodyRequired" runat="server" ControlToValidate="richTextEditor" CssClass="error" ErrorMessage="Your post must have a body" />
			</label>
			<st:RichTextEditor id="richTextEditor" runat="server" onerror="richTextEditor_Error"></st:RichTextEditor>

			<div>
				<asp:Button id="lkbPost" runat="server" Text="Post" CssClass="button" OnClick="lkbPost_Click" />&nbsp;
				<asp:Button id="lkbCancel" runat="server" CausesValidation="false" Text="Cancel" CssClass="button" OnClick="lkbCancel_Click" />
			</div>
		</fieldset>
	</asp:PlaceHolder>
	

	
</asp:Content>
