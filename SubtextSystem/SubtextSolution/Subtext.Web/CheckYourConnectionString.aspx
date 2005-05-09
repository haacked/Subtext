<%@ Page language="c#" Codebehind="CheckYourConnectionString.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.CheckYourConnectionString" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Check your connection string.</title>
		<style>
			BODY { PADDING-RIGHT: 32px; PADDING-LEFT: 32px; FONT-SIZE: 13px; BACKGROUND: #eee; PADDING-BOTTOM: 32px; MARGIN-LEFT: auto; WIDTH: 80%; COLOR: #000; MARGIN-RIGHT: auto; PADDING-TOP: 32px; FONT-FAMILY: verdana, arial, sans-serif }
			DIV { BORDER-RIGHT: #bbb 1px solid; PADDING-RIGHT: 32px; BORDER-TOP: #bbb 1px solid; PADDING-LEFT: 32px; BACKGROUND: #fff; PADDING-BOTTOM: 32px; BORDER-LEFT: #bbb 1px solid; PADDING-TOP: 32px; BORDER-BOTTOM: #bbb 1px solid }
			H1 { PADDING-RIGHT: 0px; PADDING-LEFT: 0px; FONT-WEIGHT: bold; FONT-SIZE: 150%; PADDING-BOTTOM: 36px; MARGIN: 0px; COLOR: #904; PADDING-TOP: 0px; FONT-FAMILY: "trebuchet ms", ""lucida grande"", verdana, arial, sans-serif }
			H2 { PADDING-RIGHT: 0px; PADDING-LEFT: 0px; FONT-WEIGHT: bold; FONT-SIZE: 105%; PADDING-BOTTOM: 0px; MARGIN: 0px 0px 8px; TEXT-TRANSFORM: uppercase; COLOR: #999; PADDING-TOP: 0px; BORDER-BOTTOM: #ddd 1px solid; FONT-FAMILY: "trebuchet ms", ""lucida grande"", verdana, arial, sans-serif }
			P { PADDING-RIGHT: 0px; PADDING-LEFT: 0px; PADDING-BOTTOM: 6px; MARGIN: 0px; PADDING-TOP: 6px }
			A:link { COLOR: #002c99; TEXT-DECORATION: none }
			A:visited { COLOR: #002c99; TEXT-DECORATION: none }
			A:hover { COLOR: #cc0066; BACKGROUND-COLOR: #f5f5f5; TEXT-DECORATION: underline }
		</style>
	</HEAD>
	<body>
		<!-- 
		//TODO: Make this page templated like the rest of the system.
		-->
		<form id="frmMain" method="post" runat="server">
			<div>
				<h1>Subtext - Your Blog Cannot Connect To The Database</h1>
				<h2>But I Can Help You</h2>
				<p>
					Welcome! The Subtext Blogging Engine has been installed, but has not been 
					properly configured just yet. It appears that I'm having trouble connecting to 
					your backend database.
				</p>
				<p>
					Please check the connection string in your web.config file OR run the handy 
					dandy SubtextConfigurationTool.
				</p>
				<p>
					Don't worry, you should only see this message if you're connecting to this site 
					from "localhost". Remote users should get the standard error page with much less 
					information.
				</p>
				<h2>Original Error Information</h2>
				<p><b>Message:</b><br/>
						<asp:Label id="lblErrorMessage" runat="server">Label</asp:Label>
				</p>
				<p><p><b>Stack Trace:</b><br/>
				<asp:Label id="lblStackTrace" runat="server">Label</asp:Label>
				</p>
			</div>
		</form>
	</body>
</HTML>
