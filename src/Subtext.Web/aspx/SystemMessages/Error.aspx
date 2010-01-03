<%@ Page Language="C#" EnableTheming="false"  Title="Subtext - Application Error" MasterPageFile="~/aspx/SystemMessages/SystemMessageTemplate.Master" EnableViewState="False" Codebehind="Error.aspx.cs" AutoEventWireup="True" Inherits="Subtext.Web.Pages.Error" %>

<asp:Content id="titleBar" ContentPlaceHolderID="MPTitle" runat="server">Application Error!</asp:Content>
<asp:Content id="subtitle" ContentPlaceHolderID="MPSubTitle" runat="server">Details.</asp:Content>
<asp:Content id="mainContent" ContentPlaceHolderID="Content" runat="server">
		<p>
			<asp:Label id="ErrorMessageLabel" runat="server" />
		</p>
		<p>
			<asp:HyperLink id="HomeLink" runat="server">Return to site</asp:HyperLink>
		</p>
</asp:Content>
