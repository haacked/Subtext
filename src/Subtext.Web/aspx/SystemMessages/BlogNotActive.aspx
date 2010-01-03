<%@ Page Language="C#" EnableTheming="false"  Title="Subtext - This Blog Is Inactive" MasterPageFile="~/aspx/SystemMessages/SystemMessageTemplate.Master" Codebehind="BlogNotActive.aspx.cs" AutoEventWireup="True" Inherits="Subtext.Web.BlogNotActive" %>

<asp:Content id="titleBar" ContentPlaceHolderID="MPTitle" runat="server">This Blog is Inactive</asp:Content>
<asp:Content id="subtitle" ContentPlaceHolderID="MPSubTitle" runat="server">What you can do.</asp:Content>
<asp:Content id="mainContent" ContentPlaceHolderID="Content" runat="server">
	<asp:PlaceHolder id="plcInactiveBlogMessage" runat="server" Visible="false">
		<p>
			For whatever reason, this blog is no longer active.  It can be reactivated by 
			a host admin from the <a href="~/HostAdmin/" id="hostAdminLink" runat="server">Host Admin Tool</a>.
		</p>
	</asp:PlaceHolder>
	
	<asp:PlaceHolder id="plcNothingToSeeHere" runat="server" Visible="true">
		<p>
			Hey there, I think you made it to this page by mistake.  
			This <a id="lnkBlog" runat="server">blog seems</a> to be leading an active and healthy life.
		</p>
	</asp:PlaceHolder>
</asp:Content>