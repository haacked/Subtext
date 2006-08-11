<%@ Page language="c#" Title="Subtext - Database Login Failure" MasterPageFile="~/SystemMessages/SystemMessageTemplate.Master" Codebehind="DatabaseLoginFailed.aspx.cs" AutoEventWireup="True" Inherits="Subtext.Web.DatabaseLoginFailed" %>
<asp:Content id="titleBar" ContentPlaceHolderID="MPTitle" runat="server">Database Login Failure</asp:Content>
<asp:Content id="subtitle" ContentPlaceHolderID="MPSubTitle" runat="server">But I Can Help You</asp:Content>
<asp:Content id="mainContent" ContentPlaceHolderID="Content" runat="server">
	<p>
		Greetings! Subtext cannot connect to your backend database.  Seems to be 
		a problem with the login.
	</p>
	<p>
		Please check the connection string in your web.config file.  It&#8217;s in the 
		<code>ConnectionStrings</code> section with the key &#8220;subtextData&#8221;.
	</p>
	<p>
		Also make sure that the user specified in the connection string 
		has permissions to your database.
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
			Please be kind and report the issue to the <a href="mailto:subtext-devs@lists.sourceforge.net?subject=Houston,+We+Have+a+Problem!" title="Email The Subtext Team">subtext team</a>.
		</p>
		<h2>Original Error Information</h2>
		<p><strong>Message:</strong><br />
			<asp:Label id="lblErrorMessage" runat="server">Label</asp:Label>
		</p>
		<p><strong>Stack Trace:</strong><br />
			<asp:Label id="lblStackTrace" runat="server">Label</asp:Label>
		</p>
	</asp:PlaceHolder>
</asp:Content>