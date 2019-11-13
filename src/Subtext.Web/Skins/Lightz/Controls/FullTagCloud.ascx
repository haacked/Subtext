<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.TagCloud" %>

<div class="block">
	<asp:Repeater Runat="server" ID="Tags" OnItemDataBound="Tags_ItemDataBound">
		<HeaderTemplate>
			<h1 class="block_title">Tag Cloud</h1>
			<div class="post">
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
			</div>
			
		</FooterTemplate>
	</asp:Repeater>
<div class="block_footer">&nbsp;</div>		
</div>