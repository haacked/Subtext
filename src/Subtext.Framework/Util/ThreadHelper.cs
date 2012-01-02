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
using System.Threading;
using log4net;
using Subtext.Framework.Logging;

namespace Subtext.Framework.Util
{
    public static class ThreadHelper
    {
        private readonly static ILog Log = new Log();

        public static bool FireAndForget(WaitCallback callback, string failureLogMessage)
        {
            return ThreadPool.QueueUserWorkItem(o =>
            {
                try
                {
                    callback(o);
                }
                catch (Exception e)
                {
                    Log.Error(failureLogMessage, e);
                }
            });
        }
    }
}
