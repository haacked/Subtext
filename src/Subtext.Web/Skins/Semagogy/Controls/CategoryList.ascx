<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.CategoryList" %>

<asp:Repeater ID="CatList" Runat="server" OnItemCreated="CategoryCreated">
	<ItemTemplate>
	<dl class="Links">
		<dt>	
			<asp:literal runat="server" id="Title" />
		</dt>
		<dd>
			<asp:repeater id="LinkList" runat="server" onitemcreated="LinkCreated">
				<headertemplate>
			<ul>
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
		</dd>
	</dl>
	</ItemTemplate>
</asp:Repeater>