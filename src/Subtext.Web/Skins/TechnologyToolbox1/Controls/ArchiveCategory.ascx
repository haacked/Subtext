<%@ Control Language="C#" EnableTheming="false" AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.ArchiveCategory" %>
<%@ Register TagPrefix="uc1" TagName="DayCollection" Src="DayCollection.ascx" %>
<!-- Begin: ArchiveCategory.ascx -->
<h2 class="archiveCategoryTitle">
    <asp:Literal runat="server" ID="Title" /></h2>
<uc1:DayCollection runat="server" ID="Days" />
<!-- End: ArchiveCategory.ascx -->