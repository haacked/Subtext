<%@ Control %>
<%@ Register TagPrefix="sub" Namespace="Subtext.Web.UI.WebControls" Assembly="Subtext.Web" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="Controls/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Footer" Src="Controls/Footer.ascx" %>
<%@ Register TagPrefix="uc1" TagName="BlogStats" Src="Controls/BlogStats.ascx" %>
<%@ Register TagPrefix="uc1" TagName="News" Src="Controls/News.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SingleColumn" Src="Controls/SingleColumn.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MyLinks" Src="Controls/MyLinks.ascx" %>
<div id="content">
    <uc1:Header id="Header1" runat="server"></uc1:Header>
    <div class="rbroundbox" id="newsBox">
        <div class="rbtop">
            <div>
            </div>
        </div>
        <div id="newsFlash" class="rbcontent">
            <uc1:News ID="news" runat="server" />
        </div>
        <!-- /rbcontent -->
        <div class="rbbot">
            <div>
            </div>
        </div>
    </div>
    <!-- /rbroundbox -->
    <div class="container2">
        <div class="left-element">
            <div id="navcontainer">
                <uc1:MyLinks id="MyLinks1" runat="server" />
            </div>
            
            <uc1:SingleColumn ID="singleColumn" runat="server" />
               
        </div>
        <div class="right-element">
            
            <sub:contentregion id="MPMain" runat="server"></sub:contentregion>
            
            <uc1:Footer ID="footer" runat="server" />
            <!-- /rbroundbox -->
        </div>
    </div>
</div>
