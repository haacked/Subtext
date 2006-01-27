<%@ Page %>
<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<%@ Register TagPrefix="MP" TagName="AboutLinks" Src="~/About/AboutLinks.ascx" %>
<MP:MasterPage id="MPContainer" runat="server">
	<MP:ContentRegion id="MPTitle" runat="server">Subtext - About</MP:ContentRegion>
	<MP:ContentRegion id="MPSideBar" runat="server">
		<MP:AboutLinks id="AboutLinks" runat="server" />
	</MP:ContentRegion>
	<h2>Developers</h2>
	<p>
		Developers are those with who have committed to 
		improving Subtext and have Write access to the CVS 
		repository.
	</p>
	<ul>
		<li><a href="http://haacked.com/" title="Phil&#8217;s Blog">Phil Haack</a> &#8212; Benevolent Dictator</li>
		<li><a href="http://idunno.org/">Barry Dorrans</a> &#8212; Head of Plugs and Ins</li>
		<li><a href="http://glazkov.com/blog/">Dimitri Glazkov</a> &#8212; Chief Semantics Officer</li>
		<li><a href="http://jasonkemp.ca/">Jason Kemp</a> &#8212; Installation and Facilities Management</li>
		<li><a href="http://jaysonknight.com/blog/default.aspx">Jayson Knight</a> &#8212; Schema Worker</li>
		<li><a href="http://sharpmarbles.stufftoread.com/">Robb Allen</a> &#8212; Security Guard and Master Brewer</li>
		<li><a href="http://stevenharman.net">Steve Harman</a> &#8212; That Other Guy</li>
		<li><a href="http://blogs.ugidotnet.org/piyo/">Simone Chiaretta</a> &#8212; Web2.0 Evangelist and Skinner</li>
		<li><a href="http://blogs.analystdeveloper.com/gurkaneng/">Gurkan Yeniceri</a> &#8212; YASD (Yet Another Subtext Developer)</li>
	</ul>
	<h2>Contributors and Testers</h2>
	<ul>
		<li><a href="http://andrewconnell.com/blog/">Andrew Connell</a> &#8212; CMS Guru</li>
		<li><a href="http://vernsblog.thegillfamily.us:8180/">Vern Gill &#8212; Head Bouncer</a></li>
	</ul>
	
	<p>
	To understand <strong>Subtext</strong> is to understand where it came from. 
	Subtext is a fork of the now defunct .TEXT project.  .TEXT is a popular 
	blogging engine written in C# for the ASP.NET platform.  It was started by 
	<a href="http://scottwater.com/blog" alt="Scott&#8217;s Blog">Scott Watermasysk</a> 
	who created a scalable and widely used blogging engine.
	</p>
	<p>
	However, with no new releases of .TEXT pending, we <a href="~/About/ForkingDotText/" runat="server">decided to fork it</a>.  
	</p>
	
	<h2>Want to Join the Team?</h2>
	<p>
		The easiest way to help out is to <a href="~/About/ViewTheCode/" runat="server">
		download the source code</a> and start 
		<a href="http://haacked.com/archive/2005/06/18/5153.aspx">submitting patches</a> 
		via <a href="http://sourceforge.net/projects/subtext/">SourceForge</a>. 
	</p>
	<p>
		We especially can use help with documentation.  In fact, this very site you are 
		reading is in the CVS repository.
	</p>
	<p>
		If we like what you&#8217;re contributions, we&#8217;ll give you write access.
	</p>
</MP:MasterPage>
