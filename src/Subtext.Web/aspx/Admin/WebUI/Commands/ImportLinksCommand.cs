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
using System.Collections.Generic;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Providers;

namespace Subtext.Web.Admin.Commands
{
    [Serializable]
    public class ImportLinksCommand : ConfirmCommand
    {
        protected ICollection<Link> _allLinks;
        protected int _categoryID = NullValue.NullInt32;
        protected OpmlItemCollection _linksToImport;

        protected ImportLinksCommand()
        {
            _promptMessage =
                "A total of {0} links were found in your file.<p/>Any existing links with the same url will be overwritten.<p/>Are you sure you want to import these links?";
            _executeSuccessMessage = "A total of {0} links were successfully imported.";
            _executeFailureMessage = "The import failed. Details: {0}";
            _cancelSuccessMessage = "These link import operation was canceled.";
            _cancelFailureMessage = "Could not cancel link import. Details: {0}";
        }

        public ImportLinksCommand(ObjectRepository repository, OpmlItemCollection links, int catID)
            : this()
        {
            _linksToImport = links;
            _categoryID = catID;
            Repository = repository;
        }

        public ObjectRepository Repository { get; private set; }

        public override string PromptMessage
        {
            get
            {
                if (!Utilities.IsNullorEmpty(_promptMessage))
                {
                    return FormatMessage(_promptMessage, _linksToImport.Count);
                }
                else
                {
                    return base.PromptMessage;
                }
            }
            set { _promptMessage = value; }
        }

        public override string Execute()
        {
            try
            {
                // we could do this in the provider or, better yet, just make a get all links method
                //				PagedLinkCollection allLinks = Links.GetPagedLinks(1, 1);
                //				_allLinks = Links.GetPagedLinks(1, allLinks.MaxItems);

                // process import collection
                foreach (OpmlItem item in _linksToImport)
                {
                    ImportOpmlItem(item);
                }

                return FormatMessage(ExecuteSuccessMessage, _linksToImport.Count);
            }
            catch (Exception ex)
            {
                return FormatMessage(ExecuteFailureMessage, ex.Message);
            }
        }

        public override string Cancel()
        {
            AutoRedirect = false;
            return FormatMessage(CancelSuccessMessage);
        }

        private void ImportOpmlItem(OpmlItem item)
        {
            foreach (OpmlItem childItem in item.ChildItems)
            {
                ImportOpmlItem(childItem);
            }

            var newLink = new Link
            {
                Title = item.Title,
                Url = item.HtmlUrl,
                Rss = item.XmlUrl,
                CategoryId = _categoryID,
                IsActive = true,
                NewWindow = false
            };

            // TODO: let user specify and pass as command props

            // this isn't a valid collision test really
            if (!_allLinks.Contains(newLink))
            {
                Repository.CreateLink(newLink);
            }
        }
    }
}