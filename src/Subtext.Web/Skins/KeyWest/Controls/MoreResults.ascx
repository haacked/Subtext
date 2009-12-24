<%@ Control Language="C#" EnableTheming="false"  AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.MoreResultsLikeThis" %>
<div class = "moreinfo">
<p>It seems like you found this post looking for <b><asp:Literal ID="keywords" runat="server" /></b> on a search engine.<br />
Here are a few other posts you might find interesting:</p>
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
		    <li><b><a ID="searchMore" runat="server">More on </a></b></li>
			</ul>
		</FooterTemplate>
	</asp:Repeater>
	
</div>
