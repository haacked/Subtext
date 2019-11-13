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
    public sealed class DeleteKeyWordCommand : DeleteTitledTargetCommand
    {
        ObjectRepository _repository;

        public DeleteKeyWordCommand(ObjectRepository repository, int keyWordId, string word)
        {
            AutoRedirect = true;
            _targetName = "KeyWord";
            _targetID = keyWordId;
            itemTitle = word;
            _repository = repository;
        }

        public override string Execute()
        {
            try
            {
                _repository.DeleteKeyWord(_targetID);
                return FormatMessage(ExecuteSuccessMessage, _targetName, itemTitle);
            }
            catch (Exception ex)
            {
                return FormatMessage(ExecuteFailureMessage, _targetName, itemTitle, ex.Message);
            }
        }
    }
}