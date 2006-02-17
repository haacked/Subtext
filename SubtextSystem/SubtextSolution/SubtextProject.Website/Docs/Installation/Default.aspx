<%@ Page %>
<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<%@ Register TagPrefix="MP" TagName="DocLinks" Src="~/Docs/DocLinks.ascx" %>
<MP:MasterPage id="MPContainer" runat="server">
	<MP:ContentRegion id="MPTitle" runat="server">Subtext - Docs - Installation</MP:ContentRegion>
	<MP:ContentRegion id="MPSideBar" runat="server">
		<MP:DocLinks id="AboutLinks" runat="server" />
	</MP:ContentRegion>
	
	<h2>Installation</h2>
	<h3>Developers</h3>
	<p>
		If you just want to download and play with the latest code on your local machine, follow the 
		<a href="~/About/ViewTheCode/" id="instructionsLink" runat="server">instructions here</a>.
	</p>
	<h3>Users</h3>
	<p>
		To install a Subtext release:
	</p>
	<ol>
		<li>
			Download the latest release from 
			<a href="http://sourceforge.net/projects/subtext" title="Subtext Project Site on SourceForge" rel="external">SourceForge</a>. 
		</li>
		<li>
			Unzip the package into a directory on your local machine.
		</li>
		<li>
			Make sure you have a SQL Server 2000 database setup.  Make sure the database user has db owner permissions 
			(this is necessary for the install process. You can downgrade permissions afterwards).
		</li>
		<li>
			Set up an IIS application, unless your hosting provider already has one set up for you.
		</li>
		<li>
			Open the web.config file and update the <strong>ConnectionString</strong> 
			<code>AppSetting</code> value to match your database.
		</li>
		<li>
			Copy the installation files over to your web root.
		</li>
		<li>
			Navigate to your blog via the browser and follow the installation steps.
		</li>
	</ol>
</MP:MasterPage>
