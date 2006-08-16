using System;

namespace Subtext.BlogMl.Conversion
{
	public class SubtextConversionStrategy : IntConversionStrategy
	{
		/// <summary>
		/// Generates a new int id for the specified scope starting at 1.  The originalId is ignored.
		/// </summary>
		/// <param name="scope">The scope for the id.  For example, for an auto-incrementing 
		/// int id in a table, this might be the table name.  This is how we can distinguish 
		/// between two ids that might be the same.</param>
		/// <param name="originalId">This can be used as the seed for the generated id.</param>
		/// <returns></returns>
		protected override string Generate(string scope, string originalId)
		{
			//In Subtext (prior to version 2.0), the comments and posts are 
			//in the same database table, thus they share the same scope.
			if (scope == IdScopes.Comments || scope == IdScopes.TrackBacks)
				scope = IdScopes.Posts;

			return base.Generate(scope, originalId);
		}
	}
}
