<%@ Control Language="C#" EnableTheming="false"  AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.ViewPicture" %>
<div class="singleImage">
	<div class="title"><asp:Literal id="Title" runat="server" /></div>
	<div class="image">
	    <asp:HyperLink ID="OriginalImage" Runat="server" rel="lightbox"><asp:Image ID="GalleryImage" Runat="server" /></asp:HyperLink>
	</div>
	<asp:HyperLink ID="ReturnUrl" Text="Return to Gallery" Runat="server" />
</div>
