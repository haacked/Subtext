<%@ Control Language="C#" EnableTheming="false"  AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.GalleryThumbNailViewer" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Import Namespace="Image=Subtext.Framework.Components.Image" %>

<h1><asp:Literal id="GalleryTitle" runat="server" /></h1>
<asp:Literal ID = "Description" Runat = "server" />
<div class="Thumbnail">
<asp:DataList id="ThumbNails" runat="server" OnItemCreated = "ImageCreated" RepeatDirection = "Horizontal" repeatlayout="Flow">
	<ItemTemplate>
		<a href="<%# Url.GalleryImageUrl((Image)Container.DataItem,((Image)Container.DataItem).ResizedFile) %>" 
				        title="<%# ((Image) Container.DataItem).Title %>" rel="lightbox[<%# ((Image) Container.DataItem).CategoryID %>]">
				        <img src="<%# Url.GalleryImageUrl(((Image) Container.DataItem), ((Image) Container.DataItem).ThumbNailFile) %>" alt="<%# ((Image)Container.DataItem).Title %>" />    
				    </a>
	</ItemTemplate>
</asp:DataList>
</div>