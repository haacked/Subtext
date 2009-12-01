<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.EntryList" %>

<h1><asp:Literal ID = "EntryCollectionTitle" Runat = "server" /></h1>
<asp:Literal ID = "EntryCollectionDescription" Runat="server" />
<asp:Repeater runat="Server" Runat="server" ID="Entries" OnItemCreated="PostCreated">
	<ItemTemplate>
	<div class="entry">
				<h4><asp:HyperLink Runat="server" ID="editLink" /><asp:HyperLink  Runat="server" ID="TitleUrl" /></h4>
				<div class="post">
				<asp:Literal ID = "PostText" Runat = "server" />
				</div>
				<div class="info">
					<asp:Literal id="PostDesc"  runat="server" />
				</div>
	</div>
	</ItemTemplate>
</asp:Repeater>
<p>
<asp:HyperLink Runat="server" ID="EntryCollectionReadMoreLink" />
</p>