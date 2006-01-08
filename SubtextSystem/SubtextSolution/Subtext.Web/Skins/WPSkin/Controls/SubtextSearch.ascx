<%@ Control Language="c#" AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.SubtextSearch" %>
<asp:TextBox id="txtSearch" runat="server" Width="145px" alt="lskjdflk"></asp:TextBox>
<asp:Button id="btnSearch" runat="server" Text="Go!" alt="Search here"></asp:Button>

<h3>Search Results</h3>
<asp:Repeater id="SearchResults" runat="server">
	<ItemTemplate>
		<div class="item">
			<font size="2">
			<a href="<%# DataBinder.Eval(Container.DataItem, "url") %>"> 
			<%# DataBinder.Eval(Container.DataItem, "Title") %> 
			</a>
			</font>
		</div>
	</ItemTemplate>
</asp:Repeater>
