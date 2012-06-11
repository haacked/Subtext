<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.CategoryList" %>

<asp:Repeater ID="CatList" Runat="server" OnItemCreated="CategoryCreated">
	<ItemTemplate>
		<h1 class = "listtitle"><asp:Literal runat="server" ID="Title" /></h1>
		<asp:Repeater id="LinkList" runat="server" OnItemCreated="LinkCreated">
			<HeaderTemplate>
				<ul class = "list">
			</HeaderTemplate>
			<ItemTemplate>
				<li class = "listitem"><asp:HyperLink Runat="server" CssClass="listitem" ID="Link" /> <asp:HyperLink CssClass = "listitem" Runat = "server" ID = "RssLink" Text = "(rss)" Visible = "False"/></li>
			</ItemTemplate>
			<FooterTemplate>
				</ul>
			</FooterTemplate>
		</asp:Repeater>
	</ItemTemplate>
</asp:Repeater>
