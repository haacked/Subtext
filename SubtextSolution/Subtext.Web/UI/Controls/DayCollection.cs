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
using System.Collections.Generic;
using Subtext.Framework.Components;

namespace Subtext.Web.UI.Controls
{
	/// <summary>
	/// Summary description for LastSevenDaysControl.
	/// </summary>
	public class DayCollection : BaseControl
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DayCollection"/> class.
		/// </summary>
		public DayCollection()
		{
		}

		protected System.Web.UI.WebControls.Repeater DaysList;

        private IList<EntryDay> bpdc;

        public IList<EntryDay> Days
		{
			set{bpdc = value;}
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);

			if(bpdc != null)
			{
				DaysList.DataSource = bpdc;
				DaysList.DataBind();
			}
			else
			{
				this.Visible = false;
			}
		}
	}
}

