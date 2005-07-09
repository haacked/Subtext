<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<%@ Page language="c#" Codebehind="CheckYourConnectionString.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.CheckYourConnectionString" %>
<MP:MasterPage id="MPContainer" TemplateFile="~/SystemMessages/PageTemplate.ascx" runat="server">
	<MP:ContentRegion id="MPTitleBar" runat="server">Your Blog Cannot Connect To The 
Database</MP:ContentRegion>
	<MP:ContentRegion id="MPTitle" runat="server">Your 
Blog Cannot Connect To The Database</MP:ContentRegion>
	<MP:ContentRegion id="MPSubTitle" runat="server">But I Can Help You</MP:ContentRegion>
	<p>
		Welcome! It looks like the Subtext Blogging Engine code has been installed, but has not been 
		properly configured just yet. It appears that I&#8217;m having trouble connecting to 
		your backend database.
	</p>
	<p>
		Please check the connection string in your web.config file.  It&#8217;s in the 
		<code>AppSettings</code> section with the key &#8220;ConnectionString&#8221;.
	</p>
	<asp:PlaceHolder id="plcDiagnosticInfo" runat="server" Visible="false">
		<p>
			Below is some extra diagnostic information.  You should only be seeing this extra 
			information if you&#8217;re connecting to this site 
			from "localhost". Remote users should get the standard error page with much 
			less information.
		</p>
		<p>
			If you are seeing this message as a remote user, then we have a serious problem. 
			Please be kind and report the issue to the <a href="mailto:subtext-devs@lists.sourceforge.net?subject=Houston,+We+Have+a+Problem!">subtext team</a>.
		</p>
		<H2>Original Error Information</H2>
		<p><B>Message:</B><BR>
			<asp:Label id="lblErrorMessage" runat="server">Label</asp:Label></p>
		<p>
		<p><B>Stack Trace:</B><BR>
			<asp:Label id="lblStackTrace" runat="server">Label</asp:Label>
		</p>
	</asp:PlaceHolder>
</MP:MasterPage>
