<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.CategoryList" %>

<asp:Repeater ID="CatList" Runat="server" OnItemCreated="CategoryCreated">
	<ItemTemplate>
		<div class="contentbox">
			<h2><asp:Literal runat="server" ID="Title" /></h2>
			<div class="content">
				<asp:Repeater id="LinkList" runat="server" OnItemCreated="LinkCreated">
					<HeaderTemplate>
						<ul>
					</HeaderTemplate>
					<ItemTemplate>
							<li><asp:HyperLink Runat="server" CssClass="listitem" ID="Link" /> <asp:HyperLink CssClass = "listitem" Runat = "server" ID = "RssLink" Text = "(rss)" Visible = "False"/></li>
					</ItemTemplate>
					<FooterTemplate>
						</ul>
					</FooterTemplate>
				</asp:Repeater>
			</div>
		</div>
	</ItemTemplate>
</asp:Repeater>
