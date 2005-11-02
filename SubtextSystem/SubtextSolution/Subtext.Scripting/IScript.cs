using System;
using System.Data.SqlClient;

namespace Subtext.Scripting
{
	/// <summary>
	/// Interface implemented by a script.
	/// </summary>
	public interface IScript
	{
		int Execute(SqlTransaction transaction);
	}
}
