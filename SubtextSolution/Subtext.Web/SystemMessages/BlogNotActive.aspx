<%@ Page language="c#" Codebehind="BlogNotActive.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.BlogNotActive" %>
<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<MP:MasterPage id="MPContainer" TemplateFile="~/SystemMessages/PageTemplate.ascx" runat="server">
	<MP:ContentRegion id="MPTitleBar" runat="server">This Blog is Inactive</MP:ContentRegion>
	<MP:ContentRegion id="MPTitle" runat="server">This Blog is Inactive</MP:ContentRegion>
	<MP:ContentRegion id="MPSubTitle" runat="server">What you can do.</MP:ContentRegion>
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
</MP:MasterPage>