<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.EntryList" %>
<%@ Import Namespace = "Subtext.Framework" %>
		<h2><asp:Literal ID="EntryCollectionTitle" Runat="server" /></h2>
		<asp:Literal ID="EntryCollectionDescription" Runat="server" />
		<asp:Repeater runat="Server" Runat="server" ID="Entries" OnItemCreated="PostCreated">
			<ItemTemplate>
					<div class="blogpost">
						<h1><span class="title"><asp:HyperLink Runat="server" ID="TitleUrl" /></span></h1>
						
						<asp:Literal  runat="server" ID="PostText" />
						
						<p class="postfooter">
							<asp:Literal ID = "PostDesc" Runat = "server" />
						</p>
					</div>
			</ItemTemplate>
		</asp:Repeater>

		<p>
			<asp:HyperLink Runat="server" ID="EntryCollectionReadMoreLink" />
		</p>
		