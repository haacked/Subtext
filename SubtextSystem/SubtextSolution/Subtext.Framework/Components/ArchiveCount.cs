using System;

namespace Subtext.Framework.Components
{
	/// <summary>
	/// Summary description for ArchiveCount.
	/// </summary>
	[Serializable]
	public class ArchiveCount
	{
		public ArchiveCount()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		private DateTime _date;
		public DateTime Date
		{
			get {return this._date;}
			set {this._date = value;}
		}

		private int _count;
		public int Count
		{
			get {return this._count;}
			set {this._count = value;}
		}
	}
}
