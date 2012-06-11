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
using Subtext.Framework.Services.SearchEngine;

namespace Subtext.Web.Admin.Commands
{
    [Serializable]
    public class DeletePostCommand : DeleteTargetCommand
    {
        public DeletePostCommand(ObjectRepository repository, int postID, ISearchEngineService searchEngineService)
        {
            _targetName = "Post";
            _targetID = postID;
            Repository = repository;
            SearchEngine = searchEngineService;
        }

        public ObjectRepository Repository { get; private set; }
        public ISearchEngineService SearchEngine { get; private set; }

        public override string Execute()
        {
            try
            {
                Repository.DeleteEntry(_targetID);
                SearchEngine.RemovePost(_targetID);
                return FormatMessage(ExecuteSuccessMessage, _targetName, _targetID);
            }
            catch (Exception ex)
            {
                return FormatMessage(ExecuteFailureMessage, _targetName, _targetID, ex.Message);
            }
        }
    }
}