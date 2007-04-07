<%@ Control Language="C#" EnableTheming="false"  inherits="Subtext.Web.UI.Controls.Calendar" %>

<div id="calendar">
<asp:calendar id="entryCal" runat="server" cellpadding="0"
	
	SelectionMode="None" 
	DayNameFormat="Short"
	FirstDayOfWeek="Sunday"
	TitleFormat="Month"

	DayHeaderStyle-CssClass="day-header"
	DayStyle-CssClass="day"
	NextPrevStyle-CssClass="next"
	TitleStyle-CssClass="title"
	TodayDayStyle-CssClass="today"
	OtherMonthDayStyle-CssClass="other-month"


	OnDayRender="entryCal_DayRender" 
	OnVisibleMonthChanged="entryCal_VisibleMonthChanged"
	>
</asp:calendar>
</div>