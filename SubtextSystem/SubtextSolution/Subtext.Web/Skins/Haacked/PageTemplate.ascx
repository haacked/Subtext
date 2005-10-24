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
		<div id="adBar">
			<div>
<!-- Start of Flickr Badge -->
				<table id="flickr_badge_horizontal_wrapper" cellpadding="0" cellspacing="0" border="0" width="468">
					<tr>
						<td>
							<table cellpadding="0" cellspacing="0" border="0" id="flickr_badge_wrapper" width="468">
								<caption>
									<script type="text/javascript" src="http://www.flickr.com/badge_code_v2.gne?show_name=1&amp;count=4&amp;display=random&amp;size=t&amp;layout=h&amp;source=user_tag&amp;user=95736638%40N00&amp;tag=badge"></script>
								</caption>
							</table>
						</td>
					</tr>
				</table>
<!-- End of Flickr Badge -->
			</div>
			<div id="adwrap">
				<p id="ad">
					<script type="text/javascript">
					<!--
					google_ad_client = "pub-7694059317326582";
					google_ad_width = 728;
					google_ad_height = 90;
					google_ad_format = "728x90_as";
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
			</div> <!-- end #adwrap-->
		</div><!-- end #adBar -->
		<div id="content">		
			<DT:contentregion id="MPMain" runat="server"></DT:contentregion>
		</div> <!-- end #content -->
		
		<div id="sidebar">
			<!-- //TODO: Remove for Release -->
			<div id="aboutMeBox">
				<h2><a href="/articles/AboutHaacked.aspx">About Haacked</a></h2>
			</div>
			<!-- //END TODO -->
			<div>
				<asp:HyperLink ImageUrl="~/skins/Haackify/images/rss20icon.gif" Runat="server" NavigateUrl="~/Rss.aspx" ID="XMLLink">RSS 2.0 Feed</asp:HyperLink><asp:HyperLink Runat="server" NavigateUrl="~/Rss.aspx" ID="Syndication" /><br />
				<asp:HyperLink ImageUrl="~/images/PoweredBySubtext85x33.png" Runat="server" NavigateUrl="http://subtextproject.com/" ID="PoweredByLink"></asp:HyperLink>
			</div>
			<uc1:SingleColumn id="SingleColumn1" runat="server"></uc1:SingleColumn>
			<div>
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

		</div>
				
		<div class="clear">&nbsp;</div>	
	</div> <!-- end #background -->
	<div id="bottom"></div>
	<uc1:Footer id="Footer1" runat="server"></uc1:Footer>
</div> <!-- end #main -->