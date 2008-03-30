<%@ Control Language="c#" AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.SubtextSearch" %>
<div id="searchBox">
<asp:TextBox id="txtSearch" runat="server" class="sText"></asp:TextBox>
<asp:Button id="btnSearch" runat="server"  class="sButton" CausesValidation="False"></asp:Button>
<br />

<h5>Results</h5>
<asp:Repeater id="SearchResults" runat="server">
	<ItemTemplate>
		<div id="item">
			<a href="<%# DataBinder.Eval(Container.DataItem, "url") %>"> 
			<%# DataBinder.Eval(Container.DataItem, "Title") %> 
			</a>
		</div>
	</ItemTemplate>
</asp:Repeater>
</div>