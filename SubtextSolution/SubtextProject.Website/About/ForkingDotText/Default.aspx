<%@ Page %>
<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<%@ Register TagPrefix="MP" TagName="AboutLinks" Src="~/About/AboutLinks.ascx" %>
<MP:MasterPage id="MPContainer" runat="server">
	<MP:ContentRegion id="MPTitle" runat="server">Subtext - About - Forking .TEXT</MP:ContentRegion>
	<MP:ContentRegion id="MPSideBar" runat="server">
		<MP:AboutLinks id="AboutLinks" runat="server" />
	</MP:ContentRegion>
	
	<h2>So Why Fork .TEXT?</h2>
	<p>
		There are two main reasons to fork it.
	</p>
	<h3>.TEXT is dead as an open source product.</h3>
	<p>
		.TEXT is dead as a BSD licensed open source project. Out of its ashes has risen 
		<a href="http://communityserver.org/" target="_blank">Community Server</a> which 
		integrates a new version of the .TEXT source code with Forums and Photo Galleries.  
		Community Server is now a 
		<a href="https://store.telligentsystems.com/FamilyProducts.aspx?id=1" target="_blank">product being sold</a> 
		by <a href="https://www.telligentsystems.com/" target="_blank">Telligent Systems</a>.
		There is a non-commercial license available, but it requires displaying
		the telligent logo and restricts usage to non-commercial purposes. Many 
		.TEXT users, however, prefer to use a blogging engine with an 
		<a href="http://www.opensource.org/licenses/index.php" target="_blank">OSI approved license</a>, 
		such as the <a href="http://www.opensource.org/licenses/bsd-license.php" target="_blank">BSD license</a>. 
	</p>
	<blockquote>
		<p>
		As an aside, if you're wondering how they can take an open source
		project and turn it into a commercial product, it's quite easy
		actually. Here's <a href="http://blogs.zdnet.com/BTL/index.php?p=1306" target="_blank">the story of another commercially acquired open source project</a>.
		</p>
	</blockquote>
	<h3>Community Server Targets A Different Market</h3>
	<p>
		Another reason is that Community Server has become sort of the Team 
		System of blogging engines. By virtue of it going commercial, it's 
		being targetted to a different market than your average hobbyist and 
		blogger. While the tight integration with forums and photo gallery, is 
		certainly appealing to many people, not everybody needs that all that. 
		Especially if it jeopardizes the quality of the platform.
	</p>
</MP:MasterPage>
