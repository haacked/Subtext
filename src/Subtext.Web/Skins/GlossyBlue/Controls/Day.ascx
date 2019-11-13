<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.Day" %>

<asp:HyperLink Runat="server" height="15" Width="250" BorderWidth="0" ID="ImageLink" Visible="false">
<asp:Literal ID = "DateTitle" Runat = "server" Visible="false" /></asp:HyperLink>		  
<asp:Repeater runat="Server" Runat="server" ID="DayList" OnItemCreated="PostCreated">
	<ItemTemplate>
	    <div class="post">
	        <div class="post-date"><span class="post-month"><asp:Label ID="postDate" CssClass="postTitleDate" runat="server" Format="MMM" /></span> <span class="post-day"></span></div>
	        <div class="entry">
	            <h2><asp:HyperLink Runat="server" ID="editLink" />  <asp:HyperLink Runat="server" ID="editInWlwLink" />  <asp:HyperLink  CssClass="posttitle" Runat="server" ID="TitleUrl" /></h2>        
	        </div>
	    </div>
	    <span class="post-comments"><asp:Label id="commentCount" runat="server" /> | <asp:Label id="permalink" runat="server" Format="MMM dd, yyyy" /></span>
	    <div class="post-content"><asp:Literal  runat="server" ID="PostText" /></div>        
	</ItemTemplate>
</asp:Repeater>