<%@ Control Language="c#" AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.SubtextSearch" %>
<div id="search">
<asp:TextBox id="txtSearch" text="search..." onfocus="if(this.value=='search...') this.value='';" runat="server" onblur="if(this.value=='') this.value='search...';" CssClass="searchterm"></asp:TextBox>
<asp:Button height="23" id="btnSearch" runat="server" text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" CssClass="searchButton" CausesValidation="False"></asp:Button>
	<div class="results">
		<asp:Repeater id="SearchResults" runat="server">
			<HeaderTemplate>
				<div class="title">Results</div>
			</HeaderTemplate>
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
