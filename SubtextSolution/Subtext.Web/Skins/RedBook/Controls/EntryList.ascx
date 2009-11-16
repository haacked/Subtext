<%@ Control Language="C#" EnableTheming="false" Inherits="Subtext.Web.UI.Controls.EntryList" %>
<h1><asp:Literal ID="EntryCollectionTitle" Runat="server" /></h1>
<asp:Literal ID="EntryCollectionDescription" Runat="server" />
<asp:Repeater runat="Server" Runat="server" ID="Entries" OnItemCreated="PostCreated">
	<ItemTemplate>
	<div class="journal_eintrag">
				<h2><asp:HyperLink Runat="server" ID="editLink" /><asp:HyperLink  Runat="server" ID="TitleUrl" /></h2>
				<asp:Literal ID = "PostText" Runat = "server" />
				<p class="postfooter">
					<asp:Literal id="PostDesc"  runat="server" />
				</p>
				</div>
	</ItemTemplate>
</asp:Repeater>
<p>
<asp:HyperLink Runat="server" ID="EntryCollectionReadMoreLink" />
</p>