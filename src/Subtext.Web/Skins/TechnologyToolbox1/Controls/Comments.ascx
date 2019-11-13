<%@ Control Language="C#" EnableTheming="false" AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.Comments" %>
<%@ Register Tagprefix="caelum" Namespace="TechnologyToolbox.Caelum.Website.Controls"
    Assembly="TechnologyToolbox.Caelum.Website, Version=2.0.0.0, Culture=neutral, PublicKeyToken=f55b5d7768fcda39" %>
<!-- Begin: Comments.ascx -->
<div id="postComments">
    <h3>Comments</h3>
    <asp:Literal ID="NoCommentMessage" runat="server" />
    <asp:Repeater ID="CommentList" runat="server" OnItemCreated="CommentsCreated">
        <HeaderTemplate>
            <ol>
        </HeaderTemplate>
        <ItemTemplate>
            <li class="post-comment<%# (bool)Eval("IsBlogAuthor") == true ? " author-comment" : string.Empty %>">
                <h4><asp:Literal runat="server" ID="Title" /></h4>
                <div class="published">
                    <span class="value"><%# Eval("DateCreated", "{0:MMMM d, yyyy}") %></span> <span class="value"><%# Eval("DateCreated", "{0:t}") %></span>
                </div>
                <cite class="vcard author"><span class="fn">
                    <asp:HyperLink runat="server" ID="NameLink" rel="external nofollow" /></span></cite>
                <div class="avatar">
                    <caelum:BorderlessImage runat="server" ID="GravatarImg"
                        AlternateText="Gravatar" Visible="False"
                        PlaceHolderImage="~/Skins/TechnologyToolbox1/Images/Silhouette-1.jpg"
                        Height="72" Width="72" /></div>
                <div class="url"><a href="<%# Eval("SourceUrl") %>"><%# Eval("SourceUrl") %></a></div>
                <asp:HyperLink runat="server" ID="EditCommentImgLink" />
                <blockquote>
                    <asp:Literal ID="PostText" runat="server" />
                </blockquote>
            </li>
        </ItemTemplate>
        <FooterTemplate>
            </ol>
        </FooterTemplate>
    </asp:Repeater>
</div>
<!-- End: Comments.ascx -->
