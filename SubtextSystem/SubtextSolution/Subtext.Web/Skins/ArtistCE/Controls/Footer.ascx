<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.Footer" %>
<div id="footer" align="center">
	<p id="copyright">&copy; 2006 - <asp:Literal id="FooterText" runat="server" />. </p>
	<p id="info">Theme by <a href="http://www.analystdeveloper.com/" 
	title="Click here to visit my website">AD Design</a>. 
	Valid <a href="http://validator.w3.org/check/referer" 
	title="Click here to validate the XHTML code of this page at W3.org">XHTML</a> &amp; 
	<a href="http://jigsaw.w3.org/css-validator/check/referer" 
	title="Click here to validate the CSS code of this page at W3.org">CSS</a>.</p>
	
	Powered by: 
	<br />
	<asp:HyperLink ImageUrl="~/images/PoweredBySubtext85x33.png" NavigateUrl="http://sourceforge.net/projects/subtext/" Runat="server" ID="Hyperlink2" NAME="Hyperlink1"/>
	<asp:HyperLink ImageUrl="~/images/PoweredByAsp.Net.gif" NavigateUrl="http://ASP.NET" Runat="server" ID="Hyperlink3" NAME="Hyperlink1"/>
	<br />
</div>
	
