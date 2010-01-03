<%@ Page Language="C#" Title="Subtext - Host Admin - Queue" Codebehind="Queue.aspx.cs" Inherits="Subtext.Web.HostAdmin.Queue" %>

<asp:Content id="sectionTitle" ContentPlaceHolderID="MPSectionTitle" runat="server">Subtext - Host Admin - Queue</asp:Content>
<asp:Content id="sidebar" ContentPlaceHolderID="MPSideBar" runat="server"></asp:Content>
<asp:Content id="queueStats" ContentPlaceHolderID="MPContent" runat="server">
	<p>
		<label>Active Threads:</label>
		<asp:Literal id="ltlActiveThreads" runat="server"></asp:Literal>
	</p>
	<p>
		<label>Waiting Callbacks:</label>
		<asp:Literal id="ltlWaitingCallbacks" runat="server"></asp:Literal>
	</p>

</asp:Content>