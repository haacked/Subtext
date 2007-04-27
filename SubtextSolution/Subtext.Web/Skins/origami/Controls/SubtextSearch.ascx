<%@ Control Language="C#" EnableTheming="false"  AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.SubtextSearch" %>
<div id="searchWrapper">
	<div id="search">
		<asp:TextBox id="txtSearch" runat="server" class="searchterm" /> <asp:Button id="btnSearch" runat="server" class="searchButton" Text="Search" CausesValidation="False" />
	</div>
	<asp:UpdatePanel ID="searchUpdate" runat="server" UpdateMode="conditional">
		<Triggers>
			<asp:AsyncPostBackTrigger ControlID="btnSearch" />
		</Triggers>
		<ContentTemplate>
			<asp:Repeater id="SearchResults" runat="server">
				<HeaderTemplate>
					<div id="searchResults">
						<asp:LinkButton ID="closeButton" runat="server" CssClass="closeSearch" ToolTip="close search">[x]</asp:LinkButton>
						<h5>Search Results</h5>
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
		</ContentTemplate>
	</asp:UpdatePanel>
	<asp:UpdateProgress ID="searchProgress" runat="server">
		<ProgressTemplate>
			<div id="search-progress">
			</div>
		</ProgressTemplate>
	</asp:UpdateProgress>
</div>