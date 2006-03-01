<%@ Page %>
<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<%@ Register TagPrefix="MP" TagName="DocLinks" Src="~/Docs/DocLinks.ascx" %>
<MP:MasterPage id="MPContainer" runat="server">
	<MP:ContentRegion id="MPTitle" runat="server">Subtext - Docs - Configuration</MP:ContentRegion>
	<MP:ContentRegion id="MPSideBar" runat="server">
		<MP:DocLinks id="AboutLinks" runat="server" />
	</MP:ContentRegion>
	
	<h2>Configuration</h2>
	<p>
		One goal for Subtext is to make configuration as easy 
		and as self explanatory as possible.  However, this page 
		is for the configuration information that doesn&#8217;t 
		explain itself.
	</p>
	<ul>
		<li><em>For a deeper understanding of how URLs are mapped to 
		blogs, please <a href="~/Docs/Developer/HowAnUrlIsMappedToABlog" title="Mapping URLs to Blogs" runat="server" ID="A1">read this</a>.</em></li>
		<li><em>For a list of various configurations please see <a href="ConfigurationTable/" title="Configuration Chart">this chart</a>.</em></li>	
		<li>For Configuring Aggregate Blogs, See <a href="AggregateBlogs/" title="Configuring Aggregate Blogs">this short guide</a>.</li>
	</ul>
	<h3>Single Blog Configuration</h3>
	<p>
		Single blog configuration is pretty straightforward. 
		If you run through the installation process and select 
		&#8220;Quick Create&#8221; your blog should be all set 
		up properly.
	</p>
	<p>
		The installation process ends in the Host Admin tool.  This is 
		located in the <code>/HostAdmin</code> directory of a Subtext installation.
	</p>
	<p>
		The screenshot below shows what the host admin might look 
		like after an installation.
	</p>
	
	<div class="dropshadow"><div class="innerbox"><img src="~/images/HostAdminSingleBlog.png" alt="Screen showing Host Admin list of blogs" runat="server" /></div></div>
	<div class="caption" style="clear: both;"><strong>Figure 1:</strong> Host Admin with single blog.</div>

	<p>
		This shows a single blog with the title of &#8220;Haack Attaack&#8221; 
		at the domain &#8220;localhost&#8221; in the Subfolder &#8220;Blog&#8221;. 
	</p>
	<p>
		Clicking on <em>Edit</em> allows the host admin to edit blog settings.
	</p>
	<div class="dropshadow"><div class="innerbox"><img src="~/images/HostAdminEditBlog.Png" alt="Screen showing the editing of a blog in the Host Admin" runat="server" ID="Img1"/></div></div>
	<div class="caption" style="clear: both;"><strong>Figure 2:</strong> Host Admin editing a blog's setting.</div>
	<p>
		This screen allows the admin to change the URL at which the blog is 
		accessed.
	</p>
	<p>
		To have the blog in the web root, simply leave the &#8220;Subfolder&#8221; 
		field blank. Note that if a Subfolder is specified, the url to the blog 
		must end in &#8220;Default.aspx&#8221; unless you take the extra step 
		to <a href="Custom404Mapping/" title="configuring a custom 404 page">configure a custom 404 page</a> 
		to point to the file <code>/SystemMessages/FileNotFound.aspx</code>.
	</p>
	<h3>Multiple Blog Configuration</h3>
	<p>
		If you plan on hosting multiple blogs within a single 
		installation of Subtext, then configuration gets slightly 
		more complicated.
	</p>
	<p>
		This topic is covered more <a href="~/Docs/Developer/HowAnUrlIsMappedToABlog" title="Mapping URLs to Blogs" runat="server">in-depth here</a>.
	</p>
	
</MP:MasterPage>
