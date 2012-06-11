<%@ Control Language="C#" EnableTheming="false"  AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.LinkPage" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>

<div class="block">
<h1 class="block_title">Links</h1>
<div class="post">

<asp:DataList CssClass="link_table" id="CatList" runat="Server" OnItemCreated="CategoryCreated" RepeatColumns="3" RepeatDirection="Vertical">
	<ItemTemplate>
		<h3 class="link_title">
			<asp:Literal runat="server" ID="Title" /></h3>
		<asp:Repeater id="LinkList" runat="server" OnItemCreated="LinkCreated">
			<HeaderTemplate>
				<ul class="myLinks">
			</HeaderTemplate>
			<ItemTemplate>
				<li>
					<asp:HyperLink Runat="server" ID="Link" />
					<asp:HyperLink Runat="server" ID="RssLink" Text="(rss)" Visible="False" /></li>
			</ItemTemplate>
			<FooterTemplate>
				</ul>
			</FooterTemplate>
		</asp:Repeater>
	</ItemTemplate>
</asp:DataList>

</div>
<div class="block_footer">&nbsp;</div>
</div>