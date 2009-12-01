<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.RecentComments" %>

<div>
	<h2>Recent Comments</h2>

	<asp:Repeater ID="feedList" Runat="server" OnItemCreated="EntryCreated">
	   <HeaderTemplate>
			<ul>
	   </HeaderTemplate>
	   <ItemTemplate>
				<li>
					<asp:HyperLink Runat="server" ID="Link" /> <span class="author"><asp:Literal Runat="server" ID="Author" /></span>
				</li>
	   </ItemTemplate>
	   <FooterTemplate>
			</ul>
	   </FooterTemplate>
	</asp:Repeater>

</div>