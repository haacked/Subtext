using System;
using System.Collections.Generic;
using System.Globalization;

namespace Subtext.BlogML.Conversion
{
	public class IntConversionStrategy : IdConversionStrategy
	{
		Dictionary<string, int> idScopeCounter = new Dictionary<string, int>();

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
			int currentIdForScope;
			if(this.idScopeCounter.TryGetValue(scope, out currentIdForScope))
			{
				currentIdForScope++;
				this.idScopeCounter[scope] = currentIdForScope;
				return currentIdForScope.ToString(CultureInfo.InvariantCulture);
			}
			
			this.idScopeCounter.Add(scope, 1);
			return "1";
		}
	}
}
