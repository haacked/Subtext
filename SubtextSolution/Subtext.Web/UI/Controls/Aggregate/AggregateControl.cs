namespace Subtext.Web.UI.Controls.Aggregate
{
    public class AggregateControl : BaseControl
    {
        public int? GetGroupIdFromQueryString() {
            AggregatePage page = Page as AggregatePage;
            if (page != null) {
                return page.GetGroupIdFromQueryString();
            }
            return null;
        }
    }
}
