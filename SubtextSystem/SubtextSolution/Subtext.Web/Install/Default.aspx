<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<%@ Page language="c#" Codebehind="Default.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.Install.Default" %>
<MP:MasterPage id="MPContainer" TemplateFile="~/Install/PageTemplate.ascx" runat="server">
	<MP:ContentRegion id="MPTitle" runat="server">Subtext Installation: Welcome</MP:ContentRegion>
	<MP:ContentRegion id="MPSubTitle" runat="server">Welcome</MP:ContentRegion>
	<P>Welcome to the Subtext Installation Wizard.
	</P>
	<P>Here are the following steps you will take...</P>
		<OL>
			<LI>
			Gather Installation Information
			<LI>
			Install the database
			<LI>
			Configure the Host Admin
			<LI>
				Create a Blog
			</LI>
		</OL>
	<P><asp:HyperLink id="lnkNextStep" runat="server">Off to Step 1</asp:HyperLink></P>
</MP:MasterPage>

