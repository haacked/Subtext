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
using Subtext.Framework.Providers;

namespace Subtext.Web.Admin.Commands
{
    [Serializable]
    public class DeleteCategoryCommand : DeleteTitledTargetCommand
    {
        public DeleteCategoryCommand(ObjectRepository repository, int categoryID, string categoryTitle)
        {
            _targetName = "Category";
            _targetID = categoryID;
            itemTitle = categoryTitle;
            Repository = repository;
        }

        public ObjectRepository Repository { get; private set; }

        public override string Execute()
        {
            try
            {
                Repository.DeleteLinkCategory(_targetID);
                return FormatMessage(ExecuteSuccessMessage, _targetName, itemTitle);
            }
            catch (Exception ex)
            {
                return FormatMessage(ExecuteFailureMessage, _targetName, itemTitle, ex.Message);
            }
        }
    }
}