<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.CategoryList" %>
<%@ Import Namespace = "Subtext.Framework" %>
<asp:Repeater ID="CatList" Runat="server" OnItemCreated="CategoryCreated">
	<ItemTemplate>
		<div class="leftbox">
			<h2><asp:Literal runat="server" ID="Title" /></h2>
			<asp:Repeater id="LinkList" runat="server" OnItemCreated="LinkCreated">
				<HeaderTemplate>
					<div>
						<ul class="sidebarlist">
				</HeaderTemplate>
				<ItemTemplate>
							<li><asp:HyperLink Runat="server" ID="Link" /></li>
				</ItemTemplate>
				<FooterTemplate>
						</ul>
					</div>
				</FooterTemplate>
			</asp:Repeater>
		</div>
	</ItemTemplate>
</asp:Repeater>