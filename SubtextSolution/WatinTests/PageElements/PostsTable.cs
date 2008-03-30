using System;
using WatiN.Core;
using WatiN.Core.Interfaces;

namespace WatinTests.PageElements
{
	public class PostsTable : AdminTable<PostRow>
	{
		public PostsTable(IElementsContainer browser, string id) : base(browser, id)
		{
		}

		public override PostRow CreateRow(TableRow row)
		{
			return new PostRow(row);
		}

		public PostRow FindRowByDescription(string text)
		{
			PostRow foundRow = this.TableRows.Find(delegate(PostRow row)
			                    	{
			                    		return (row.DescriptionLink.Text == text);
			                    	});
			return foundRow;
		}
	}

	public class PostRow : AdminTableRow
	{
		public PostRow(TableRow row) : base(row)
		{
		}

		public Link DescriptionLink
		{
			get { return GetLink(0); }
		}

		public bool Active
		{
			get { return GetBool(1); }
		}

		public int WebViews
		{
			get { return GetInt(2);}
		}

		public int AggViews
		{
			get { return GetInt(3); }
		}

		public Link ReferralsLink
		{
			get { return GetLink(4); }
		}

		public Link EditLink
		{
			get { return GetLink(5); }
		}

		public Link DeleteLink
		{
			get { return GetLink(6); }
		}
	}
}
