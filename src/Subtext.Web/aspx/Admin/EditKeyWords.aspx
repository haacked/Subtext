<%@ Page Language="C#" EnableTheming="false" Title="Subtext Admin - Edit Keywords"  Codebehind="EditKeyWords.aspx.cs" AutoEventWireup="True" Inherits="Subtext.Web.Admin.Pages.EditKeyWords" %>

<asp:Content ContentPlaceHolderID="actionsHeading" runat="server">
    <h2>Options</h2>
</asp:Content>

<asp:Content ContentPlaceHolderID="categoryListHeading" runat="server" />

<asp:Content ContentPlaceHolderID="categoryListLinks" runat="server" />

<asp:Content ID="entryEditor" ContentPlaceHolderID="pageContent" runat="server">
    <st:MessagePanel id="Messages" runat="server" />
	<h2>Keywords</h2>
	<asp:PlaceHolder id="Results" runat="server">
		<asp:Repeater id="rprSelectionList" runat="server" OnItemCommand="rprSelectionList_ItemCommand">
			<HeaderTemplate>
				<table id="Listing" class="listing highlightTable" cellspacing="0" cellpadding="0" border="0" style="<%= CheckHiddenStyle() %>">
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
				<tr class="alt">
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
	
	    <asp:Button id="btnCreate" runat="server" CssClass="buttonSubmit" Text="Create New" OnClick="btnCreate_Click" />
	</asp:PlaceHolder>
	
	<st:AdvancedPanel id="Edit" runat="server" LinkStyle="Image" DisplayHeader="false" HeaderCssClass="CollapsibleTitle"
		HeaderText="Edit KeyWord" Collapsible="False">
		<fieldset class="section">
		    <legend>Edit Keyword</legend>
			<label>Word
				<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ControlToValidate="txbWord" ForeColor="#990066"
					ErrorMessage="You must enter a word (Text to replace)"></asp:RequiredFieldValidator>
		    </label>
			<asp:TextBox id="txbWord" runat="server" max="100" columns="255" width="98%" />
			<label>Text
				<asp:RequiredFieldValidator id="Requiredfieldvalidator2" runat="server" ControlToValidate="txbText" ForeColor="#990066"
					ErrorMessage="You must enter the Text to be displayed" />
			</label>
			<asp:TextBox id="txbText" runat="server" columns="255" width="98%" />
			<label>Url
				<asp:RequiredFieldValidator id="Requiredfieldvalidator3" runat="server" ControlToValidate="txbUrl" ForeColor="#990066"
					ErrorMessage="You must enter a Url" />
			</label>
			<asp:TextBox id="txbUrl" runat="server" columns="255" width="98%" />
			
			<label>Title</label>
			<asp:TextBox id="txbTitle" runat="server" columns="255" width="98%" />
			
			<label>Rel data</label>
			<asp:TextBox id="txbRel" runat="server" columns="255" width="98%" />
			<span class="checkbox">
				<asp:CheckBox id="chkFirstOnly" runat="server" Text="Replace First Occurrence Only" />
				<asp:CheckBox id="chkCaseSensitive" runat="server" Text="Is Case Sensitive" />
			</span>
			<div>
				<asp:Button id="lkbPost" runat="server" Text="Post" OnClick="lkbPost_Click"></asp:Button>
				<asp:Button id="lkbCancel" runat="server" Text="Cancel" CausesValidation="false" OnClick="lkbCancel_Click" />&nbsp;
			</div>
		</fieldset>
	</st:AdvancedPanel>
</asp:Content>