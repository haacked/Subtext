<%@ Control %>
<%@ Register TagPrefix="SP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<h2>Docs</h2>
<ul>
	<SP:MenuItem href="~/Docs/" parentpath="~/Docs/" title="Introduction" runat="server" ID="docsLink">Introduction</SP:MenuItem>
	<SP:MenuItem href="~/Docs/Installation/" parentpath="~/Docs/" title="Installing This Sucker" runat="server" ID="docInstallLink">Installation</SP:MenuItem>
	<SP:MenuItem href="~/Docs/FAQ/" parentpath="~/About/" title="Frequently Asked Questions" runat="server" ID="docFaqLink">FAQ</SP:MenuItem>
	<SP:MenuItem href="~/Docs/Developer/" parentpath="~/About/" title="Developer Documentation" runat="server" ID="docDeveloperLink">Developer</SP:MenuItem>
</ul>