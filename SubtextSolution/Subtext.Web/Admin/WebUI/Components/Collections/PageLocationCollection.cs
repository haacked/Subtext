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
using System.Collections;
using System.Collections.Generic;

namespace Subtext.Web.Admin
{
	/// <summary>
	/// Implements a strongly typed collection of <see cref="PageLocation"/> elements.
	/// </summary>
	/// <remarks>
	/// <b>PageLocationCollection</b> provides an <see cref="ArrayList"/> 
	/// that is strongly typed for <see cref="PageLocation"/> elements.
	/// </remarks>
    [Serializable]
    public class PageLocationCollection : List<PageLocation>
    {
    }
}

