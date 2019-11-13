<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.EntryList" %>

<h1><asp:Literal ID="EntryCollectionTitle" Runat="server" /></h1>
<asp:Literal ID="EntryCollectionDescription" Runat="server" />
<asp:Repeater runat="Server" Runat="server" ID="Entries" OnItemCreated="PostCreated">
	<ItemTemplate>
			<div class="post">
				<h2><asp:HyperLink Runat="server" ID="editLink" /><asp:HyperLink Runat="server" ID="TitleUrl" /></h2>
				
				<asp:Literal  runat="server" ID="PostText" />
				
				<p class="postInfo">		
					<asp:Literal ID="PostDesc" Runat = "server" />
				</p>
			</div>
	</ItemTemplate>
</asp:Repeater>

<p>
	<asp:HyperLink Runat="server" ID="EntryCollectionReadMoreLink" />
</p>
