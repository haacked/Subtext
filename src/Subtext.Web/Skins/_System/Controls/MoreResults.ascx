<%@ Control Language="C#" EnableTheming="false"  AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.MoreResultsLikeThis" %>
<div id="related-results">
    <h2>
        More Results searching for &#8220;<span class="search-term"><asp:Literal ID="keywords" runat="server" /></span>&#8221;
        <a href="#related-results" class="close"></a>
     </h2>
     <div class="content">
         <p>
            It looks like you found this post via a search engine result.<br />
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
			    </ul>
			    <a id="searchMore" class="more-results" runat="server">Click for all Search Results for </a>
		    </FooterTemplate>
	    </asp:Repeater>
	</div>
</div>
