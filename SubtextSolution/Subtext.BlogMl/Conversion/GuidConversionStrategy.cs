using System;

namespace Subtext.BlogML.Conversion
{
	public class GuidConversionStrategy : IdConversionStrategy
	{
		/// <summary>
		/// Generates a new guid for the specified scope.  The originalId is ignored.
		/// </summary>
		/// <param name="scope">The scope for the id.  For example, for an auto-incrementing 
		/// int id in a table, this might be the table name.  This is how we can distinguish 
		/// between two ids that might be the same.</param>
		/// <param name="originalId">This can be used as the seed for the generated id.</param>
		/// <returns></returns>
		protected override string Generate(string scope, string originalId)
		{
			return Guid.NewGuid().ToString();
		}
	}
}
