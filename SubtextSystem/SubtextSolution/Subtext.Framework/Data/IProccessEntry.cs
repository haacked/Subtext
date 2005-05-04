using System;
using Subtext.Framework.Components;

namespace Subtext.Framework.Data
{
	/// <summary>
	/// Summary description for IProccessEntry.
	/// </summary>
	public interface IProccessEntry
	{
		int Create(Entry entry);
		bool Update(Entry entry);
	}
}
