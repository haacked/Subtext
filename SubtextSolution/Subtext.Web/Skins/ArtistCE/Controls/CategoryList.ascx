<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.CategoryList" %>
<%@ Import Namespace = "Subtext.Framework" %>
<asp:Repeater ID="CatList" Runat="server" OnItemCreated="CategoryCreated">
	<HeaderTemplate>
		<div>
	</HeaderTemplate>
	<ItemTemplate>
			<h5><asp:Literal runat="server" ID="Title"/></h5>
			<asp:Repeater id="LinkList" runat="server" OnItemCreated="LinkCreated">
					<HeaderTemplate>
						<ul>
					</HeaderTemplate>
		    		<ItemTemplate>
		    			<li>
							<asp:HyperLink Runat="server" ID = "RssLink" Text="rss" ImageUrl="~/skins/artistce/Images/rssButton.gif" Visible="False" />
							<asp:HyperLink Runat="server" ID="Link" />
						</li>
					</ItemTemplate>
					<FooterTemplate>
						</ul>
					</FooterTemplate>
			</asp:Repeater>
	</ItemTemplate>
	<FooterTemplate>
		</div>
	</FooterTemplate>
</asp:Repeater>