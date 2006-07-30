<%@ Control Language="c#" AutoEventWireup="false" Codebehind="EntryEditor.ascx.cs" Inherits="Subtext.Web.Admin.UserControls.EntryEditor"%>
<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
<%@ Register TagPrefix="ANW" Namespace="Subtext.Web.Admin.WebUI" Assembly="Subtext.Web" %>
<%@ Register TagPrefix="st" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<%@ Import Namespace = "Subtext.Web.Admin" %>

<ANW:MessagePanel id="Messages" runat="server"></ANW:MessagePanel>

<ANW:AdvancedPanel id="Results" runat="server" LinkStyle="Image" LinkBeforeHeader="True" DisplayHeader="True" HeaderCssClass="CollapsibleHeader" LinkText="[toggle]" Collapsible="True">
	<asp:Repeater id="rprSelectionList" runat="server">
		<HeaderTemplate>
			<table id="Listing" class="Listing highlightTable" cellspacing="0" cellpadding="0" border="0" style="<%= CheckHiddenStyle() %>">
				<tr>
					<th>Description</th>
					<th width="50">Active</th>
					<th width="75">Web Views</th>
					<th width="75">Agg Views</th>
					<th width="50">Referrals</th>
					<th width="50">&nbsp;</th>
					<th width="50">&nbsp;</th>
				</tr>
		</HeaderTemplate>
		<ItemTemplate>
			<tr>
				<td>
					<%# DataBinder.Eval(Container.DataItem, "Title") %>
				</td>
				<td>
					<%# DataBinder.Eval(Container.DataItem, "IsActive") %>
				</td>												
				<td>
					<%# DataBinder.Eval(Container.DataItem, "WebCount") %>
				</td>
				<td>
					<%# DataBinder.Eval(Container.DataItem, "AggCount") %>
				</td>				
				<td>
					<a href="Referrers.aspx?EntryID=<%# DataBinder.Eval(Container.DataItem, "Id") %>">View</a>
				</td>				
				<td>
					<asp:LinkButton id="lnkEdit" CausesValidation = "False" CommandName="Edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' Text="Edit" runat="server" />
				</td>
				<td>
					<asp:LinkButton id="lnkDelete" CausesValidation = "False" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' Text="Delete" runat="server" />
				</td>
			</tr>
		</ItemTemplate>
		<AlternatingItemTemplate>
			<tr class="Alt">
				<td>
					<%# DataBinder.Eval(Container.DataItem, "Title") %>
				</td>
				<td>
					<%# DataBinder.Eval(Container.DataItem, "IsActive") %>
				</td>
				<td>
					<%# DataBinder.Eval(Container.DataItem, "WebCount") %>
				</td>
				<td>
					<%# DataBinder.Eval(Container.DataItem, "AggCount") %>
				</td>					
				<td>
					<a href="Referrers.aspx?EntryID=<%# DataBinder.Eval(Container.DataItem, "Id") %>">View</a>
				</td>				
				<td>
					<asp:LinkButton id="lnkEditAlt" CommandName="Edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' Text="Edit" runat="server" />
				</td>
				<td>
					<asp:LinkButton id="lnkDeleteAlt" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' Text="Delete" runat="server" />
				</td>
			</tr>
		</AlternatingItemTemplate>
		<FooterTemplate>
			</table>
		</FooterTemplate>
	</asp:Repeater>
	
	<p id="NoMessagesLabel" runat="server" visible="false">No entries found.</p>
		
	<st:PagingControl id="resultsPager" runat="server" 
			PrefixText="<div>Goto page</div>" 
			LinkFormatActive='<a href="{0}" class="Current">{1}</a>' 
			UrlFormat="EditPosts.aspx?pg={0}" 
			CssClass="Pager" />
	<br class="clear" />
</ANW:AdvancedPanel>

<ANW:AdvancedPanel id="Edit" runat="server" LinkImageCollapsed="~/admin/resources/toggle_gray_down.gif" LinkImage="~/admin/resources/toggle_gray_up.gif" LinkStyle="Image" DisplayHeader="True" HeaderCssClass="CollapsibleTitle" Collapsible="False" HeaderText="Edit Post">
	<div class="Edit">
		<!-- DEBUG -->
		<p class="Label"><asp:HyperLink id="hlEntryLink" Target="_blank" Runat="server"></asp:HyperLink></p>
		<p>
			<label for="Editor_Edit_txbTitle" accesskey="t">Post <u>T</u>itle</label>&nbsp;<asp:RequiredFieldValidator id="valTitleRequired" runat="server" ControlToValidate="txbTitle" ForeColor="#990066" ErrorMessage="Your post must have a title"></asp:RequiredFieldValidator>
		</p>
		<p>
			<asp:TextBox id="txbTitle" runat="server" CssClass="textinput" MaxLength="250"></asp:TextBox>
		</p>
		<p>
			<label for="Editor_Edit_richTextEditor" accesskey="b">Post <u>B</u>ody</label>&nbsp;<asp:RequiredFieldValidator id="valtbBodyRequired" runat="server" ControlToValidate="richTextEditor" ForeColor="#990066" ErrorMessage="Your post must have a body"></asp:RequiredFieldValidator></p>
		<p>
			<st:RichTextEditor id="richTextEditor" runat="server" onerror="richTextEditor_Error"></st:RichTextEditor>
		</p>
		<p><label>Categories</label></p>
		<p><asp:CheckBoxList id="cklCategories" runat="server" RepeatColumns="5" RepeatDirection="Horizontal"></asp:CheckBoxList></p>
		<div>
			<asp:Button id="lkbPost" runat="server" CssClass="buttonSubmit" Text="Post"  />
			<asp:Button id="lkUpdateCategories" runat="server" CssClass="buttonSubmit" Text="Categories" />
			<asp:Button id="lkbCancel" runat="server" CssClass="buttonSubmit" CausesValidation="False" Text="Cancel" />
			&nbsp;
		</div>
	</div>
	
	<ANW:AdvancedPanel id="Advanced" runat="server" Collapsible="True" LinkText="[toggle]" HeaderCssClass="CollapsibleTitle" DisplayHeader="True" LinkBeforeHeader="True" LinkStyle="Image" HeaderText="Advanced Options" BodyCssClass="Edit" Collapsed="true" HeaderTextCssClass="CollapsibleTitle">
		<p class="ValueLabel">
			<asp:CheckBox id="ckbPublished" runat="server" Text = "Published" textalign="Left" />
			<asp:CheckBox id="chkComments" runat="server" Text = "Show Comments" textalign="Left" />
			<asp:CheckBox id="chkCommentsClosed" runat="server" Text = "Comments Closed" textalign="Left" />
			<asp:CheckBox id="chkDisplayHomePage" runat="server" Text = "Display on HomePage" textalign="Left" />

			<asp:CheckBox id="chkMainSyndication" runat="server" Text = "Syndicate on Main Feed" textalign="Left" />
			<asp:CheckBox id="chkSyndicateDescriptionOnly" runat="server" Text = "Syndicate Description Only" textalign="Left" />
			<asp:CheckBox id="chkIsAggregated" runat="server" Text = "Include in Aggregated Site" textalign="Left" />
		</p>
		<p>
			<label for="Editor_Edit_txbEntryName" accesskey="n">Entry <u>N</u>ame (page name)</label> <asp:RegularExpressionValidator ID="vRegexEntryName" ControlToValidate = "txbEntryName" ValidationExpression = "^[a-zA-Z][\w-]{1,149}$" Text = "Invalid EntryName Format. Must match the follwing pattern: ^[a-zA-Z][\w-]{1,149}$" runat="server"/>
		</p>
		<p>
			<asp:TextBox id="txbEntryName" runat="server" CssClass="textinput" MaxLength="150"></asp:TextBox>
		</p>
		<p>
			<label for="Editor_Edit_txbExcerpt" accesskey="e"><u>E</u>xcerpt</label></p>
		<p>
			<asp:TextBox id="txbExcerpt" runat="server" CssClass="textarea" rows="5" textmode="MultiLine" MaxLength="500"></asp:TextBox>
		</p>
		<p>
			<label for="Editor_Edit_txbTitleUrl" accesskey="u">Title <u>U</u>rl</label>
		</p>
		<p>
			<asp:TextBox id="txbTitleUrl" runat="server" CssClass="textinput" MaxLength="250"></asp:TextBox>
		</p>
		<p>
			<label for="Editor_Edit_txbSourceName" accesskey="s"><u>S</u>ource Name</label>
		</p>
		<p>
			<asp:TextBox id="txbSourceName" runat="server" CssClass="textinput"></asp:TextBox>
		</p>
		<p>
			<label for="Editor_Edit_txbSourceUrl" accesskey="o">S<u>o</u>urce Url</label>
		</p>
		<p><asp:TextBox id="txbSourceUrl" runat="server" CssClass="textinput"></asp:TextBox></p>
	</ANW:AdvancedPanel>
	
</ANW:AdvancedPanel>
