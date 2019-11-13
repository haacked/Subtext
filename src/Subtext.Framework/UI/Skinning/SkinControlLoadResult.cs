#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Web.UI;

namespace Subtext.Framework.UI.Skinning
{
    public class SkinControlLoadResult
    {
        public SkinControlLoadResult(string controlPath, Control control, Exception exception)
        {
            SkinControl = control;
            ControlPath = controlPath;
            Exception = exception;
        }

        public Control SkinControl { get; private set; }
        public string ControlPath { get; private set; }
        public Exception Exception { get; private set; }
    }
}
