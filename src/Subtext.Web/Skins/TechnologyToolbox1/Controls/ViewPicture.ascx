<%@ Control Language="C#" EnableTheming="false" AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.ViewPicture" %>
<!-- Begin: ViewPicture.ascx -->
<div class="singleImage">
    <div class="title">
        <asp:Literal ID="Title" runat="server" /></div>
    <div class="image">
        <asp:Image ID="GalleryImage" runat="server" /></div>
    <asp:HyperLink ID="ReturnUrl" Text="Return to Gallery" runat="server" />&nbsp;|
    <asp:HyperLink ID="OriginalImage" Text="Original Image" runat="server" Target="_New" />
</div>
<!-- End: ViewPicture.ascx -->