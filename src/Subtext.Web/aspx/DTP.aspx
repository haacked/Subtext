<%@ Page Language="C#" Inherits="Subtext.Web.UI.Pages.SubtextMasterPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" lang="en" xml:lang="en">
<head runat="server">
    <title>
        <asp:Literal ID="pageTitle" runat="server" /></title>
    <meta http-equiv="content-type" content="text/html; charset=UTF-8" />
    <st:MetaTagsControl ID="metaTags" runat="server" />
    <asp:Literal ID="authorMetaTag" runat="server" />
    <asp:Literal ID="versionMetaTag" runat="server" />
    <link id="RSSLink" title="RSS" type="application/rss+xml" rel="alternate" runat="Server" />
    <asp:Literal ID="styles" runat="server"></asp:Literal>
    <link id="CustomCss" runat="server" type="text/css" rel="stylesheet" />
    <link id="Rsd" rel="EditURI" type="application/rsd+xml" title="RSD" runat="server" />
    <link id="wlwmanifest" rel="wlwmanifest" type="application/wlwmanifest+xml" runat="server" />
    <link id="opensearch" rel="search" type="application/opensearchdescription+xml" runat="server" />
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.6.0/jquery.min.js"></script>
    <script type="text/javascript" src="<%= VirtualPathUtility.ToAbsolute("~/Scripts/common.js") %>"></script>
    <script type="text/javascript">
            <%= AllowedHtmlJavascriptDeclaration %>
    </script>
    <asp:Literal ID="scripts" runat="server" />
    <asp:PlaceHolder ID="coCommentPlaceholder" runat="server" />
    <asp:Literal ID="pinbackLinkTag" runat="server" />
    <asp:Literal ID="openIDServer" runat="server" />
    <asp:Literal ID="openIDDelegate" runat="server" />
    <% if (Request.IsAuthenticated && User.IsAdministrator())
       { %>
    <script type="text/javascript" src="<%= VirtualPathUtility.ToAbsolute("~/aspx/admin/js/jquery.undoable.js") %>"></script>
    <script type="text/javascript">
        $(function() {
            $(".undoable a").undoable({
                url: '<%= Url.CommentUpdateStatus() %>',
                getUndoPostData: function(clickSource, target) {
                    var data = this.getPostData(clickSource, target);
                    data.status = 'Approved';
                    return data;
                },
                getPostData: function(clickSource, target) {
                    var data = this.getPostData(clickSource, target);
                    data.status = clickSource.attr('class');
                    return data;
                },
                getTarget: function(clickSource) {
                    return clickSource.closest('.target');
                }
            });
        });
    </script>
    <%} %>
</head>
<body>
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager ID="SubtextScriptManager" runat="server" EnablePartialRendering="true">
    </asp:ScriptManager>
    <st:MasterPage ID="MPContainer" runat="server">
                <st:ContentRegion id="MPMain" runat="server">
                    <asp:PlaceHolder id="CenterBodyControl" runat="server" />
                </st:ContentRegion>
    </st:MasterPage>
    </form>
    <asp:Literal ID="customTrackingCode" runat="server" />
    <div id="stylesheetTest">
    </div>
</body>
</html>
