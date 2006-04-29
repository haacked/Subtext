<%@ Control %>
<%@ Register TagPrefix="DT" Namespace="Subtext.Web.UI.WebControls" Assembly="Subtext.Web" %>	
<%@ Register TagPrefix="uc1" TagName="Header" Src="Controls/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Footer" Src="Controls/Footer.ascx" %>
<%@ Register TagPrefix="uc1" TagName="News" Src="Controls/News.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SingleColumn" Src="Controls/SingleColumn.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MyLinks" Src="Controls/MyLinks.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SubtextSearch" Src="Controls/SubtextSearch.ascx" %>

<div id="container">
	<div id="header">
		<uc1:MyLinks id="MyLinks1" runat="server"></uc1:MyLinks>
		<table class="toptable" cellspacing="0" cellpadding="0" style="border-collapse: collapse;border:1px solid #ccc;">
			<tr>
				<td class="cella">
					<uc1:Header id="Header1" runat="server"></uc1:Header>
				</td>
				<td valign="top" style="border:1px solid #ccc; padding-right:0">
					<uc1:News id="News1" runat="server"></uc1:News>
				</td>
			</tr>
		</table>
	</div>
	<div id="navigation">
		<uc1:SubtextSearch id="SubtextSearch1" runat="server"></uc1:SubtextSearch>
		<uc1:singlecolumn id="SingleColumn1" runat="server" />
		<div class="disclaimer">
			<div class="title">Disclaimer</div>
			<div class="laimer">
				The information in this weblog is provided "AS IS" 
				with no warranties, and confers no rights. This weblog 
				does not represent the thoughts, intentions, plans or 
				strategies of my employer. It is solely my opinion. 
				Inappropriate comments will be deleted at the authors 
				discretion. All code samples are provided "AS IS" without 
				warranty of any kind, either express or implied, including 
				but not limited to the implied warranties of merchantability 
				and/or fitness for a particular purpose.
			</div>
		</div>
		<br />
		<div align="center">
		<div class="disclaimer">
		<div class="title">Advertisement</div>
			<iframe src="http://rcm.amazon.com/e/cm?t=httpwwwanalys-20&o=1&p=14&l=bn1&mode=software&browse=491286&=1&fc1=&lt1=&lc1=&bg1=&f=ifr" marginwidth="0" marginheight="0" width="160" height="600" border="0" frameborder="0" style="border:none;" scrolling="no"></iframe>
		</div>
		</div>
	</div>
	<div id="content">
		<dt:contentregion id="MPMain" runat="server" />
	</div>
	<div id="footer">
		<uc1:footer id="Footer1" runat="server" />
		<div align="center" id="CCommon">
		<!--Creative Commons License-->
		<a rel="license" href="http://creativecommons.org/licenses/by-nd/2.1/au/">
		<img alt="Creative Commons License" border="0" src="http://creativecommons.org/images/public/somerights20.png"/>
		</a><br/>This work is licensed under a <br/><a rel="license" href="http://creativecommons.org/licenses/by-nd/2.1/au/">
		Creative Commons Attribution-NoDerivs 2.1 Australia License</a>.
		<!--/Creative Commons License--><!-- 
		<rdf:RDF xmlns="http://web.resource.org/cc/" xmlns:dc="http://purl.org/dc/elements/1.1/" xmlns:rdf="http://www.w3.org/1999/02/22-rdf-syntax-ns#">
		<Work rdf:about="">
		<license rdf:resource="http://creativecommons.org/licenses/by-nd/2.1/au/" />
		<dc:title>Blog</dc:title>
		<dc:date>2006</dc:date>
		<dc:description>Everything published on www.analystdeveloper.com</dc:description>
		<dc:creator><Agent><dc:title>Gurkan Yeniceri</dc:title></Agent></dc:creator>
		<dc:rights><Agent><dc:title>Gurkan Yeniceri</dc:title></Agent></dc:rights>
		<dc:type rdf:resource="http://purl.org/dc/dcmitype/Text" />
		<dc:source rdf:resource="www.analystdeveloper.com" />
		</Work>
		<License rdf:about="http://creativecommons.org/licenses/by-nd/2.1/au/"><permits rdf:resource="http://web.resource.org/cc/Reproduction"/><permits rdf:resource="http://web.resource.org/cc/Distribution"/><requires rdf:resource="http://web.resource.org/cc/Notice"/><requires rdf:resource="http://web.resource.org/cc/Attribution"/></License></rdf:RDF> -->
		</div>
	</div>
</div>