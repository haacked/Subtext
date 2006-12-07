<%@ Page language="c#" Title="Subtext Admin - Edit Keywords" MasterPageFile="~/Admin/WebUI/AdminPageTemplate.Master"  Codebehind="EditKeyWords.aspx.cs" AutoEventWireup="True" Inherits="Subtext.Web.Admin.Pages.EditKeyWords" %>
<%@ Register TagPrefix="ANW" Namespace="Subtext.Web.Admin.WebUI" Assembly="Subtext.Web" %>
<%@ Register TagPrefix="st" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>

<asp:Content ID="actions" ContentPlaceHolderID="actionsHeading" runat="server">
</asp:Content>

<asp:Content ID="categoryListTitle" ContentPlaceHolderID="categoryListHeading" runat="server">
</asp:Content>

<asp:Content ID="categoriesLinkListing" ContentPlaceHolderID="categoryListLinks" runat="server">
</asp:Content>

<asp:Content ID="entryEditor" ContentPlaceHolderID="pageContent" runat="server">
    <ANW:MessagePanel id="Messages" runat="server"></ANW:MessagePanel>
	<ANW:AdvancedPanel id="Results" runat="server" LinkStyle="Image" LinkBeforeHeader="True" DisplayHeader="True"
		HeaderCssClass="CollapsibleHeader" HeaderText="KeyWords" LinkText="[toggle]" Collapsible="True">
		<ASP:Repeater id="rprSelectionList" runat="server" OnItemCommand="rprSelectionList_ItemCommand">
			<HeaderTemplate>
				<table id="Listing" class="Listing" cellSpacing="0" cellPadding="0" border="0">
					<tr>
						<th width="50">Word</th>
						<th width="150">Text</th>
						<th>Url</th>
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
						<a target =_blank href='<%# DataBinder.Eval(Container.DataItem, "Url") %>'>
							<%# DataBinder.Eval(Container.DataItem, "Url") %>
						</a>
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
						<a target =_blank href='<%# DataBinder.Eval(Container.DataItem, "Url") %>'>
							<%# DataBinder.Eval(Container.DataItem, "Url") %>
						</a>
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
		</ASP:Repeater>
		<st:PagingControl id="resultsPager" runat="server" 
			PrefixText="<div>Goto page</div>" 
			LinkFormatActive='<a href="{0}" class="Current">{1}</a>' 
			UrlFormat="EditKeyWords.aspx?pg={0}" 
			CssClass="Pager" />
		<br class="clear" />
	</ANW:AdvancedPanel>
	
	<asp:Button id="btnCreate" runat="server" CssClass="button" Text="Create New" OnClick="btnCreate_Click" />
	
	<asp:PlaceHolder id="Edit" runat="server">
		<fieldset>
			<legend>Edit Keyword</legend>
			
			<label for="txbWord">Word</label>
			<asp:TextBox id="txbWord" runat="server" MaxLength="100" CssClass="textbox" />
			<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ControlToValidate="txbWord" CssClass="error"
					ErrorMessage=" * You must enter a word (Text to replace)" />
						
			<label for="txbText">Text</label>
			<asp:TextBox id="txbText" runat="server" CssClass="textbox" MaxLength="100" />
			<asp:RequiredFieldValidator id="Requiredfieldvalidator2" runat="server" ControlToValidate="txbText" CssClass="error"
					ErrorMessage=" * You must enter the Text to be displayed" />
			
			<label for="txbUrl">Url</label>
			<asp:TextBox id="txbUrl" runat="server" MaxLength="255" CssClass="textbox" />
			<asp:RequiredFieldValidator id="Requiredfieldvalidator3" runat="server" ControlToValidate="txbUrl" CssClass="error"
					ErrorMessage=" * You must enter a Url" />
			
			<label for="txbTitle">Title</label>
			<asp:TextBox id="txbTitle" runat="server" MaxLength="100" CssClass="textbox" />
			
			<div>
				<asp:CheckBox id="chkNewWindow" runat="server" Text="New Window" CssClass="checkbox" />
				<asp:CheckBox id="chkFirstOnly" runat="server" Text="Replace First Occurrence Only" CssClass="checkbox" />
				<asp:CheckBox id="chkCaseSensitive" runat="server" Text="Is CaseSensitive" CssClass="checkbox" />
			</div>
			
			<div class="buttons">
				<asp:Button id="lkbPost" runat="server" CssClass="button" Text="Post" OnClick="lkbPost_Click" />
				<asp:Button id="lkbCancel" runat="server" CssClass="button" Text="Cancel" CausesValidation="false" OnClick="lkbCancel_Click" />
			</div>
		</fieldset>
	</asp:PlaceHolder>
</asp:Content>