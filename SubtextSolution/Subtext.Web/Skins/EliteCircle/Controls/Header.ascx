<%@ Control Language="C#" EnableTheming="false" Inherits="Subtext.Web.UI.Controls.Header" %>
<%@ Register TagPrefix="st" TagName="MyLinks" Src="MyLinks.ascx" %>

<!-- header starts here -->
<div id="header">
	<div id="header-content">	
		<h1 id="logo"><asp:HyperLink id="blogTitleFirstWord" runat="server" Text="<%# Words(Title, 0, 1) %>" NavigateUrl="<%# HomeUrl %>" /><span class="orange"><asp:HyperLink id="blogTitleRestOfWords" runat="server" Text="<%# Words(Title, 1) %>" NavigateUrl="<%# HomeUrl %>" /></span></h1>	
		<h2 id="slogan"><asp:Literal id="blogSubtitle" runat="server" Text="<%# Subtitle %>" /></h2>		

		<st:MyLinks id="myLinks" runat="server" />

	</div>
</div>
<!-- header ends here -->
