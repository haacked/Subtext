<%@ Control Language="c#" AutoEventWireup="false" Codebehind="EntryEditor.ascx.cs" Inherits="Subtext.Web.Admin.UserControls.EntryEditor" %>
<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
<%@ Register TagPrefix="ANW" Namespace="Subtext.Web.Admin.WebUI" Assembly="Subtext.Web" %>
<%@ Import Namespace = "Subtext.Web.Admin" %>

<ANW:MessagePanel id="Messages" runat="server"></ANW:MessagePanel>

<ANW:AdvancedPanel id="Results" runat="server" LinkStyle="Image" LinkBeforeHeader="True" DisplayHeader="True" HeaderCssClass="CollapsibleHeader" LinkText="[toggle]" Collapsible="True">
	<asp:Repeater id="rprSelectionList" runat="server">
		<HeaderTemplate>
			<table id="Listing" class="Listing" cellSpacing="0" cellPadding="0" border="0" style="<%= CheckHiddenStyle() %>">
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
					<a href="Referrers.aspx?EntryID=<%# DataBinder.Eval(Container.DataItem, "EntryID") %>">View</a>
				</td>				
				<td>
					<asp:LinkButton id="lnkEdit" CssClass="buttonSubmit" CausesValidation = "False" CommandName="Edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "EntryID") %>' Text="Edit" runat="server" />
				</td>
				<td>
					<asp:LinkButton id="lnkDelete" CssClass="buttonSubmit" CausesValidation = "False" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "EntryID") %>' Text="Delete" runat="server" />
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
					<a href="Referrers.aspx?EntryID=<%# DataBinder.Eval(Container.DataItem, "EntryID") %>">View</a>
				</td>				
				<td>
					<asp:LinkButton id="lnkEditAlt" CssClass="buttonSubmit" CommandName="Edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "EntryID") %>' Text="Edit" runat="server" />
				</td>
				<td>
					<asp:LinkButton id="lnkDeleteAlt" CssClass="buttonSubmit" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "EntryID") %>' Text="Delete" runat="server" />
				</td>
			</tr>
		</AlternatingItemTemplate>
		<FooterTemplate>
			</table>
		</FooterTemplate>
	</asp:Repeater>
	
	<p id="NoMessagesLabel" runat="server" visible="false">No entries found.</p>
	
	<ANW:Pager id="ResultsPager" runat="server" CssClass="Pager" UrlFormat="EditPosts.aspx?pg={0}"
		LinkFormatActive='<a href="{0}" class="Current">{1}</a>' PrefixText="<div>Goto page</div>"
		UseSpacer="False"></ANW:Pager>
	<br class="clear">
</ANW:AdvancedPanel>

<ANW:AdvancedPanel id="Edit" runat="server" LinkImageCollapsed="~/admin/resources/toggle_gray_down.gif" LinkImage="~/admin/resources/toggle_gray_up.gif" LinkStyle="Image" DisplayHeader="True" HeaderCssClass="CollapsibleTitle" Collapsible="False" HeaderText="Edit Post">
	
	<div class="Edit">
		<!-- DEBUG -->
		<p class="Label"><asp:HyperLink id="hlEntryLink" Target="_blank" Runat="server"></asp:HyperLink></p>
		<p>
			<label for="Editor_Edit_txbTitle" AccessKey="t">Post <u>T</u>itle</label>&nbsp;<asp:RequiredFieldValidator id="valTitleRequired" runat="server" ControlToValidate="txbTitle" ForeColor="#990066" ErrorMessage="Your post must have a title"></asp:RequiredFieldValidator>
		</p>
		<p>
			<asp:TextBox id="txbTitle" runat="server" columns="255" width="98%" MaxLength = "250"></asp:TextBox>
		</p>
		<p>
			<label for="Editor_Edit_freeTextBox" AccessKey="b">Post <u>B</u>ody</label>&nbsp;<asp:RequiredFieldValidator id="valtbBodyRequired" runat="server" ControlToValidate="freeTextBox" ForeColor="#990066" ErrorMessage="Your post must have a body"></asp:RequiredFieldValidator></p>
		<p>
			<FTB:FreeTextBox 
				id="freeTextBox"
				toolbarlayout="Bold,Italic,Underline,Strikethrough;Superscript,Subscript,RemoveFormat|FontFacesMenu,FontSizesMenu,FontForeColorsMenu|InsertTable|JustifyLeft,JustifyRight,JustifyCenter,JustifyFull;BulletedList,NumberedList,Indent,Outdent;CreateLink,Unlink,Insert,InsertRule|Cut,Copy,Paste;Undo,Redo|ieSpellCheck,WordClean|InsertImage,InsertImageFromGallery"
				runat="Server" 
				RemoveServerNamefromUrls="false"/>
		</p>
		<p><label>Categories</label></p>
		<p><asp:CheckBoxList id="cklCategories" runat="server" RepeatColumns="5" RepeatDirection="Horizontal"></asp:CheckBoxList></p>
		<div>
			<asp:Button id="lkbPost" runat="server" CssClass="buttonSubmit" Text="Post" />
			<asp:Button id="lkUpdateCategories" runat="server" CssClass="buttonSubmit" Text="Categories" />
			<asp:Button id="lkbCancel" runat="server" CssClass="buttonSubmit" CausesValidation="False" Text="Cancel" />
			&nbsp;
		</div>
	</div>
	
	<ANW:AdvancedPanel id="Advanced" runat="server" Collapsible="True" LinkText="[toggle]" HeaderCssClass="CollapsibleTitle" DisplayHeader="True" LinkBeforeHeader="True" LinkStyle="Image" HeaderText="Advanced Options" BodyCssClass="Edit" Collapsed="true" HeaderTextCssClass="CollapsibleTitle">
		<p class="ValueLabel">
			<asp:CheckBox id="ckbPublished" runat="server" Text = "Published" textalign="Left" />
			<asp:CheckBox id="chkComments" runat="server" Text = "Allow Comments" textalign="Left" />
			<asp:CheckBox id="chkDisplayHomePage" runat="server" Text = "Display on HomePage" textalign="Left" />

			<asp:CheckBox id="chkMainSyndication" runat="server" Text = "Syndicate on Main Feed" textalign="Left" />
			<asp:CheckBox id="chkSyndicateDescriptionOnly" runat="server" Text = "Syndicate Description Only" textalign="Left" />
			<asp:CheckBox id="chkIsAggregated" runat="server" Text = "Include in Aggregated Site" textalign="Left" />
		</p>
		<p>
			<label for="Editor_Edit_txbEntryName" AccessKey="n">Entry <u>N</u>ame (page name)</label> <asp:RegularExpressionValidator ID="vRegexEntryName" ControlToValidate = "txbEntryName" ValidationExpression = "^[a-zA-Z][\w-]{1,149}$" Text = "Invalid EntryName Format. Must match the follwing pattern: ^[a-zA-Z][\w-]{1,149}$" runat="server"/>
		</p>
		<p>
			<asp:TextBox id="txbEntryName" runat="server" columns="255" width="98%" MaxLength = "150"></asp:TextBox>
		</p>
		<p>
			<label for="Editor_Edit_txbExcerpt" AccessKey="e"><u>E</u>xcerpt</label></p>
		<p>
			<asp:TextBox id="txbExcerpt" runat="server" width="98%" rows="5" textmode="MultiLine" MaxLength = "500"></asp:TextBox>
		</p>
		<p>
			<label for="Editor_Edit_txbTitleUrl" AccessKey="u">Title <u>U</u>rl</label>
		</p>
		<p>
			<asp:TextBox id="txbTitleUrl" runat="server" columns="255" width="98%" MaxLength = "250"></asp:TextBox>
		</p>
		<p>
			<label for="Editor_Edit_txbSourceName" AccessKey="s"><u>S</u>ource Name</label>
		</p>
		<p>
			<asp:TextBox id="txbSourceName" runat="server" columns="255" width="98%"></asp:TextBox>
		</p>
		<p>
			<label for="Editor_Edit_txbSourceUrl" AccessKey="o">S<u>o</u>urce Url</label>
		</p>
		<p><asp:TextBox id="txbSourceUrl" runat="server" columns="255" width="98%"></asp:TextBox></p>
	</ANW:AdvancedPanel>
	
</ANW:AdvancedPanel>
