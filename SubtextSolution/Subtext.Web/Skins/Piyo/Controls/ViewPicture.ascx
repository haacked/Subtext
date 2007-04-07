<%@ Control Language="C#" EnableTheming="false"  AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.ViewPicture" %>
<div class="imageFrame">
	<h1 style="margin-bottom:10px;"><asp:Literal id="Title" runat="server" /></h1>
	<asp:Image ID="GalleryImage" Runat="server" />
	<br />
	<br />
	<asp:HyperLink ID="ReturnUrl" Text="Return to Gallery" Runat="server" />&nbsp;|
	<asp:HyperLink ID="OriginalImage" Text="Original Image" Runat="server" Target="_New" />
</div>
