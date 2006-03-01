<%@ Page %>
<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<%@ Register TagPrefix="MP" TagName="DocLinks" Src="~/Docs/DocLinks.ascx" %>
<MP:MasterPage id="MPContainer" runat="server">
	<MP:ContentRegion id="MPTitle" runat="server">Subtext - Docs - Configuration</MP:ContentRegion>
	<MP:ContentRegion id="MPSideBar" runat="server">
		<MP:DocLinks id="AboutLinks" runat="server" />
	</MP:ContentRegion>
	
	<h2>Configuration Chart</h2>
	<p>[<a href="../../Configuration/" title="Configuration">Back to Configuration</a>]</p>
	<h3>Root Web Application</h3>
	<p>
		Each of these scenarios assumes that the Subtext code 
		is deployed to the root of the website.
	</p>
	<p>
		In these examples, the subtext code is in the physical 
		directory c:\inetpub\wwwroot with an IIS website pointing 
		to that location.
	</p>
	<table class="configuration">
		<tr>
			<th>Intended Url</th>
			<th>Virtual Directory</th>
			<th>Subfolder Setting</th>
		</tr>
		<tr class="alt">
			<td>http://example.com/</td>
			<td>None</td>
			<td>{BLANK}</td>
		</tr>
		<tr>
			<td>http://example.com/blog/default.aspx</td>
			<td>None</td>
			<td>blog</td>
		</tr>
		<tr class="alt">
			<td>http://example.com/blog/</td>
			<td>Virtual Directory named &#8220;Blog&#8221; points to c:\inetpub\wwwroot</td>
			<td>blog</td>
		</tr>
	</table>
	<h3>IIS Virtual Application</h3>
	<p>
		These scenarious assume that subtext is deployed to the root of an IIS 
		virtual application.  This is a typical configuration on a WinXP 
		development machine.
	</p>
	<p>
		In these examples, there is an IIS website pointing to c:\inetpub\wwwroot.  
		There is also a virtual application (not to be confused with a 
		virtual directory) named &#8220;Blog&#8221; points to c:\inetpub\wwwroot\Blog. 
		The subtext code is deployed to c:\inetpub\wwwroot\Blog
	</p>
	<table class="configuration">
		<tr>
			<th>Intended Url</th>
			<th>Virtual Directory</th>
			<th>Subfolder Setting</th>
		</tr>
		<tr class="alt">
			<td>http://example.com/</td>
			<td colspan="2">n/a</td>
		</tr>
		<tr>
			<td>http://example.com/blog/</td>
			<td>None</td>
			<td>none</td>
		</tr>
		<tr class="alt">
			<td>http://example.com/blog/MyBlog/default.aspx</td>
			<td>none</td>
			<td>MyBlog</td>
		</tr>
	</table
</MP:MasterPage>
