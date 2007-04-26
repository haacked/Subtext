<%@ Control Language="C#" EnableTheming="false"  AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.SubtextSearch" %>
<div id="search">
	<asp:UpdateProgress ID="searchProgress" runat="server">
		<ProgressTemplate>
			<div id="search-progress">
			</div>
		</ProgressTemplate>
	</asp:UpdateProgress>
	<div id="search-box">
		<asp:TextBox id="txtSearch" runat="server" cssclass="searchterm" /> <asp:Button id="btnSearch" runat="server" cssclass="searchButton" Text="Search" CausesValidation="False" />
	</div>
	<asp:UpdatePanel ID="searchUpdate" runat="server" UpdateMode="conditional">
		<Triggers>
			<asp:AsyncPostBackTrigger ControlID="btnSearch" />
		</Triggers>
		<ContentTemplate>
			<asp:Repeater id="SearchResults" runat="server">
				<HeaderTemplate>
					<div id="search-results">
						<div class="dropshadow">
							<div class="innerbox">
								<asp:LinkButton ID="closeButton" runat="server" CssClass="close" ToolTip="close search">[x]</asp:LinkButton>
						<h5>Search Results</h5>
						<ul>
				</HeaderTemplate>
				<ItemTemplate>
							<li><a href="<%# DataBinder.Eval(Container.DataItem, "url") %>"><%# DataBinder.Eval(Container.DataItem, "Title") %></a></li>
				</ItemTemplate>
				<FooterTemplate>
						</ul>
							</div>
						</div>
					</div>
				</FooterTemplate>
			</asp:Repeater>
		</ContentTemplate>
	</asp:UpdatePanel>
</div>