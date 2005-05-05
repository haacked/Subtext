<%@ Page language="c#" Codebehind="EditPosts.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.Admin.Pages.EditPosts" %>
<%@ Register TagPrefix="EED" TagName="EntryEditor" Src="UserControls/EntryEditor.ascx" %>
<%@ Register TagPrefix="ANW" Namespace="Subtext.Web.Admin.WebUI" Assembly="Subtext.Web.Admin" %>
<ANW:Page id="PageContainer" CategoryType="PostCollection" TabSectionID="Posts" runat="server">
	<EED:EntryEditor id="Editor" CategoryType="PostCollection" EntryType="BlogPost" ResultsTitle="Posts" runat="server" />
</ANW:Page>
