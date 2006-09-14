<%@ Control Language="c#" AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.GalleryThumbNailViewer" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<div id="gallery">
<div class="title"><asp:Literal id="GalleryTitle" runat="server" /></div>
<div class="description"><asp:Literal ID = "Description" Runat = "server" /></div>
<div class="thumbnails">
<asp:DataList id="ThumbNails" runat="server" OnItemCreated = "ImageCreated" RepeatColumns = "6" RepeatDirection = "Vertical">
	<ItemTemplate>
		<a href="<%#BaseImagePath + ((Subtext.Framework.Components.Image)Container.DataItem).ResizedFile %>" rel="lightbox" title="<%#((Subtext.Framework.Components.Image)Container.DataItem).Title %>">
			<img src="<%#BaseImagePath + ((Subtext.Framework.Components.Image)Container.DataItem).ThumbNailFile %>">
		</a>
		<!--<div class="thumbnail"><asp:HyperLink Runat="server" rel="lightbox" ID="ThumbNailImage"/></div>-->
	</ItemTemplate>
</asp:DataList>
</div>
</div>