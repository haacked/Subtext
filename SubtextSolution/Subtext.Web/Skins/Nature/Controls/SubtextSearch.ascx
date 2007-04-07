<%@ Control Language="C#" EnableTheming="false"  AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.SubtextSearch" %>
<div id="search">
    <label for="txtSearch">Search:</label>
    <asp:TextBox id="txtSearch" runat="server" CssClass="searchterm"></asp:TextBox>
    <asp:Button id="btnSearch" runat="server" CssClass="searchButton" CausesValidation="False"></asp:Button>

    <div id="searchResults">
        <h5>Results</h5>
        <asp:Repeater id="Repeater1" runat="server">
	        <ItemTemplate>
		        <div id="item">
			        <a href="<%# DataBinder.Eval(Container.DataItem, "url") %>"> 
			        <%# DataBinder.Eval(Container.DataItem, "Title") %> 
			        </a>
		        </div>
	        </ItemTemplate>
        </asp:Repeater>
    </div>
</div>