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
using System.Text;

namespace Subtext.Extensibility.Plugins
{
	public class STEventArgs: EventArgs
	{
		private ObjectState _state;

		public ObjectState State
		{
			get{ return _state;}
		}

		public STEventArgs(ObjectState state)
		{
			_state = state;
		}

		public STEventArgs() : this(ObjectState.None) { }

	}

	public enum ObjectState
	{
		Create,
		Update,
		Delete,
		None,
		Runtime
	}
}
