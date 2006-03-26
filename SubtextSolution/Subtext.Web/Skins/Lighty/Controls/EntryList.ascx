<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.EntryList" %>
<%@ Import Namespace = "Subtext.Framework" %>
<h1 class="block_title"><asp:Literal ID = "EntryCollectionTitle" Runat = "server" /></h1>
<asp:Literal ID = "EntryCollectionDescription" Runat = "server" />
<asp:Repeater runat="Server" Runat="server" ID="Entries" OnItemCreated="PostCreated">
	<HeaderTemplate>
		<div class="block">
	</HeaderTemplate>
	<ItemTemplate>
		<div class="entrylistitem">
			<asp:HyperLink Runat="server" ID="editLink" /><asp:HyperLink  CssClass="entrylisttitle" Runat="server" ID="TitleUrl" />
			<asp:Literal ID = "PostText" Runat = "server" />
			<div class="itemdesc">			
				<asp:Literal ID = "PostDesc" Runat = "server" />
			</div>
		</div>
	</ItemTemplate>
	<FooterTemplate>
			<div class="block_footer">&nbsp;</div>
		</div>
	</FooterTemplate>
</asp:Repeater>
<p>
<asp:HyperLink Runat="server" ID="EntryCollectionReadMoreLink" />
</p>
