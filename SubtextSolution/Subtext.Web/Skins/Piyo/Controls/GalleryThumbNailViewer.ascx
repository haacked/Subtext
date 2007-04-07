<%@ Control Language="C#" EnableTheming="false"  AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.GalleryThumbNailViewer" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<h1><asp:Literal id="GalleryTitle" runat="server" /></h1>
<asp:Literal ID = "Description" Runat = "server" />
<div class="Thumbnail">
<asp:DataList id="ThumbNails" runat="server" OnItemCreated = "ImageCreated" RepeatDirection = "Horizontal" repeatlayout="Flow">
	<ItemTemplate>
		<a href="<%#BaseImagePath + ((Subtext.Framework.Components.Image)Container.DataItem).ResizedFile %>" rel="lightbox" title="<%#((Subtext.Framework.Components.Image)Container.DataItem).Title %>"><img src="<%#BaseImagePath + ((Subtext.Framework.Components.Image)Container.DataItem).ThumbNailFile %>"></a>
	</ItemTemplate>
</asp:DataList>
</div>