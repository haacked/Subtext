<%@ Control Language="C#" EnableTheming="false"  AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.RelatedLinks" %>
<a name = "links">
<div id="relatedlinks">
	<h3>Related Links</h3>
		<asp:Repeater id="Links" runat="server" OnItemCreated="MoreReadingCreated">
			<HeaderTemplate>
				<ul>
			</HeaderTemplate>
			<ItemTemplate>
				<li>
					<asp:HyperLink Runat="server" ID="Link" /> (<asp:Literal runat="server" ID="DatePublished" />)
			    </li>	
		</ItemTemplate>
		<FooterTemplate>
			</ul>
		</FooterTemplate>
	</asp:Repeater>
</div>
