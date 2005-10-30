<%@ Import Namespace = "Subtext.Framework" %>
<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.SingleColumn" %>
<%@ Register TagPrefix="uc1" TagName="CategoryList" Src="CategoryList.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Syn" Src="Syndacations.ascx" %>
<div id="links">
<uc1:CategoryList id="Categories" runat="server"></uc1:CategoryList>
<uc1:Syn id="links" runat="server" />

<div id="subtext">
	<p><a href="http://subtextproject.com/" title="Click here to visit the homepage of the SubText project"><img src="Images/PoweredBySubtext85x33.png" alt="Powered by SubText" /></a></p>
</div>
</div>