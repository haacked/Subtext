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
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Text;
using Subtext.Framework.Util;

namespace Subtext.Web.Admin
{
	// -----------------------------------------------------------------------------------------
	// Abstract commands
	// -----------------------------------------------------------------------------------------

	#region ConfirmCommand
	[Serializable]
	public abstract class ConfirmCommand
	{
		protected const string DEFAULT_PROMPT = "Are you sure you want to do this?";
		protected const string DEFAULT_EXECUTE_SUCCESS = "Operation succeeded.";
		protected const string DEFAULT_EXECUTE_FAILURE = "Operation failed. Details: {1}";
		protected const string DEFAULT_CANCEL_SUCCESS = "Operation canceled.";
		protected const string DEFAULT_CANCEL_FAILURE = "Could not cancel operation. Details: {1}";
		
		protected string _promptMessage;
		protected string _executeSuccessMessage;
		protected string _executeFailureMessage; 
		protected string _cancelSuccessMessage; 
		protected string _cancelFailureMessage;

		protected bool _autoRedirect;
		protected string _redirectUrl;

		#region Accessors
		public virtual bool AutoRedirect
		{
			get { return _autoRedirect; }
			set { _autoRedirect = value; }
		}

		public virtual string RedirectUrl
		{
			get { return _redirectUrl; }
			set { _redirectUrl = value; }
		}

		public virtual string PromptMessage
		{
			get 
			{ 
				if (!Utilities.IsNullorEmpty(_promptMessage))
					return _promptMessage;
				else
					return DEFAULT_PROMPT;
			}
			set { _promptMessage = value; }
		}

		public virtual string ExecuteSuccessMessage
		{
			get 
			{ 
				if (!Utilities.IsNullorEmpty(_executeSuccessMessage))
					return _executeSuccessMessage;
				else
					return DEFAULT_EXECUTE_SUCCESS;
			}
			set { _executeSuccessMessage = value; }
		}

		public virtual string ExecuteFailureMessage
		{
			get 
			{ 
				if (!Utilities.IsNullorEmpty(_executeFailureMessage))
					return _executeFailureMessage;
				else
					return DEFAULT_EXECUTE_FAILURE;
			}
			set { _executeFailureMessage = value; }
		}

		public virtual string CancelSuccessMessage
		{
			get 
			{ 
				if (!Utilities.IsNullorEmpty(_cancelSuccessMessage))
					return _cancelSuccessMessage;
				else
					return DEFAULT_CANCEL_SUCCESS;
			}
			set { _cancelSuccessMessage = value; }
		}

		public virtual string CancelFailureMessage
		{
			get 
			{ 
				if (!Utilities.IsNullorEmpty(_cancelFailureMessage))
					return _cancelFailureMessage;
				else
					return DEFAULT_CANCEL_FAILURE;
			}
			set { _cancelFailureMessage = value; }
		}

		#endregion

		public virtual string FormatMessage(string message)
		{
			return FormatMessage(message, null);
		}

		/// <summary>
		/// Formats the message.
		/// </summary>
		/// <param name="format">Format.</param>
		/// <param name="args">Args.</param>
		/// <returns></returns>
		public virtual string FormatMessage(string format, params object[] args)
		{
			try
			{
				return string.Format(CultureInfo.InvariantCulture, format, args); 
			}
			catch (ArgumentNullException)
			{
				return format;
			}
		}

		/// <summary>
		/// Simply concatenates an array of integers for display as text 
		/// using commas.  ex... "1, 2, 3"
		/// </summary>
		/// <param name="integers">Integers.</param>
		/// <returns></returns>
		protected string GetDisplayTextFromIntArray(ICollection<int> integers)
		{
			if (integers == null || integers.Count == 0)
				return string.Empty;

			if(integers.Count == 2)
			{
				return integers.First() + " and " + integers.ElementAt(1);
			}

			string display = string.Empty;
			foreach(int integer in integers)
			{
				display += integer + ", ";
			}
			return display.Left(display.Length - 2);
		}

		public abstract string Cancel();
		public abstract string Execute();
	}
	#endregion

	#region DeleteTargetCommand		
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
		
		protected DeleteTargetCommand(int targetID) : this()
		{
			_targetID = targetID;
		}

		protected DeleteTargetCommand(string targetName, int targetID) : this()
		{
			_targetName = targetName;
			_targetID = targetID;
		}

		public override string PromptMessage
		{
			get 
			{
				return FormatMessage(base.PromptMessage, _targetName, _targetID); 
			}
			set { _promptMessage = value; }
		}

		public override string Cancel()
		{
			_autoRedirect = true;
			return FormatMessage(CancelSuccessMessage, _targetName, _targetID);
		}
	}
	#endregion

	#region DeleteTargetsCommand		
	/// <summary>
	/// Represents a command for deleting multiple targets.
	/// </summary>
	[Serializable]
	public abstract class DeleteTargetsCommand : ConfirmCommand
	{
		protected ICollection<int> _targetIDs;
		protected string _targetName = "Items";

		/// <summary>
		/// Creates a new <see cref="DeleteTargetsCommand"/> instance.
		/// </summary>
		protected DeleteTargetsCommand() 
		{
			_promptMessage = "Are you sure you want to delete {0} {1}?";
			_executeSuccessMessage = "{0} {1} were deleted.";
			_executeFailureMessage = "{0} {1} could not be deleted. Details: {2}";
			_cancelSuccessMessage = "{0} {1} will not be deleted.";
			_cancelFailureMessage = "Could not cancel deletion of {0} {1}. Details: {2}";		
		}
		
		/// <summary>
		/// Creates a new <see cref="DeleteTargetsCommand"/> instance.
		/// </summary>
		/// <param name="targetIDs">Target ID.</param>
		protected DeleteTargetsCommand(ICollection<int> targetIDs) : this()
		{
			_targetIDs = targetIDs;

			if(_targetIDs.Count == 1)
			{
				_targetName = "Item";
				_executeSuccessMessage = "{0} {1} was deleted.";
			}
		}

		/// <summary>
		/// Deletes the target command.
		/// </summary>
		/// <param name="targetName">Name of the target.</param>
		/// <param name="targetIDs">Target ID.</param>
		protected DeleteTargetsCommand(string targetName, int[] targetIDs) : this(targetIDs)
		{
			_targetName = targetName;
		}

		/// <summary>
		/// Gets or sets the prompt message.
		/// </summary>
		/// <value></value>
		public override string PromptMessage
		{
			get 
			{
				string ids = GetDisplayTextFromIntArray(_targetIDs);

				return FormatMessage(base.PromptMessage, _targetName, ids); 
			}
			set { _promptMessage = value; }
		}

		/// <summary>
		/// Cancels the command.
		/// </summary>
		/// <returns></returns>
		public override string Cancel()
		{
			_autoRedirect = true;
			return FormatMessage(CancelSuccessMessage, _targetName, _targetIDs);
		}
	}
	#endregion

	#region DeleteTitledTargetCommand		
	[Serializable]
	public abstract class DeleteTitledTargetCommand : DeleteTargetCommand
	{
		protected string itemTitle;

		protected DeleteTitledTargetCommand() 
		{
			_promptMessage = "Are you sure you want to delete {0} \"{1}\"?";
			_executeSuccessMessage = "{0} \"{1}\" was deleted.";
			_executeFailureMessage = "{0} \"{1}\" could not be deleted. Details: {2}";
			_cancelSuccessMessage = "{0} \"{1}\" will not be deleted.";
			_cancelFailureMessage = "Could not cancel deletion of {0} \"{1}\". Details: {2}";		
		}

		protected DeleteTitledTargetCommand(int targetID, string itemTitle) : this()
		{	
			_targetID = targetID;
		    this.itemTitle = itemTitle;
		}

		public override string PromptMessage
		{
			get 
			{
				if (!Utilities.IsNullorEmpty(_promptMessage))
					return FormatMessage(_promptMessage, _targetName, itemTitle); 
				else
					return base.PromptMessage;				
			}
			set { _promptMessage = value; }
		}

		public override string Cancel()
		{
			_autoRedirect = true;
			return FormatMessage(CancelSuccessMessage, _targetName, itemTitle);
		}
	}
	#endregion

	// -----------------------------------------------------------------------------------------
	// Concrete commands
	// -----------------------------------------------------------------------------------------

	#region DeletePostCommand		
	[Serializable]
	public class DeletePostCommand : DeleteTargetCommand
	{
		public DeletePostCommand(int postID)
		{
			_targetName = "Post";
			_targetID = postID;
		}

		public override string Execute()
		{
			try
			{
				Entries.Delete(_targetID);
				return FormatMessage(ExecuteSuccessMessage, _targetName, _targetID);
			}
			catch (Exception ex)
			{
				return FormatMessage(ExecuteFailureMessage, _targetName, _targetID, ex.Message);
			}
		}
	}
	#endregion

	#region DeleteComment		
	[Serializable]
	public class DeleteCommentCommand : DeleteTargetCommand
	{
		/// <summary>
		/// Creates a new <see cref="DeleteCommentCommand"/> instance.
		/// </summary>
		/// <param name="postID">Post ID.</param>
		public DeleteCommentCommand(int postID)
		{
			_targetName = "Feedback item";
			_targetID = postID;
		}

		/// <summary>
		/// Executes this instance.
		/// </summary>
		/// <returns></returns>
		public override string Execute()
		{
			try
			{
				Entries.Delete(_targetID);
				return FormatMessage(ExecuteSuccessMessage, _targetName, _targetID);
			}
			catch (Exception ex)
			{
				return FormatMessage(ExecuteFailureMessage, _targetName, _targetID, ex.Message);
			}
		}
	}
	#endregion

	#region DeleteComments
	/// <summary>
	/// Command to delete multiple comments.
	/// </summary>
	[Serializable]
	public class DeleteCommentsCommand : DeleteTargetsCommand
	{
		/// <summary>
		/// Creates a new <see cref="DeleteCommentsCommand"/> instance.
		/// </summary>
		/// <param name="postIDs">Post IDs.</param>
		public DeleteCommentsCommand(ICollection<int> postIDs) : base(postIDs)
		{
			if(postIDs.Count > 1)
			{
				_targetName = "Feedback items";
			}
			else
			{
				_targetName = "Feedback item";
			}
			_targetIDs = postIDs;
		}

		/// <summary>
		/// Executes this command.
		/// </summary>
		/// <returns></returns>
		public override string Execute()
		{
			string ids = GetDisplayTextFromIntArray(_targetIDs);
			try
			{
				foreach(int targetId in _targetIDs)
				{
					Entries.Delete(targetId);
				}
				
				return FormatMessage(ExecuteSuccessMessage, _targetName, ids);
			}
			catch (Exception ex)
			{
				return FormatMessage(ExecuteFailureMessage, _targetName, ids, ex.Message);
			}
		}
	}
	#endregion

	#region DeleteCategoryCommand		
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
			try
			{
				Links.DeleteLinkCategory(_targetID);
				return FormatMessage(ExecuteSuccessMessage, _targetName, itemTitle);
			}
			catch (Exception ex)
			{
				return FormatMessage(ExecuteFailureMessage, _targetName, itemTitle, ex.Message);
			}
		}
	}
	#endregion

	#region DeleteGalleryCommand		
	// TODO: derivation is fine, but expose the prompts so you can add a WARNING WILL DELETE 
	// FILES TOO message for this one (makes general sense as well)
	[Serializable]
	public class DeleteGalleryCommand : DeleteTitledTargetCommand
	{		
		public DeleteGalleryCommand(int galleryID, string galleryTitle)
		{	
			_targetID = galleryID;
		    itemTitle = galleryTitle;
		}

		public override string Execute()
		{
			try
			{
				ICollection<Image> imageList = Images.GetImagesByCategoryID(_targetID, false);
				
				// delete the folder
				string galleryFolder = Images.LocalGalleryFilePath(_targetID);
				if (Directory.Exists(galleryFolder))
					Directory.Delete(galleryFolder, true);

				if (imageList.Count > 0)
				{
					// delete from data provider
					foreach (Image currentImage in imageList)
					{
						Images.DeleteImage(currentImage);
					}					
				}

				// finally, delete the gallery (category) itself from the data provider
				Links.DeleteLinkCategory(_targetID);
				return FormatMessage(ExecuteSuccessMessage, _targetName, itemTitle);
			}
			catch (Exception ex)
			{
				return FormatMessage(ExecuteFailureMessage, _targetName, _targetID, ex.Message);
			}
		}
	}
	#endregion

	#region DeleteKeyWordCommand		
	[Serializable]
	public class DeleteKeyWordCommand : DeleteTitledTargetCommand
	{
		public DeleteKeyWordCommand(int keyWordID, string word)
		{	
			_autoRedirect = true;
			_targetName = "KeyWord";
			_targetID = keyWordID;
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
	#endregion

	#region DeleteLinkCommand		
	[Serializable]
	public class DeleteLinkCommand : DeleteTitledTargetCommand
	{
		public DeleteLinkCommand(int linkID, string linkTitle)
		{	
			_autoRedirect = true;
			_targetName = "Link";
			_targetID = linkID;
		    itemTitle = linkTitle;
		}

		public override string Execute()
		{
			try
			{
				Links.DeleteLink(_targetID);
				return FormatMessage(ExecuteSuccessMessage, _targetName, itemTitle);
			}
			catch (Exception ex)
			{
				return FormatMessage(ExecuteFailureMessage, _targetName, itemTitle, ex.Message);
			}
		}
	}
	#endregion

	#region DeleteImageCommand
	[Serializable]
	public class DeleteImageCommand : DeleteTitledTargetCommand
	{		
		public DeleteImageCommand(int imageID)
		{	
			_targetName = "Image";
			_targetID = imageID;
		    itemTitle = "Image " + imageID.ToString(CultureInfo.InvariantCulture);
		}

		public DeleteImageCommand(int imageID, string imageTitle)
		{	
			_targetName = "Image";
			_targetID = imageID;
		    itemTitle = imageTitle;
		}

		public override string Execute()
		{
			try
			{
				Image currentImage = Images.GetSingleImage(_targetID, false);

				// The following line should be fully encapsulated and handle files + data
				// For now, I commented out the file trys in the the object so it can do just
				// data without exception. I'll do the files locally until we decide to really
				// do the framework class
				
				Images.DeleteImage(currentImage);

				// now delete the associated files if they exist
				string galleryFolder = Images.LocalGalleryFilePath(currentImage.CategoryID);
				if (Directory.Exists(galleryFolder))
				{
					DeleteFile(galleryFolder, currentImage.OriginalFile);
					DeleteFile(galleryFolder, currentImage.ResizedFile);
					DeleteFile(galleryFolder, currentImage.ThumbNailFile);
				}

				return FormatMessage(ExecuteSuccessMessage, _targetName, itemTitle);
			}
			catch (Exception ex)
			{
				return FormatMessage(ExecuteFailureMessage, _targetName, _targetID, ex.Message);
			}
		}

		private void DeleteFile(string path, string filename)
		{
			string localPath = Path.Combine(path, filename);
			if (File.Exists(localPath))
			{
				File.Delete(localPath);
			}
		}
	}
	#endregion

	#region ImportLinksCommand
	[Serializable]
	public class ImportLinksCommand : ConfirmCommand
	{		
		protected OpmlItemCollection _linksToImport;
		protected ICollection<Link> _allLinks;
		protected int _categoryID = NullValue.NullInt32;

		protected ImportLinksCommand() 
		{
			_promptMessage = "A total of {0} links were found in your file.<p/>Any existing links with the same url will be overwritten.<p/>Are you sure you want to import these links?";
			_executeSuccessMessage = "A total of {0} links were successfully imported.";
			_executeFailureMessage = "The import failed. Details: {0}";
			_cancelSuccessMessage = "These link import operation was canceled.";
			_cancelFailureMessage = "Could not cancel link import. Details: {0}";		
		}

		public ImportLinksCommand(OpmlItemCollection links, int catID)
			: this()
		{	
			_linksToImport = links;
			_categoryID = catID;

		}

		public override string PromptMessage
		{
			get 
			{
				if (!Utilities.IsNullorEmpty(_promptMessage))
					return FormatMessage(_promptMessage, _linksToImport.Count); 
				else
					return base.PromptMessage;				
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
					ImportOpmlItem(item);

				return FormatMessage(ExecuteSuccessMessage, _linksToImport.Count);
			}
			catch (Exception ex)
			{
				return FormatMessage(ExecuteFailureMessage, ex.Message);
			}
		}

		public override string Cancel()
		{
			_autoRedirect = false;
			return FormatMessage(CancelSuccessMessage);
		}

		private void ImportOpmlItem(OpmlItem item)
		{
			foreach (OpmlItem childItem in item.ChildItems)
				ImportOpmlItem(childItem);			

			Link newLink = new Link();
			newLink.Title = item.Title;
			newLink.Url = item.HtmlUrl;
			newLink.Rss = item.XmlUrl;
			newLink.CategoryID = _categoryID;

			// TODO: let user specify and pass as command props
			newLink.IsActive = true;
			newLink.NewWindow = false;

			// this isn't a valid collision test really
			if (!_allLinks.Contains(newLink))
				Links.CreateLink(newLink);
		}
	}
	#endregion

}

