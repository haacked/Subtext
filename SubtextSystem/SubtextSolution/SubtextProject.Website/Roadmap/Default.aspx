<%@ Page %>
<%@ Register TagPrefix="MP" Namespace="SubtextProject.Website" Assembly="SubtextProject.Website" %>
<MP:SubtextMasterPage id="MPContainer" runat="server">
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
	This document is a work in progress and feedback is welcome.
	</p>
	<h3>Administrative Road Map</h3>
	<ol>
	<li>Documenting existing source code and features. (priority: high)</li>
	<li>Fill specific project roles (patch manager, forum manager, etc...) (priority: high)</li>
	<li>Set up a website and Wiki for Subtext (unfortunately subtext.com is taken). (priority: med)</li>
	<li>Set up an automated build process (NAnt) (priority: low)</li>
	</ol>
	<h3>Upcoming Releases</h3>
	<p>
	As we flesh out the roadmap, we&#8217;ll divide it into sections based on 
	planned future individual releases.  For now, this document will 
	simply list goals and features planned for the near and far future.
	</p>
	<h3>Gotta Have It Features Immediately (priority 1)</h3>
	<p>
	These features will directly support the principles of the Subtext project.  
	<em><span style="color:red">UPDATE:</span> We are rethinking the single vs 
	multiple blog support. More details later.</em>
	One important "feature" that must be discussed is the dropping of the 
	"multiple blogs on one installation" feature.  In order to maintain 
	Subtext&#8217;s goals of simplicity and it&#8217;s focus on the hobbyist 
	and individual blogger, it makes sense to focus on the scenario where users 
	are using Subtext to create a single blog.  This will distinguish Subtext 
	from Community Server which is geared towards corporations and groups that 
	wish to host multiple blogs.  Please provide feedback on this decision.
	</p>
	<ul>
		<li>
		<b>Installer for local setup</b>: We&#8217;ve started an installer using 
		the WiX toolkit.  Initially, this will be an MSI package that will install 
		both a website and the database when run locally.  Eventually, it will have 
		to be able to upgrade an existing installation.
		</li>
		<li>
		<b>Simplified configuration (single blog)</b>: By removing the multiple 
		blogs feature, configuration can be simplified immensely.
		</li>
		<li>
		<b>Configuration utility</b>: Upon first installing Subtext, the 
		configuration utility will be an easy to use WinForms app used to 
		set the connection string (and certain other settings if any) within 
		the web.config file.  This utility can be run at any time to tweak 
		web.config settings without having to muck around the XML by hand.
		</li>
		<li>
		<b>Kick ass documentation</b>: Can&#8217;t stress this enough.  
		We&#8217;ll use NDoc to generate code and API documentation.  As for 
		user documentation, we&#8217;ll have both a project site and a wiki.
		</li>
		<li>
		<b>Comments Automatically Expire</b>: This is currently hard-coded 
		into Subtext and needs to be made configurable.  Allow the user to have 
		comments turned off after a configurable number of days. Existing comments 
		will still be displayed, but no new comments will be allowed.
		</li>
	</ul>
	<h3>Gotta Have It, But Just not yet (priority 1.5)</h3>
	<ul>
		<li><b>Friendly Urls</b>: Currently, Subtext creates permalinks 
		that look like <a href="http://haacked.com/archive/2005/05/04/2953.aspx" target="_blank">http://haacked.com/archive/2005/05/04/2953.aspx</a>.  
		In a future version, we want the permalink to have a more human readable URL.  
		For example, this might be converted to 
		http://haacked.com/archive/2005/05/04/AnnouncingSubtext.aspx.
		</li>
		<li>
		<b>Improved Usability</b>: One of my pet peeves about .TEXT is how 
		hard it is to edit a really old post.  You have to page through the 
		data grid of posts till you find it.  Instead, a simple option is to 
		create a new admin token that skin creators can place in their skin 
		where a post is rendered.  When a user is logged in as an admin, the 
		token is displayed as an icon with a link that the admin can click to 
		edit that post.  Thus, to edit an old post, simply make sure you&#8217;re 
		logged in as an admin and leverage Google to find the post, and then 
		click on the admin token.
		</li>
		<li>
		<b>Replace/Upgrade FreeTextBox.dll</b>: Hopefully with something 
		that won&#8217;t mangle HTML.
		</li>
		<li><b>Comment Moderation</b>: This is merely one tool in the constant 
		battle to fight comment spam.  Allows users to turn on and off comment 
		moderation.</li>
		<li><b>Simple Comment Filtering Rules</b>
		Currently, haacked.com uses a simple trigger that filters out comments 
		with a certain number of links.  This exceedingly simple filter does 
		remarkably well.  To fight comment spam, we should start with a few 
		simple (and configurable) rules for filtering comment spam.  We can 
		add more complex rules later.
		</li>
	</ul>
	<h3>Important, But Maybe Next Release Features (priority 2)</h3>
	<ul>
		<li><b>Membership <a href="http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dnaspnet/html/asp02182004.asp" target="_blank">Provider Model</a> 
		implementation</b>: This will be a very simple system that allows a blog 
		owner to create accounts with certain roles (reader, author, admin).  Thus 
		a blog can have multiple authors for a single blog.
		</li>
		<li><b>New CSS based Templates</b>: These will be templates that 
		can be "skinned" purely via CSS (ala 
		<a href="http://www.csszengarden.com/">CSS Zen Garden</a>).  We&#8217;ll 
		provide a tool for a blog owner to edit and switch CSS for this particular template.
		</li>
		<li><b>XHTML compliance</b>: Both transitional and strict.</li>
		<li>
		<b>Comment Filtering Rules Engine</b>: This will be similar to the Junk 
		Mail rules engine in Outlook.  We&#8217;ll provide a web based interface 
		for creating filtering rule used to combat comment spam.
		</li>
	</ul>
	<h3>Features to dream about (priority we&#8217;re dreaming)</h3>
	<ul>
		<li><b>A Spell Checker</b>: For all those bad spelers out there.</li>
		<li><b>Migration utility</b>: We&#8217;re not so arrogant as to 
		believe you&#8217;ll never use another blogging engine again.  
		If you do, we want to help you migrate your permalinks and posts 
		to it.</li>
		<li><b>MySql Provider</b>: because not everyone wants to pay for 
		SQL Server hosting and some people want to honor their license 
		agreement for MSDN Universal. ;)
		</li>
		<li><b>Mono support</b>: This may be way down the road, but 
		supporting Mono would be a nice way to introduce the Linux 
		crowd to the beauty of ASP.NET and Subtext.  Besides, we&#8217;ll 
		finally get props from the Slashdot crowd for our 
		<a href="http://www.urbandictionary.com/define.php?term=1337" target="_blank">1337</a> sk1llz.
		</li>
		<li>
		<b>Intelligent comment filtering</b>: Whether it be via Bayes filtering 
		or some other means, but a more autonomous method of spam filtering is 
		called for.
		</li>
	</ul>

</MP:SubtextMasterPage>
