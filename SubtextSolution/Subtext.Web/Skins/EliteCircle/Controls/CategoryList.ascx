<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.CategoryList" %>
<%@ Import Namespace = "Subtext.Framework" %>
<asp:Repeater ID="CatList" Runat="server" OnItemCreated="CategoryCreated">
	<ItemTemplate>
		<h1><asp:literal runat="server" id="Title" /></h1>
		<asp:repeater id="LinkList" runat="server" onitemcreated="LinkCreated">
			<headertemplate>
				<ul class="sidemenu">
			</headertemplate>
			<itemtemplate>
				<li>
					<asp:hyperlink runat="server" id="Link" />
				</li>
			</itemtemplate>
			<footertemplate>
				</ul>
			</footertemplate>
		</asp:repeater>
	</ItemTemplate>
</asp:Repeater>