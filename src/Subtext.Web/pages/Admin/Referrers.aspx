<%@ Page Language="C#" EnableTheming="false"  Title="Subtext Admin - Referrers" Codebehind="Referrers.aspx.cs" AutoEventWireup="True" Inherits="Subtext.Web.Admin.Pages.Referrers" %>

<asp:Content ContentPlaceHolderID="actionsHeading" runat="server">
    <h2>Stats</h2>
</asp:Content>

<asp:Content ContentPlaceHolderID="categoryListHeading" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="categoryListLinks" runat="server">
</asp:Content>

<asp:Content ID="referrersContent" ContentPlaceHolderID="pageContent" runat="server">
	<st:MessagePanel id="Messages" runat="server"></st:MessagePanel>
	<h2>Referrers</h2>
	<asp:PlaceHolder id="Results" runat="server">
		<asp:Repeater id="rprSelectionList" runat="server">
			<HeaderTemplate>
				<table id="Listing" class="listing highlightTable" cellspacing="0" cellpadding="0" border="0">
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
						<asp:linkbutton CausesValidation = "false" id="Linkbutton1" CommandName="Create" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "EntryId")+ "|" + DataBinder.Eval(Container.DataItem, "ReferrerUrl")  %>' Text="Create" runat="server" />
					</td>
				</tr>
			</ItemTemplate>
			<AlternatingItemTemplate>
				<tr class="alt">
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
						<asp:linkbutton CausesValidation = "false" id="Linkbutton2" CommandName="Create" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "EntryId")+ "|" + DataBinder.Eval(Container.DataItem, "ReferrerUrl")  %>' Text="Create" runat="server" />
					</td>
				</tr>
			</AlternatingItemTemplate>
			<FooterTemplate>
			</table>
		</FooterTemplate>
		</asp:Repeater>
		<st:PagingControl id="resultsPager" runat="server" CssClass="Pager" UrlFormat="Referrers.aspx?pg={0}"
			LinkFormatActive='<a href="{0}" class="Current">{1}</a>' PrefixText="<div>Goto page</div>" />
		<br class="clear" />
	</asp:PlaceHolder>
	<asp:PlaceHolder id="Edit" runat="server" Visible="false">
		<fieldset class="edit-form">
		    <legend>Create Trackback</legend>
			<label for="txbTitle">
				Title
				<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ControlToValidate="txbTitle" ForeColor="#990066"
					ErrorMessage="You must enter a title"></asp:RequiredFieldValidator>
			</label>
			<asp:TextBox id="txbTitle" runat="server" max="100" columns="255" width="98%" />
			<label for="txbUrl">Url
				<asp:RequiredFieldValidator id="Requiredfieldvalidator3" runat="server" ControlToValidate="txbUrl" ForeColor="#990066"
					ErrorMessage="You must enter a Url"></asp:RequiredFieldValidator>
			</label>
		    <asp:TextBox id="txbUrl" runat="server" columns="255" width="98%"></asp:TextBox>
			<label for="txbBody">Comments/Content</label>
			<asp:TextBox id="txbBody" runat="server" columns="255" width="98%" TextMode="MultiLine"></asp:TextBox>
			<div>
				<asp:Button id="lkbPost" runat="server" CssClass="buttonSubmit" Text="Post" onclick="lkbPost_Click"></asp:Button>
				<asp:Button id="lkbCancel" runat="server" CssClass="buttonSubmit" Text="Cancel" CausesValidation="false" onclick="lkbCancel_Click"></asp:Button><br />
				&nbsp;
			</div>
		</fieldset>
	</asp:PlaceHolder>
</asp:Content>
