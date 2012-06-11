<%@ Control Language="C#" EnableTheming="false"  AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.RelatedLinks" %>
<div class = "relatedlinks">
	<div class = "relatedlinkstitle">
		Related Posts
	</div>
	<asp:Repeater id="urlRelatedLinks" runat="server" OnItemCreated="MoreReadingCreated">
		<ItemTemplate>
			<div class="relateditem">
				<a href="<%# DataBinder.Eval(Container.DataItem, "url") %>"> 
					<%# DataBinder.Eval(Container.DataItem, "Title") %> 
				</a>
			</div>
		</ItemTemplate>
	</asp:Repeater>
</div>