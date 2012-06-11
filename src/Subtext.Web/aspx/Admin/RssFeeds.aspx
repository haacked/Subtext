<%@ Page Language="C#" EnableTheming="false"  Title="Subtext Admin - Rss Feed Generator"  Codebehind="RssFeeds.aspx.cs" AutoEventWireup="True" Inherits="Subtext.Web.Admin.Pages.RssFeeds" %>

<asp:Content ContentPlaceHolderID="actionsHeading" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="categoryListHeading" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="categoryListLinks" runat="server">
</asp:Content>

<asp:Content ID="referrersContent" ContentPlaceHolderID="pageContent" runat="server">   
<script type="text/javascript">
	function UpdateForm()
	{
		var page = '<%= AdminUrl.Rss() %>';
		var type = document.getElementById("selType").value;
		var count = document.getElementById("txtCount").value;
		var title = document.getElementById("txtTitle").value;
		
		if(type.length==0)
		{
			document.getElementById('urlText').innerText = "Select a feed type.";
			return;
		}
		var filter = "";
		if(type=="Comment")
		{
			document.getElementById("trCommentFilter").style.display="";
			if(document.getElementById('cbFilterNeedsApproval').checked)
				filter = AddFilter(filter,"NeedsApproval");
			if(document.getElementById('cbFilterSpam').checked)
				filter = AddFilter(filter,"Spam");
		}else
		{
			document.getElementById("trCommentFilter").style.display="none";
		}
			
		page += "?Type=" + type;
		
		if(count.length>0)
			page += "&Count=" + count;
		
		if(title.length>0)
			page += "&Title=" + title;
		    
		page += filter;
		document.getElementById('urlText').innerHTML = page;
}
	
	function AddFilter(filter, value)
	{
		if(filter.length==0)
		{
			filter = "&Filter=" + value;
		}
		else
		{
			filter += escape("+" + value);
		}
		return filter;
	}
</script>

	Feed Url:<br />
	<div id="urlText" >Select a feed type.</div>
	<hr />
	<table>
	<tr><td>Type:</td><td><select id="selType" onchange="UpdateForm();">
		<option value=""></option>
		<option value="Comment">Comment</option>
		<option value="Referral">Referral</option>
		<option value="Log">Log</option>
	</select></td>
	</tr>
	<tr>
		<td>Count:</td>
		<td><input id="txtCount" onchange="UpdateForm();" /></td>
	</tr>
	<tr>
		<td>Title:</td>
		<td><input id="txtTitle" onchange="UpdateForm();" /></td>
	</tr>
	<tr id="trCommentFilter" style="display:none;">
		<td>Filters:</td>
		<td>
			<input id="cbFilterNeedsApproval" onclick="UpdateForm();" type="checkbox" value="NeedsApproval" />Needs Approval
			<input type="checkbox" id="cbFilterSpam" onclick="UpdateForm();" value="Spam"   />Spam
		</td>
	</tr>
	</table>
	
	<script type="text/javascript">
		document.getElementById("selType").value = '';
	</script>
</asp:Content>