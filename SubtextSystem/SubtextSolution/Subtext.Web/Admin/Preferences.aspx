<%@ Register TagPrefix="ANW" Namespace="Subtext.Web.Admin.WebUI" Assembly="Subtext.Web" %>
<%@ Page language="c#" Codebehind="Preferences.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.Admin.Pages.EditPreferences" %>
<ANW:Page runat="server" id="PageContainer" TabSectionID="Options" CategoriesLabel="Other Items">
	<ANW:AdvancedPanel id="Edit" runat="server" DisplayHeader="True" HeaderText="Preferences" HeaderCssClass="CollapsibleHeader"
		BodyCssClass="Edit">
		<P style="MARGIN-TOP: 8px">Default number of items to display in listings &nbsp;
			<asp:DropDownList id="ddlPageSize" runat="server" AutoPostBack="false">
				<asp:ListItem Value="5">5</asp:ListItem>
				<asp:ListItem Value="10">10</asp:ListItem>
				<asp:ListItem Value="15">15</asp:ListItem>
				<asp:ListItem Value="20">20</asp:ListItem>
				<asp:ListItem Value="25">25</asp:ListItem>
				<asp:ListItem Value="30">30</asp:ListItem>
				<asp:ListItem Value="40">40</asp:ListItem>
				<asp:ListItem Value="50">50</asp:ListItem>
				<asp:ListItem Value="60">60</asp:ListItem>
			</asp:DropDownList></P>
		<P style="MARGIN-TOP: 8px">Always create new items as Published &nbsp;
			<asp:DropDownList id="ddlPublished" runat="server" AutoPostBack="false">
				<asp:ListItem Value="true">Yes</asp:ListItem>
				<asp:ListItem Value="false">No</asp:ListItem>
			</asp:DropDownList></P>
		<P style="MARGIN-TOP: 8px">Always expand advanced options &nbsp;
			<asp:DropDownList id="ddlExpandAdvanced" runat="server" AutoPostBack="false">
				<asp:ListItem Value="true">Yes</asp:ListItem>
				<asp:ListItem Value="false">No</asp:ListItem>
			</asp:DropDownList></P>
		<P style="MARGIN-TOP: 8px">
			<asp:CheckBox id="EnableComments" runat="Server" Text="Enable Comments" TextAlign="Left"></asp:CheckBox>&nbsp;
		</P>
		<DIV style="MARGIN-TOP: 12px">
			<ASP:LinkButton id="lkbUpdate" runat="server" CssClass="Button" Text="Save"></ASP:LinkButton>
			<ASP:LinkButton id="lkbCancel" runat="server" CssClass="Button" Text="Cancel"></ASP:LinkButton><BR>
			&nbsp;
		</DIV>
	</ANW:AdvancedPanel>
</ANW:Page>
