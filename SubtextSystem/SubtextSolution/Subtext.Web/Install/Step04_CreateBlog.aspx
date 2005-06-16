<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<%@ Page language="c#" Codebehind="Step04_CreateBlog.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.Install.Step04_CreateBlog" %>
<MP:MasterPage id="MPContainer" TemplateFile="~/Install/PageTemplate.ascx" runat="server">
	<MP:ContentRegion id="MPTitle" runat="server">Subtext Installation: Step 4 - Create Blog</MP:ContentRegion>
	<MP:ContentRegion id="MPSubTitle" runat="server">Step 4 - Create Blog</MP:ContentRegion>
	<ol>
		<li>Gather Installation Information</li>
		<li>Install the database</li>
		<li>Configure the Host Admin</li>
		<li><strong>Create a Blog</strong></li>
	</ol>
		<p>
			<table border="0">
				<tr>
					<td>
					There are currently no blogs within your system  In the future .
					you can manage blogs via the Host Admin tool.  However, if you plan 
					on only having one blog, then click on the quick create button.
					</td>
				</tr>
				<tr>
					<td>
						If you only plan on having one blog in this system, click on 
						the &#8220;Quick Create&#8221; button below.  This will create 
						a blog using the Username and Password you already specified 
						for the HostAdmin.  Afterwards, we&#8217;ll take you to the 
						admin tool where you can configure your blog.
					</td>
				</tr>
				<tr>
					<TD>
						<asp:Button id="btnQuickCreate" Text="Quick Create!" runat="server"></asp:Button>
					</TD>
				</TR>
			</table>
		</p>
</MP:MasterPage>
