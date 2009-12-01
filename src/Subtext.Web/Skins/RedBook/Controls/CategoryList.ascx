<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.CategoryList" %>

<asp:Repeater ID="CatList" Runat="server" OnItemCreated="CategoryCreated">
	<ItemTemplate>
		<div>
			<h2><asp:Literal runat="server" ID="Title" /></h2>
			<asp:Repeater id="LinkList" runat="server" OnItemCreated="LinkCreated">
				<HeaderTemplate>
					<ul>
				</HeaderTemplate>
				<ItemTemplate>
						<li><asp:HyperLink Runat="server" ID="Link" /></li>
				</ItemTemplate>
				<FooterTemplate>
					</ul>
				</FooterTemplate>
			</asp:Repeater>
		</div>
	</ItemTemplate>
</asp:Repeater>