<%@ Control Language="C#" EnableTheming="false"  AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.RelatedLinks" %>
<a name = "links">
<div class = "moreinfo">
	<div class = "moreinfotitle">
		Related Links
	</div>
		<asp:Repeater id="Links" runat="server" OnItemCreated="MoreReadingCreated">
			<HeaderTemplate>
				<ul class = "morelist">
			</HeaderTemplate>
			<ItemTemplate>
				<li class = "morelistitem">
					<asp:HyperLink Target="_blank" Runat="server" ID="Link" />
					<asp:Literal Runat="server" ID="DisplayType" />
					<asp:LinkButton Runat="server" ID="EditReadingLink" CausesValidation="False" />								
			</li>	
		</ItemTemplate>
		<FooterTemplate>
			</ul>
		</FooterTemplate>
	</asp:Repeater>
</div>
