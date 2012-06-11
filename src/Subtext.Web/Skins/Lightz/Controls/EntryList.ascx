<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.EntryList" %>

<div class=block>
<h1 class="block_title"><asp:Literal ID = "EntryCollectionTitle" Runat = "server" /></h1>
<div class="post">
<asp:Literal ID = "EntryCollectionDescription" Runat = "server" />
</div>
<asp:Repeater runat="Server" Runat="server" ID="Entries" OnItemCreated="PostCreated">
	<HeaderTemplate>
		<br/>
	</HeaderTemplate>
	<ItemTemplate>
		<div class="entrylistitem">
			<asp:HyperLink  Runat="server" ID="editLink" /><asp:HyperLink  CssClass="entrylisttitle" Runat="server" ID="TitleUrl" />
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
