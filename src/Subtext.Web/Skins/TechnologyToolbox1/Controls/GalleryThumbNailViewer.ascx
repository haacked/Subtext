<%@ Control Language="C#" EnableTheming="false" AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.GalleryThumbNailViewer"
    TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<!-- Begin: GalleryThumNailViewer.ascx -->
<div class="post type-post hentry">
    <h1>
        <asp:Literal ID="GalleryTitle" runat="server" /></h1>
    <div class="description">
        <asp:Literal ID="Description" runat="server" /></div>
    <div class="thumbnails">
        <asp:DataList ID="ThumbNails" runat="server" OnItemCreated="ImageCreated" RepeatColumns="6"
            RepeatDirection="Vertical">
            <ItemTemplate>
                <div class="thumbnail">
                    <asp:HyperLink runat="server" ID="ThumbNailImage" /></div>
            </ItemTemplate>
        </asp:DataList>
    </div>
</div>
<!-- End: GalleryThumNailViewer.ascx -->