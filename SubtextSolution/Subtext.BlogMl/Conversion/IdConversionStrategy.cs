using System;
using System.Collections.Generic;

namespace Subtext.BlogML.Conversion
{
	public abstract class IdConversionStrategy
	{
		public static IdConversionStrategy Empty = new NullConversionStrategy();
		Dictionary<ScopedId, string> idMap = new Dictionary<ScopedId, string>();

		/// <summary>
		/// Converts the specified id into a new Id.  If we've already converted this id, 
		/// then returns the original conversion.  This method is for creating outgoing 
		/// ids.
		/// </summary>
		/// <param name="scope">The scope for the id.  For example, for an auto-incrementing 
		/// int id in a table, this might be the table name.  This is how we can distinguish 
		/// between two ids that might be the same.</param>
		/// <param name="originalId"></param>
		/// <returns></returns>
		public string GetConvertedId(string scope, string originalId)
		{
			ScopedId scopedId = new ScopedId(scope, originalId);
			
			string convertedId;
			if(this.idMap.TryGetValue(scopedId, out convertedId))
				return convertedId;

			convertedId = Generate(scope, originalId);
			this.idMap.Add(scopedId, convertedId);
			return convertedId;
		}
		
		/// <summary>
		/// This method is used when importing. It provides a means to 
		/// tell the conversion that we now have an internal id to map 
		/// to the blogml id.
		/// </summary>
		/// <remarks>
		/// When importing a category, you might receive an id from blogml like "abcd-efg".  
		/// When you create a Category record in your system, you might have an ID of "3134".  
		/// Call this method to tell the conversion that from now on, "abcd-efg" is mapped 
		/// to "3134".
		/// </remarks>
		/// <param name="scope"></param>
		/// <param name="originalId"></param>
		/// <param name="newId"></param>
		/// <returns></returns>
		public string MapConvertedIdToImportedId(string scope, string originalId, string newId)
		{
			ScopedId scopedId = new ScopedId(scope, originalId);
			if(!idMap.ContainsKey(scopedId))
				idMap[scopedId] = newId;
			return newId;
		}
		
		/// <summary>
		/// Generates a new id for the specified scope.  The originalId may be ignored if 
		/// not needed as a "seed" value.
		/// </summary>
		/// <param name="scope">The scope for the id.  For example, for an auto-incrementing 
		/// int id in a table, this might be the table name.  This is how we can distinguish 
		/// between two ids that might be the same.</param>
		/// <param name="originalId">This can be used as the seed for the generated id.</param>
		/// <returns></returns>
		protected abstract string Generate(string scope, string originalId);
		
		struct ScopedId
		{
			internal ScopedId(string scope, string id)
			{
				this.Scope = scope;
				this.Id = id;
			}

			public readonly string Id;
			public readonly string Scope;
		}
	}
	
	public static class IdScopes
	{
		public const string Posts = "posts";
		public const string Categories = "categories";
		public const string CategoryParents = "categoryparents";
		public const string Comments = "comments";
		public const string TrackBacks = "trackbacks";
	}
}
