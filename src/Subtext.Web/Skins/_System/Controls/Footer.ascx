<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.Footer" %>
	<div class="copyright">
		Copyright &copy; <%= DateTime.Now.Year %> <asp:Literal id="FooterText" runat="server" />
	</div>
