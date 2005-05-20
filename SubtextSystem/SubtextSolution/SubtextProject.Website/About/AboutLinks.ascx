<%@ Control %>
<%@ Register TagPrefix="SP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<ul>
	<h2>About</h2>
	<SP:MenuItem href="~/About/" parentpath="~/About/" title="About Introduction" runat="server">Introduction</SP:MenuItem>
	<SP:MenuItem href="~/About/ForkingDotText/" parentpath="~/About/" title="About Forking .TEXT" runat="server">Why Fork .TEXT</SP:MenuItem>
	<SP:MenuItem href="~/About/Requirements/" parentpath="~/About/" title="What you need to run Subtext" runat="server" ID="Menuitem1">Requirements</SP:MenuItem>
	<SP:MenuItem href="~/About/TheTeam/" parentpath="~/About/" title="The Volunteers Who Keep Subtext Running" runat="server" ID="Menuitem2">Meet the Team</SP:MenuItem>
</ul>	