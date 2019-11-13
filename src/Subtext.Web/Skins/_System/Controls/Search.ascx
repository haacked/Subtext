<%@ Control Language="C#" EnableTheming="false"  AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.Search" %>
<div class="search-results">
    <div id="search">
	    <asp:TextBox id="txtSearch" runat="server" class="searchterm" /> <asp:Button id="btnSearch" runat="server" class="searchButton" Text="Search"  CausesValidation="False" OnClick="btnSearch_Click" />
    </div>
    <h2>Results</h2>
        <asp:PlaceHolder ID="noresults" runat="server" Visible="false">No Results for the search term: <asp:Literal ID="terms" runat="server"></asp:Literal> </asp:PlaceHolder>
	    <asp:Repeater id="results" runat="server" OnItemCreated="SearchResultsCreated" Visible="false">
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