<%@ Register TagPrefix="ANW" Namespace="Subtext.Web.Admin.WebUI" Assembly="Subtext.Web" %>
<%@ Page language="c#" Codebehind="Referrers.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.Admin.Pages.Referrers" %>
<ANW:Page id="PageContainer" TabSectionID="Stats" runat="server">
	<ANW:MessagePanel id="Messages" runat="server"></ANW:MessagePanel>
	<ANW:AdvancedPanel id="Results" runat="server" DisplayHeader="true" HeaderCssClass="CollapsibleHeader"
		HeaderText="Referrers" Collapsible="False" LinkStyle="Image" LinkBeforeHeader="True" LinkText="[toggle]">
		<ASP:Repeater id="rprSelectionList" runat="server">
			<HeaderTemplate>
				<table id="Listing" class="Listing" cellSpacing="0" cellPadding="0" border="0" style="<%= CheckHiddenStyle() %>">
					<tr>
						<th>
							Referred</th>
						<th>
							Referrer</th>
						<th>
							Count</th>
						<th>
							Last Referred</th>
						<th>
							TrackBack</th>
					</tr>
			</HeaderTemplate>
			<ItemTemplate>
				<tr>
					<td nowrap>
						<%# GetTitle(Container.DataItem) %>
					</td>
					<td nowrap>
						<%# GetReferrer(Container.DataItem) %>
					</td>
					<td nowrap>
						<%# DataBinder.Eval(Container.DataItem, "Count") %>
					</td>
					<td nowrap>
						<%# DataBinder.Eval(Container.DataItem, "LastReferDate", "{0:M/d/yy h:mmt}") %>
					</td>
					<td>
						<asp:linkbutton CausesValidation = "false" id="Linkbutton1" CommandName="Create" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "EntryID")+ "|" + DataBinder.Eval(Container.DataItem, "ReferrerURL")  %>' Text="Create" runat="server" />
					</td>
				</tr>
			</ItemTemplate>
			<AlternatingItemTemplate>
				<tr class="Alt">
					<td nowrap>
						<%# GetTitle(Container.DataItem) %>
					</td>
					<td nowrap>
						<%# GetReferrer(Container.DataItem) %>
					</td>
					<td nowrap>
						<%# DataBinder.Eval(Container.DataItem, "Count") %>
					</td>
					<td nowrap>
						<%# DataBinder.Eval(Container.DataItem, "LastReferDate", "{0:M/d/yy h:mmt}") %>
					</td>
					<td>
						<asp:linkbutton CausesValidation = "false" id="Linkbutton2" CommandName="Create" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "EntryID")+ "|" + DataBinder.Eval(Container.DataItem, "ReferrerURL")  %>' Text="Create" runat="server" />
					</td>
				</tr>
			</AlternatingItemTemplate>
			<FooterTemplate>
			</table>
		</FooterTemplate>
		</ASP:Repeater>
		<ANW:Pager id="ResultsPager" runat="server" CssClass="Pager" UrlFormat="Referrers.aspx?pg={0}"
			LinkFormatActive='<a href="{0}" class="Current">{1}</a>' PrefixText="<div>Goto page</div>"
			UseSpacer="False"></ANW:Pager>
		<BR class="Clear">
	</ANW:AdvancedPanel>
	<ANW:AdvancedPanel id="Edit" runat="server" DisplayHeader="True" HeaderCssClass="CollapsibleTitle"
		HeaderText="Create TrackBack" Collapsible="False" LinkStyle="Image" Visible="False">
		<DIV class="Edit">
			<P class="Label">Title
				<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ControlToValidate="txbTitle" ForeColor="#990066"
					ErrorMessage="You must enter a title"></asp:RequiredFieldValidator></P>
			<P>
				<asp:TextBox id="txbTitle" runat="server" max="100" columns="255" width="98%"></asp:TextBox></P>
			<P class="Label">Url
				<asp:RequiredFieldValidator id="Requiredfieldvalidator3" runat="server" ControlToValidate="txbUrl" ForeColor="#990066"
					ErrorMessage="You must enter a Url"></asp:RequiredFieldValidator></P>
			<P>
				<asp:TextBox id="txbUrl" runat="server" columns="255" width="98%"></asp:TextBox></P>
			<P class="Label">Comments/Content
			</P>
			<P>
				<asp:TextBox id="txbBody" runat="server" columns="255" width="98%" TextMode="MultiLine"></asp:TextBox></P>
			<DIV>
				<asp:LinkButton id="lkbPost" runat="server" CssClass="Button" Text="Post"></asp:LinkButton>
				<asp:LinkButton id="lkbCancel" runat="server" CssClass="Button" Text="Cancel" CausesValidation="false"></asp:LinkButton><BR>
				&nbsp;
			</DIV>
		</DIV>
	</ANW:AdvancedPanel>
</ANW:Page>
