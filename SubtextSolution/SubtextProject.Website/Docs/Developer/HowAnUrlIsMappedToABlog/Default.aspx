<%@ Page %>
<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<%@ Register TagPrefix="MP" TagName="DocLinks" Src="~/Docs/DocLinks.ascx" %>
<MP:MasterPage id="MPContainer" runat="server">
	<MP:ContentRegion id="MPTitle" runat="server">Subtext - Docs - Developer</MP:ContentRegion>
	<MP:ContentRegion id="MPSideBar" runat="server">
		<MP:DocLinks id="AboutLinks" runat="server" />
	</MP:ContentRegion>
	
	<h2>How URLs Are Mapped To A Blog</h2>
	[<a href="../../Developer/" title="Dev Docs">Back To Developer Docs</a>]
	<h3>Introduction</h3>
	<p>
		The manner in which Subtext maps a request to the final 
		rendered output is worth describing in detail.  Because 
		Subtext supports multiple blogs in a single installation, 
		it must map the incoming request URL to a specific blog 
		instance.  This document describes how this is done.
	</p>
	<h3>Anatomy of a Blog URL</h3>
	<p>
		If you&#8217;ve visited the host admin and edited a blog, 
		you&#8217;ll notice that the URL to the blog is represented 
		like so:
	</p>
	<div class="dropshadow"><div class="innerbox" style="padding: 0 30px 0 5px;">
	<code>http://<span style="color: rgb(153, 0, 0);">localhost</span>/Subtext.Web/<span style="color: rgb(0, 0, 153);">blog</span>/default.aspx</code>
	</div></div>
	<p style="clear:both;">
		This is the example of an URL when Subtext is configured to 
		run in a Virtual Application in <acronym title="Internet Information Services">IIS</acronym>.
	</p>
	<p>In this case, </p>
	<ul>
		<li>the Host Domain is &#8220;<span style="color: rgb(153, 0, 0);">localhost</span>&#8221;</li>
		<li>the Virtual Application is named &#8220;Subtext.Web&#8221;</li> 
		<li>and the Subfolder is named &#8220;<span style="color: rgb(0, 0, 153);">blog</span>&#8221;</li>
	</ul>
	</p>
	<p>
		If IIS is configured to run Subtext in a webroot (most likely 
		matching your production setup), then the URL would look more like:
	</p>
	<div class="dropshadow"><div class="innerbox" style="padding: 0 30px 0 5px;">
	<code>http://<span style="color: rgb(153, 0, 0);">example.com</span>/<span style="color: rgb(0, 0, 153);">blog</span>/default.aspx</code>
	</div></div>
	<p style="clear:both;">In which case, </p>
	<ul>
		<li>the Host Domain is &#8220;<span style="color: rgb(153, 0, 0);">localhost</span>&#8221;</li>
		<li>there is no Virtual Application value</li> 
		<li>and the Subfolder is named &#8220;<span style="color: rgb(0, 0, 153);">blog</span>&#8221;</li>
	</ul>
	<p>
		In both the above examples, the blog is configured with a subfolder 
		named &#8220;blog&#8221;, which is one common way many configure their 
		blogs.
	</p>
	<p>
		However, a blog can decide to be in the host domain root rather than 
		a subfolder as in this URL.
	</p>
	<div class="dropshadow"><div class="innerbox" style="padding: 0 30px 0 5px;">
	<code>http://<span style="color: rgb(153, 0, 0);">example.com</span>/default.aspx</code>
	</div></div>
	<p style="clear:both;">In which case, </p>
	<ul>
		<li>the Host Domain is &#8220;<span style="color: rgb(153, 0, 0);">localhost</span>&#8221;</li>
		<li>there is no Virtual Application value</li> 
		<li>and the Subfolder is blank</li>
	</ul>
	<p>
		In the Host Admin tool, this is configured by editing the blog 
		and setting the Subfolder field as in the image below:
	</p>
	<div class="dropshadow"><div class="innerbox"><img src="~/images/HostAdminEditBlog.Png" alt="Screen showing the editing of a blog in the Host Admin" runat="server" ID="Img1"/></div></div>
	<div class="caption" style="clear: both;"><strong>Figure 1:</strong> Host Admin editing a blog's setting.</div>
	<h3>Incoming Request Parsing</h3>
	<p>
		When an incoming request comes in, Subtext first parses the host 
		and looks up blogs (in table subtext_Config) that have a matching 
		host value.
	</p>
	<p>
		The query below is a simplified version of the one Subtext runs. 
		(NOTE: The <em>Application</em> database column corresponds to the Subfolder 
		in the Host Admin.  We did not change the column name yet.)
	</p>
	
	<div class="dropshadow"><div class="innerbox" style="font-family: Courier New; font-size: 10pt; color: black; background: white; padding: 0 30px; 0; 5px;">
	<p style="margin: 0px;"><span style="color: blue;">SELECT </span>BlogId, Host, Application</p>
	<p style="margin: 0px;"><span style="color: blue;">FROM </span>subtext_Config</p>
	</div></div>
	
	<p style="clear:both;">
		If only one blog is found, it will assume that the record corresponds 
		to the incoming request.
	</p>
	<h3>Multiple blogs</h3>
	<p>
		If multiple blogs match, then Subtext has a little bit more work 
		to do. The results of such a request might look like so:
	</p>
	<div class="dropshadow"><div class="innerbox"><img src="~/images/BlogRecords.png" alt="Screen showing blog records" runat="server" /></div></div>
	<div class="caption" style="clear: both;"><strong>Figure 2:</strong> Two blogs with the same host.</div>
	<p>	
		It then tries to parse out the subfolder of the incoming 
		request by looking at the part after the host name.
	</p>
	<h3>Application Path</h3>
	<p>
		In the case of a blog installation within a virtual application, 
		Subtext knows to look at the part of the URL after the application 
		portion.
	</p>
	<p>
		For example, on a local install of Subtext on Windows XP within the Subtext.Web 
		virtual application, Subtext knows to look for the subfolder value after the 
		Application value which is obtained via <code>HttpContext.Current.Request.ApplicationPath</code>.
	</p>
	<p>
		Thus in the above example of an incoming request with the following URL:
	</p>
	<div class="dropshadow"><div class="innerbox" style="padding: 0 30px 0 5px;">
	<code>http://<span style="color: rgb(153, 0, 0);">localhost</span>/Subtext.Web/<span style="color: rgb(0, 0, 153);">blog</span>/default.aspx</code>
	</div></div>
	<p style="clear:both;">
		Subtext is able to figure out that the subfolder name is &#8220;blog&#8221; 
		and is thus a request for the blog with the id of 1 and NOT the blog with 
		an id of 2.
	</p>
	<p>
		Subtext then parses the rest of the URL to figure out what page 
		exactly is being requested.  That&#8217;ll be covered in another 
		topic.
	</p>
	<h3>Potential Blog Conflicts</h3>
	<p>
		Suppose Subtext receives an incoming request like so:
	</p>
	<div class="dropshadow"><div class="innerbox" style="padding: 0 30px 0 5px;">
	<code>http://<span style="color: rgb(153, 0, 0);">example.com</span>/<span style="color: rgb(0, 0, 153);">blog2</span>/default.aspx</code>
	</div></div>
	<p style="clear:both;">
		But Subtext has the following two blogs configured.
	</p>
	<div class="dropshadow"><div class="innerbox"><img src="~/images/BlogRecordsNoApplicationOnOne.Png" alt="Screen showing two blog records, both with the same host name, but one with an empty subfolder name." runat="server" /></div></div>
	<div class="caption" style="clear: both;"><strong>Figure 3:</strong> Two blogs with the same host, but one with no subfolder value.</div>
	<p>
		At first glance, it seems obvious that we would select the blog with 
		the id of 2.  But we cannot be sure this is correct.
	</p>
	<p>
		What if the first blog has a physical subfolder named &#8220;Blog2&#8221;.  
		How can we be sure the request is not for a file in that folder?
	</p>
	<h3>Blogs With The Same Host Name Must Have A Subfolder Value</h3>
	<p>
		At the moment, Subtext does not allow for such a configuration.  
		If two blogs have the same host name, they must have non empty 
		distinct subfolder values.
	</p>
	<h3>Supports Blogs With different Host Names</h3>
	<p>
		Subtext supports any number of blogs with different host names. 
		Two blogs with different host names do not have the restriction 
		that both have non-empty Subfolder values.
	</p>
	<p>
		As an example, here is a list of several blog records within 
		a single installation of Subtext that are all valid.
	</p>
	<div class="dropshadow"><div class="innerbox"><img src="~/images/ListOfLegalBlogs.Png" alt="Screen showing Multiple valid blogs within the same installation of Subtext." runat="server" /></div></div>
	<div class="caption" style="clear: both;"><strong>Figure 4:</strong> Multiple valid blogs within the same installation of Subtext.</div>
	<p style="clear:both;">
		Notice that even though the first two blogs have the same 
		subfolder value (in the application column), that is fine 
		because they have different host names.
	</p>
	<p>
		Likewise, it is fine that the haack.org blog does not have 
		an application value defined because it is the only blog with 
		the host name of &#8220;haack.org&#8221;.
	</p>
</MP:MasterPage>