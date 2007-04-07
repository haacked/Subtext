using System;

namespace Subtext
{
	public abstract class Command
	{
		/// <summary>
		/// Execute the command.
		/// </summary>
		/// <remarks>
		/// Template method.
		/// </remarks>
		public void Execute(Arguments arguments)
		{
			//Pre command
			ExecuteCommand(arguments);
			//Post command
		}

		protected abstract void ExecuteCommand(Arguments arguments);
	}
}
