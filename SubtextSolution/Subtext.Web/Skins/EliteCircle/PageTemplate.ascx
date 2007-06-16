<%@ Control %>
<%@ Register TagPrefix="st" Namespace="Subtext.Web.UI.WebControls" Assembly="Subtext.Web" %>	
<%@ Register TagPrefix="st" TagName="Header" Src="Controls/Header.ascx" %>
<%@ Register TagPrefix="st" TagName="Footer" Src="Controls/Footer.ascx" %>
<%@ Register TagPrefix="st" TagName="News" Src="Controls/News.ascx" %>
<%@ Register TagPrefix="st" TagName="TagCloud" Src="Controls/TagCloud.ascx" %>
<%@ Register TagPrefix="st" TagName="Search" Src="Controls/SubtextSearch.ascx" %>
<%@ Register TagPrefix="st" TagName="CategoryDisplay" Src="Controls/CategoryDisplay.ascx" %>


<st:Header id="Header" runat="server" />
	
<!-- content-wrap starts here -->
<div id="content-wrap">
	<div id="content">	 
		<div id="main">		
			<st:ContentRegion id="MPMain" runat="server" />
		</div>		

		<div id="sidebar">	
			<h1>Search Box</h1>	
			<p>
				<st:Search ID="search" runat="server" />
			</p>		
				
			<h1>About Me</h1>
			<st:News id="News" runat="server" />
					
			<h1>Tags</h1>
			<st:TagCloud ID="tagCloud" runat="server" ItemCount="20" />

			<st:CategoryDisplay id="CategoryDisplay" runat="server" />
								
		</div>	

	<!-- content-wrap ends here -->		
	</div>
</div>

<st:Footer ID="footer" runat="server" />
