#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at SourceForge at http://sourceforge.net/projects/subtext
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

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

        private String _title;
        public String Title
        {
            get { return _title; }
            set { _title = value; }
        }

        private int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; }
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
