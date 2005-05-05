<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.CategoryList" %>
<%@ Import Namespace = "Subtext.Framework" %>
<asp:Repeater ID="CatList" Runat="server" OnItemCreated="CategoryCreated">
	<ItemTemplate>
		<h2><asp:Literal runat="server" ID="Title" /></h2>
		<asp:Repeater id="LinkList" runat="server" OnItemCreated="LinkCreated">
			<HeaderTemplate>
				<ul class="list">
			</HeaderTemplate>
			<ItemTemplate>
				<li class = "listitem"><asp:HyperLink Runat="server" CssClass="listitem" ID="Link" />  <asp:HyperLink Runat = "server" ID = "RssLink" Text = "(rss)" Visible = "False"/></li>
			</ItemTemplate>
			<FooterTemplate>
				</ul>
			</FooterTemplate>
		</asp:Repeater>
	</ItemTemplate>
</asp:Repeater>
