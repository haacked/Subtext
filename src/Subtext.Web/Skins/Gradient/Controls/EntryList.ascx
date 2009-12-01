<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.EntryList" %>

		<h3><asp:Literal ID="EntryCollectionTitle" Runat="server" /></h3>
		<asp:Literal ID="EntryCollectionDescription" Runat="server" />
		<asp:Repeater runat="Server" Runat="server" ID="Entries" OnItemCreated="PostCreated">
			<ItemTemplate>
					<div class="blogpost">
						<h2 class="postTitle"><asp:HyperLink Runat="server" ID="editLink" /><asp:HyperLink Runat="server" ID="TitleUrl" /></h2>
						<asp:Literal  runat="server" ID="PostText" />
						<div class="postfooter">
							<ul class="postInfo">
								<li class="permalink"><asp:Label id="date" runat="server" Format="MMM dd, yyyy" /></li>
								<li class="commentCount"><asp:Label id="commentCount" runat="server" /></li>
							</ul>
						</div>
					</div> <!-- end #blogpost -->
				</ItemTemplate>
		</asp:Repeater>

		<p>
			<asp:HyperLink Runat="server" ID="EntryCollectionReadMoreLink" />
		</p>
		