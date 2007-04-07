<%@ Control Language="C#" EnableTheming="false"  AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.ArchiveCategory" %>
<%@ Register TagPrefix="uc1" TagName="DayCollection" Src="DayCollection.ascx" %>
<h1><asp:Literal Runat="server" ID="Title" /></h1>
<uc1:DayCollection id="Days" runat="server" />
