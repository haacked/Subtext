<%@ Control %>
<%@ Register TagPrefix="DT" Namespace="Subtext.Web.UI.WebControls" Assembly="Subtext.Web" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="Controls/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Footer" Src="Controls/Footer.ascx" %>
<%@ Register TagPrefix="uc1" TagName="BlogStats" Src="Controls/BlogStats.ascx" %>
<%@ Register TagPrefix="uc1" TagName="News" Src="Controls/News.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SingleColumn" Src="Controls/SingleColumn.ascx" %>
<div id="main">
	<div id="backgroundleft">
	<div id="background">
		<div id="logo"></div>
		<div id="nav">
			<uc1:Header id="Header1" runat="server"></uc1:Header>
		</div><!-- end #nav -->
		<div id="content">
			<div id="adwrap">
				<p class="ad">
					<script type="text/javascript">
					<!--
					google_ad_client = "pub-7694059317326582";
					google_ad_width = 468;
					google_ad_height = 60;
					google_ad_format = "468x60_as";
					google_ad_channel ="3629720158";
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
				Hello, I'm Phil Haack (pronounced "hack") though I've been known to go by 
				the <a href="http://haacked.com/archive/2005/03/12/2350.aspx">blogger handle</a> 
				Haacked (creative, huh?).
				</p>
				<p>
				I'm an <a href="http://haacked.com/archive/2005/02/22/2168.aspx">independent consultant</a> 
				specializing in Microsoft technologies (such as .NET) living in 
				sunny (&lt;-- required when mentioning LA) Los Angeles.
				</p>
				<p>
				I'm also a member of the <a href="http://www.rssbandit.org/">RSS Bandit team</a> and enjoy documenting its nooks and 
				crannies all the while hacking away at its code base in my free time.
				</p>
			</div>
			<div>
				<asp:HyperLink ImageUrl="~/skins/HaackedOrange/images/rss20icon.gif" Runat="server" NavigateUrl="~/Rss.aspx" ID="XMLLink">RSS 2.0 Feed</asp:HyperLink><asp:HyperLink Runat="server" NavigateUrl="~/Rss.aspx" ID="Syndication" />
			</div>
			<uc1:SingleColumn id="SingleColumn1" runat="server"></uc1:SingleColumn>
			<div>
				<p>
					<a href="http://jigsaw.w3.org/css-validator/validator?uri=http://haacked.com/">
					<img style="border:0;width:88px;height:31px"
						src="/images/vcss.png" 
						alt="Valid CSS!" />
					</a>
				</p>
			</div>
		</div>
				
		<div class="clear">&nbsp;</div>	
	</div> <!-- end #background -->
	</div> <!-- end #backgroundleft -->
	<div id="bottom"></div>
	<div id="footer">
			<div id="copyright">Copyright © 2005 Haacked</div>
			<div id="license">
				<a href="http://creativecommons.org/licenses/by/2.0/"><img src="/images/ccLicense.gif" width="88" height="31" alt="Creative Commons License" /></a><br />
				This work is licensed under a <a href="http://creativecommons.org/licenses/by/2.0/" rel="license">
					Creative Commons License</a>
			</div>
		</div> <!-- end #footer -->
</div> <!-- end #main -->