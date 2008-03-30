using System;
using System.Collections.Generic;
using WatiN.Core;
using WatiN.Core.Interfaces;

namespace WatinTests.PageElements
{
	/// <summary>
	/// Represents a table in the admin section. 
	/// Since these tables have a pretty common setup.
	/// </summary>
	public abstract class AdminTable<T> where T : AdminTableRow
	{
		private readonly Table table;
		private readonly AdminTableRowCollection<T> rows;
		private T headerRow;

		public AdminTable(IElementsContainer browser, string id)
		{
			this.table = browser.Table(Find.ById(id));
			this.rows = new AdminTableRowCollection<T>(this, this.table.TableRows);
		}

		public T HeaderRow
		{
			get
			{
				if(headerRow == null)
					headerRow = this.CreateRow(this.table.TableRows[0]);
				return headerRow;
			}
		}
		
		public AdminTableRowCollection<T> TableRows
		{
			get
			{
				return rows;
			}
		}

		public abstract T CreateRow(TableRow row);
	}

	public class AdminTableRowCollection<T> : List<T> where T : AdminTableRow
	{
		private readonly TableRowCollection rows;
		private readonly AdminTable<T> table;

		public AdminTableRowCollection(AdminTable<T> table, TableRowCollection rows)
		{
			this.table = table;
			this.rows = rows;
			foreach(TableRow row in rows)
			{
				Add(table.CreateRow(row));
			}
			RemoveAt(0);
			
		}

		public AdminTableRowCollection<T> Filter(AttributeConstraint findBy)
		{
			return new AdminTableRowCollection<T>(this.table, rows.Filter(findBy));
		}
	}

	public class AdminTableRow
	{
		private TableRow row;
		
		public AdminTableRow(TableRow row)
		{
			this.row = row;
		}

		protected TableRow Row
		{
			get { return this.row; }
		}

		public string InnerHtml
		{
			get
			{
				return this.row.InnerHtml;
			}
		}

		public bool Exists
		{
			get
			{
				return this.row != null && this.row.Exists;
			}
		}

		protected Link GetLink(int columnIndex)
		{
			return Row.TableCells[columnIndex].Links[0];
		}

		protected int GetInt(int columnIndex)
		{
			return int.Parse(GetInnerHtml(columnIndex));
		}

		protected bool GetBool(int columnIndex)
		{
			return bool.Parse(GetInnerHtml(columnIndex));
		}

		protected string GetInnerHtml(int columnIndex)
		{
			return Row.TableCells[columnIndex].InnerHtml;
		}
	}
}