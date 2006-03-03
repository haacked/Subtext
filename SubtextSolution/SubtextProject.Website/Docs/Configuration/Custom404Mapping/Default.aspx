<%@ Page %>
<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<%@ Register TagPrefix="MP" TagName="DocLinks" Src="~/Docs/DocLinks.ascx" %>
<MP:MasterPage id="MPContainer" runat="server">
	<MP:ContentRegion id="MPTitle" runat="server">Subtext - Docs - Configuration</MP:ContentRegion>
	<MP:ContentRegion id="MPSideBar" runat="server">
		<MP:DocLinks id="AboutLinks" runat="server" />
	</MP:ContentRegion>
	
	<h2>Configuring Custom 404 page</h2>
	<p>[<a href="../../Configuration/" title="Configuration">Back to Configuration</a>]</p>
	<h3>Intro</h3>
	<p>
		Unfortunately, the Subtext installation process cannot automatically 
		configure a custom 404 (file not found) page for you.  But this 
		guide will walk you through this.
	</p>
	<p>
		The first step is to open up the IIS properties dialog for your 
		site and click on the <strong>Custom Errors</strong> tab.  Sites 
		at hosting providers such as WebHost4Life will have some web-based 
		interface for setting this.
	</p>
	<div class="dropshadow"><div class="innerbox"><img src="~/images/IISCustomErrorsDialog.png" alt="Screen showing the Custom Errors tab of the IIS property dialog" runat="server" ID="Img1" width="472" height="464" /></div></div>
	<div class="caption" style="clear: both;"><strong>Figure 1:</strong> Setting a Custom 404 Page.</div>
	<p>
		Scroll down till you see 404 in the <strong>HTTP Error</strong> column 
		and double click that item.  Select <strong>URL</strong> for the <strong>Message Type</strong> field 
		and enter <strong>/SystemMessages/FileNotFound.aspx</strong> for the <strong>URL</strong> field.
	</p>
	<div class="dropshadow"><div class="innerbox"><img src="~/images/ErrorMappingPropertiesDialog.png" alt="Screen showing the Custom Errors tab of the IIS property dialog" runat="server" ID="Img2" width="392" height="211" /></div></div>
	<div class="caption" style="clear: both;"><strong>Figure 1:</strong> Setting a Custom 404 Page.</div>
	<p>
		The URL to enter may differ depending on your IIS setup.  For example, 
		on my dev server Subtext is running within a virtual application named 
		<strong>Subtext.Web</strong> so the URL is <strong>/Subtext.Web/SystemMessages/FileNotFound.aspx</strong>.
	</p>
	<p>
		Click <strong>OK</strong> and you are done.
	</p>
</MP:MasterPage>
