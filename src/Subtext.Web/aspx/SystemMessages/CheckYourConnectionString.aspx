<%@ Page Language="C#" EnableTheming="false"  Title="Subtext - Your Blog Cannot Connect To The Database" MasterPageFile="~/aspx/SystemMessages/SystemMessageTemplate.Master" Codebehind="CheckYourConnectionString.aspx.cs" AutoEventWireup="True" Inherits="Subtext.Web.CheckYourConnectionString" %>
<asp:Content id="titleBar" ContentPlaceHolderID="MPTitle" runat="server">Your Blog Cannot Connect To The Database</asp:Content>
<asp:Content id="subtitle" ContentPlaceHolderID="MPSubTitle" runat="server">But I Can Help You</asp:Content>
<asp:Content id="mainContent" ContentPlaceHolderID="Content" runat="server">
	<p>
		Welcome! Subtext is having a bit of trouble connecting to 
		your backend database. Apologies for the inconvenience, but maybe these 
		steps can help.
	</p>
	<p>
        <span class="pullout">
		    <em><a href="http://connectionstrings.com/" title="Connection String Reference">connectionstrings.com</a> provides 
		    a nice reference for connection string formats.</em>
	    </span>
		Please check the connection string in your web.config file.  It&#8217;s in the 
		<code>ConnectionStrings</code> section with the key &#8220;subtextData&#8221; or &#8220;subtextExpress&#8221;. 
	</p>
	<p>
		Also check to make sure that you have correctly set up the SQL Server user 
		you are using to connect to the database with.
	</p>
	<asp:PlaceHolder id="plcDiagnosticInfo" runat="server" Visible="false">
		<p>
			Below is some extra diagnostic information.  You should only be seeing this extra 
			information if you&#8217;re connecting to this site 
			from &#8220;localhost&#8221;. Remote users should get the standard error page with much 
			less information.
		</p>
		<p>
			If you are seeing this message as a remote user, then we have a serious problem. 
			Please be kind and report the issue to the <a href="mailto:subtext@googlegroups.com?subject=Houston,+We+Have+a+Problem!" title="Email The Subtext Team">subtext team</a>.
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