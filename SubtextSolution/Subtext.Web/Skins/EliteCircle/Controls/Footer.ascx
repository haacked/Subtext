<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.Footer" %>
<%@ Register TagPrefix="st" TagName="RecentComments" Src="RecentComments.ascx" %>
<%@ Register TagPrefix="st" TagName="RecentPosts" Src="RecentPosts.ascx" %>
<!-- footer starts here -->	
<div id="footer">
	<div id="footer-content">

		<st:RecentPosts id="recentPosts" runat="server" />
		<st:RecentComments id="recentPosts1" runat="server" />
		
		<div class="col2 float-right">
			<p>
				&copy; Copyright 2006 <strong><asp:Literal id="FooterText" runat="server" /></strong><br /> 
				Original Design by: <a href="http://www.styleshout.com/templates/preview/EliteCircle1-0/index.html" title="Elite Circle at Styleshout">styleshout</a>
				<br />Valid <a href="http://jigsaw.w3.org/css-validator/check/referer" title="CSS Validator">CSS</a> | <a href="http://validator.w3.org/check/referer" title="XHTML Validator">XHTML</a>
				<br />Ported to Subtext by <a href="http://haacked.com/" title="Phil Haack">Phil Haack</a>
			</p>
			<ul>
				<li><a href="http://subtextproject.com/" title="Subtext Project Website"><asp:Image runat="server" ID="subtextPoweredImage" ImageUrl="~/images/PoweredBySubtext85x33.png" width="85" height="33" alt="Powered By Subtext Logo" BorderStyle="none" /></a></li>
			</ul>
		</div>
		<br class="clear" />
	</div>
</div>
<!-- footer ends here -->
