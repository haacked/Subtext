<%@ Import Namespace = "Subtext.Framework" %>
<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.SingleColumn" %>
<%@ Register TagPrefix="origami" TagName="CategoryList" Src="CategoryList.ascx" %>
<div id="links">
<origami:CategoryList id="Categories" runat="server"></origami:CategoryList>
</div>