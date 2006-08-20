using System;

namespace Subtext.BlogML.Conversion
{
	public class NullConversionStrategy : IdConversionStrategy
	{
		/// <summary>
		/// This is the null conversion strategy. It just returns the original id.
		/// </summary>
		/// <param name="scope">The scope for the id.  For example, for an auto-incrementing 
		/// int id in a table, this might be the table name.  This is how we can distinguish 
		/// between two ids that might be the same.</param>
		/// <param name="originalId"></param>
		/// <returns></returns>
		protected override string Generate(string scope, string originalId)
		{
			return originalId;
		}
	}
}
