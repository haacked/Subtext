<%@ Page CodeBehind="AggDefault.aspx.cs" EnableViewState="false" Language="C#" EnableTheming="false"  AutoEventWireup="false" Inherits="Subtext.Web.AggDefault" %>
<%@ OutputCache VaryByParam="GroupID" VaryByHeader="Accept-Language" CacheProfile="ChangedFrequently" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" lang="en" xml:lang="en">
    <head runat="server">
        <title><asp:Literal id="title" runat="server" Text="<%$ AppSettings:AggregateTitle %>" /></title>
        <link href="../skins/_system/csharp.css" rel="stylesheet" type="text/css" />
        <link href="../skins/_system/commonstyle.css" rel="stylesheet" type="text/css" />
        <link href="../skins/_system/commonlayout.css" rel="stylesheet" type="text/css" />
        <asp:Literal id="Style" runat="Server" />
        <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.6.0/jquery.min.js"></script>
        <script type="text/javascript" src="<%= VirtualPathUtility.ToAbsolute("~/Scripts/common.js") %>"></script>
        <link href="../css/lightbox.css" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="<%= VirtualPathUtility.ToAbsolute("~/Scripts/lightbox.js") %>"></script>
    </head>
    <body>
        <form id="Form1" method="post" runat="server">
            <st:MasterPage id="MPContainer" runat="server">
                <st:ContentRegion id="MPMain" runat="server">
                    <asp:PlaceHolder id="CenterBodyControl" runat="server" />
                </st:ContentRegion>
            </st:MasterPage>
        </form>
    </body>
</html>
