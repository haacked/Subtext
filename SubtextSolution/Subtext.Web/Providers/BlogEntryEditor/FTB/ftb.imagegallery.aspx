<%@ Page Language="c#" ValidateRequest="false" Trace="false" CodeBehind="ftb.imagegallery.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.Admin.ftb_imagegallery" %>
<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
<html>
<head>
	<title>Image Gallery</title>
</head>
<body>

    <form id="Form1" runat="server" enctype="multipart/form-data">  
		<FTB:ImageGallery AllowImageDelete=false AllowImageUpload=true AllowDirectoryCreate=false AllowDirectoryDelete=false id="imageGallery" runat="Server" />
	</form>

</body>
</html>
