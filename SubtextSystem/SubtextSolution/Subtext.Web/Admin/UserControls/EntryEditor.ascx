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
					<asp:LinkButton id="lnkEdit" CausesValidation = "False" CommandName="Edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "EntryID") %>' Text="Edit" runat="server" />
				</td>
				<td>
					<asp:LinkButton id="lnkDelete" CausesValidation = "False" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "EntryID") %>' Text="Delete" runat="server" />
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
					<asp:LinkButton id="lnkEditAlt" CommandName="Edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "EntryID") %>' Text="Edit" runat="server" />
				</td>
				<td>
					<asp:LinkButton id="lnkDeleteAlt" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "EntryID") %>' Text="Delete" runat="server" />
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
	<br class="Clear">
</ANW:AdvancedPanel>

<ANW:AdvancedPanel id="Edit" runat="server" LinkImageCollapsed="~/admin/resources/toggle_gray_down.gif" LinkImage="~/admin/resources/toggle_gray_up.gif" LinkStyle="Image" DisplayHeader="True" HeaderCssClass="CollapsibleTitle" Collapsible="False" HeaderText="Edit Post">
	
	<div class="Edit">
		<!-- DEBUG -->
		<p class="Label"><asp:HyperLink id="hlEntryLink" Target="_blank" Runat="server"></asp:HyperLink></p>
		<p class="Label">Post Title&nbsp;<asp:RequiredFieldValidator id="valTitleRequired" runat="server" ControlToValidate="txbTitle" ForeColor="#990066" ErrorMessage="Your post must have a title"></asp:RequiredFieldValidator></p>
		<p><asp:TextBox id="txbTitle" runat="server" columns="255" width="98%" MaxLength = "250"></asp:TextBox></p>
		<p class="Label">Post Body&nbsp;<%-- <asp:RequiredFieldValidator id="valtbBodyRequired" runat="server" ControlToValidate="txbBody" ForeColor="#990066" ErrorMessage="Your post must have a body"></asp:RequiredFieldValidator>--%></p>
<%--		<asp:RequiredFieldValidator id="valftbBodyRequired" runat="server" ControlToValidate="ftbBody" ForeColor="#990066" ErrorMessage="Your post must have a body"></asp:RequiredFieldValidator></P> --%>
		<p>
<%--			<FTB:FreeTextBox id="ftbBody" runat="server" width="98%" visible="False" 
				toolbarlayout="Bold,Italic,Underline,Strikethrough;Superscript,Subscript,RemoveFormat|FontFacesMenu,FontSizesMenu,FontForeColorsMenu|JustifyLeft,JustifyRight,JustifyCenter,JustifyFull;BulletedList,NumberedList,Indent,Outdent;CreateLink,Unlink,Insert,InsertRule|Cut,Copy,Paste;Undo,Redo|ieSpellCheck,WordClean"
				toolbarbackcolor="Transparent" backcolor="Transparent" height="300px" toolbartype="Custom"
				showstatusarea="False" downlevelmode="TextArea" gutterbackcolor="Transparent" GutterBorderColorDark="Transparent"
				EditorBorderColorDark="Transparent" EditorBorderColorLight="Transparent" />
--%>				
			<script language="javascript" type="text/javascript" src="Resources/TinyMCE/tiny_mce.js"></script>
			<script language="javascript" type="text/javascript">
			//<![CDATA[
				tinyMCE.init({
					theme : "subtext",
					mode : "exact",
					elements : "Editor:Edit:txbBody",
					extended_valid_elements : "a[href|target|name]",
					plugins : "table,contextmenu,emotions,insertdatetime,preview,print,searchreplace",
				    theme_subtext_buttons1_add : "separator,preview,print,separator,search,replace",
					theme_subtext_buttons2_add : "emotions",
				    theme_subtext_buttons2_add_before : "insertdate,inserttime,separator",
					theme_subtext_buttons3_add_before : "tablecontrols,separator",
				    plugin_insertdate_dateFormat : "%Y-%m-%d",
					plugin_insertdate_timeFormat : "%H:%M:%S",
					plugin_preview_width : "500",
					plugin_preview_height : "600"
				});
			//]]>
			</script>
			<asp:TextBox id="txbBody" runat="server" width="98%" rows="20" textmode="MultiLine" />
		</p>
		<p class="Label">Categories</p>
		<p><asp:CheckBoxList id="cklCategories" runat="server" RepeatColumns="5" RepeatDirection="Horizontal"></asp:CheckBoxList></p>
		<div>
			<asp:LinkButton id="lkbPost" runat="server" CssClass="Button" Text="Post"></asp:LinkButton>
			<asp:LinkButton id="lkUpdateCategories" runat="server" Width="160" CssClass="Button" Text="<nobr>Update Categories Only</nobr>"></asp:LinkButton>
			<asp:LinkButton id="lkbCancel" runat="server" CssClass="Button" Text="Cancel" CausesValidation="False">Cancel</asp:LinkButton><br>
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
		<p class="Label">EntryName (page name) <asp:RegularExpressionValidator ID="vRegexEntryName" ControlToValidate = "txbEntryName" ValidationExpression = "^[a-zA-Z][\w-]{1,149}$" Text = "Invalid EntryName Format. Must match the follwing pattern: ^[a-zA-Z][\w-]{1,149}$" runat="server"/> </p>
		<p><asp:TextBox id="txbEntryName" runat="server" columns="255" width="98%" MaxLength = "150"></asp:TextBox></p>
		<p class="Label">Excerpt</p>
		<p><asp:TextBox id="txbExcerpt" runat="server" width="98%" rows="5" textmode="MultiLine" MaxLength = "500"></asp:TextBox></p>
		<p class="Label">Title Url</p>
		<p><asp:TextBox id="txbTitleUrl" runat="server" columns="255" width="98%" MaxLength = "250"></asp:TextBox></p>
		<p class="Label">Source Name</p>
		<p><asp:TextBox id="txbSourceName" runat="server" columns="255" width="98%"></asp:TextBox></p>
		<p class="Label">Source Url</p>
		<p><asp:TextBox id="txbSourceUrl" runat="server" columns="255" width="98%"></asp:TextBox></p>
	</ANW:AdvancedPanel>
	
</ANW:AdvancedPanel>
