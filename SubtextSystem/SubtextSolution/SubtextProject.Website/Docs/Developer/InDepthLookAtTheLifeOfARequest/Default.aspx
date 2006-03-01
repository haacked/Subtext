<%@ Page %>
<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<%@ Register TagPrefix="MP" TagName="DocLinks" Src="~/Docs/DocLinks.ascx" %>
<MP:MasterPage id="MPContainer" runat="server">
	<MP:ContentRegion id="MPTitle" runat="server">Subtext - Docs - Developer</MP:ContentRegion>
	<MP:ContentRegion id="MPSideBar" runat="server">
		<MP:DocLinks id="AboutLinks" runat="server" />
	</MP:ContentRegion>
	
	<h2>In Depth Look At The Life Of A Request In Subtext</h2>
	[<a href="../../Developer/" title="Dev Docs">Back To Developer Docs</a>]
	<h3>Contents</h3>
	<ul>
		<li><a href="#intro" title="">Intro</a></li>
		<li><a href="#firstStop" title="">First Stop, Application_BeginRequest</a></li>
		<li><a href="#webconfig" title="">Next Stop, Web.config Http Handlers</a></li>
		<li><a href="#urlReWriteHandlerFactory" title="">All Aboard The UrlReWriteHandlerFactory</a></li>
		<li><a href="#dtpAhoy" title="">DTP.aspx Ahoy!</a></li>
		<li><a href="#whoIsTheMaster" title="">Who Is The Master Page!?</a></li>
		<li><a href="#whoAmI" title="">Who Am I?</a></li>
		<li><a href="#slapMeSomeSkin" title="">Slap Me Some Skin</a></li>
		<li><a href="#dtpAndSkinControls" title="">DTP.aspx and SKin Controls</a></li>
		<li><a href="#moreOnSkins" title="">More On Skins</a></li>
	</ul>
	<h3 id="intro">Introduction</h3>
	<p>
		In the entry entitled 
		<a href="../HowAnUrlIsMappedToABlog/" title="Mapping an Url to Blog">How an URL Is Mapped To A Blog</a> 
		we covered at a high level how urls are mapped to blogs.  In this post we 
		get into the nitty gritty and look at how Subtext handles requests at the 
		code level.  Much of this is still a legacy implementation from the code&#8217;s 
		.TEXT days.
	</p>
	<p>
		As an example, we&#8217;ll trace the code that executes to service a 
		request for the following URL:
	</p>
	<code><a href="http://haacked.com/archive/2006/01/25/ISwear.aspx" title="Frustations" rel="external">http://<span style="color: rgb(153, 0, 0);">haacked.com</span>/archive/2006/01/25/ISwear.aspx</a></code>
	<p>
		This request will end up serving up the page for a single blog post.
	</p>
	<h3 id="firstStop">First Stop, Application_BeginRequest</h3>
	<p>
		The first stop on our tour is the <code>Application_BeginRequest</code> method 
		within Global.asax.cs.
	</p>
	<p>
		This method attempts to set the current culture based on the languages 
		specified by the user&#8217;s browser within the request headers.
	</p>
	<p>
		Afterwards, it checks the state of the Subtext installation.  If the 
		installation is not complete, it will redirect to the install directory.  
		Othwerwise execution continues normally.
	</p>
	<h3 id="webconfig">Next Stop, Web.config Http Handlers</h3>
	<p>
		Next, the ASP.NET runtime takes a peek at the web.config to figure out 
		which handler is responsible for handling the request.  The list of 
		handlers is configured within the <span class="kwrd">&lt;</span><span class="html">httpHandlers</span><span class="kwrd">&gt;</span> 
		section of the <span class="kwrd">&lt;</span><span class="html">system.web</span><span class="kwrd">&gt;</span> section.
	</p>
	<div class="dropshadow"><div class="innerbox">

	<!-- code formatted by http://manoli.net/csharpformat/ -->
<pre class="csharpcode" style="padding-right: 15px;"><span class="kwrd">&lt;</span><span class="html">add</span> <span class="attr">verb</span><span class="kwrd">="*"</span> <span class="attr">path</span><span class="kwrd">="Install/*.aspx"</span> 
	<span class="attr">type</span><span class="kwrd">="System.Web.UI.PageHandlerFactory"</span> <span class="kwrd">/&gt;</span>
<span class="kwrd">&lt;</span><span class="html">add</span> <span class="attr">verb</span><span class="kwrd">="*"</span> <span class="attr">path</span><span class="kwrd">="SystemMessages/*.aspx"</span> 
	<span class="attr">type</span><span class="kwrd">="System.Web.UI.PageHandlerFactory"</span> <span class="kwrd">/&gt;</span>
<span class="kwrd">&lt;</span><span class="html">add</span> <span class="attr">verb</span><span class="kwrd">="*"</span> <span class="attr">path</span><span class="kwrd">="HostAdmin/*.aspx"</span> 
	<span class="attr">type</span><span class="kwrd">="System.Web.UI.PageHandlerFactory"</span> <span class="kwrd">/&gt;</span>
<span class="kwrd">&lt;</span><span class="html">add</span> <span class="attr">verb</span><span class="kwrd">="*"</span> <span class="attr">path</span><span class="kwrd">="HostAdmin/*/*.aspx"</span> 
	<span class="attr">type</span><span class="kwrd">="System.Web.UI.PageHandlerFactory"</span> <span class="kwrd">/&gt;</span>
<span class="kwrd">&lt;</span><span class="html">add</span> <span class="attr">verb</span><span class="kwrd">="*"</span> <span class="attr">path</span><span class="kwrd">="Logout.aspx"</span> 
	<span class="attr">type</span><span class="kwrd">="System.Web.UI.PageHandlerFactory"</span> <span class="kwrd">/&gt;</span>
<span class="kwrd">&lt;</span><span class="html">add</span> <span class="attr">verb</span><span class="kwrd">="*"</span> <span class="attr">path</span><span class="kwrd">="BlogMLExport.ashx"</span> 
	<span class="attr">type</span><span class="kwrd">="Subtext.Web.Admin.Handlers.BlogMLExport, Subtext.Web"</span> <span class="kwrd">/&gt;</span>
	    
<span class="rem">&lt;!-- This will process any ext mapped to aspnet_isapi.dll --&gt;</span>
<span class="kwrd">&lt;</span><span class="html">add</span> <span class="attr">verb</span><span class="kwrd">="*"</span> <span class="attr">path</span><span class="kwrd">="*"</span> 
  <span class="attr">type</span><span class="kwrd">="Subtext.Common.UrlManager.UrlReWriteHandlerFactory, Subtext.Common"</span> <span class="kwrd">/&gt;</span></pre>

	</div></div>
	<p class="clear">
		The first five entries in this list are handled by the <code>PageHandlerFactory</code>. 
		That is the default ASP.NET handler for an *.aspx page, so they execute like any normal 
		request for a *.aspx page would.  These correspond to system pages and subdirectories 
		that Subtext does not perform URL Rewriting on.
	</p>
	<p>
		The very last request maps all other requests to the <code>UrlReWriteHandlerFactory</code>. 
		Note, by &#8220;all other requests&#8221; we mean all other requests with a file extension 
		that IIS normally hands off to the ASP.NET runtime.
	</p>
	<p>
		Since the request we are following does not match any of the first five handlers, it is 
		handled by the <code>UrlReWriteHandlerFactory</code>.
	</p>
	<h3 id="urlReWriteHandlerFactory">All Aboard The UrlReWriteHandlerFactory</h3>
	<p>
		The <code>UrlReWriteHandlerFactory</code> is somewhat misnamed as it doesn&#8217;t 
		actually rewrite URLs.  Instead, it is used to map URLs to controls.
	</p>
	<p>
		This handler has its own configuration section within <code>web.config</code>. 
		It is defined within the <span class="kwrd">&lt;</span><span class="html">HandlerConfiguration</span><span class="kwrd">&gt;</span> 
		section.  It has a single <span class="kwrd">&lt;</span><span class="html">HttpHandlers</span><span class="kwrd">&gt;</span> node 
		which itself has multiple <span class="kwrd">&lt;</span><span class="html">HttpHandler</span><span class="kwrd">&gt;</span> nodes.
	</p>
	<p>
		The code below is a small snippet from the HttpHandlers section.
	</p>
	
	<!-- code formatted by http://manoli.net/csharpformat/ -->
<!-- code formatted by http://manoli.net/csharpformat/ -->
<div class="dropshadow"><div class="innerbox">
<pre class="csharpcode">
<span class="kwrd">&lt;</span><span class="html">HttpHandler</span> <span class="attr">pattern</span><span class="kwrd">="(?:/archive/\d{4}/\d{2}/\d{2}/\d+\.aspx)$"</span> 
    <span class="attr">controls</span><span class="kwrd">="viewpost.ascx,Comments.ascx,PostComment.ascx"</span> <span class="kwrd">/&gt;</span>
<strong><span class="kwrd">&lt;</span><span class="html">HttpHandler</span> <span class="attr">pattern</span><span class="kwrd">="(?:/archive/\d{4}/\d{2}/\d{2}/[-_,+\.\w]+\.aspx)$"</span> 
    <span class="attr">controls</span><span class="kwrd">="viewpost.ascx,Comments.ascx,PostComment.ascx"</span> <span class="kwrd">/&gt;</span></strong>
<span class="kwrd">&lt;</span><span class="html">HttpHandler</span> <span class="attr">pattern</span><span class="kwrd">="(?:/archive/\d{4}/\d{1,2}/\d{1,2}\.aspx)$"</span> 
    <span class="attr">controls</span><span class="kwrd">="ArchiveDay.ascx"</span> <span class="kwrd">/&gt;</span></pre>
</div></div>
	<p class="clear">
		Each <span class="kwrd">&lt;</span><span class="html">HttpHandler</span><span class="kwrd">&gt;</span> 
		has a <em>pattern</em> property which is in the form of a regular expression.  If the incoming request 
		matches the pattern, then that handler handles the request.
	</p>
	<p>
		This pattern matching occurs within the <code>GetHandler</code> method of the <code>UrlReWriteHandlerFactory</code> class.  
		Once a matching handler is found, its <code>handlerType</code> property is looked at. If it is not specified in the 
		config settings, it has a value of &#8220;Page&#8221; by default.
	</p>
	<p>
		In our case, the request we are following matches the pattern in the second handler above (in bold). 
		That handler has a <code>handlerType</code> of &#8220;Page&#8221;, which causes the the factory to 
		call the <code>ProcessHandlerTypePage</code> method.
	</p>
	<p>
		<code>ProcessHandlerTypePage</code> then adds the list of controls configured in the handler&#8217;s 
		<code>controls</code> property and calls <code>System.Web.UI.PageParser.GetCompiledPageInstance</code>. 
		This returns a compiled instance of &#8220;DTP.aspx&#8221;.  Remember, all this code is executing within 
		a factory which has the single goal of returning an <code>IHttpHandler</code> to the ASP.NET runtime.
	</p>
	<p>
		Scott Watermasysk, the original creator of .TEXT wrote up a short description of this 
		<a href="http://scottwater.com/blog/articles/UrlRewrite1.aspx" title=".TEXT UrlReWriting" rel="external">technique of URL Rewriting here</a>.
	</p>
	<h3 id="dtpAhoy">DTP.aspx Ahoy!</h3>
	<p>
		Our journey is by no means over yet, mate.  DTP.aspx is the base skeleton template for pretty 
		much every page that is handled by the <code>UrlReWriteHandlerFactory</code>.
	</p>
	<p>
		It has several placeholder server controls for such things as link tags to stylesheets, auto discoverable 
		RSS tag, javascripts, and more.
	</p>
	<p>
		It also has a <code>MasterPage</code> control declared with a <code>ContentRegion</code>.  
		These act very similar to the corresponding controls in ASP.NET 2.0.
	</p>
	<h3 id="whoIsTheMaster">Who Is The Master Page!?</h3>
	<p>
		The next stop in our travels takes us to the <code>Subtext.Web.UI.WebControls.MasterPage</code> 
		control declared on DTP.aspx.  This control is an adaptation of 
		<a href="http://authors.aspalliance.com/paulwilson/Articles/?id=14" title="Master Page" rel="external">Paul Wilson&#8217;s Master Page</a> 
		control.
	</p>
	<p>
		In the <code>OnInit</code> method, the MasterPage control attempts to load in a template 
		control.  This control defines the template for the current skin. However, at this point, 
		we have to take a diversion as Subtext does not yet know which blog we need to load the 
		skin for.  When attempting to look up the current skin, Subtext calls the property 
		<code>Config.CurrentBlog</code>
	</p>
	<h3 id="whoAmI">Who Am I?</h3>
	<p>
		<code>Config.CurrentBlog</code> calls the <code>GetBlogInfo</code> method on the specified 
		Configuration provider (there is only one, the <code>Subtext.Framework.Configuration.UrlBasedBlogInfoProvider</code>).  
	</p>
	<p>
		The <code>UrlBasedBlogInfoProvider</code> is responsible for mapping URLs to Blogs 
		as described in <a href="../HowAnUrlIsMappedToABlog/" title="How An Url Is Mapped To A Blog">How An Url Is Mapped To A Blog</a>. 
		This all happens in the <code>GetBlogInfo</code> method.
	</p>
	<h3 id="slapMeSomeSkin">Slap Me Some Skin</h3>
	<p>
		Now that we know which blog the request is for, we can get the skin name of 
		the current blog from the <code>BlogInfo.Skin.SkinName</code> property.  The 
		SkinName property corresponds to the subdirectory of the &#8220;Skins&#8221; 
		directory in which files for the current skin are located.
	</p>
	<p>
		The TemplateFile referred to earlier is a file named &#8220;PageTemplate.ascx&#8221; found 
		in the skin directory.
	</p>
	<p>
		The master page then loads this template control, and removes the template subcontrols, 
		adds those controls to itself, and finally adds the template to its own controls collection 
		at position 0.
	</p>
	<p>
		The next step is to move these controls into the ContentRegion named MPMain.
	</p>
	<h3 id="dtpAndSkinControls">DTP.aspx and SKin Controls</h3>
	<p>
		Ok, finally we are back at the codebehind for the DTP.aspx page.  The code behind 
		class is <code>Subtext.Web.UI.Pages.SubtextMasterPage</code>.  
	</p>
	<p>
		The initialization method for this page looks at the Skin configuration file 
		(/Admin/Skins.config) to find out which javascript and css files need to be 
		referenced.  It places references to these files in the &lt;head&gt; section 
		of the page.
	</p>
	<p>
		The method then loads each control specified by the HttpHandler (remember that guy?) 
		into the center body control.  In this example, the page should load in the following 
		controls:
	</p>
	<ul>
		<li>viewpost.ascx</li>
		<li>Comments.ascx</li>
		<li>PostComment.ascx</li>
	</ul>
	<p>
		These are skin controls that are loaded from the current Blog&#8217;s 
		skin directory.
	</p>
	<h3 id="moreOnSkins">More On Skins</h3>
	<p>
		Skin controls in Subtext are simply user controls.  They do not have a code-behind 
		file but instead inherit from classes defined in the &#8220;Subtext.Web/UI/Controls&#8221; 
		directory.
	</p>
	<p>
		Each skin directory is responsible for containing the &#8220;Code In Front&#8221; 
		ascx file for each of these controls it makes use of.
	</p>
		For example, the Redbook skin directory and the Piyo skin directory each will 
		contain a &#8220;ViewPost.ascx&#8221; file.  The respective files may differ 
		in layout, but they will have the same Inherits declaration at top like so:
	</p>
	
<pre class="csharpcode">
<span class="asp">&lt;%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.ViewPost" %&gt;</span></pre>

	<p>
		So when DTP.aspx is loading in these skin controls from the proper skin 
		directory, each control will execute its <code>OnInit</code> method as 
		well as its <code>OnLoad</code> at the right time.
	</p>	
</MP:MasterPage>