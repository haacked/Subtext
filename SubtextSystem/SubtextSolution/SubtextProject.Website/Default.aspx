<%@ Page %>
<%@ Register TagPrefix="MP" Namespace="SubtextProject.Website" Assembly="SubtextProject.Website" %>
<MP:SubtextMasterPage id="MPContainer" runat="server">
	<MP:ContentRegion id="MPTitle" runat="server">Subtext - Home</MP:ContentRegion>
	<MP:ContentRegion id="MPSideBar" runat="server"></MP:ContentRegion>
	<blockquote>
		<p>
		<b>subtext</b><br/>
		Function: <i>noun</i><br/>
			1. The implicit or metaphorical meaning (as of a literary text)<br/>
			2. A story within the story.
		</p>
	</blockquote>
	<p>
	<strong>Subtext</strong> is a personal blog publishing platform that focuses 
	on usability, elegance, and simplicity.  If you&#8217;ve ever caught yourself throwing 
	your hands in the air and declaring that you&#8217;re going to write your own blogging 
	engine, then Subtext is for you.
	</p>
	<p>SCREENSHOT GOES HERE, NOT STEWIE</p>
	<div class="dropshadow" style="float:right"><div class="innerbox"><img src="~/images/screenshot.jpg" alt="Screenshot" runat="server" style="width:149px;height:112px;" width="149" height="112" /></div></div>
	<p>
	The guiding philosophy behind Subtext is to remove hindrances to online expression. 
	A blogging platform should be easy to understand, set up, and use.
	</p>
	<p>
	Subtext is an open source project licensed under the 
	<a href="http://www.opensource.org/licenses/bsd-license.php">BSD license</a>.  It is a fork 
	of the popular .TEXT blogging platform.
	</p>
</MP:SubtextMasterPage>
