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
using Subtext.Framework.Util;

namespace Subtext.Web.Admin.Commands
{
    [Serializable]
    public class DeleteKeyWordCommand : DeleteTitledTargetCommand
    {
        public DeleteKeyWordCommand(int keyWordId, string word)
        {
            AutoRedirect = true;
            _targetName = "KeyWord";
            _targetID = keyWordId;
            itemTitle = word;
        }

        public override string Execute()
        {
            try
            {
                KeyWords.DeleteKeyWord(_targetID);
                return FormatMessage(ExecuteSuccessMessage, _targetName, itemTitle);
            }
            catch (Exception ex)
            {
                return FormatMessage(ExecuteFailureMessage, _targetName, itemTitle, ex.Message);
            }
        }
    }
}