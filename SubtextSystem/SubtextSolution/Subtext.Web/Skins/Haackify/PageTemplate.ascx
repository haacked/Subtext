<%@ Control %>
<%@ Register TagPrefix="DT" Namespace="Subtext.Web.UI.WebControls" Assembly="Subtext.Web" %>	
<%@ Register TagPrefix="uc1" TagName="Header" Src="Controls/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Footer" Src="Controls/Footer.ascx" %>
<%@ Register TagPrefix="uc1" TagName="BlogStats" Src="Controls/BlogStats.ascx" %>
<%@ Register TagPrefix="uc1" TagName="News" Src="Controls/News.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SingleColumn" Src="Controls/SingleColumn.ascx" %>
<div id="main">
	<div id="background">
		<uc1:Header id="Header1" runat="server"></uc1:Header>
		<div id="content">
			<div id="adwrap">
				<p id="ad">
					<script type="text/javascript">
					<!--
					google_ad_client = "pub-7694059317326582";
					google_ad_width = 468;
					google_ad_height = 60;
					google_ad_format = "468x60_as";
					google_ad_channel = "3629720158";
					google_color_border = "FFFFFF";
					google_color_bg = "FFFFFF";
					google_color_link = "444444";
					google_color_url = "BF5010";
					google_color_text = "444444";
					//-->
					</script>
					<script type="text/javascript" src="http://pagead2.googlesyndication.com/pagead/show_ads.js"></script>
				</p>
			</div> <!-- end google ad -->

			
			<DT:contentregion id="MPMain" runat="server"></DT:contentregion>
			
		</div> <!-- end #content -->
		
		<div id="sidebar">
			<div id="aboutMeBox">
				<h2>About Haacked</h2>
				<p>
				In case you&#8217;re wondering, no, &#8220;Haacked&#8221; is NOT my real 
				name.  My parents aren&#8217;t so cruel.  In the offline world, people tend 
				to call me Phil Haack (pronounced "hack"), unless of course they are calling 
				me more colorful names, as is often the case. Haacked is simply my 
				<a href="http://haacked.com/archive/2005/03/12/2350.aspx">blogger handle</a>. 
				(creative, huh?).
				</p>
				<p>
				Professionally speaking, I&#8217;m currently an 
				<a href="http://haacked.com/archive/2005/02/22/2168.aspx">independent consultant</a> 
				specializing in Microsoft technologies (such as .NET) living and working in 
				sunny (&lt;-- required when mentioning LA) Los Angeles.
				</p>
				<p>
				I&#8217;m also a member of the <a href="http://www.rssbandit.org/">RSS Bandit team</a> 
				and enjoy documenting its nooks and crannies all the while hacking away at its code 
				base in my free time.
				</p>
				<p>
				I&#8217;ve recently started a new open source project named 
				<a href="http://subtextproject.com/">Subtext</a> which has absorbed 
				the rest of my free time.
				</p>
			</div>
			<div>
				<asp:HyperLink ImageUrl="~/skins/Haackify/images/rss20icon.gif" Runat="server" NavigateUrl="~/Rss.aspx" ID="XMLLink">RSS 2.0 Feed</asp:HyperLink><asp:HyperLink Runat="server" NavigateUrl="~/Rss.aspx" ID="Syndication" />
			</div>
			<div id="flickrBadge">
			<!-- Start of Flickr Badge -->
				<table id="flickr_badge_uber_wrapper" cellpadding="0" cellspacing="0" border="0">
					<tr>
						<td>
							<table cellpadding="0" cellspacing="2" border="0" id="flickr_badge_wrapper">
								<caption>
									<script type="text/javascript" src="http://www.flickr.com/badge_code_v2.gne?show_name=1&amp;count=4&amp;display=random&amp;size=t&amp;layout=v&amp;source=user_tag&amp;user=95736638%40N00&amp;tag=badge"></script>
								</caption>
								<tr>
									<td align="center" id="flickr_badge_source_txt">View more of<br /> <a href="http://www.flickr.com/photos/haacked/tags/badge/">Haacked's photos</a></td>
								</tr>
							</table>
						</td>
					</tr>
				</table>
<!-- End of Flickr Badge -->
			</div>

			<uc1:SingleColumn id="SingleColumn1" runat="server"></uc1:SingleColumn>
		</div>
				
		<div class="clear">&nbsp;</div>	
	</div> <!-- end #background -->
	<div id="bottom"></div>
	<uc1:Footer id="Footer1" runat="server"></uc1:Footer>
</div> <!-- end #main -->