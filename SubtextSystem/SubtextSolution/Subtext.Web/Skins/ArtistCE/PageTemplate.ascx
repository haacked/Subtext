<%@ Control %>
<%@ Register TagPrefix="DT" Namespace="Subtext.Web.UI.WebControls" Assembly="Subtext.Web" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="Controls/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Footer" Src="Controls/Footer.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SingleColumn" Src="Controls/SingleColumn.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MyLinks" Src="Controls/MyLinks.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Calendar" Src="Controls/SubTextBlogCalendar.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ArchiveLinks" Src="Controls/ArchiveLinks.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CategoryDisplay" Src="Controls/CategoryDisplay.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SubtextSearch" Src="Controls/SubtextSearch.ascx" %>
<%@ Register TagPrefix="uc1" TagName="BlogStats" Src="Controls/BlogStats.ascx" %>
<%@ Register TagPrefix="uc1" TagName="RecentComments" Src="Controls/RecentComments.ascx" %>

<div id="top">
<table border="0" id="table1" cellspacing="0" cellpadding="0" width="98%">
	<tr>
		<td valign="bottom">
			<uc1:MyLinks id="MyLinks1" runat="server"></uc1:MyLinks>
		</td>		
		<td>
			<div align="center">
				<uc1:Header id="Header1" runat="server"></uc1:Header>
			</div>
		</td>
		<td valign="bottom">
			
		</td>
	</tr>
</table>
</div>
<div id="leftmenu">
	<DT:contentregion id="MPLeftColumn" runat="server">
		<uc1:Calendar id="cal" runat="server" />
		<br />
		<uc1:BlogStats id="BlogStats1" runat="server"></uc1:BlogStats>
		<br />
		<uc1:SubtextSearch id="SubtextSearch1" runat="server"></uc1:SubtextSearch>
		<br />
		<uc1:RecentComments id="RecentComments1" runat="server"></uc1:RecentComments>
		<br />
		<uc1:ArchiveLinks id="ArchiveLinks1" runat="server"></uc1:ArchiveLinks>

		<uc1:CategoryDisplay id="CategoryDisplay1" runat="server"></uc1:CategoryDisplay>
		<h5>Disclaimer</h5>
		<div align="center" id="disclaimer">
			The information in this weblog is provided "AS IS" 
			with no warranties, and confers no rights. This weblog 
			does not represent the thoughts, intentions, plans 
			or strategies of my employer. It is solely my opinion. 
			Inappropriate comments will be deleted at the authors 
			discretion. All code samples are provided "AS IS" without 
			warranty of any kind, either express or implied, 
			including but not limited to the implied warranties 
			of merchantability and/or fitness for a particular purpose.
		</div>
		<!--Hacker logo-->
		<p align="center">
			<a href='http://www.catb.org/hacker-emblem/'>
				<img src='http://www.catb.org/hacker-emblem/glider.png' alt='hacker emblem' />
			</a>
		</p>
		<br />
		<!--Sourceforge-->
		<h5>Sourceforge.net</h5>
		<div align="center" id="sourceforge">
			<a href="http://sourceforge.net/projects/subtext">
				Subtext
			</a>
			<br />
			<a href="http://sourceforge.net/donate/index.php?group_id=137896">
				<img src="http://images.sourceforge.net/images/project-support.jpg" width="88" height="32" border="0" alt="Support This Project" /> 
			</a>
		</div>
	</DT:contentregion>
</div>
<div id="main">
	<DT:contentregion id="MPMain" runat="server"></DT:contentregion>
</div>
<p>
	<uc1:Footer id="Footer1" runat="server"></uc1:Footer>
</p>

