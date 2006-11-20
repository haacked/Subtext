using System;
using System.Collections.Generic;

namespace Subtext.BlogML.Conversion
{
	public abstract class IdConversionStrategy
	{
		public static readonly IdConversionStrategy Empty = new NullConversionStrategy();
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
        public const string Authors = "authors";
	}
}
