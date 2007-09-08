<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.EntryList" %>
<%@ Import Namespace = "Subtext.Framework" %>
<h2><asp:Literal ID = "EntryCollectionTitle" Runat = "server" /></h2>
<asp:Literal ID = "EntryCollectionDescription" Runat = "server" />
<asp:Repeater runat="Server" ID="Entries" OnItemCreated="PostCreated">
	<ItemTemplate>
			<div class="post">
				<h5><asp:HyperLink Runat="server" ID="editLink" /><asp:HyperLink Runat="server" ID="TitleUrl" /></h5>
				
				<asp:Literal  runat="server" ID="PostText" />
				
				<p class="postfoot">		
				<asp:Literal ID = "PostDesc" Runat = "server" />
				</p>
			</div>
	</ItemTemplate>
</asp:Repeater>

<p>
<asp:HyperLink Runat="server" ID="EntryCollectionReadMoreLink" />
</p>