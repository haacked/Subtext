<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.Footer" %>
<div id="footer" width="100%">
	Copyright &copy; <% = DateTime.Now.Year %> <asp:Literal id="FooterText" runat="server" />
	
	<p>
	<a href="http://creativecommons.org/licenses/by/2.0/"><img src="/images/ccLicense.gif" width="88" height="31" alt="Creative Commons License" /></a><br />
	This work is licensed under a <a href="http://creativecommons.org/licenses/by/2.0/" rel=license>Creative Commons License</a>
	</p>
</div>