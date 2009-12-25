<%@ Control Language="C#" EnableTheming="false"  AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.MoreResultsLikeThis" %>
<div class="related-results">
    <h2>
        It seems like you found this post looking for 
        &#8220;<span class="search-term"><asp:Literal ID="keywords" runat="server" /></span>&#8221; 
        on a search engine.
     </h2>
     <p>
        Here are a few other posts you might find interesting:
     </p>
	<asp:Repeater id="Links" runat="server" OnItemCreated="MoreReadingCreated">
		<HeaderTemplate>
			<ul class="morelist">
		</HeaderTemplate>
		<ItemTemplate>
			    <li class="morelistitem">
				    <asp:HyperLink Target="_blank" Runat="server" ID="Link" />
				    <asp:Literal Runat="server" ID="DisplayType" />
				    <asp:LinkButton Runat="server" ID="EditReadingLink" CausesValidation="False" />								
		        </li>	
	    </ItemTemplate>
		<FooterTemplate>
		        <li><strong><a id="searchMore" runat="server">All Search Results for </a></strong></li>
			</ul>
		</FooterTemplate>
	</asp:Repeater>
</div>
