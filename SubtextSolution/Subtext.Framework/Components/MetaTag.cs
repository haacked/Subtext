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
using log4net;
using Subtext.Framework.Logging;

namespace Subtext.Framework.Components
{
	[Serializable]
	public class MetaTag
	{
		private readonly static ILog log = new Log();


		public MetaTag()
		{
		}

		public MetaTag(string content)
		{
			this.content = content;
		}

		#region Getters and Setters

		private int id;
		public int Id
		{
			get { return id; }
			set { id = value; }
		}

		private string content;
		public string Content
		{
			get { return content; }
			set { content = value; }
		}

		private string name;
		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		private string httpEquiv;
		public string HttpEquiv
		{
			get { return httpEquiv; }
			set { httpEquiv = value; }
		}

	    private int blogId;
        public int BlogId
	    {
	        get { return blogId; }
	        set { blogId = value; }
	    }

	    private int? entryId;
        public int? EntryId
	    {
	        get { return entryId; }
	        set { entryId = value; }
	    }

	    private DateTime dateCreated;
		public DateTime DateCreated
		{
			get { return dateCreated; }
			set { dateCreated = value; }
		}

	    #endregion

        /// <summary>
        /// Validates that this MetaTag is Valid:
        /// - Content must not be null nor empty
        /// - Must have either a name or http-equiv, but not both
        /// </summary>
        public bool IsValid
        {
            get
            {
                if (string.IsNullOrEmpty(this.Content))
                    return false;

                // to be valid, a MetaTag requires etiher the Name or HttpEquiv attribute, but never both.
                if (string.IsNullOrEmpty(this.Name) && string.IsNullOrEmpty(this.HttpEquiv))
                    return false;

                if (!string.IsNullOrEmpty(this.Name) && !string.IsNullOrEmpty(this.HttpEquiv))
                    return false;
                
                return true;
            }
        }
	}
}