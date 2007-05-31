<%@ Page Language="C#" EnableTheming="false" Title="Subtext Admin - Edit Keywords" MasterPageFile="~/Admin/WebUI/AdminPageTemplate.Master"  Codebehind="EditKeyWords.aspx.cs" AutoEventWireup="True" Inherits="Subtext.Web.Admin.Pages.EditKeyWords" %>

<asp:Content ID="actions" ContentPlaceHolderID="actionsHeading" runat="server">
</asp:Content>

<asp:Content ID="categoryListTitle" ContentPlaceHolderID="categoryListHeading" runat="server">
</asp:Content>

<asp:Content ID="categoriesLinkListing" ContentPlaceHolderID="categoryListLinks" runat="server">
</asp:Content>

<asp:Content ID="entryEditor" ContentPlaceHolderID="pageContent" runat="server">
    <st:MessagePanel id="Messages" runat="server" />
	<st:AdvancedPanel id="Results" runat="server" LinkStyle="Image" LinkBeforeHeader="True" DisplayHeader="True"
		HeaderCssClass="CollapsibleHeader" HeaderText="KeyWords" LinkText="[toggle]" Collapsible="True">
		<asp:Repeater id="rprSelectionList" runat="server" OnItemCommand="rprSelectionList_ItemCommand">
			<HeaderTemplate>
				<table id="Listing" class="Listing highlightTable" cellspacing="0" cellpadding="0" border="0">
					<tr>
						<th width="50">Word</th>
						<th width="150">Text</th>
						<th>Url</th>
						<th width="150">Rel</th>
						<th width="50">&nbsp;</th>
						<th width="50">&nbsp;</th>
					</tr>
			</HeaderTemplate>
			<ItemTemplate>
				<tr>
					<td>
						<%# DataBinder.Eval(Container.DataItem, "Word") %>
					</td>
					<td>
						<%# DataBinder.Eval(Container.DataItem, "Text") %>
					</td>
					<td>
						<a target="_blank" href='<%# DataBinder.Eval(Container.DataItem, "Url") %>'>
							<%# DataBinder.Eval(Container.DataItem, "Url") %>
						</a>
					</td>
					<td>
						<%# DataBinder.Eval(Container.DataItem, "Rel") %>
					</td>
					<td>
						<asp:LinkButton id="lnkEdit" CssClass="buttonSubmit" CommandName="Edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' Text="Edit" runat="server" /></td>
					<td>
						<asp:LinkButton id="lnkDelete" CssClass="buttonSubmit" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' Text="Delete" runat="server" /></td>
				</tr>
			</ItemTemplate>
			<AlternatingItemTemplate>
				<tr class="Alt">
					<td>
						<%# DataBinder.Eval(Container.DataItem, "Word") %>
					</td>
					<td>
						<%# DataBinder.Eval(Container.DataItem, "Text") %>
					</td>
					<td>
						<a target="_blank" href='<%# DataBinder.Eval(Container.DataItem, "Url") %>'>
							<%# DataBinder.Eval(Container.DataItem, "Url") %>
						</a>
					</td>
					<td>
						<%# DataBinder.Eval(Container.DataItem, "Rel") %>
					</td>
					<td>
						<asp:LinkButton id="Linkbutton1" CssClass="buttonSubmit" CommandName="Edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' Text="Edit" runat="server" /></td>
					<td>
						<asp:LinkButton id="Linkbutton2" CssClass="buttonSubmit" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' Text="Delete" runat="server" /></td>
				</tr>
			</AlternatingItemTemplate>
			<FooterTemplate>
				</table>
			</FooterTemplate>
		</asp:Repeater>
		<st:PagingControl id="resultsPager" runat="server" 
			PrefixText="<div>Goto page</div>" 
			LinkFormatActive='<a href="{0}" class="Current">{1}</a>' 
			UrlFormat="EditKeyWords.aspx?pg={0}" 
			CssClass="Pager" />
		<br class="clear" />
	</st:AdvancedPanel>
	<asp:Button id="btnCreate" runat="server" CssClass="buttonSubmit" Text="Create New" OnClick="btnCreate_Click" />
	<st:AdvancedPanel id="Edit" runat="server" LinkStyle="Image" DisplayHeader="True" HeaderCssClass="CollapsibleTitle"
		HeaderText="Edit KeyWord" Collapsible="False">
		<div class="Edit"><!-- DEBUG -->
			<p class="Label">Word
				<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ControlToValidate="txbWord" ForeColor="#990066"
					ErrorMessage="You must enter a word (Text to replace)"></asp:RequiredFieldValidator></p>
			<p>
				<asp:TextBox id="txbWord" runat="server" max="100" columns="255" width="98%"></asp:TextBox></p>
			<p class="Label">Text
				<asp:RequiredFieldValidator id="Requiredfieldvalidator2" runat="server" ControlToValidate="txbText" ForeColor="#990066"
					ErrorMessage="You must enter the Text to be displayed"></asp:RequiredFieldValidator></p>
			<p>
				<asp:TextBox id="txbText" runat="server" columns="255" width="98%"></asp:TextBox></p>
			<p class="Label">Url
				<asp:RequiredFieldValidator id="Requiredfieldvalidator3" runat="server" ControlToValidate="txbUrl" ForeColor="#990066"
					ErrorMessage="You must enter a Url"></asp:RequiredFieldValidator></p>
			<p>
				<asp:TextBox id="txbUrl" runat="server" columns="255" width="98%"></asp:TextBox></p>
			<p class="Label">Title</p>
			<p>
				<asp:TextBox id="txbTitle" runat="server" columns="255" width="98%"></asp:TextBox></p>
			<p class="Label">Rel data</p>
			<p>
				<asp:TextBox id="txbRel" runat="server" columns="255" width="98%"></asp:TextBox></p>
			<p class="ValueLabel">New Window
				<asp:CheckBox id="chkNewWindow" runat="server" textalign="Left"></asp:CheckBox>&nbsp; 
				Replace First Occurrence Only
				<asp:CheckBox id="chkFirstOnly" runat="server" textalign="Left"></asp:CheckBox>&nbsp; 
				Is CaseSensitive
				<asp:CheckBox id="chkCaseSensitive" runat="server" textalign="Left"></asp:CheckBox></p>
			<div>
				<asp:Button id="lkbPost" runat="server" CssClass="buttonSubmit" Text="Post" OnClick="lkbPost_Click"></asp:Button>
				<asp:Button id="lkbCancel" runat="server" CssClass="buttonSubmit" Text="Cancel" CausesValidation="false" OnClick="lkbCancel_Click" />&nbsp;
			</div>
		</div>
	</st:AdvancedPanel>
</asp:Content>