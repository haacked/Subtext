using System;

namespace Subtext.Framework.Components
{
	/// <summary>
	/// Represents a trackback within this system. This is essentially 
	/// a comment created via the Trackback/Pingback API.
	/// </summary>
	public class Trackback : Entry
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Trackback"/> class.
		/// </summary>
		/// <param name="parentId">The parent id.</param>
		/// <param name="title">The title.</param>
		/// <param name="titleUrl">The title URL.</param>
		/// <param name="author">The author.</param>
		/// <param name="body">The body.</param>
		public Trackback(int parentId, string title, string titleUrl, string author, string body) : this(parentId, title, titleUrl, author, body, DateTime.Now)
		{
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Trackback"/> class.
		/// </summary>
		/// <param name="parentId">The parent id.</param>
		/// <param name="title">The title.</param>
		/// <param name="titleUrl">The title URL.</param>
		/// <param name="author">The author.</param>
		/// <param name="body">The body.</param>
		/// <param name="dateCreated">The date created.</param>
		public Trackback(int parentId, string title, string titleUrl, string author, string body, DateTime dateCreated) : base(Subtext.Extensibility.PostType.PingTrack)
		{
			ParentID = parentId;
			Title = title;
			AlternativeTitleUrl = titleUrl;
			Author = author;
			Body = body;
			
			IsActive = true;
			DateCreated = DateUpdated = dateCreated;
		}
	}
}
