<%@ Page language="c#" Title="Subtext - Your Blog Cannot Connect To The Database" MasterPageFile="~/SystemMessages/SystemMessageTemplate.Master" Codebehind="CheckYourConnectionString.aspx.cs" AutoEventWireup="True" Inherits="Subtext.Web.CheckYourConnectionString" %>
<asp:Content id="titleBar" ContentPlaceHolderID="MPTitle" runat="server">Your Blog Cannot Connect To The Database</asp:Content>
<asp:Content id="subtitle" ContentPlaceHolderID="MPSubTitle" runat="server">But I Can Help You</asp:Content>
<asp:Content id="mainContent" ContentPlaceHolderID="Content" runat="server">
	<p>
		Welcome! It looks like the Subtext Blogging Engine code has been installed, but has not been 
		properly configured just yet. It appears that I&#8217;m having trouble connecting to 
		your backend database.
	</p>
	<p>
		Please check the connection string in your web.config file.  It&#8217;s in the 
		<code>ConnectionStrings</code> section with the key &#8220;subtextData&#8221;.
	</p>
	<p>
		Also check to make sure that you have correctly set up the SQL Server user 
		you are using to connect to the database with.
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
			<asp:Label id="lblErrorMessage" runat="server">Label</asp:Label></p>
		<p>
		<p><strong>Stack Trace:</strong><br />
			<asp:Label id="lblStackTrace" runat="server">Label</asp:Label>
		</p>
	</asp:PlaceHolder>
</asp:Content>