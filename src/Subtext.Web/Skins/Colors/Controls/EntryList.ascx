<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.EntryList" %>

<h1><asp:Literal ID = "EntryCollectionTitle" Runat = "server" /></h1>
<asp:Literal ID = "EntryCollectionDescription" Runat = "server" />
<asp:Repeater runat="Server" Runat="server" ID="Entries" OnItemCreated="PostCreated">
	<HeaderTemplate>
		<ul class = "entrylist">
	</HeaderTemplate>
	<ItemTemplate>
		<li class = "entrylistitem">
			<asp:HyperLink Runat="server" ID="editLink" /><asp:HyperLink  CssClass="entrylisttitle" Runat="server" ID="TitleUrl" />
			<asp:Literal ID = "PostText" Runat = "server" />
			<div class="itemdesc">		
				<asp:Literal ID = "PostDesc" Runat = "server" />
			</div>
		</li>
	</ItemTemplate>
	<FooterTemplate>
		</ul>
	</FooterTemplate>
</asp:Repeater>
<p>
<asp:HyperLink Runat="server" ID="EntryCollectionReadMoreLink" />
</p>