<%@ Register TagPrefix="ANW" Namespace="Subtext.Web.Admin.WebUI" Assembly="Subtext.Web.Admin" %>
<%@ Page language="c#" Codebehind="EditLinks.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.Admin.Pages.EditLinks" %>
<ANW:Page runat="server" id="PageContainer" TabSectionID="Links" CategoryType="LinkCollection">
	<ANW:MessagePanel id="Messages" runat="server"></ANW:MessagePanel>
	<ANW:AdvancedPanel id="Results" runat="server" LinkStyle="Image" LinkBeforeHeader="True" DisplayHeader="True"
		HeaderCssClass="CollapsibleHeader" HeaderText="Links" LinkText="[toggle]" Collapsible="True">
		<ASP:Repeater id="rprSelectionList" runat="server">
			<HeaderTemplate>
				<table id="Listing" class="Listing" cellSpacing="0" cellPadding="0" border="0" style="<%= CheckHiddenStyle() %>">
					<tr>
						<th>
							Description</th>
						<th width="50">
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
						<%# DataBinder.Eval(Container.DataItem, "Title") %>
					</td>
					<td>
						<%# DataBinder.Eval(Container.DataItem, "Url") %>
					</td>
					<td>
						<asp:linkbutton id="lnkEdit" CommandName="Edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "LinkID") %>' Text="Edit" runat="server" /></td>
					<td>
						<asp:linkbutton id="lnkDelete" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "LinkID") %>' Text="Delete" runat="server" /></td>
				</tr>
			</ItemTemplate>
			<AlternatingItemTemplate>
				<tr class="Alt">
					<td>
						<%# DataBinder.Eval(Container.DataItem, "Title") %>
					</td>
					<td>
						<%# DataBinder.Eval(Container.DataItem, "Url") %>
					</td>
					<td>
						<asp:linkbutton id="lnkEdit" CommandName="Edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "LinkID") %>' Text="Edit" runat="server" /></td>
					<td>
						<asp:linkbutton id="lnkDelete" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "LinkID") %>' Text="Delete" runat="server" /></td>
				</tr>
			</AlternatingItemTemplate>
			<FooterTemplate>
				</table>
			</FooterTemplate>
		</ASP:Repeater>
		<ANW:Pager id="ResultsPager" runat="server" UseSpacer="False" PrefixText="<div>Goto page</div>"
			LinkFormatActive='<a href="{0}" class="Current">{1}</a>' UrlFormat="EditLinks.aspx?pg={0}"
			CssClass="Pager"></ANW:Pager>
		<BR class="Clear">
	</ANW:AdvancedPanel>
	<ANW:AdvancedPanel id="ImportExport" runat="server" LinkStyle="Image" LinkBeforeHeader="True" DisplayHeader="True"
		HeaderCssClass="CollapsibleTitle" HeaderText="Import/Export" Collapsible="True" BodyCssClass="Edit"
		visible="false">
		<DIV style="HEIGHT: 0px"><!-- IE bug hides label in following div without this -->
			<DIV>
				<DIV>
					<P class="Block"><LABEL class="Block">Local File Location (*.opml)</LABEL></P>
					<INPUT class="FileUpload" id="OpmlImportFile" type="file" size="62" name="ImageFile" runat="server">
					<P class="Label">Categories</P>
					<P>
						<ASP:DropDownList id="ddlImportExportCategories" runat="server"></ASP:DropDownList></P>
				</DIV>
				<DIV style="MARGIN-TOP: 8px">
					<asp:linkbutton id="lkbImportOpml" runat="server" CssClass="Button" Text="Import"></asp:linkbutton><A class="Button" href="Export.aspx?command=opml">Export</A>
					<BR class="Clear">
					&nbsp;
				</DIV>
	</ANW:AdvancedPanel>
	<ANW:AdvancedPanel id="Edit" runat="server" LinkStyle="Image" DisplayHeader="True" HeaderCssClass="CollapsibleTitle"
		HeaderText="Edit Link" Collapsible="False">
		<DIV class="Edit"><!-- DEBUG -->
			<P class="Label">Link ID
				<asp:Label id="lblEntryID" runat="server"></asp:Label></P>
			<P class="Label">Link Title
				<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ControlToValidate="txbTitle" ForeColor="#990066"
					ErrorMessage="Your link must have a title"></asp:RequiredFieldValidator></P>
			<P>
				<asp:TextBox id="txbTitle" runat="server" columns="255" width="98%"></asp:TextBox></P>
			<P class="Label">Web Url
				<asp:RequiredFieldValidator id="Requiredfieldvalidator2" runat="server" ControlToValidate="txbUrl" ForeColor="#990066"
					ErrorMessage="Your link must have a url"></asp:RequiredFieldValidator></P>
			<P>
				<asp:TextBox id="txbUrl" runat="server" columns="255" width="98%"></asp:TextBox></P>
			<P class="Label">Rss Url</P>
			<P>
				<asp:TextBox id="txbRss" runat="server" columns="255" width="98%"></asp:TextBox></P>
			<P class="Label">Categories</P>
			<P>
				<ASP:DropDownList id="ddlCategories" runat="server"></ASP:DropDownList></P>
			<P class="ValueLabel">Visible
				<asp:CheckBox id="ckbIsActive" runat="server" textalign="Left"></asp:CheckBox>&nbsp; 
				New Window
				<asp:CheckBox id="chkNewWindow" runat="server" textalign="Left"></asp:CheckBox></P>
			<DIV>
				<asp:LinkButton id="lkbPost" runat="server" CssClass="Button" Text="Post"></asp:LinkButton>
				<asp:LinkButton id="lkbCancel" runat="server" CssClass="Button" Text="Cancel"></asp:LinkButton><BR>
				&nbsp;
			</DIV>
		</DIV>
	</ANW:AdvancedPanel>
</ANW:Page></DIV></DIV>
