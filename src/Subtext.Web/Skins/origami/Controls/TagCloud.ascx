<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.TagCloud" %>

<div>
	<asp:Repeater Runat="server" ID="Tags" OnItemDataBound="Tags_ItemDataBound">
		<HeaderTemplate>
			<h3>Tag Cloud</h3>
			<ul id="tag-cloud">
		</HeaderTemplate>
		<ItemTemplate>
			<li>
				<asp:HyperLink  Runat="server" ID="TagUrl" CssClass='<%# Eval("Weight", "tag-style-{0} tag-item") %>' 
					Text='<%# UrlDecode(Eval("TagName")) %>' ToolTip='<%# UrlDecode(Eval("TagName", "{0} (" + Eval("Count") + ")")) %>'/>
			</li>
		</ItemTemplate>
		<FooterTemplate>
			</ul>
			<asp:HyperLink CssClass="more-link" runat="server" id="DefaultTagLink">more tags...</asp:HyperLink>
		</FooterTemplate>
	</asp:Repeater>
</div>