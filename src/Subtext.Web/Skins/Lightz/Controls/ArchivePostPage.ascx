<%@ Control Language="C#" EnableTheming="false"  AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.ArchivePostPage" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<div>
<h1 class="block_title">Browse Archives</h1>
<div class="post">
<table class="archiveposts">
	<tr>
		<th>By Category</th>
		<th>By Month</th>
	</tr>
	<tr>
		<td>
			<asp:repeater id="CatList" runat="Server" OnItemCreated="CategoryCreated">
				<HeaderTemplate><br /></HeaderTemplate>
				<ItemTemplate>
					<asp:HyperLink ID="CatLink" Runat="server"></asp:HyperLink><br />
					<asp:Label ID="Description" Runat="server"></asp:Label><br /><br />
				</ItemTemplate>
			</asp:repeater>
		</td>
		<td>
			<asp:repeater ID="DateItemList" Runat="server" OnItemCreated="DateItemCreated">
				<HeaderTemplate><br /></HeaderTemplate>
				<ItemTemplate>
					<asp:HyperLink ID="DateLink" Runat="server"></asp:HyperLink><br />
				</ItemTemplate>
			</asp:repeater>
		</td>
	</tr>
</table>
</div>
<div class="block_footer">&nbsp;</div>
</div>

