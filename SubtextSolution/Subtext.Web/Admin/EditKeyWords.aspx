<%@ Page language="c#" Codebehind="EditKeyWords.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.Admin.Pages.EditKeyWords" %>
<%@ Register TagPrefix="ANW" Namespace="Subtext.Web.Admin.WebUI" Assembly="Subtext.Web" %>
<ANW:Page runat="server" id="PageContainer" TabSectionID="Options">
	<ANW:MessagePanel id="Messages" runat="server"></ANW:MessagePanel>
	<ANW:AdvancedPanel id="Results" runat="server" LinkStyle="Image" LinkBeforeHeader="True" DisplayHeader="True"
		HeaderCssClass="CollapsibleHeader" HeaderText="KeyWords" LinkText="[toggle]" Collapsible="True">
		<ASP:Repeater id="rprSelectionList" runat="server">
			<HeaderTemplate>
				<table id="Listing" class="Listing" cellSpacing="0" cellPadding="0" border="0" style="<%= CheckHiddenStyle() %>">
					<tr>
						<th width="50">
							Word</th>
						<th width="50">
							Text</th>
						<th>
							Url</th>
						<th width="50">
							&nbsp;</th>
						<th width="50">
							&nbsp;</th>
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
						<asp:LinkButton id="lnkEdit" CssClass="buttonSubmit" CommandName="Edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "KeyWordID") %>' Text="Edit" runat="server" /></td>
					<td>
						<asp:LinkButton id="lnkDelete" CssClass="buttonSubmit" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "KeyWordID") %>' Text="Delete" runat="server" /></td>
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
						<asp:LinkButton id="Linkbutton1" CssClass="buttonSubmit" CommandName="Edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "KeyWordID") %>' Text="Edit" runat="server" /></td>
					<td>
						<asp:LinkButton id="Linkbutton2" CssClass="buttonSubmit" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "KeyWordID") %>' Text="Delete" runat="server" /></td>
				</tr>
			</AlternatingItemTemplate>
			<FooterTemplate>
				</table>
			</FooterTemplate>
		</ASP:Repeater>
		<ANW:Pager id="ResultsPager" runat="server" UseSpacer="False" PrefixText="<div>Goto page</div>"
			LinkFormatActive='<a href="{0}" class="Current">{1}</a>' UrlFormat="EditKeyWords.aspx?pg={0}"
			CssClass="Pager"></ANW:Pager>
		<BR class="clear">
	</ANW:AdvancedPanel>
	<asp:Button id="btnCreate" runat="server" CssClass="buttonSubmit" Text="Create New"></asp:Button>
	<ANW:AdvancedPanel id="Edit" runat="server" LinkStyle="Image" DisplayHeader="True" HeaderCssClass="CollapsibleTitle"
		HeaderText="Edit KeyWord" Collapsible="False">
		<DIV class="Edit"><!-- DEBUG -->
			<P class="Label">Word
				<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ControlToValidate="txbWord" ForeColor="#990066"
					ErrorMessage="You must enter a word (Text to replace)"></asp:RequiredFieldValidator></P>
			<P>
				<asp:TextBox id="txbWord" runat="server" max="100" columns="255" width="98%"></asp:TextBox></P>
			<P class="Label">Text
				<asp:RequiredFieldValidator id="Requiredfieldvalidator2" runat="server" ControlToValidate="txbText" ForeColor="#990066"
					ErrorMessage="You must enter the Text to be displayed"></asp:RequiredFieldValidator></P>
			<P>
				<asp:TextBox id="txbText" runat="server" columns="255" width="98%"></asp:TextBox></P>
			<P class="Label">Url
				<asp:RequiredFieldValidator id="Requiredfieldvalidator3" runat="server" ControlToValidate="txbUrl" ForeColor="#990066"
					ErrorMessage="You must enter a Url"></asp:RequiredFieldValidator></P>
			<P>
				<asp:TextBox id="txbUrl" runat="server" columns="255" width="98%"></asp:TextBox></P>
			<P class="Label">Title
			</P>
			<P>
				<asp:TextBox id="txbTitle" runat="server" columns="255" width="98%"></asp:TextBox></P>
			<P class="ValueLabel">New Window
				<asp:CheckBox id="chkNewWindow" runat="server" textalign="Left"></asp:CheckBox>&nbsp; 
				Replace First Occurrence Only
				<asp:CheckBox id="chkFirstOnly" runat="server" textalign="Left"></asp:CheckBox>&nbsp; 
				Is CaseSensitive
				<asp:CheckBox id="chkCaseSensitive" runat="server" textalign="Left"></asp:CheckBox></P>
			<DIV>
				<asp:Button id="lkbPost" runat="server" CssClass="buttonSubmit" Text="Post"></asp:Button>
				<asp:Button id="lkbCancel" runat="server" CssClass="buttonSubmit" Text="Cancel" CausesValidation="false"></asp:Button>&nbsp;
			</DIV>
		</DIV>
	</ANW:AdvancedPanel>
</ANW:Page>
<DIV></DIV>
<DIV></DIV>
