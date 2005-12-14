<%@ Page language="c#" Codebehind="ImportExport.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.Admin.Pages.ImportExportPage" %>
<%@ Register TagPrefix="ANW" Namespace="Subtext.Web.Admin.WebUI" Assembly="Subtext.Web" %>
<ANW:PAGE id="PageContainer" runat="server" TabSectionID="ImportExport">
	<ANW:MessagePanel id="Messages" runat="server"></ANW:MessagePanel>
	<ANW:AdvancedPanel id="Action" runat="server" BodyCssClass="Edit" DisplayHeader="true" HeaderCssClass="CollapsibleHeader"
		HeaderText="Options" Collapsible="False">
		<FIELDSET>
			<LEGEND>Import &amp; Export BlogML files.</LEGEND>
			<H4>Persist this blog to BlogML Format.</H4>
			<P>This function will generate BlogML for your blog and output it as an xml file. 
				After the BlogML generation phase you will be presented with a link to the 
				file.</P>
			<P>
				<asp:CheckBox id="chkEmbedAttach" runat="server" Text="Embed Attachments?" Checked="True"></asp:CheckBox><BR>
				<asp:CheckBox id="chkDisplayOnScreen" runat="server" Text="Display Output on Screen?" Enabled="False"></asp:CheckBox><BR>
				<BR>
				<asp:Button id="btnSave" runat="server" Text="Save" CssClass="buttonSubmit"></asp:Button>&nbsp;&nbsp;
				<asp:HyperLink id="hypBlogMLFile" runat="server" Visible="False">Download BlogML File</asp:HyperLink></P>
			<HR>
			<!-- BlogML Reader -->
			<H4>Import from BlogML.</H4>
			<P>Allows you to import an existing blog by loading BlogML content.</P>
			<P><INPUT id="importBlogMLFile" type="file" name="filImportBlogML" runat="server"><BR>
				<BR>
				<asp:Button id="btnLoad" runat="server" Text="Load!" CssClass="buttonSubmit"></asp:Button></P>
		</FIELDSET>
	</ANW:AdvancedPanel>
</ANW:PAGE>
