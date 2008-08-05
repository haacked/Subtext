<%@ Page Language="C#" EnableTheming="false"  Title="Subtext Installation: Step 3 - Create Blog" MasterPageFile="~/Install/InstallTemplate.Master" Codebehind="Step03_CreateBlog.aspx.cs" AutoEventWireup="True" Inherits="Subtext.Web.Install.Step03_CreateBlog" %>

<asp:Content ID="mainContent" ContentPlaceHolderID="Content" runat="server">
    <fieldset>
        <legend>Create a Blog</legend>
        <ol>
		    <li>Step 1: Install Database</li>
		    <li>Step 2: Configure the Admin</li>
		    <li><strong>Step 3: Create A Blog</strong></li>
	    </ol>

		<div>
			<h3>InstaBlogger - Instant </h3>
			<p>
			You&#8217;re one click away from unleashing your creativity on the 
			world with your bad blogging self.
			<p>
			Click the button below to instantly create your blog.
			</p>
			<p>
			You can manage blogs via the <asp:HyperLink runat="server" ID="hostAdminlink" NavigateUrl="Host Admin Tool" />. However, if you plan on only 
			having one blog, then click on the quick create button.
			</p>
		    <asp:Button id="btnQuickCreate" runat="server" Text="Create!" CssClass="big-button" onclick="btnQuickCreate_Click" />
			<p class="footnote">
			If you have a .TEXT database, you can import it 
			using our <asp:HyperLink ID="importLink" runat="server" NavigateUrl="~/HostAdmin/Import/ImportStart.aspx" Text=".TEXT importer" />.
			</p>
		</div>
	</fieldset>
</asp:Content>