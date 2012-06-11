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

namespace Subtext.Web.Admin.Commands
{
    [Serializable]
    public abstract class DeleteTargetCommand : ConfirmCommand
    {
        protected int _targetID;
        protected string _targetName = "Item";

        protected DeleteTargetCommand()
        {
            _promptMessage = "Are you sure you want to delete {0} {1}?";
            _executeSuccessMessage = "{0} {1} was deleted.";
            _executeFailureMessage = "{0} {1} could not be deleted. Details: {2}";
            _cancelSuccessMessage = "{0} {1} will not be deleted.";
            _cancelFailureMessage = "Could not cancel deletion of {0} {1}. Details: {2}";
        }

        protected DeleteTargetCommand(int targetID)
            : this()
        {
            _targetID = targetID;
        }

        protected DeleteTargetCommand(string targetName, int targetID)
            : this()
        {
            _targetName = targetName;
            _targetID = targetID;
        }

        public override string PromptMessage
        {
            get { return FormatMessage(base.PromptMessage, _targetName, _targetID); }
            set { _promptMessage = value; }
        }

        public override string Cancel()
        {
            AutoRedirect = true;
            return FormatMessage(CancelSuccessMessage, _targetName, _targetID);
        }
    }
}