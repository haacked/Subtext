<%@ Page language="c#" Codebehind="Step04_CreateBlog.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.Install.Step04_CreateBlog" %>
<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<MP:MasterPage id="MPContainer" TemplateFile="~/Install/PageTemplate.ascx" runat="server">
	<MP:Contentregion id="MPTitle" runat="server">Subtext Installation: Step 4 - Create Blog</MP:Contentregion>
	<MP:Contentregion id="MPSubTitle" runat="server">Step 4 - Create Blog</MP:Contentregion>
	<ol>
		<li>Gather Installation Information</li>
		<li>Install the database</li>
		<li>Configure the Host Admin</li>
		<li>
			<strong>Create a Blog</strong>
		</li>
	</ol>
	<P>
		<table border="0">
			<tr valign="top">
				<td>
					<h3>Blog Quick Create</h3>
					<p>
					There are currently no blogs within your system. In the future, you can manage 
					blogs via the Host Admin tool. However, if you plan on only having one blog, 
					then click on the quick create button.
					</p>
					<p>
					This will create a blog using the Username and Password you 
					already specified for the HostAdmin. Afterwards, we’ll take you to the admin 
					tool where you can configure your blog.
					</p>
				</td>
				<td>
					<h3>Import .TEXT Blogs</h3>
					<p>
						If you plan to import a blog or blogs from a .TEXT database, 
						click on the button below.  This is a one-time only operation 
						as it does a FULL import from .TEXT.
					</p>
					<p>
						This has only been tested with .TEXT 0.95 databases.
					</p>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Button id="btnQuickCreate" runat="server" Text="Quick Create"></asp:Button>&nbsp;&nbsp;
				</td>
				<td>
					<asp:Button id="btnImportBlog" runat="server" Text="Import .TEXT"></asp:Button>
				</td>
			</tr>
		</table>
	</P>
</MP:MasterPage>
