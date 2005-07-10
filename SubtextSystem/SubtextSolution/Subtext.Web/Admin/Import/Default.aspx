<%@ Page language="c#" Codebehind="Default.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.Admin.Import._Default" %>
<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<MP:MasterPage id="MPContainer" TemplateFile="~/Admin/Import/PageTemplate.ascx" runat="server">
	<MP:ContentRegion id="MPTitle" runat="server">Subtext - Welcome to the Import Tool</MP:ContentRegion>
	<MP:ContentRegion id="MPSubTitle" runat="server">Welcome!</MP:ContentRegion>
	<p>Welcome to the Subtext Import tool!</p>
	<p>
		Since you&#8217;re here, you are most likely interested 
		in importing blog data from another blog engine.  Glad to 
		see that you&#8217;re giving Subtext a try.
	</p>
	<p>
		We&#8217;ll walk you through the steps to get you up 
		and running in no time.  But first, a warning.
	</p>
	<div>
		<strong>Warning!</strong>  Importing blog data from another 
		blog engine will overwrite any data you might currently have 
		within the blog.  Please backup your database before continuing.
	</div>
	<p>
		Ok, now that we have that out of the way, click &#8220;Next&#8221; 
		to select the blog engine you wish to import from.
	</p>
	<p>
		<asp:Button id="btnNext" runat="server" text="Next - Select an Import Provider"></asp:Button>
	</p>
	
</MP:MasterPage>
