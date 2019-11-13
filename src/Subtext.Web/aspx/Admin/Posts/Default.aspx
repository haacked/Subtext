<%@ Page Language="C#" MasterPageFile="~/aspx/Admin/Posts/Posts.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Subtext.Web.Admin.Posts.Default" Title="Subtext Admin - Posts" %>

<asp:Content ContentPlaceHolderID="postsContent" runat="server">
    <st:MessagePanel id="Messages" runat="server" />
    <st:EntriesList id="entries" runat="server" EntryType="BlogPost" HeaderText="Posts" />
    <div class="footnote">
        <sup>*</sup>Note: For the <em>Edit in WLW</em> link to work, you&#8217;ll need this 
        <a href="http://aovestdipaperino.com/downloads/WLWDownloader.msi" title="WLW Post Downloader Plugin">Windows Live Writer plugin.</a> 
        For more details, read <a href="http://codeclimber.net.nz/archive/2010/07/10/How-to-edit-very-old-posts-with-Windows-Live-Writer.aspx" title="Edit old posts in WLW">this blog post</a>.
    </div>
</asp:Content>
