<%@ Page %>
<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<MP:MasterPage id="MPContainer" runat="server">
	<MP:ContentRegion id="MPTitle" runat="server">Subtext - Roadmap</MP:ContentRegion>
	<MP:ContentRegion id="MPSideBar" runat="server">
		<h2>RoadMap</h2>
	</MP:ContentRegion>

	<h2>Roadmap</h2>
	<p>
	This document describes the goals for future versions of Subtext as well as 
	a plan for achieving them.  The goals for this roadmap are the following:
	</p>
	<ul>
	<li>Communicate to end users what features are planned for future releases</li>
	<li>Elicit feedback from users about upcoming releases</li>
	<li>Provides a prioritization of features</li>
	</ul>
	<p>
		This document is a work in progress and is constantly changing. 
		Feedback is welcome.
	</p>
	<h3>Administrative Road Map</h3>
	<ol>
		<li>Documenting existing source code and features. (priority: high)</li>
		<li>Fill specific project roles (patch manager, forum manager, etc...) (priority: high)</li>
		<li>Set up an automated build process (NAnt) (priority: low)</li>
	</ol>
	<h3 class="version">Version 1.0 - Code named &#8220;Nautilus&#8221;</h3>
	<p>
		<strong>Estimated release - October 1, 2005</strong>.
	</p>
	<p>
		Nautilus is focused on usability, its dashing good looks, 
		and simple deployment.
	</p>
	<p>
		Entries marked with <span class="implemented">*</span> have been 
		implemented in CVS, though they may require testing and fixes.
	</p>
	<ol>
		<li><strong>Web Based Installer</strong> - Supports clean install and future upgrades.</li>
		<li><strong>.TEXT 0.95 Import Wizard</strong></li>
		<li>
			<strong>Table and Stored Procedures Prefix</strong> - Allows users to specify 
			a prefix for all tables and stored procedures within the web.config file.  This is 
			especially useful for those on shared hosting providers.  It prevents overwriting existing 
			tables with the same name.
		</li>
		<li>
			<span class="implemented">*</span><strong>Simplified Configuration</strong> - There is one 
			and only one web.config file.
		</li>
		<li><span class="implemented">*</span><strong>Host Admin Tool</strong> - Create and manage multiple blogs via an easy to use interface.</li>
		<li><strong>Inline Admin Documentation</strong> - This includes tooltips within the admin section.</li>
		<li>
			<span class="implemented">*</span><strong>Edit Link Control</strong>
			 - Skins can provide a direct link to edit an item when an admin is logged in.
		</li>
		<li>
			<span class="implemented">*</span><strong>Multiple Comment Deletion</strong> 
			- UI enhancement to delete multiple comments at a time, rather than one at a time.
		</li>
		<li>
			<span class="implemented">*</span><strong>RSS GZIP Compression</strong> 
			- For RSS Aggregators that support it, feeds can be compressed to save on bandwidth.
		</li>
		<li>
			<span class="implemented">*</span><strong>RFC3229 Delta Encoding</strong> 
			- Saves bandwidth. For more information, read Bob Wyman&#8217;s 
			<a href="http://bobwyman.pubsub.com/main/2004/09/using_rfc3229_w.html">post on the subject</a>.
		</li>
		<li>
			<span class="implemented">*</span><strong>Comments disabled after N days</strong> 
			- You can automatically disable comments on a post after a specified number of days.
		</li>
		<li>
			<span class="implemented">*</span><strong>Comment Throttling</strong> 
			- You can specify a required delay between posts from the same source.  
			This makes Subtext more robust against massive comment spam attacks.
		</li>
		<li>
			<span class="implemented">*</span><strong>Duplicate Comment Blocking</strong> 
			- Comment Spammers often use bots that simply spam the same message over 
			and over again.  You can have subtext block comments that are exact duplicates.
		</li>
		<li>
			<strong>MT BlackList</strong>
		</li>
		<li>
			<strong>Granular Control Over Comments/Trackbacks/Pingbacks</strong> - 
			Users will be able to specify which of these features they want on or off.
		</li>
		<li>
			<strong>Logging Console</strong> - Inspired by DotNetNuke&#8217;s implementation, 
			this allows users to view and clear the log and choose logging levels.
		</li>
		<li>
			<strong>Simple Keyword Comment Filter</strong> - This will be replaced with a 
			more full fledged implementation later.
		</li>
		<li>
			<span class="implemented">*</span><strong>Replace/Upgrade FreeTextBox.dll</strong>
		</li>
		<li><strong>Improved Documentation</strong> - Want all around better documentation.</li>
		<li><strong>Great New Skins</strong></li>
		<li><strong>BUG FIX: MetaBlogApi</strong> - You can edit query and edit old posts 
		in w.bloggar.</li>
	</ol>
	
	<h3>Version 1.1 - Code named &#8220;Daedelus&#8221;</h3>
	<p>
		<strong>Estimated release - March, 2006</strong>.
	</p>
	<p>
		Daedelus takes an inward look and fixes up the plumbing a bit, 
		providing an extensibility (plug-in) model.
	</p>
	<ol>
		<li>
			<strong>ASP.NET 2.0</strong>
		</li>
		<li>
			<strong>Code refactorings</strong> - Focus on simplifying the existing 
			providers (especially the ObjectProvider) so that other providers can 
			easily be built.
		</li>
		<li>
			<strong>Plug-in Framework</strong>
		</li>
		<li>
			<strong>Improved user management</strong> - Membership provider.
		</li>
		<li>
			<strong>Comment Moderation and Whitelisting</strong>
		</li>
		<li>
			<strong>Comment Filters Refactored to Plug-ins</strong>
		</li>
		<li>
			<strong>Skin Gallery</strong>
		</li>
		<li>
			<strong>Skin Upload tool</strong>
		</li>
	</ol>
	
	<h3>Version 1.2 - Code named &#8220;Daedelus&#8221;</h3>
	<ol>
		<li><strong>Improved XHTML (and other standards) Support</strong></li>
		<li><strong>Localization and Internationalization</strong></li>
		<li><strong>Admin UI (look and feel) overhaul</strong></li>
		<li><strong>Improved Skin Architecture</strong></li>
		<li><strong>Simple Comment Filtering Rules Plugin</strong>
			Currently, haacked.com uses a simple trigger that filters out comments 
			with a certain number of links.  This exceedingly simple filter does 
			remarkably well.  To fight comment spam, we should start with a few 
			simple (and configurable) rules for filtering comment spam.  We can 
			add more complex rules later.
		</li>
	</ol>
	
	<h3>Version 2.0 - Code named &#8220;Red October&#8221;</h3>
	<ol>
		<li><strong>Texturize</strong> - Used to convert ASCII into 
		typographically correct XHTML entities.  For example, replacing 
		"Quotes" with nicer &#8220;Quotes&#8221;.
		</li>
		<li>
			<strong>REST Architectural</strong> - There are several places where 
			having a RESTful architecture could benefit Subtext.  We don&#8217;t 
			plan to use REST as a golden hammer, merely use it where it makes sense.
		</li>
		<li>
			<strong>Implement AJAX.NET</strong> - There are several places that 
			an AJAX implementation could make Subtext more usable.
		</li>
		<li>
			<strong>Friendly Url Support</strong>: Currently, Subtext allows the user to 
			specify a friendly URL in the &#8220;Entry Name&#8221; field when 
			creating a post in the admin section.  We want to create a way for 
			users to allow the system to automatically create these friendly 
			URLs.  Also want to have some means for friendly URL creation 
			when posting via the MetaBlogAPI.
		</li>
	</ol>
	
	<h3>Features way down the road</h3>
	<ul>
		<li><strong>Flickr Integration</strong> Not sure yet what this means exactly, but have some ideas in my head.</li>
		<li><strong>A Spell Checker</strong>: For all those bad spelers out there.</li>
		<li><strong>Migration utility for other systems</strong>: We&#8217;re not so arrogant as to 
		believe you&#8217;ll never use another blogging engine again.  
		If you do, we want to help you migrate your permalinks and posts 
		to it.</li>
		<li><strong>MySql Provider</strong>: because not everyone wants to pay for 
		SQL Server hosting and some people want to honor their license 
		agreement for MSDN Universal. ;)
		</li>
		<li><strong>Mono support</strong>: This may be way down the road, but 
		supporting Mono would be a nice way to introduce the Linux 
		crowd to the beauty of ASP.NET and Subtext.  Besides, we&#8217;ll 
		finally get props from the Slashdot crowd for our 
		<a href="http://www.urbandictionary.com/define.php?term=1337" target="_blank">1337</a> sk1llz.
		</li>
		<li>
		<strong>Intelligent comment filtering</strong>: Whether it be via Bayes filtering 
		or some other means, but a more autonomous method of spam filtering is 
		called for.
		</li>
	</ul>

</MP:MasterPage>
