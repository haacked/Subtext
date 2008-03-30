<%@ Page Language="C#" EnableTheming="false"  Title="Subtext Installation: Step 4 - Create Blog" MasterPageFile="~/Install/InstallTemplate.Master" Codebehind="Step03_CreateBlog.aspx.cs" AutoEventWireup="True" Inherits="Subtext.Web.Install.Step03_CreateBlog" %>

<asp:Content id="subTitleContent" ContentPlaceHolderID="MPSubTitle" runat="server">Step 4 - Create Blog</asp:Content>
<asp:Content ID="mainContent" ContentPlaceHolderID="Content" runat="server">
	<ol>
		<li>Gather Installation Information</li>
		<li>Install the database</li>
		<li>Configure the Host Admin</li>
		<li>
			<strong>Create a Blog</strong>
		</li>
	</ol>
	<p>
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
						After the import, you may need to change the Subfolder 
						value as well as reset the password for the imported blogs.
					</p>
					<p>
						This has only been tested with .TEXT 0.95 databases.
					</p>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Button id="btnQuickCreate" runat="server" Text="Quick Create" onclick="btnQuickCreate_Click"></asp:Button>&nbsp;&nbsp;
				</td>
				<td>
					<asp:Button id="btnImportBlog" runat="server" Text="Import .TEXT" onclick="btnImportBlog_Click"></asp:Button>
				</td>
			</tr>
		</table>
	</p>
</asp:Content>