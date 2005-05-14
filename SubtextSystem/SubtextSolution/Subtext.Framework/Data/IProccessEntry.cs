using System;
using Subtext.Framework.Components;

namespace Subtext.Framework.Data
{
	/// <summary>
	/// Summary description for IProccessEntry.
	/// </summary>
	public interface IProcessEntry
	{
		int Create(Entry entry);
		bool Update(Entry entry);
	}
}
