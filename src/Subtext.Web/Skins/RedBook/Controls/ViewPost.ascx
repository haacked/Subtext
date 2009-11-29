<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.ViewPost" %>
<%@ Register TagPrefix="uc1" TagName="PreviousNext" Src="PreviousNext.ascx" %>

<uc1:PreviousNext id="PreviousNext" runat="server" />

<div class="journal_eintrag">
	<h2><asp:HyperLink Runat="server" ID="editLink" /><asp:HyperLink  CssClass="singleposttitle" Runat="server" ID="TitleUrl" /></h2>
	<asp:Literal id="Body"  runat="server" />

		<p class="enclosure">
		    <asp:Label id="Enclosure"  runat="server" DisplaySize="True" />
		</p>

		<p class="postfooter">
			<a href="javascript:window.print();" class="printIcon"><span>Print</span></a>
			posted @ <asp:Literal id="PostDescription"  runat="server" />
		</p>
		</div>
	<asp:Literal ID = "TrackBack" Runat="server" />