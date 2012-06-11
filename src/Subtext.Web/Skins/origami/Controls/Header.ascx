<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.Header" %>
<div id="header">
                    <div id="jump-menu">
                        <h3>Jump Menu</h3>
                        <ul class="menu horizontal clearfix">
                            <li><a id="skipToNav" title="Jump to Navigation" href="#sidebar">Navigation</a> </li>
                            <li><a id="skipToContent" title="Jump to Content" href="#content">Content</a> </li>
                            <li><a id="skipToSearch" title="Jump to Search" onclick="document.getElementById('q').focus(); return false;" href="#q">Search</a> </li>
                        </ul>
                    </div>
                    <div id="personalize">
                        <div id="personalization-help">
                            <h2><a title="What do all these icons mean?" href="#"></a></h2>
                            <div id="personalization-instructions" style="display: none">
                                <h3>So what are all these buttons for?</h3>
                                <p>This blog has personalition features for you, the reader. Below is a key of what all the icons mean and which aspects of the layout they change.</p>
                                <ol class="menu">
                                    <li><asp:image runat="server" AlternateText="Set font size to medium (default)" ImageUrl="~/skins/origami/images/icon_mediumText.png" />Set font size to medium (default) </li>
                                    <li><asp:image runat="server" AlternateText="Set font size to large" ImageUrl="~/skins/origami/images/icon_largeText.png" />Set font size to large </li>
                                    <li><asp:image runat="server" AlternateText="Set font size to x-large" ImageUrl="~/skins/origami/images/icon_xLargeText.png" />Set font size to x-large </li>
                                    <li><asp:image runat="server" AlternateText="Set layout to 'Jello' (min-width:770px \ max-width:1200px)" ImageUrl="~/skins/origami/images/icon_jello.png" />Set layout to "Jello" (min-width:770px \ max-width:1200px) (default) </li>
                                    <li><asp:image runat="server" AlternateText="Set layout to 'Fluid' (width: 100%)" ImageUrl="~/skins/origami/images/icon_fluid.png" />Set layout to "Fluid" (width: 100%) </li>
                                    <li><asp:image runat="server" AlternateText="Set layout to 'Fixed' (width: 760px) (default)" ImageUrl="~/skins/origami/images/icon_fixed.png" />Set layout to "Fixed" (width: 760px) </li>
                                    <li><asp:image runat="server" AlternateText="Place navigation on the right of the screen (default)" ImageUrl="~/skins/origami/images/icon_rightSideBar.png" />Place navigation on the right of the screen (default) </li>
                                    <li><asp:image runat="server" AlternateText="Place navigation on the left of the screen" ImageUrl="~/skins/origami/images/icon_leftSideBar.png" />Place navigation on the left of the screen </li>
                                </ol>
                                <p id="personalization-instructions-close"><a title="Close Help Instructions" href="#">Close Window</a></p>
                            </div>
                        </div>
                        <h3>Personalize</h3>
                        <h4>Text Size</h4>
                        <ul class="menu horizontal clearfix" id="text-size">
                            <li><a id="mediumText" title="Medium Font Size (Default)" href="pages/javascript-error">Medium Font Size (Default)</a> </li>
                            <li><a id="largeText" title="Large Font Size (Large Font Size)" href="pages/javascript-error">Large Font Size</a> </li>
                            <li><a id="xLargeText" title="X-Large Font Size (X-Large Font Size)" href="pages/javascript-error">X-Large Font Size</a> </li>
                        </ul>
                        <h4>Layout</h4>
                        <ul class="menu horizontal clearfix" id="layout">
                            <li><a id="jelloLayout" title="Jello Layout (Minimum Width:770px \ Max-Width:1200px)" href="pages/javascript-error">Jello Layout</a> </li>
                            <li><a id="fluidLayout" title="Fluid Layout (100% Width)" href="pages/javascript-error">Fluid Width Layout</a> </li>
                            <li><a id="fixedLayout" title="Fixed Width Layout (770px Wide)" href="pages/javascript-error">Fixed Width Layout</a> </li>
                        </ul>
                        <h4>Navigation Position</h4>
                        <ul class="menu horizontal clearfix" id="navigation-position">
                            <li><a id="leftNav" title="Left Navigation" href="pages/javascript-error">Left Navigation</a> </li>
                            <li><a id="rightNav" title="Right Navigation" href="pages/javascript-error">Right Navigation</a> </li>
                        </ul>
                    </div>
                    <div id="logo">
                        <h1 id="site-name"><asp:HyperLink ID="HeaderTitle" runat="server" /></h1>
                        <h2 id="sub-title"><asp:Literal ID="HeaderSubTitle" runat="server" /></h2>
                    </div>
                </div>