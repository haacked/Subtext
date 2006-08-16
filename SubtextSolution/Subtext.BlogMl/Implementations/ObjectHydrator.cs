using System;
using System.Data;
using System.Globalization;
using BlogML;
using Subtext.BlogMl.Interfaces;
using Subtext.Framework.Components;
using Subtext.Framework.Data;

namespace Subtext.BlogMl.Implementations
{
	/// <summary>
	/// Class used to hydrade blogml objects from a data reader.
	/// </summary>
	public static class ObjectHydrator
	{
		/// <summary>
		/// Loads the post from data reader.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <returns></returns>
		public static IBlogMlPost LoadPostFromDataReader(IDataReader reader)
		{
			Entry entry = DataHelper.LoadEntry(reader);
			return new BlogMlPost(entry.Id.ToString(CultureInfo.InvariantCulture), entry.Title, ContentTypes.Xhtml, entry.FullyQualifiedUrl.ToString(), entry.IsActive, entry.Body, entry.DateCreated, entry.DateUpdated);
		}

		/// <summary>
		/// Loads the comment from data reader.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <returns></returns>
		public static IBlogMlComment LoadCommentFromDataReader(IDataReader reader)
		{
			Entry entry = DataHelper.LoadEntry(reader);
			return new BlogMlComment(entry.Id.ToString(CultureInfo.InvariantCulture), entry.Title, ContentTypes.Xhtml, entry.FullyQualifiedUrl.ToString(), entry.Body, ContentTypes.Xhtml, entry.Email, entry.Author, true, entry.DateCreated, entry.DateUpdated);
		}

		/// <summary>
		/// Loads the trackback from data reader.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <returns></returns>
		public static IBlogMlTrackback LoadTrackbackFromDataReader(IDataReader reader)
		{
			Entry entry = DataHelper.LoadEntry(reader);
			return new BlogMlTrackback(entry.Id.ToString(CultureInfo.InvariantCulture), entry.Title, ContentTypes.Xhtml, entry.FullyQualifiedUrl.ToString(), true, entry.DateCreated, entry.DateUpdated);
		}
	}
}