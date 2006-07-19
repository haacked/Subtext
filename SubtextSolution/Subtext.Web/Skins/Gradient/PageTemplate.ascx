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
            <div id="footer">
				<uc1:Footer ID="footer2" runat="server" />
			</div>
        </div>
        
    </div>
    
</div>
<!--
Original Design: Two Point Oh by Jason Kingery aka Denial http://denial-design.com.
Found on the Open Source Web Design site: http://www.oswd.org/design/preview/id/2834

Original Design licensed under the GPL (http://www.opensource.org/licenses/gpl-license.php)
Design adapted and modified by the Subtext team 2006.
//-->