<%@ Control Language="C#" EnableTheming="false"  AutoEventWireup="True" Codebehind="EntryEditor.ascx.cs" Inherits="Subtext.Web.Admin.UserControls.EntryEditor"%>
<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
<%@ Register TagPrefix="ANW" Namespace="Subtext.Web.Admin.WebUI" Assembly="Subtext.Web" %>
<%@ Register TagPrefix="st" Namespace="Subtext.Web.UI.WebControls" Assembly="Subtext.Web" %>
<%@ Register TagPrefix="st" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>

<%@ Import Namespace = "Subtext.Web.Admin" %>

<ANW:MessagePanel id="Messages" runat="server" />

<ANW:AdvancedPanel id="Results" runat="server" LinkStyle="Image" LinkBeforeHeader="True" DisplayHeader="True" HeaderCssClass="CollapsibleHeader" LinkText="[toggle]" Collapsible="True">
	<asp:Repeater id="rprSelectionList" runat="server">
		<HeaderTemplate>
			<table id="Listing" class="Listing highlightTable" cellspacing="0" cellpadding="0" border="0">
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
				    <asp:HyperLink runat="server" NavigateUrl='<%# DataBinder.Eval(Container.DataItem, "FullyQualifiedUrl") %>' ToolTip="View Entry" >
				        <%# DataBinder.Eval(Container.DataItem, "Title") %></asp:HyperLink>
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
					<a href="Referrers.aspx?EntryID=<%# DataBinder.Eval(Container.DataItem, "Id") %>" title="View Referrals">View</a>
				</td>				
				<td>
					<asp:LinkButton id="lnkEdit" CausesValidation="False" CommandName="Edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' Text="Edit" runat="server" />
				</td>
				<td>
					<asp:LinkButton id="lnkDelete" CausesValidation="False" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' Text="Delete" runat="server" />
				</td>
			</tr>
		</ItemTemplate>
		<AlternatingItemTemplate>
			<tr class="Alt">
				<td>
					<asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# DataBinder.Eval(Container.DataItem, "FullyQualifiedUrl") %>' ToolTip="View Entry" >
				        <%# DataBinder.Eval(Container.DataItem, "Title") %></asp:HyperLink>
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
					<a href="Referrers.aspx?EntryID=<%# DataBinder.Eval(Container.DataItem, "Id") %>" title="View Referrals">View</a>
				</td>				
				<td>
					<asp:LinkButton id="lnkEditAlt" CausesValidation="False" CommandName="Edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' Text="Edit" runat="server" />
				</td>
				<td>
					<asp:LinkButton id="lnkDeleteAlt" CausesValidation="False" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' Text="Delete" runat="server" />
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
<asp:PlaceHolder id="Edit" runat="server">
	<fieldset id="editPost">
		<legend>Edit</legend>
		<p class="Label"><asp:HyperLink id="hlEntryLink" Runat="server" /></p>
		
		<label for="Editor_Edit_txbTitle" accesskey="t">Post <u>T</u>itle
		&nbsp;<asp:RequiredFieldValidator id="valTitleRequired" runat="server" ControlToValidate="txbTitle" CssClass="error" ErrorMessage="* Your post must have a title" />
		</label>
		<asp:TextBox id="txbTitle" runat="server" CssClass="textbox" />
			
		<label for="Editor_Edit_richTextEditor" accesskey="b">Post <u>B</u>ody
		&nbsp;<asp:RequiredFieldValidator id="valtbBodyRequired" runat="server" ControlToValidate="richTextEditor" CssClass="error" ErrorMessage="Your post must have a body" />
		</label>
		<st:RichTextEditor id="richTextEditor" runat="server" onerror="richTextEditor_Error"></st:RichTextEditor>

		<label>Categories</label>
		<asp:CheckBoxList id="cklCategories" runat="server" RepeatColumns="5" RepeatDirection="Horizontal" CssClass="checkbox" />
		
		<div>
			<asp:Button id="lkbPost" runat="server" Text="Post" CssClass="button" />
			<asp:Button id="lkUpdateCategories" runat="server" CausesValidation="false" Text="Categories" CssClass="button" />
			<asp:Button id="lkbCancel" runat="server" CausesValidation="false" Text="Cancel" CssClass="button" />
		</div>
	</fieldset>
	
	<ANW:AdvancedPanel id="Advanced" runat="server" LinkStyle="Image" LinkBeforeHeader="True" DisplayHeader="True" HeaderCssClass="CollapsibleHeader" LinkText="[toggle]" Collapsible="True" Collapsed="False" HeaderText="Advanced Options" BodyCssClass="Edit">
		<!-- todo, make this more css based than table driven -->
		<table cellpadding="4">
			<tr>
				<td width="200"><asp:CheckBox id="ckbPublished" runat="server" Text="Published" textalign="Right" />&nbsp;</td>
				<td width="200"><asp:CheckBox id="chkComments" runat="server" Text="Show Comments" textalign="Right" />&nbsp;</td>	
				<td width="200"><asp:CheckBox id="chkCommentsClosed" runat="server" Text="Comments Closed" textalign="Right" />&nbsp;</td>
				<td width="200"><asp:CheckBox id="chkDisplayHomePage" runat="server" Text="Display on HomePage" textalign="Right" />&nbsp;</td>
			</tr>
			<tr>
				<td><asp:CheckBox id="chkMainSyndication" runat="server" Text = "Syndicate on Main Feed" textalign="Right" />&nbsp;</td>
				<td><asp:CheckBox id="chkSyndicateDescriptionOnly" runat="server" Text = "Syndicate Description Only" textalign="Right" />&nbsp;</td>
				<td><asp:CheckBox id="chkIsAggregated" runat="server" Text = "Include in Aggregated Site" textalign="Right" />&nbsp;</td>
			</tr>
		</table>
		<p style="margin-top: 10px;">
		    <label for="Editor_Edit_txtPostDate" accesskey="d">Post <u>D</u>ate</label> 
            <asp:CustomValidator ID="vCustomPostDate" runat="server" Text="Invalid PostDate format. Must be a valid date/time expression" ControlToValidate="txtPostDate"></asp:CustomValidator>
		</p>
		<p>
            <asp:TextBox ID="txtPostDate" runat="server" CssClass="textinput" MaxLength="25"></asp:TextBox>
		</p>
		<p>
			<label for="Editor_Edit_txbEntryName" accesskey="n">Entry <u>N</u>ame (page name)</label> <asp:RegularExpressionValidator ID="vRegexEntryName" ControlToValidate="txbEntryName" ValidationExpression="^([a-zA-Z0-9-]*([a-zA-Z0-9-_]+\.)*[a-zA-Z0-9-_]+)$" Text = "Invalid EntryName Format. Must only contain characters allowable in an URL." runat="server"/>
		</p>
		<p>
			<asp:TextBox id="txbEntryName" runat="server" CssClass="textinput" MaxLength="150"></asp:TextBox>
		</p>
		<p>
			<label for="Editor_Edit_txbExcerpt" accesskey="e"><u>E</u>xcerpt</label></p>
		<p>
			<asp:TextBox id="txbExcerpt" runat="server" CssClass="textarea" rows="5" textmode="MultiLine" MaxLength="500"></asp:TextBox>
		</p>
	</ANW:AdvancedPanel>
	
</asp:PlaceHolder>
