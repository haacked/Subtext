<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.CategoryList" %>
<%@ Import Namespace = "Subtext.Framework" %>
<asp:Repeater ID="CatList" Runat="server" OnItemCreated="CategoryCreated">
	
	<ItemTemplate>
		<div class="box">
			<div class="innerBox">
				<h3><asp:Literal runat="server" ID="Title" /></h3>
				<asp:Repeater id="LinkList" runat="server" OnItemCreated="LinkCreated">
					<HeaderTemplate>
						<ul class="sidebarlist">
					</HeaderTemplate>
					<ItemTemplate>
							<li><asp:HyperLink Runat="server" ID="Link" /></li>
					</ItemTemplate>
					<FooterTemplate>
						</ul>
					</FooterTemplate>
				</asp:Repeater>
			</div>
		</div>
	</ItemTemplate>
</asp:Repeater>