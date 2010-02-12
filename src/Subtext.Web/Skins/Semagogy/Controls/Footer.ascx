<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.Footer" %>
	<div class="Copyright">
		Copyright &copy; <%= DateTime.Now.Year %> <asp:Literal id="FooterText" runat="server" />
	</div>
	<div class="License">
		This work is licensed under a <a href="http://creativecommons.org/licenses/by/2.0/" rel="license">Creative Commons License</a>
	</div>
