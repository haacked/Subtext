<%@ Control Language="C#" EnableTheming="false" AutoEventWireup="false" 
    Inherits="Subtext.Web.UI.Controls.GalleryThumbNailViewer" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Import Namespace="Image=Subtext.Framework.Components.Image" %>
<div id="gallery">
	<div class="title"><asp:Literal id="GalleryTitle" runat="server" /></div>
	<div class="description"><asp:Literal id="Description" runat="server" /></div>
	<div class="thumbnails">
		<asp:DataList id="ThumbNails" runat="server" RepeatColumns="5" RepeatDirection="Horizontal">
			<ItemTemplate>
				<div class="thumbnail">
					<a href="<%# Url.GalleryImageUrl((Image) Container.DataItem,((Image) Container.DataItem).ResizedFile) %>" 
				        title="<%# ((Image) Container.DataItem).Title %>" rel="lightbox[<%# ((Image) Container.DataItem).CategoryID %>]">
				        <img src="<%# Url.GalleryImageUrl(((Image) Container.DataItem), ((Image) Container.DataItem).ThumbNailFile) %>" alt="<%# ((Image)Container.DataItem).Title %>" />    
				    </a>
				</div>
			</ItemTemplate>
		</asp:DataList>
	</div>
</div>