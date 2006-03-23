<%@ Control Language="c#" AutoEventWireup="false" Inherits="Subtext.Web.Skins.Naked.Controls.RecentComments" CodeBehind="RecentComments.ascx.cs" %>
<div class="title">Recent Comments.</div>
<div id="recentComments">
	<asp:Repeater id="feedList" Runat="server" OnItemCreated="EntryCreated">
		<HeaderTemplate>
				<ul>
		</HeaderTemplate>
		<ItemTemplate>
				<li><asp:HyperLink Runat="server" id="Link" /><br /><asp:Literal runat="server" id="Author" /></li>
		</ItemTemplate>
		<FooterTemplate>
				</ul>
		</FooterTemplate>
	</asp:Repeater>
</div>