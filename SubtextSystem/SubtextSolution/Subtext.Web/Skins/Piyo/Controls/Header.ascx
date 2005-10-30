<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.Header" %>
<%@ Register TagPrefix="uc1" TagName="BlogStats" Src="BlogStats.ascx" %>
<div id="name">
	<h1><asp:HyperLink id="HeaderTitle" runat="server" /></h1>

	<h2><asp:Literal id="HeaderSubTitle" runat="server" /></h2>
</div>
<div id="controls">
	<uc1:BlogStats id="stats" runat="server" />
	<div id="switcher">
			<script type="text/javascript">
				writeSwitcher();
			</script>
			<noscript>
				<p></p>
			</noscript>
	</div>
</div>
