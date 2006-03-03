<%@ Page %>
<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<%@ Register TagPrefix="MP" TagName="AboutLinks" Src="~/About/AboutLinks.ascx" %>
<MP:MasterPage id="MPContainer" runat="server">
	<MP:ContentRegion id="MPTitle" runat="server">Subtext - About</MP:ContentRegion>
	<MP:ContentRegion id="MPSideBar" runat="server">
		<MP:AboutLinks id="AboutLinks" runat="server" />
	</MP:ContentRegion>
	<h2>Origins</h2>
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
	<p align="center">		
			<a href="http://sourceforge.net/projects/subtext">
				Subtext
			</a>
			<br />
			<a href="http://sourceforge.net/donate/index.php?group_id=137896">
				<img src="http://images.sourceforge.net/images/project-support.jpg" width="88" height="32" border="0" alt="Support This Project" /> 
			</a>
	</p>
	<h2>Current Work</h2>
	<p>
	As an open source product, Subtext depends on a committed and passionate 
	group of volunteers to crank out code in their spare time.  Although we 
	have many wonderful contributors (thank you), we have a core team that 
	has write access to the CVS repository.  As contributors provide solid 
	code patches via <a href="http://sourceforge.net/">SourceForge</a>, we 
	will add more developers to the core team.
	</p>
	<p>
	<a href="~/About/TheTeam/" runat="server">Meet The Team</a>	
	</p>
	
	<h2>Logo</h2>
	<p>
		The Subtext Logo was designed by a fantastic team 
		of designers who have since formed the company, 
		<a href="http://turbomilk.com/" title="Powerful and healthy GUI design for the human" rel="external">TurboMilk</a>.  
	</p>
	<p>
		They were great to work with and we hope to persuade them 
		to contribute some more design to Subtext in the future.
	</p>
</MP:MasterPage>
