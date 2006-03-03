<%@ Page %>
<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<%@ Register TagPrefix="MP" TagName="DocLinks" Src="~/Docs/DocLinks.ascx" %>
<MP:MasterPage id="MPContainer" runat="server">
	<MP:ContentRegion id="MPTitle" runat="server">Subtext - Docs - Configuration</MP:ContentRegion>
	<MP:ContentRegion id="MPSideBar" runat="server">
		<MP:DocLinks id="AboutLinks" runat="server" />
	</MP:ContentRegion>
	
	<h2>Configuring Aggregate Blogs</h2>
	<p>[<a href="../../Configuration/" title="Configuration">Back to Configuration</a>]</p>
	<h3>Intro</h3>
	<p>
		Subtext supports having an aggregate blog for all blogs on a system.  
		For example, suppose you have the following blogs installed:
	</p>
	<ul>
		<li>http://yourdomain.com/blog1/</li>
		<li>http://yourdomain.com/blog2/</li>
		<li>http://yourdomain.com/blog3/</li>
	</ul>
	<p>
		It is possible to have the url <strong>http://yourdomain.com/</strong> 
		contain an aggregate of blog entries from the three blogs.
	</p>
	<p>
		To configure this, you will have to edit the web.config by 
		hand (this will be taken care of in a future version of Subtext).
	</p>
	<p>
		Within the <code>appSettings</code> section, you should see the 
		following settings.
	</p>
	<!-- code formatted by http://manoli.net/csharpformat/ -->
<div class="dropshadow"><div class="innerbox"><pre class="csharpcode">
<span class="kwrd">&lt;</span><span class="html">add</span> <span class="attr">key</span><span class="kwrd">="AggregateEnabled"</span> <span class="attr">value</span><span class="kwrd">="<strong>false</strong>"</span> <span class="kwrd">/&gt;</span>
<span class="kwrd">&lt;</span><span class="html">add</span> <span class="attr">key</span><span class="kwrd">="AggregateTitle"</span> <span class="attr">value</span><span class="kwrd">="A Subtext Community"</span> <span class="kwrd">/&gt;</span>
<span class="kwrd">&lt;</span><span class="html">add</span> <span class="attr">key</span><span class="kwrd">="AggregateUrl"</span> <span class="attr">value</span><span class="kwrd">="http://localhost/Subtext.Web"</span> <span class="kwrd">/&gt;</span>
<span class="kwrd">&lt;</span><span class="html">add</span> <span class="attr">key</span><span class="kwrd">="AggregateDescription"</span> <span class="attr">value</span><span class="kwrd">=".NET by Subtext"</span> <span class="kwrd">/&gt;</span>
<span class="kwrd">&lt;</span><span class="html">add</span> <span class="attr">key</span><span class="kwrd">="AggregateHost"</span> <span class="attr">value</span><span class="kwrd">="localhost"</span> <span class="kwrd">/&gt;</span>
</pre></div></div>
	<p class="clear">
		To turn on the aggregate blog, simply set the <code>AggregateEnabled</code> 
		value to true and modify the <code>AggregateUrl</code> and <code>AggregateHost</code> 
		values according to your domain.
	</p>
</MP:MasterPage>
