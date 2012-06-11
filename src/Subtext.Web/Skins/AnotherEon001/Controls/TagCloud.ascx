<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.TagCloud" %>


<asp:Repeater Runat="server" ID="Tags" OnItemDataBound="Tags_ItemDataBound">
	<HeaderTemplate>
		<h3>Tag Cloud</h3>
			<ul id="tag-cloud">
	</HeaderTemplate>
	<ItemTemplate>
		<li>
			<asp:HyperLink  Runat="server" ID="TagUrl" CssClass='<%# Eval("Weight", "tag-style-{0} tag-item") %>' 
				Text='<%# UrlDecode(Eval("TagName")) %>' ToolTip='<%# Eval("Count") %>'/>
		</li>
	</ItemTemplate>
	<FooterTemplate>
		</ul>
	</FooterTemplate>
</asp:Repeater>
