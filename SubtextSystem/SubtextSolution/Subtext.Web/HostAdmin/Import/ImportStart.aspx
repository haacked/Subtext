<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<%@ Page language="c#" Codebehind="ImportStart.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.HostAdmin.ImportStart" %>
<MP:MasterPage id="MPContainer" runat="server" masterpagefile="~/HostAdmin/PageTemplate.ascx">
	<MP:ContentRegion id="MPTitle" runat="server">Subtext - Host Admin - Import Wizard</MP:ContentRegion>
	<MP:ContentRegion id="MPSideBar" runat="server">
		<asp:Button id="btnRestartWizard" runat="server" text="Restart Import"></asp:Button>
	</MP:ContentRegion>
	<!-- now, lay out our content for the DefaultContent Region (MPContent) -->
		<P>Welcome to the Subtext Import Wizard!</P>
		<P>Since you’re here, you are most likely interested in importing blog data from 
			another blog engine. Glad to see that you’re giving Subtext a try.
		</P>
		<P>We’ll walk you through the steps to get you up and running in no time. But 
			first, a warning.
		</P>
		<DIV><STRONG>Warning!</STRONG> Importing blog data from another blog engine will 
			overwrite any data you might currently have within the blog. Please backup your 
			database before continuing.
		</DIV>
		<P>Ok, now that we have that out of the way, click “Next” to select the blog engine 
			you wish to import from.
		</P>
		<P>
			<asp:Button id="btnNext" runat="server" text="Next - Select an Import Provider"></asp:Button>
		</P>
	<!-- END DefaultContent -->
</MP:MasterPage>
