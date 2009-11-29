<%@ Control Language="C#" EnableTheming="false"  AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.SubtextSearch" %>
<div id="search">
	<label for="txtSearch">search for term</label> <asp:TextBox id="txtSearch" runat="server" CssClass="searchterm" /> <asp:Button id="btnSearch" runat="server" CssClass="searchButton" Text="GO" CausesValidation="False" />
	
	<asp:Repeater id="SearchResults" runat="server">
		<HeaderTemplate>
			<div id="searchResults">
				<h5>Results</h5>
				<div class="searchClose">
					<a href="<%# HttpContext.Current.Request.Url %>" title="Close Results">[x]</a>
				</div>
				<ul>
		</HeaderTemplate>
		<ItemTemplate>
					<li><a href="<%# DataBinder.Eval(Container.DataItem, "url") %>"><%# DataBinder.Eval(Container.DataItem, "Title") %></a></li>
		</ItemTemplate>
		<FooterTemplate>
				</ul>
			</div>
		</FooterTemplate>
	</asp:Repeater>
</div>