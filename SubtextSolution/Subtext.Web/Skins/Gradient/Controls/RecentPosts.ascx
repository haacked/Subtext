<%@ Control Language="C#" AutoEventWireup="true" Inherits="Subtext.Web.UI.Controls.RecentPosts" %>
<asp:Repeater id="postList" Runat="server" OnItemCreated="PostCreated">
	<HeaderTemplate>
			<ul>
	</HeaderTemplate>
	<ItemTemplate>
			<li><a href="<%# Entry.Url %>" title="Recent post"><%# Entry.Title %></a></li>
	</ItemTemplate>
	<FooterTemplate>
			</ul>
	</FooterTemplate>
</asp:Repeater>

