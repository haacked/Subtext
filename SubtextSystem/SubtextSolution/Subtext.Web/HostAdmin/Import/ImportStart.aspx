<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<%@ Page language="c#" Codebehind="ImportStart.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.HostAdmin.ImportStart" %>
<MP:MasterPage id="MPContainer" runat="server" masterpagefile="~/HostAdmin/PageTemplate.ascx">
	<MP:ContentRegion id="MPTitle" runat="server">Subtext - Host Admin - Import Wizard</MP:ContentRegion>
	<MP:ContentRegion id="MPSideBar" runat="server">
		<asp:Button id="btnRestartWizard" runat="server" text="Restart Import"></asp:Button>
	</MP:ContentRegion>
	<!-- now, lay out our content for the DefaultContent Region (MPContent) -->
		<p>
			Welcome to the .TEXT Import Wizard!
		</p>
		<p>
			This wizard is strictly for bulk importing .TEXT data.  If you wish to import from 
			other blogging engines, you can do that in the admin section of your specific blog.
		</p>
		<p>
			We’ll walk you through the steps to get you up and running in no time.
		</p>
		<P>
			<asp:Button id="btnNext" runat="server" text="Next - Specify Connection Information"></asp:Button>
		</p>
	<!-- END DefaultContent -->
</MP:MasterPage>
