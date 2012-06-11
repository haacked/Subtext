<%@ Page Language="C#" EnableTheming="false"  ValidateRequest="false" Trace="false" CodeBehind="ftb.imagegallery.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.Admin.ftb_imagegallery" %>
<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
<html>
<head>
	<title>Image Gallery</title>
</head>
<body>

    <form id="Form1" runat="server" enctype="multipart/form-data">  
		<FTB:ImageGallery AllowImageDelete=false AllowImageUpload=true AllowDirectoryCreate=false AllowDirectoryDelete=false id="imageGallery" runat="Server" />
		<asp:PlaceHolder runat="server" ID="errorMsg">You don't have enough permission to access folder <asp:Label ID="folderName" runat="server"></asp:Label> </asp:PlaceHolder>
	</form>

</body>
</html>
