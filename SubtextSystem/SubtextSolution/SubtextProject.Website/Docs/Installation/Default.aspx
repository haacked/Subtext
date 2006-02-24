<%@ Page %>
<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<%@ Register TagPrefix="MP" TagName="DocLinks" Src="~/Docs/DocLinks.ascx" %>
<MP:MasterPage id="MPContainer" runat="server">
	<MP:ContentRegion id="MPTitle" runat="server">Subtext - Docs - Installation</MP:ContentRegion>
	<MP:ContentRegion id="MPSideBar" runat="server">
		<MP:DocLinks id="AboutLinks" runat="server" />
	</MP:ContentRegion>
	
	  <script language='javascript'>
    function popup_window( url, id, width, height )
    {
       //extract the url parameters if any, and pass them to the called html
       var tempvar=document.location.toString(); // fetch the URL string
       var passedparams = tempvar.lastIndexOf("?");
       if(passedparams > -1)
          url += tempvar.substring(passedparams);
      popup = window.open( url, id, 'toolbar=no,scrollbars=no,location=no,statusbar=no,menubar=no,resizable=no,width=' + width + ',height=' + height + '' );
      popup.focus();
    }
  </script>
	
	<h2>Installation</h2>
	
	<!-- I would remove this paragraph from here 
	
	<h3>Developers</h3>
	<p>
		If you just want to download and play with the latest code on your local machine, follow the 
		<a href="~/About/ViewTheCode/" id="instructionsLink" runat="server">instructions here</a>.
	</p>
	
	 
	 
	<h3>Users</h3>
	
	to here   -->
	
	<h3>Installation step by step guide</h3>
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

	<h3>Installation Screencasts</h3>
	<p>
		In order to simplify the installation of SubText in an hosting enviroment we produced 2 screencasts with the complete installation procedure (available both in broadband and low quality):
		<ul>
			<li>
				Complete installation: takes you through all the steps from the download of the packege, to the installation, till the creation of your first blog post.
				<ul>
					<li><a href="javascript:popup_window( 'screencasts/low/installation_slideshow_viewlet_swf.html', 'installation_slideshow', 876, 716 );">SubText Installation Procedure - low quality (1.3 Mb)</a></li>
					<li><a href="javascript:popup_window( 'screencasts/hi/installation_slideshow_viewlet_swf.html', 'installation_slideshow', 876, 716 );">SubText Installation Procedure - hi quality (2.9 Mb)</a></li>
				</ul>
			</li>
			<li>
				Migration from .TEXT 0.95: an add-on of the previous screencast, this one explain how to import the posts you have on your previous .TEXT blog.
				<ul>
					<li><a href="javascript:popup_window( 'screencasts/low/import_slideshow_viewlet_swf.html', 'import_slideshow', 876, 716 );">.TEXT to SubText Migration Procedure - low quality (400 Kb)</a></li>
					<li><a href="javascript:popup_window( 'screencasts/hi/import_slideshow_viewlet_swf.html', 'import_slideshow', 876, 716 );">.TEXT to SubText Migration Procedure - hi quality (500 Kb)</a></li>
				</ul>
			</li>
		</ul>
	</p>

</MP:MasterPage>
