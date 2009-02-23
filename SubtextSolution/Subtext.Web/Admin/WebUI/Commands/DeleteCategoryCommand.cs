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
using Subtext.Framework;

namespace Subtext.Web.Admin.Commands
{
    [Serializable]
    public class DeleteCategoryCommand : DeleteTitledTargetCommand
    {
        public DeleteCategoryCommand(int categoryID, string categoryTitle)
        {
            _targetName = "Category";
            _targetID = categoryID;
            itemTitle = categoryTitle;
        }

        public override string Execute()
        {
            try {
                Links.DeleteLinkCategory(_targetID);
                return FormatMessage(ExecuteSuccessMessage, _targetName, itemTitle);
            }
            catch (Exception ex)
            {
                return FormatMessage(ExecuteFailureMessage, _targetName, itemTitle, ex.Message);
            }
        }
    }
}