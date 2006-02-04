<%@ Register TagPrefix="ANW" Namespace="Subtext.Web.Admin.WebUI" Assembly="Subtext.Web" %>
<%@ Page language="c#" Codebehind="EditKeyWords.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.Admin.Pages.EditKeyWords" %>
<ANW:Page runat="server" id="PageContainer" TabSectionID="Options">
	<ANW:MessagePanel id="Messages" runat="server"></ANW:MessagePanel>
	<ANW:AdvancedPanel id="Results" runat="server" Collapsible="True" LinkText="[toggle]" HeaderText="KeyWords"
		HeaderCssClass="CollapsibleHeader" DisplayHeader="True" LinkBeforeHeader="True" LinkStyle="Image">
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
		<ANW:Pager id="ResultsPager" runat="server" CssClass="Pager" UrlFormat="EditKeyWords.aspx?pg={0}"
			LinkFormatActive='<a href="{0}" class="Current">{1}</a>' PrefixText="<div>Goto page</div>"
			UseSpacer="False"></ANW:Pager>
		<BR class="clear">
	</ANW:AdvancedPanel>
	<ANW:AdvancedPanel id="Edit" runat="server" Collapsible="False" HeaderText="Edit KeyWord" HeaderCssClass="CollapsibleTitle"
		DisplayHeader="True" LinkStyle="Image">
		<DIV class="Edit"><!-- DEBUG -->
			<P class="Label">Word
				<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ErrorMessage="You must enter a word (Text to replace)"
					ForeColor="#990066" ControlToValidate="txbWord"></asp:RequiredFieldValidator></P>
			<P>
				<asp:TextBox id="txbWord" runat="server" width="98%" columns="255" max="100"></asp:TextBox></P>
			<P class="Label">Text
				<asp:RequiredFieldValidator id="Requiredfieldvalidator2" runat="server" ErrorMessage="You must enter the Text to be displayed"
					ForeColor="#990066" ControlToValidate="txbText"></asp:RequiredFieldValidator></P>
			<P>
				<asp:TextBox id="txbText" runat="server" width="98%" columns="255"></asp:TextBox></P>
			<P class="Label">Url
				<asp:RequiredFieldValidator id="Requiredfieldvalidator3" runat="server" ErrorMessage="You must enter a Url"
					ForeColor="#990066" ControlToValidate="txbUrl"></asp:RequiredFieldValidator></P>
			<P>
				<asp:TextBox id="txbUrl" runat="server" width="98%" columns="255"></asp:TextBox></P>
			<P class="Label">Title
			</P>
			<P>
				<asp:TextBox id="txbTitle" runat="server" width="98%" columns="255"></asp:TextBox></P>
			<P class="ValueLabel">New Window
				<asp:CheckBox id="chkNewWindow" runat="server" textalign="Left"></asp:CheckBox>&nbsp; 
				Replace First Occurrence Only
				<asp:CheckBox id="chkFirstOnly" runat="server" textalign="Left"></asp:CheckBox>&nbsp; 
				Is CaseSensitive
				<asp:CheckBox id="chkCaseSensitive" runat="server" textalign="Left"></asp:CheckBox></P>
			<DIV>
				<asp:Button id="lkbPost" runat="server" CssClass="buttonSubmit" Text="Post" />
				<asp:Button id="lkbCancel" runat="server" CssClass="buttonSubmit" Text="Cancel" CausesValidation="false" />
				&nbsp;
			</DIV>
		</DIV>
	</ANW:AdvancedPanel>
</ANW:Page>
<DIV></DIV>
<DIV></DIV>
